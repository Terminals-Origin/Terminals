using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Credentials;
using Terminals.Forms;
using Terminals.History;
using Terminals.Integration.Import;

namespace Terminals
{
    internal partial class FavsList : UserControl
    {
        private MethodInvoker _historyInvoker;
        private Boolean _eventDone = false;
        private Object _historyLock = new Object();
        private Boolean _dirtyHistory = false;
        private HistoryByFavorite _historyByFavorite = null;
        private HistoryController _historyController = new HistoryController();
        private List<String> _nodeTextListHistory;
        private MainForm _mainForm;
        public static CredentialSet credSet = new CredentialSet();

        public FavsList()
        {
            InitializeComponent();
            this._historyInvoker = new MethodInvoker(UpdateHistory);

            // Update the old treeview theme to the new theme from Win Vista and up
            NativeApi.SetWindowTheme(this.favsTree.Handle, "Explorer", null);
            NativeApi.SetWindowTheme(this.historyTreeView.Handle, "Explorer", null);
        }

        private MainForm GetMainForm()
        {
            if (this._mainForm == null)
                this._mainForm = MainForm.GetMainForm();

            return this._mainForm;
        }

        public void LoadFavs()
        {
            FavoriteTreeListLoader loader = new FavoriteTreeListLoader(this.favsTree);
            loader.Load();
        }

        public void RecordHistoryItem(String Name)
        {
            this._historyController.RecordHistoryItem(Name, true);
            this._dirtyHistory = true;
        }

        #region private
        private void UpdateHistory()
        {
            lock (this._historyLock)
            {
                this._nodeTextListHistory = new List<String>();
                foreach (TreeNode node in historyTreeView.Nodes)
                {
                    if (node.IsExpanded)
                        this._nodeTextListHistory.Add(node.Text);
                }

                this._dirtyHistory = true;
                if (tabControl1.SelectedTab == this.HistoryTabPage)
                {
                    //update history now!
                    if (!this._eventDone)
                    {
                        this.historyTreeView.DoubleClick += new EventHandler(this.HistoryTreeView_DoubleClick);
                        this._eventDone = true;
                    }

                    historyTreeView.Nodes.Clear();
                    Dictionary<String, List<String>> uniqueFavsPerGroup = new Dictionary<String, List<String>>();
                    SerializableDictionary<String, List<History.HistoryItem>> GroupedByDate = this._historyByFavorite.GroupedByDate;
                    foreach (String name in GroupedByDate.Keys)
                    {
                        List<String> uniqueList = null;
                        if (uniqueFavsPerGroup.ContainsKey(name))
                            uniqueList = uniqueFavsPerGroup[name];

                        if (uniqueList == null)
                        {
                            uniqueList = new List<String>();
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

                    this._dirtyHistory = false;
                }

                foreach (TreeNode node in this.historyTreeView.Nodes)
                {
                    if (this._nodeTextListHistory.Contains(node.Text))
                        node.Expand();
                }
            }
        }

        private void HistoryTreeView_DoubleClick(object sender, EventArgs e)
        {
            this.StartConnection(this.historyTreeView);
        }

        private void History_OnHistoryLoaded(HistoryByFavorite History)
        {
            this._historyByFavorite = History;
            this.Invoke(this._historyInvoker);
        }

        private void FavsList_Load(object sender, EventArgs e)
        {
            this.favsTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.FavsTree_NodeMouseClick);
            this.LoadFavs();
            this._historyController.OnHistoryLoaded += new HistoryController.HistoryLoaded(this.History_OnHistoryLoaded);
            this._historyController.LazyLoadHistory();
        }

        private void FavsTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.consoleToolStripMenuItem.Checked = false;
                this.newWindowToolStripMenuItem.Checked = false;
                this.consoleAllToolStripMenuItem.Checked = false;
                this.newWindowAllToolStripMenuItem.Checked = false;

                this.favsTree.SelectedNode = e.Node;

                FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);

