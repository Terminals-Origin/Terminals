using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            if(favorites != null) {
                foreach(string key in favorites.Keys) {

                    FavoriteConfigurationElement fav = favorites[key];
                    if(fav.TagList.Count > 0) {
                        foreach(string tag in fav.TagList) {
                            TreeNode FavNode = new TreeNode(fav.Name);
                            FavNode.Tag = fav;
                            if(!SortedTags.ContainsKey(tag)) {
                                TreeNode tagNode = new TreeNode(tag);
                                FavsTree.Nodes.Add(tagNode);
                                SortedTags.Add(tag, tagNode);
                            }
                            if(!SortedTags[tag].Nodes.Contains(FavNode)) SortedTags[tag].Nodes.Add(FavNode);
                        }
                    } else {
                        TreeNode FavNode = new TreeNode(fav.Name);
                        FavNode.Tag = fav;

                        if(!SortedTags[UntaggedKey].Nodes.Contains(FavNode)) SortedTags[UntaggedKey].Nodes.Add(FavNode);
                    }
                }
            }
        }

        void FavsTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            if(e.Button == MouseButtons.Right)
            {
                FavsTree.SelectedNode = e.Node;
            }
            if(FavsTree.SelectedNode != null) {
                FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
                if(e.Button == MouseButtons.Right) {
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
                }
          
            }
        }


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            contextMenuStrip1.Items[1].Visible = true;
            if(FavsTree.SelectedNode != null)
            {
                if(FavsTree.SelectedNode.Tag == null)
                {                    
                    if(FavsTree.SelectedNode.Nodes.Count <= 0) e.Cancel = true;
                    contextMenuStrip1.Items[0].Text = "Connect to All";
                    contextMenuStrip1.Items[1].Visible = false;
                }
                else
                {
                    contextMenuStrip1.Items[0].Text = "Connect to " + FavsTree.SelectedNode.Text;
                }
            }            
        }
        private MainForm MainForm
        {
            get
            {
                return (this.ParentForm as MainForm);
            }
        }
        private void connectToolStripMenuItem_MouseUp(object sender, MouseEventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);

            ToolStripMenuItem menuItem = (sender as ToolStripMenuItem);
            ConnectToMenuItem(menuItem, fav);
        }
        private bool ConnectToMenuItem(ToolStripMenuItem menuItem, FavoriteConfigurationElement fav) {
            if(menuItem != null) {
                if(menuItem.Text.StartsWith("Connect to All")) {
                    if(System.Windows.Forms.MessageBox.Show("Are you sure you want to connect to all of these Terminals?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        foreach(TreeNode child in FavsTree.SelectedNode.Nodes) {
                            fav = (child.Tag as FavoriteConfigurationElement);
                            if(fav != null) {
                                MainForm.Connect(fav.Name, false);
                            }
                        }
                    }
                    return true;
                } else if(menuItem.Text.StartsWith("Connect to ")) {
                    if(fav != null) {
                        MainForm.Connect(fav.Name, false);
                        return true;
                    }
                }
            }
            return false;
        }

        private void pingToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav!=null) MainForm.OpenNetworkingTools("Ping", fav.ServerName);
        }

        private void dNSToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("DNS", fav.ServerName);
        }

        private void traceRouteToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("Trace", fav.ServerName);
        }

        private void tSAdminToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("TSAdmin", fav.ServerName);
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.ShowManageTerminalForm(fav);
        }


        private void FavsTree_DoubleClick(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.Connect(fav.Name, false);
        }

        private void connectConsoleToolStripMenuItem_Click(object sender, EventArgs e) {
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.Connect(fav.Name, true);
        }

        private string ShutdownCommand
        {
            get
            {
                return System.IO.Path.Combine(SystemFolder, "shutdown.exe");
            }
        }
        private string SystemFolder
        {
            get
            {
                return System.Environment.GetFolderPath(Environment.SpecialFolder.System);
            }
        }
        private void rebootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string stdOut = "";
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null)
            {
                if(MessageBox.Show("Are you sure you want to reboot this machine: " + fav.ServerName, "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(ShutdownCommand, string.Format("/m {0} /r /f", fav.ServerName));
                    psi.CreateNoWindow = true;
                    psi.ErrorDialog = true;
                    psi.UseShellExecute = false;
                    psi.ErrorDialogParentHandle = this.Handle;
                    psi.RedirectStandardError = true;
                    psi.RedirectStandardInput = true;
                    psi.RedirectStandardOutput=true;
                    System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);                   
                    p.WaitForExit();
                    string s = p.StandardOutput.ReadToEnd().Trim();
                    if(s != "") {
                        MessageBox.Show(s);
                    }
                    stdOut = p.StandardError.ReadToEnd().Trim() + "\r\n" + s;
                    int exit = p.ExitCode;
                    
                    if(exit == 0) return;
                }
            }
            System.Windows.Forms.MessageBox.Show("Terminals was not able to reboot the machine remotely ("+stdOut+").");
        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string stdOut = "";
            FavoriteConfigurationElement fav = (FavsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null)
            {
                if(MessageBox.Show("Are you sure you want to shutdown this machine: " + fav.ServerName, "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(ShutdownCommand, string.Format("/s /m {0} /f", fav.ServerName));
                    psi.CreateNoWindow = true;
                    psi.ErrorDialog = true;
                    psi.UseShellExecute = false;
                    psi.ErrorDialogParentHandle = this.Handle;
                    psi.RedirectStandardError = true;
                    psi.RedirectStandardInput = true;
                    psi.RedirectStandardOutput = true;
                    System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
                    string s = p.StandardOutput.ReadToEnd().Trim();
                    if(s != "") {
                        MessageBox.Show(s);
                    }
                    stdOut = p.StandardError.ReadToEnd().Trim() + "\r\n" + s;
                    int exit = p.ExitCode;
                    if(exit == 0) return;
                }
            }
            System.Windows.Forms.MessageBox.Show("Terminals was not able to shutdown the machine remotely (" + stdOut + ").");
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

        }
    }
}