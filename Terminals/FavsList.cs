using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

using Terminals.History;

namespace Terminals
{
    public partial class FavsList : UserControl
    {
        private static string _untaggedKey = "Untagged";
        private MethodInvoker _historyInvoker;
        private bool _eventDone = false;
        private object _historyLock = new object();
        private bool _dirtyHistory = false;        
        private HistoryByFavorite _historyByFavorite = null;
        private HistoryController _historyController = new HistoryController();        
        private List<string> _nodeTextList;
        private List<string> _nodeTextListHistory;
        private MainForm _mainForm;

        public FavsList()
        {
            InitializeComponent();
            _historyInvoker = new MethodInvoker(UpdateHistory);            
        }

        private MainForm GetMainForm() 
        { 
            if(_mainForm == null)
                _mainForm = MainForm.GetMainForm();
            return _mainForm;
        }

        public void LoadFavs()
        {
            _nodeTextList = new List<string>();
            foreach (TreeNode node in favsTree.Nodes)
            {
                if (node.IsExpanded)
                    _nodeTextList.Add(node.Text);
            }

            favsTree.Nodes.Clear();
            SortedDictionary<string, FavoriteConfigurationElement> favorites = Settings.GetSortedFavorites(Settings.DefaultSortProperty);
            SortedDictionary<string, TreeNode> SortedTags = new SortedDictionary<string, TreeNode>();
            SortedTags.Add(_untaggedKey, new TreeNode(_untaggedKey));
            favsTree.Nodes.Add(SortedTags[_untaggedKey]);
            if (favorites != null)
            {
                foreach (string key in favorites.Keys)
                {
                    FavoriteConfigurationElement fav = favorites[key];
                    if (fav.TagList.Count > 0)
                    {
                        foreach (string tag in fav.TagList)
                        {
                            TreeNode favNode = new TreeNode(fav.Name);
                            favNode.Tag = fav;
                            if (!SortedTags.ContainsKey(tag))
                            {
                                TreeNode tagNode = new TreeNode(tag);
                                favsTree.Nodes.Add(tagNode);
                                SortedTags.Add(tag, tagNode);
                            }

                            if (!SortedTags[tag].Nodes.Contains(favNode)) 
                                SortedTags[tag].Nodes.Add(favNode);
                        }
                    }
                    else
                    {
                        TreeNode favNode = new TreeNode(fav.Name);
                        favNode.Tag = fav;

                        if (!SortedTags[_untaggedKey].Nodes.Contains(favNode)) 
                            SortedTags[_untaggedKey].Nodes.Add(favNode);
                    }
                }
            }
            favsTree.Sort();

            foreach (TreeNode node in favsTree.Nodes)
            {
                if (_nodeTextList.Contains(node.Text))
                    node.Expand();
            }
        }
        public void RecordHistoryItem(string Name)
        {
            _historyController.RecordHistoryItem(Name, true);
            _dirtyHistory = true;
        }             

