using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Windows.Forms;
using Microsoft.Win32;
using Terminals.Configuration;
using Terminals.Credentials;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Forms.Controls;
using Terminals.Integration;

namespace Terminals
{
    internal partial class FavsList : UserControl
    {
        private MainForm _mainForm;

        private IFavorites PersistedFavorites
        {
            get { return Persistance.Instance.Favorites; }
        }

        public FavsList()
        {
            InitializeComponent();

            // Update the old treeview theme to the new theme from Win Vista and up
            Native.Methods.SetWindowTheme(this.favsTree.Handle, "Explorer", null);
            Native.Methods.SetWindowTheme(this.historyTreeView.Handle, "Explorer", null);

            this.historyTreeView.DoubleClick += new EventHandler(this.HistoryTreeView_DoubleClick);
            this.favsTree.Load();
            LoadState();
        }

        #region Private methods
        
        private MainForm GetMainForm()
        {
            if (this._mainForm == null)
                this._mainForm = MainForm.GetMainForm();

            return this._mainForm;
        }

        private void Connect(TreeNode SelectedNode, bool AllChildren, bool Console, bool NewWindow)
        {
            if (AllChildren)
            {
                foreach (TreeNode node in SelectedNode.Nodes)
                {
                    Favorite fav = node.Tag as Favorite;
                    if (fav != null)
                    {
                        this.GetMainForm().Connect(fav.Name, Console, NewWindow);
                    }
                }
            }
            else
            {
                IFavorite fav = this.favsTree.SelectedFavorite;
                if (fav != null)
                {
                    this.GetMainForm().Connect(fav.Name, Console, NewWindow);
                }
            }

            this.contextMenuStrip1.Close();
            this.contextMenuStrip2.Close();
        }

        #endregion

        #region Private event handler methods

        private void FavsList_Load(object sender, EventArgs e)
        {
            this.favsTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.FavsTree_NodeMouseClick);
        }

