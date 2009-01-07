using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    public partial class FavsList : UserControl
    {
        public FavsList()
        {
            InitializeComponent();
        }

        private void FavsList_Load(object sender, EventArgs e)
        {
            FavsTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(FavsTree_NodeMouseClick);
            LoadFavs();
        }
        public string UntaggedKey = "Untagged";
        public void LoadFavs()
        {

            FavsTree.Nodes.Clear();

            SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.DefaultSortProperty);
            SortedDictionary<string, TreeNode> SortedTags = new SortedDictionary<string, TreeNode>();
            SortedTags.Add(UntaggedKey, new TreeNode(UntaggedKey));
            FavsTree.Nodes.Add(SortedTags[UntaggedKey]);
            if(favorites != null)
            {
                foreach(string key in favorites.Keys)
                {

                    FavoriteConfigurationElement fav = favorites[key];
                    if(fav.TagList.Count > 0)
                    {
                        foreach(string tag in fav.TagList)
                        {
                            TreeNode FavNode = new TreeNode(fav.Name);
                            FavNode.Tag = fav;
                            if(!SortedTags.ContainsKey(tag))
                            {
                                TreeNode tagNode = new TreeNode(tag);
                                FavsTree.Nodes.Add(tagNode);
                                SortedTags.Add(tag, tagNode);
                            }
                            if(!SortedTags[tag].Nodes.Contains(FavNode)) SortedTags[tag].Nodes.Add(FavNode);
                        }
                    }
                    else
                    {
                        TreeNode FavNode = new TreeNode(fav.Name);
                        FavNode.Tag = fav;

                        if(!SortedTags[UntaggedKey].Nodes.Contains(FavNode)) SortedTags[UntaggedKey].Nodes.Add(FavNode);
                    }
                }
            }
            FavsTree.Sort();
        }

        void FavsTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                FavsTree.SelectedNode = e.Node;
            }
            if(FavsTree.SelectedNode != null)
            {
                FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
                if(e.Button == MouseButtons.Right)
                {
                    FavsTree.SelectedNode = e.Node;
                }
                pingToolStripMenuItem.Visible = true;
                dNSToolStripMenuItem.Visible = true;
                traceRouteToolStripMenuItem.Visible = true;
                tSAdminToolStripMenuItem.Visible = true;
                propertiesToolStripMenuItem.Visible = true;
                rebootToolStripMenuItem.Visible = true;
                shutdownToolStripMenuItem.Visible = true;
                enableRDPToolStripMenuItem.Visible = true;
                manageAllFavoritesToolStripMenuItem.Visible = false;
                setDomainByTagToolStripMenuItem.Visible = false;
                setUsernameByTagToolStripMenuItem.Visible = false;
                if(fav == null)
                {
                    pingToolStripMenuItem.Visible = false;
                    dNSToolStripMenuItem.Visible = false;
                    traceRouteToolStripMenuItem.Visible = false;
                    tSAdminToolStripMenuItem.Visible = false;
                    propertiesToolStripMenuItem.Visible = false;
                    rebootToolStripMenuItem.Visible = false;
                    shutdownToolStripMenuItem.Visible = false;
                    enableRDPToolStripMenuItem.Visible = false;
                    manageAllFavoritesToolStripMenuItem.Visible = true;
                    setDomainByTagToolStripMenuItem.Visible = true;
                    setUsernameByTagToolStripMenuItem.Visible = true;
                }

            }
        }


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            connectToolStripMenuItem.Visible = !(FavsTree.SelectedNode.Tag == null);
            connectToAllToolStripMenuItem.Visible = (FavsTree.SelectedNode.Tag == null);


        }
        private MainForm MainForm
        {
            get
            {
                return (this.ParentForm as MainForm);
            }
        }

        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("Ping", fav.ServerName);
        }

        private void dNSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("DNS", fav.ServerName);
        }

        private void traceRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("Trace", fav.ServerName);
        }

        private void tSAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("TSAdmin", fav.ServerName);
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.ShowManageTerminalForm(fav);
        }


        private void FavsTree_DoubleClick(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.Connect(fav.Name, false, false);
        }

        private void connectConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.Connect(fav.Name, true, false);
        }

        private void rebootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null)
            {
                if(MessageBox.Show("Are you sure you want to reboot this machine: " + fav.ServerName, Program.Resources.GetString("Confirmation"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    if(NetTools.MagicPacket.ForceReboot(fav.ServerName, NetTools.MagicPacket.ShutdownStyles.ForcedReboot) == 0)
                    {
                        MessageBox.Show("Terminals successfully sent the shutdown command.");
                        return;
                    }
                }
            }
            System.Windows.Forms.MessageBox.Show("Terminals was not able to reboot the machine remotely.");
        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null)
            {
                if(MessageBox.Show("Are you sure you want to shutdown this machine: " + fav.ServerName, Program.Resources.GetString("Confirmation"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if(NetTools.MagicPacket.ForceReboot(fav.ServerName, NetTools.MagicPacket.ShutdownStyles.ForcedShutdown) == 0)
                    {
                        MessageBox.Show("Terminals successfully sent the shutdown command.");
                        return;
                    }
                }
            }
            System.Windows.Forms.MessageBox.Show("Terminals was not able to shutdown the machine remotely.");
        }

        private void enableRDPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null)
            {

                Microsoft.Win32.RegistryKey reg = Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, fav.ServerName);
                Microsoft.Win32.RegistryKey ts = reg.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server", true);
                object deny = ts.GetValue("fDenyTSConnections");
                if(deny != null)
                {
                    int d = Convert.ToInt32(deny);
                    if(d == 1)
                    {
                        ts.SetValue("fDenyTSConnections", 0);
                        if(System.Windows.Forms.MessageBox.Show("Terminals was able to enable the RDP on the remote machine, would you like to reboot that machine for the change to take effect?", "Reboot Required", MessageBoxButtons.YesNo) == DialogResult.OK)
                        {
                            rebootToolStripMenuItem_Click(null, null);
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Terminals did not need to enable RDP because it was already set.");
                    }
                    return;
                }
            }
            System.Windows.Forms.MessageBox.Show("Terminals was not able to enable RDP remotely.");
        }



        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect(FavsTree.SelectedNode, false, false, false);
        }

        private void Connect(TreeNode SelectedNode, bool AllChildren, bool Console, bool NewWindow)
        {
            if(AllChildren)
            {
                foreach(TreeNode node in SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (node.Tag as FavoriteConfigurationElement);
                    if(fav != null)
                    {
                        MainForm.Connect(fav.Name, Console, NewWindow);
                    }
                }
            }
            else
            {
                FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
                if(fav != null)
                {
                    MainForm.Connect(fav.Name, Console, NewWindow);
                }
            }
        }
        private void normallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectToolStripMenuItem_Click(null, null);
        }

        private void forcedConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect(FavsTree.SelectedNode, false, true, false);
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect(FavsTree.SelectedNode, false, true, true);

        }

        private void newWindowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Connect(FavsTree.SelectedNode, false, false, true);

        }

        private void connectToAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect(FavsTree.SelectedNode, true, false, false);
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect(FavsTree.SelectedNode, true, true, false);
        }

        private void newWindowToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Connect(FavsTree.SelectedNode, true, false, true);
        }

        private void newWindowToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Connect(FavsTree.SelectedNode, true, true, true);
        }

        private void computerManagementMMCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null)
            {
                System.Diagnostics.Process.Start("mmc.exe", "compmgmt.msc /a /computer=" + fav.ServerName);
            }

        }

        private void systemInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null)
            {
                string programFiles = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                //if(programFiles.Contains("(x86)")) programFiles = programFiles.Replace(" (x86)","");
                string path = string.Format(@"{0}\common files\Microsoft Shared\MSInfo\msinfo32.exe", programFiles);
                if(System.IO.File.Exists(path))
                {
                    System.Diagnostics.Process.Start(string.Format("\"{0}\"", path), string.Format("/computer={0}", fav.ServerName));
                }
            }
        }

        private void manageAllFavoritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tagName = FavsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Password by Tag\r\n\r\nThis will replace the password for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Password" + " - " + tagName, '*');
            if (result.ReturnCode == DialogResult.OK)
            {
                this.MainForm.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                FavoriteConfigurationElementCollection favs = Settings.GetFavorites();
                foreach (FavoriteConfigurationElement elm in favs)
                {
                    if (elm.TagList.Contains(tagName))
                    {
                        elm.Password = result.Text;
                        Settings.EditFavorite(elm.Name, elm);
                    }
                }
                this.MainForm.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Password by Tag Complete.");
            }
        }

        private void setDomainByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tagName = FavsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Domain by Tag\r\n\r\nThis will replace the Domain for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Domain" + " - " + tagName);
            if (result.ReturnCode == DialogResult.OK)
            {
                this.MainForm.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                FavoriteConfigurationElementCollection favs = Settings.GetFavorites();
                foreach (FavoriteConfigurationElement elm in favs)
                {
                    if (elm.TagList.Contains(tagName))
                    {
                        elm.DomainName = result.Text;                        
                        Settings.EditFavorite(elm.Name, elm);
                    }
                }
                this.MainForm.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Domain by Tag Complete.");
            }
        }

        private void setUsernameByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
                string tagName = FavsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Username by Tag\r\n\r\nThis will replace the Username for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Username" + " - " + tagName);
            if (result.ReturnCode == DialogResult.OK)
            {
                this.MainForm.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                FavoriteConfigurationElementCollection favs = Settings.GetFavorites();
                foreach (FavoriteConfigurationElement elm in favs)
                {
                    if (elm.TagList.Contains(tagName))
                    {
                        elm.UserName = result.Text;
                        Settings.EditFavorite(elm.Name, elm);
                    }
                }
                this.MainForm.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Username by Tag Complete.");
            }
        }

    }
}