        #region private
        private MainForm MainForm
        {
            get
            {
                return (this.ParentForm as MainForm);
            }
        }
        private void UpdateHistory()
        {
            lock (_historyLock)
            {
                _nodeTextListHistory = new List<string>();
                foreach (TreeNode node in historyTreeView.Nodes)
                {
                    if (node.IsExpanded)
                        _nodeTextListHistory.Add(node.Text);
                }

                _dirtyHistory = true;
                if (tabControl1.SelectedTab == HistoryTabPage)
                {
                    //update history now!
                    if (!_eventDone)
                    {
                        historyTreeView.DoubleClick += new EventHandler(HistoryTreeView_DoubleClick);
                        _eventDone = true;
                    }
                    historyTreeView.Nodes.Clear();
                    Dictionary<string, List<string>> uniqueFavsPerGroup = new Dictionary<string, List<string>>();
                    SerializableDictionary<string, List<History.HistoryItem>> GroupedByDate = _historyByFavorite.GroupedByDate;
                    foreach (string name in GroupedByDate.Keys)
                    {
                        List<string> uniqueList = null;
                        if (uniqueFavsPerGroup.ContainsKey(name)) uniqueList = uniqueFavsPerGroup[name];
                        if (uniqueList == null)
                        {
                            uniqueList = new List<string>();
                            uniqueFavsPerGroup.Add(name, uniqueList);
                        }

                        TreeNode NameNode = historyTreeView.Nodes.Add(name);
                        foreach (History.HistoryItem fav in GroupedByDate[name])
                        {
                            if (!uniqueList.Contains(fav.Name))
                            {
                                TreeNode FavNode = NameNode.Nodes.Add(fav.Name);
                                FavNode.Tag = fav.ID;
                                uniqueList.Add(fav.Name);
                            }
                        }
                    }
                    _dirtyHistory = false;
                }

                foreach (TreeNode node in historyTreeView.Nodes)
                {
                    if (_nodeTextListHistory.Contains(node.Text))
                        node.Expand();
                }
            }
        }
        private void HistoryTreeView_DoubleClick(object sender, EventArgs e)
        {
            StartConnection(historyTreeView);
        }
        private void History_OnHistoryLoaded(HistoryByFavorite History)
        {
            _historyByFavorite = History;
            this.Invoke(_historyInvoker);
        }
        private void FavsList_Load(object sender, EventArgs e)
        {
            favsTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(FavsTree_NodeMouseClick);
            LoadFavs();
            _historyController.OnHistoryLoaded += new HistoryController.HistoryLoaded(History_OnHistoryLoaded);
            _historyController.LazyLoadHistory();
        }
        private void FavsTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                favsTree.SelectedNode = e.Node;
                
                FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);