                if (fav != null)
                    this.favsTree.ContextMenuStrip = this.contextMenuStrip1;
                else
                    this.favsTree.ContextMenuStrip = this.contextMenuStrip2;
            }
        }

        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (fav != null)
                this.GetMainForm().OpenNetworkingTools("Ping", fav.ServerName);
        }

        private void dNSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (fav != null)
                this.GetMainForm().OpenNetworkingTools("DNS", fav.ServerName);
        }

        private void traceRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (fav != null)
                this.GetMainForm().OpenNetworkingTools("Trace", fav.ServerName);
        }

        private void tSAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (fav != null)
                this.GetMainForm().OpenNetworkingTools("TSAdmin", fav.ServerName);
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (fav != null)
                this.GetMainForm().ShowManageTerminalForm(fav);
        }

        private void FavsTree_DoubleClick(object sender, EventArgs e)
        {
            this.StartConnection(favsTree);
        }

        private void ShutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            String msg = String.Empty;
            NetTools.MagicPacket.ShutdownCommands shutdownStyle;

            FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (fav != null)
            {
                if (menuItem.Equals(this.shutdownToolStripMenuItem))
                {
                    msg = String.Format("Are you sure you want to shutdown this machine: {0}", fav.ServerName);
                    shutdownStyle = NetTools.MagicPacket.ShutdownCommands.ForcedShutdown;
                }
                else if (menuItem.Equals(this.rebootToolStripMenuItem))
                {
                    msg = String.Format("Are you sure you want to reboot this machine: {0}", fav.ServerName);
                    shutdownStyle = NetTools.MagicPacket.ShutdownCommands.ForcedReboot;
                }
                else
                {
                    return;
                }

                if (MessageBox.Show(msg, Program.Resources.GetString("Confirmation"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(fav.UserName, fav.Password, fav.DomainName);
                    if (String.IsNullOrEmpty(credentials.Domain))
                        credentials.Domain = Settings.DefaultDomain;

                    if (String.IsNullOrEmpty(credentials.UserName))
                        credentials.UserName = Settings.DefaultUsername;

                    if (String.IsNullOrEmpty(credentials.Password))
                        credentials.Password = Settings.DefaultPassword;

                    try
                    {
                        if (NetTools.MagicPacket.ForceShutdown(fav.ServerName, shutdownStyle, credentials) == 0)
                        {
                            MessageBox.Show("Terminals successfully sent the shutdown command.");
                            return;
                        }
                    }
                    catch (System.Management.ManagementException ex)
                    {
                        Terminals.Logging.Log.Info(ex.ToString(), ex);
                        MessageBox.Show(Program.Resources.GetString("UnableToRemoteShutdown") + "\r\nPlease check the log file.");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show(Program.Resources.GetString("UnableToRemoteShutdown") + "\r\n\r\nAccess is Denied.");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show(Program.Resources.GetString("UnableToRemoteShutdown"));
            }
        }

        private void enableRDPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (fav != null)
            {
                Microsoft.Win32.RegistryKey reg = Microsoft.Win32.RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, fav.ServerName);
                Microsoft.Win32.RegistryKey ts = reg.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server", true);
                Object deny = ts.GetValue("fDenyTSConnections");
                if (deny != null)
                {
                    Int32 d = Convert.ToInt32(deny);
                    if (d == 1)
                    {
                        ts.SetValue("fDenyTSConnections", 0);
                        if (MessageBox.Show("Terminals was able to enable the RDP on the remote machine, would you like to reboot that machine for the change to take effect?", "Reboot Required", MessageBoxButtons.YesNo) == DialogResult.OK)
                        {
                            this.ShutdownToolStripMenuItem_Click(this.rebootToolStripMenuItem, null);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Terminals did not need to enable RDP because it was already set.");
                    }

                    return;
                }
            }

            MessageBox.Show("Terminals was not able to enable RDP remotely.");
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Connect(this.favsTree.SelectedNode, false, this.consoleToolStripMenuItem.Checked, this.newWindowToolStripMenuItem.Checked);
        }

        private void Connect(TreeNode SelectedNode, bool AllChildren, bool Console, bool NewWindow)
        {
            if (AllChildren)
            {
                foreach (TreeNode node in SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (node.Tag as FavoriteConfigurationElement);
                    if (fav != null)
                    {
                        this.GetMainForm().Connect(fav.Name, Console, NewWindow);
                    }
                }
            }
            else
            {
                FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
                if (fav != null)
                {
                    this.GetMainForm().Connect(fav.Name, Console, NewWindow);
                }
            }

            this.contextMenuStrip1.Close();
            this.contextMenuStrip2.Close();
        }

        private void normallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.connectToolStripMenuItem_Click(null, null);
        }

        private void connectToAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Connect(this.favsTree.SelectedNode, true, this.consoleAllToolStripMenuItem.Checked, this.newWindowAllToolStripMenuItem.Checked);
        }

        private void computerManagementMMCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (fav != null)
            {
                System.Diagnostics.Process.Start("mmc.exe", "compmgmt.msc /a /computer=" + fav.ServerName);
            }

        }

        private void systemInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (fav != null)
            {
                String programFiles = System.Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                //if(programFiles.Contains("(x86)")) programFiles = programFiles.Replace(" (x86)","");
                String path = String.Format(@"{0}\common files\Microsoft Shared\MSInfo\msinfo32.exe", programFiles);
                if (System.IO.File.Exists(path))
                {
                    System.Diagnostics.Process.Start(String.Format("\"{0}\"", path), String.Format("/computer {0}", fav.ServerName));
                }
            }
        }

        private void setCredentialByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String tagName = this.favsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Credential by Tag\r\n\r\nThis will replace the credential used for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Credential" + " - " + tagName);
            if (result.ReturnCode == DialogResult.OK)
            {
                if (StoredCredentials.Instance.GetByName(result.Text) == null)
                {
                    MessageBox.Show("The credential you specified does not exist.");
                    return;
                }

                this.GetMainForm().Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Settings.DelayConfigurationSave = true;
                foreach (TreeNode favNode in this.favsTree.SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (favNode.Tag as FavoriteConfigurationElement);
                    if (fav != null)
                    {
                        fav.Credential = result.Text;
                        Settings.EditFavorite(fav.Name, fav);
                    }
                }
                Settings.DelayConfigurationSave = false;
                Settings.Config.Save();
                this.GetMainForm().Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Credential by Tag Complete.");
            }
        }

        private void setPasswordByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String tagName = this.favsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Password by Tag\r\n\r\nThis will replace the password for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Password" + " - " + tagName, '*');
            if (result.ReturnCode == DialogResult.OK)
            {
                this.GetMainForm().Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Settings.DelayConfigurationSave = true;
                foreach (TreeNode favNode in this.favsTree.SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (favNode.Tag as FavoriteConfigurationElement);
                    if (fav != null)
                    {
                        fav.Password = result.Text;
                        Settings.EditFavorite(fav.Name, fav);
                    }
                }

                Settings.DelayConfigurationSave = false;
                Settings.Config.Save();
                this.GetMainForm().Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Password by Tag Complete.");
            }
        }

        private void setDomainByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String tagName = this.favsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Domain by Tag\r\n\r\nThis will replace the Domain for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Domain" + " - " + tagName);
            if (result.ReturnCode == DialogResult.OK)
            {
                this.GetMainForm().Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Settings.DelayConfigurationSave = true;
                foreach (TreeNode favNode in this.favsTree.SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (favNode.Tag as FavoriteConfigurationElement);
                    if (fav != null)
                    {
                        fav.DomainName = result.Text;
                        Settings.EditFavorite(fav.Name, fav);
                    }
                }

                Settings.DelayConfigurationSave = false;
                Settings.Config.Save();
                this.GetMainForm().Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Domain by Tag Complete.");
            }
        }

        private void setUsernameByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String tagName = this.favsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Username by Tag\r\n\r\nThis will replace the Username for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Username" + " - " + tagName);
            if (result.ReturnCode == DialogResult.OK)
            {
                this.GetMainForm().Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Settings.DelayConfigurationSave = true;
                foreach (TreeNode favNode in this.favsTree.SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (favNode.Tag as FavoriteConfigurationElement);
                    if (fav != null)
                    {
                        fav.UserName = result.Text;
                        Settings.EditFavorite(fav.Name, fav);
                    }
                }

                Settings.DelayConfigurationSave = false;
                Settings.Config.Save();
                this.GetMainForm().Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Username by Tag Complete.");
            }
        }

        private void deleteAllFavoritesByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String tagName = this.favsTree.SelectedNode.Text;
            DialogResult result = MessageBox.Show("Delete all Favorites by Tag\r\n\r\nThis will DELETE all Favorites within this tag.\r\n\r\nUse at your own risk!", "Delete all Favorites by Tag" + " - " + tagName, MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                this.GetMainForm().Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                Settings.DelayConfigurationSave = true;
                foreach (TreeNode favNode in this.favsTree.SelectedNode.Nodes)
                {
                    FavoriteConfigurationElement fav = (favNode.Tag as FavoriteConfigurationElement);
                    if (fav != null)
                    {
                        Settings.DeleteFavorite(fav.Name);
                    }
                }

                Settings.Config.Save();
                this.GetMainForm().Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Delete all Favorites by Tag Complete.");
                this.LoadFavs();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab == this.HistoryTabPage && this._dirtyHistory)
            {
                this.Invoke(this._historyInvoker);
            }
        }

        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement favorite = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (favorite != null)
            {
                Settings.DeleteFavorite(favorite.Name);
                Settings.DeleteFavoriteButton(favorite.Name);
            }

            this.LoadFavs();
        }

        private void favsTree_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
        }

        private void favsTree_DragDrop(object sender, DragEventArgs e)
        {
            String[] files = e.Data.GetData(DataFormats.FileDrop) as String[];
            if (files != null)
            {
                this.Cursor = Cursors.WaitCursor;
                List<FavoriteConfigurationElement> favoritesToImport = Importers.ImportFavorites(files);
                Settings.AddFavorites(favoritesToImport, true);

                if (favoritesToImport.Count > 0)
                    this.GetMainForm().LoadFavorites();

                this.Cursor = Cursors.Default;
                OrganizeFavoritesForm.ShowImportResultMessage(favoritesToImport.Count);
            }
        }

        private void StartConnection(TreeView tv)
        {
            // connections are always under some parent node in History and in Favorites
            if (tv.SelectedNode != null && this.favsTree.SelectedNode.Level > 0)
            {
                MainForm mainForm = this.GetMainForm();
                mainForm.Connect(tv.SelectedNode.Text, false, false);
            }
        }

        #endregion

        private void historyTreeView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.StartConnection(historyTreeView);
        }

        private void connectAsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            this.connectAsToolStripMenuItem.DropDownItems.Clear();
            this.connectAsToolStripMenuItem.DropDownItems.Add(this.userConnectToolStripMenuItem);

            List<CredentialSet> list = StoredCredentials.Instance.Items;

            foreach (CredentialSet s in list)
            {
                this.connectAsToolStripMenuItem.DropDownItems.Add(s.Name, null, new EventHandler(this.connectAsCred_Click));
            }
        }

        private void connectAsCred_Click(object sender, EventArgs e)
        {
            FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
            if (fav != null)
            {
                this.GetMainForm().Connect(fav.Name, this.consoleToolStripMenuItem.Checked,
                                           this.newWindowToolStripMenuItem.Checked,
                                           StoredCredentials.Instance.GetByName(sender.ToString()));
            }
        }

        private void userConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form usrForm = new UserSelectForm();
            usrForm.ShowDialog(this.GetMainForm());
            if (credSet != null)
            {
                FavoriteConfigurationElement fav = (this.favsTree.SelectedNode.Tag as FavoriteConfigurationElement);
                if (fav != null)
                {
                    this.GetMainForm().Connect(fav.Name, this.consoleToolStripMenuItem.Checked, this.newWindowToolStripMenuItem.Checked, credSet);
                }
            }
        }

        private void displayWindow_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip1.Show();
        }

        private void displayAllWindow_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip2.Show();
        }
    }
}