        private void HistoryTreeView_DoubleClick(object sender, EventArgs e)
        {
            this.StartConnection(this.historyTreeView);
        }

        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
                this.GetMainForm().OpenNetworkingTools("Ping", fav.ServerName);
        }

        private void dNSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
                this.GetMainForm().OpenNetworkingTools("DNS", fav.ServerName);
        }

        private void traceRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
                this.GetMainForm().OpenNetworkingTools("Trace", fav.ServerName);
        }

        private void tSAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
                this.GetMainForm().OpenNetworkingTools("TSAdmin", fav.ServerName);
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
                this.GetMainForm().ShowManageTerminalForm(fav);
        }

        private void ShutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            String msg = String.Empty;
            NetTools.MagicPacket.ShutdownCommands shutdownStyle;

            IFavorite fav = this.favsTree.SelectedFavorite;
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
                    SecurityOptions security = fav.Security.GetResolvedCredentials();
                    NetworkCredential credentials = new NetworkCredential(security.UserName, security.Password, security.DomainName);

                    try
                    {
                        if (NetTools.MagicPacket.ForceShutdown(fav.ServerName, shutdownStyle, credentials) == 0)
                        {
                            MessageBox.Show("Terminals successfully sent the shutdown command.");
                            return;
                        }
                    }
                    catch (ManagementException ex)
                    {
                        Logging.Log.Info(ex.ToString(), ex);
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
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
            {
                Microsoft.Win32.RegistryKey reg = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, fav.ServerName);
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

        private void connectToAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Connect(this.favsTree.SelectedNode, true, this.consoleAllToolStripMenuItem.Checked, this.newWindowAllToolStripMenuItem.Checked);
        }

        private void computerManagementMMCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
            {
                Process.Start("mmc.exe", "compmgmt.msc /a /computer=" + fav.ServerName);
            }

        }

        private void systemInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
            {
                String programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                //if(programFiles.Contains("(x86)")) programFiles = programFiles.Replace(" (x86)","");
                String path = String.Format(@"{0}\common files\Microsoft Shared\MSInfo\msinfo32.exe", programFiles);
                if (File.Exists(path))
                {
                    Process.Start(String.Format("\"{0}\"", path), String.Format("/computer {0}", fav.ServerName));
                }
            }
        }

        private void setCredentialByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String tagName = this.favsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Credential by Tag\r\n\r\nThis will replace the credential used for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Credential" + " - " + tagName);
            if (result.ReturnCode == DialogResult.OK)
            {
                if (Persistance.Instance.Credentials.GetByName(result.Text) == null)
                {
                    MessageBox.Show("The credential you specified does not exist.");
                    return;
                }

                this.GetMainForm().Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                var selectedFavorites  = GetSelectedFavorites();
                PersistedFavorites.ApplyCredentialsToAllFavorites(selectedFavorites, result.Text);

                this.GetMainForm().Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Set Credential by Tag Complete.");
            }
        }

        private List<IFavorite> GetSelectedFavorites()
        {
            return this.favsTree.SelectedNode.Nodes.Cast<FavoriteTreeNode>()
                .Select(node => node.Favorite)
                .ToList();
        }

        private void setPasswordByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String tagName = this.favsTree.SelectedNode.Text;
            InputBoxResult result = InputBox.Show("Set Password by Tag\r\n\r\nThis will replace the password for all Favorites within this tag.\r\n\r\nUse at your own risk!", "Change Password" + " - " + tagName, '*');
            if (result.ReturnCode == DialogResult.OK)
            {
                this.GetMainForm().Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                var selectedFavorites = GetSelectedFavorites();
                PersistedFavorites.SetPasswordToAllFavorites(selectedFavorites, result.Text);

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

                var selectedFavorites = GetSelectedFavorites();
                PersistedFavorites.ApplyDomainNameToAllFavorites(selectedFavorites, result.Text);

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

                var selectedFavorites = GetSelectedFavorites();
                PersistedFavorites.ApplyUserNameToAllFavorites(selectedFavorites, result.Text);

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

                var selectedFavorites = GetSelectedFavorites();
                Persistance.Instance.Favorites.Delete(selectedFavorites);
                
                this.GetMainForm().Cursor = Cursors.Default;
                Application.DoEvents();
                MessageBox.Show("Delete all Favorites by Tag Complete.");
            }
        }

        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite favorite = this.favsTree.SelectedFavorite;
            if (favorite != null)
            {
                PersistedFavorites.Delete(favorite);
            }
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

                if (this.favsTree.SelectedFavorite != null)
                    this.favsTree.ContextMenuStrip = this.contextMenuStrip1;
                else
                    this.favsTree.ContextMenuStrip = this.contextMenuStrip2;
            }
        }

        private void FavsTree_DoubleClick(object sender, EventArgs e)
        {
            this.StartConnection(favsTree);
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
                List<FavoriteConfigurationElement> favoritesToImport = Integrations.Importers.ImportFavorites(files);
                var managedImport = new ImportWithDialogs(this.ParentForm);
                managedImport.Import(favoritesToImport);
            }
        }

        private void StartConnection(TreeView tv)
        {
            // connections are always under some parent node in History and in Favorites
            if (tv.SelectedNode != null && tv.SelectedNode.Level > 0)
            {
                MainForm mainForm = this.GetMainForm();
                mainForm.Connect(tv.SelectedNode.Text, false, false);
            }
        }

        private void historyTreeView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.StartConnection(historyTreeView);
        }

        private void connectAsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            this.connectAsToolStripMenuItem.DropDownItems.Clear();
            this.connectAsToolStripMenuItem.DropDownItems.Add(this.userConnectToolStripMenuItem);

            List<ICredentialSet> list = Persistance.Instance.Credentials.Items;
            foreach (ICredentialSet s in list)
            {
                this.connectAsToolStripMenuItem.DropDownItems.Add(s.Name, null, new EventHandler(this.connectAsCred_Click));
            }
        }

        private void connectAsCred_Click(object sender, EventArgs e)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
            {
                var credential = Persistance.Instance.Credentials.GetByName(sender.ToString());
                this.GetMainForm().Connect(fav.Name, this.consoleToolStripMenuItem.Checked,
                                           this.newWindowToolStripMenuItem.Checked, credential);
            }
        }

        private void userConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var usrForm = new UserSelectForm();
            if (usrForm.ShowDialog() == DialogResult.OK)
            {
                IFavorite selectedFavorite = this.favsTree.SelectedFavorite;
                if (selectedFavorite != null)
                {
                    this.GetMainForm().Connect(selectedFavorite.Name, this.consoleToolStripMenuItem.Checked,
                        this.newWindowToolStripMenuItem.Checked, usrForm.Credentials);
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

        #endregion


        public void SaveState()
        {
            List<string> expanded = new List<string>();
            foreach (TreeNode n in this.favsTree.Nodes)
            {
                if (n.IsExpanded) expanded.Add(n.Text);
            }
            Settings.ExpandedFavoriteNodes = string.Join("%%", expanded.ToArray());

            List<string> expandedHistory = new List<string>();
            foreach (TreeNode n in this.historyTreeView.Nodes)
            {
                if (n.IsExpanded) expandedHistory.Add(n.Text);
            }
            Settings.ExpandedHistoryNodes = string.Join("%%", expandedHistory.ToArray());


            
            
        }
        public void LoadState()
        {
            List<string> expanded = new List<string>();
            string nodes = Settings.ExpandedFavoriteNodes;
            if (!string.IsNullOrEmpty(nodes))
            {
                expanded.AddRange(System.Text.RegularExpressions.Regex.Split(nodes, "%%"));
            }
            if (expanded != null && expanded.Count > 0)
            {
                foreach (TreeNode n in this.favsTree.Nodes)
                {
                    if (expanded.Contains(n.Text)) n.Expand();
                }
            }

            List<string> expandedHistory = new List<string>();
            string historyNodes = Settings.ExpandedHistoryNodes;
            if (!string.IsNullOrEmpty(historyNodes))
            {
                expandedHistory.AddRange(System.Text.RegularExpressions.Regex.Split(historyNodes, "%%"));
            }
            if (expandedHistory != null && expandedHistory.Count > 0)
            {
                foreach (TreeNode n in this.historyTreeView.Nodes)
                {
                    if (expandedHistory.Contains(n.Text)) n.Expand();
                }
            }
        }
    }
}