                if (fav != null)
                    this.favsTree.ContextMenuStrip = this.contextMenuStrip1;
                else
                    this.favsTree.ContextMenuStrip = this.contextMenuStrip2;
            }
        }
        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("Ping", fav.ServerName);
        }
        private void dNSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("DNS", fav.ServerName);
        }
        private void traceRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("Trace", fav.ServerName);
        }
        private void tSAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) MainForm.OpenNetworkingTools("TSAdmin", fav.ServerName);
        }
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null) 
                MainForm.ShowManageTerminalForm(fav);
        }
        private void FavsTree_DoubleClick(object sender, EventArgs e)
        {
            StartConnection(favsTree);
        }
        private void connectConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void rebootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
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
            FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
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
            FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
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
            Connect(favsTree.SelectedNode, false, false, false);
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
                FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
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
            Connect(favsTree.SelectedNode, false, true, false);
        }
        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect(favsTree.SelectedNode, false, true, true);

        }
        private void newWindowToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Connect(favsTree.SelectedNode, false, false, true);

        }
        private void connectToAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect(favsTree.SelectedNode, true, false, false);
        }
        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect(favsTree.SelectedNode, true, true, false);
        }
        private void newWindowToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Connect(favsTree.SelectedNode, true, false, true);
        }
        private void newWindowToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Connect(favsTree.SelectedNode, true, true, true);
        }
        private void computerManagementMMCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if(fav != null)
            {
                System.Diagnostics.Process.Start("mmc.exe", "compmgmt.msc /a /computer=" + fav.ServerName);
            }

        }
        private void systemInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
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
        private void setCredentialByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tagName = favsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Credential by Tag\r\n\r\nThis will replace the credential used for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Credential" + " - " + tagName);
            if (result.ReturnCode == DialogResult.OK)
            {
                if (Credentials.CredentialSet.CredentialByName(result.Text) == null)
                {
                    MessageBox.Show("The credential you specified does not exist.");
                    return;
                }

                this.MainForm.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Settings.DelayConfigurationSave = true;
                foreach (TreeNode favNode in favsTree.SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (favNode.Tag as FavoriteConfigurationElement);
                    if (fav != null)
                    {
                        fav.Credential = result.Text;
                        Settings.EditFavorite(fav.Name, fav);
                    }
                }
                Settings.Config.Save();
                this.MainForm.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Credential by Tag Complete.");
            }
        }
        private void setPasswordByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tagName = favsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Password by Tag\r\n\r\nThis will replace the password for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Password" + " - " + tagName, '*');
            if (result.ReturnCode == DialogResult.OK)
            {
                this.MainForm.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Settings.DelayConfigurationSave = true;
                foreach (TreeNode favNode in favsTree.SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (favNode.Tag as FavoriteConfigurationElement);
                    if (fav != null)
                    {
                        fav.Password = result.Text;
                        Settings.EditFavorite(fav.Name, fav);
                    }
                }
                Settings.Config.Save();
                this.MainForm.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Password by Tag Complete.");
            }
        }
        private void setDomainByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tagName = favsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Domain by Tag\r\n\r\nThis will replace the Domain for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Domain" + " - " + tagName);
            if (result.ReturnCode == DialogResult.OK)
            {
                this.MainForm.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Settings.DelayConfigurationSave = true;
                foreach (TreeNode favNode in favsTree.SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (favNode.Tag as FavoriteConfigurationElement);
                    if (fav != null)
                    {
                        fav.DomainName = result.Text;
                        Settings.EditFavorite(fav.Name, fav);
                    }
                }
                Settings.Config.Save();
                this.MainForm.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Domain by Tag Complete.");
            }
        }
        private void setUsernameByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tagName = favsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Username by Tag\r\n\r\nThis will replace the Username for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Username" + " - " + tagName);
            if (result.ReturnCode == DialogResult.OK)
            {
                this.MainForm.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Settings.DelayConfigurationSave = true;
                foreach (TreeNode favNode in favsTree.SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (favNode.Tag as FavoriteConfigurationElement);
                    if (fav != null)
                    {
                        fav.UserName = result.Text;
                        Settings.EditFavorite(fav.Name, fav);
                    }
                }
                Settings.Config.Save();
                this.MainForm.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Username by Tag Complete.");
            }
        }
        private void deleteAllFavoritesByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tagName = favsTree.SelectedNode.Text;
            DialogResult result = MessageBox.Show("Delete all Favorites by Tag\r\n\r\nThis will DELETE all Favorites within this tag.\r\n\r\nUse at your own risk!", "Delete all Favorites by Tag" + " - " + tagName, MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                this.MainForm.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Settings.DelayConfigurationSave = true;
                foreach (TreeNode favNode in favsTree.SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (favNode.Tag as FavoriteConfigurationElement);
                    if (fav != null)
                    {
                        Settings.DeleteFavorite(fav.Name);
                    }
                }
                Settings.Config.Save();
                this.MainForm.Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Delete all Favorites by Tag Complete.");
                LoadFavs();
            }
        }        
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == HistoryTabPage && _dirtyHistory)
            {
                this.Invoke(_historyInvoker);
            }
        }
        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement favorite = (favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (favorite != null)
            {
                Settings.DeleteFavorite(favorite.Name);
                Settings.DeleteFavoriteButton(favorite.Name);
            }
            LoadFavs();
        }
        private void favsTree_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
        }
        private void favsTree_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                string str = Path.GetExtension(file).ToLower();
                if (str.Equals(".xml"))
                {
                    ExportImport.ExportImport.ImportXML(file, true);
                    GetMainForm().LoadFavorites();
                }
                else
                {
                    MessageBox.Show("This are not a XML file, Quiting");
                }
            }
        }

        private void StartConnection(TreeView tv)
        {
            if (tv.SelectedNode != null)
                this.MainForm.Connect(tv.SelectedNode.Text, false, false);
        }
        #endregion

        private void historyTreeView_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                StartConnection(historyTreeView);
        }
    }
}