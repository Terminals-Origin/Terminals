using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text.RegularExpressions;
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

        private static IFavorites PersistedFavorites
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
                    var fav = node.Tag as IFavorite;
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
                    ISecurityOptions security = fav.Security.GetResolvedCredentials();
                    NetworkCredential credentials = new NetworkCredential(security.UserName, security.Password, security.Domain);

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
                RegistryKey reg = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, fav.ServerName);
                RegistryKey ts = reg.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server", true);
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
            const string variable = "Credential";
            InputBoxResult result = this.PromptForVariableChange(variable);
            if (result.ReturnCode == DialogResult.OK)
            {
                ICredentialSet credential = Persistance.Instance.Credentials[result.Text];
                if (credential == null)
                {
                    MessageBox.Show("The credential you specified does not exist.");
                    return;
                }

                List<IFavorite> selectedFavorites = this.StartBatchUpdate();
                PersistedFavorites.ApplyCredentialsToAllFavorites(selectedFavorites, credential);
                this.FinishBatchUpdate(variable);
            }
        }

        private void setPasswordByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string variable = "Password";
            InputBoxResult result = this.PromptForVariableChange(variable, '*');
            if (result.ReturnCode == DialogResult.OK)
            {
                List<IFavorite> selectedFavorites = this.StartBatchUpdate();
                PersistedFavorites.SetPasswordToAllFavorites(selectedFavorites, result.Text);
                this.FinishBatchUpdate(variable);
            }
        }

        private void setDomainByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string variable = "Domain name";
            InputBoxResult result = this.PromptForVariableChange(variable);
            if (result.ReturnCode == DialogResult.OK)
            {
                List<IFavorite> selectedFavorites = this.StartBatchUpdate();
                PersistedFavorites.ApplyDomainNameToAllFavorites(selectedFavorites, result.Text);
                this.FinishBatchUpdate(variable);
            }
        }

        private void setUsernameByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string variable = "User name";
            InputBoxResult result = this.PromptForVariableChange(variable);
            if (result.ReturnCode == DialogResult.OK)
            {
                List<IFavorite> selectedFavorites = this.StartBatchUpdate();
                PersistedFavorites.ApplyUserNameToAllFavorites(selectedFavorites, result.Text);
                this.FinishBatchUpdate(variable);
            }
        }

        private List<IFavorite> StartBatchUpdate()
        {
            this.GetMainForm().Cursor = Cursors.WaitCursor;
            return this.GetSelectedFavorites();
        }

        private List<IFavorite> GetSelectedFavorites()
        {
            return this.favsTree.SelectedNode.Nodes.Cast<FavoriteTreeNode>()
                .Select(node => node.Favorite)
                .ToList();
        }

        private void FinishBatchUpdate(string variable)
        {
            this.GetMainForm().Cursor = Cursors.Default;
            string message = string.Format("Set {0} by group Complete.", variable);
            MessageBox.Show(message);
        }

        private InputBoxResult PromptForVariableChange(string variable, char passwordChar = '\0')
        {
            String groupName = this.favsTree.SelectedNode.Text;
            string prompt = String.Format("This will replace the {0} for all Favorites within this group.\r\nUse at your own risk!\r\n\r\nEnter new {0}:",
                                            variable);
            string title = string.Format("Change {0} - {1}", variable, groupName);
            if (passwordChar != '\0')
                return InputBox.Show(prompt, title, passwordChar);

            return InputBox.Show(prompt, title);
        }

        private void deleteAllFavoritesByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String groupName = this.favsTree.SelectedNode.Text;
            string title = "Delete all Favorites by group - " + groupName;
            const string prompt = "This will DELETE all Favorites within this group.\r\nUse at your own risk!\r\n\r\nDo you realy want to delete them?";
            DialogResult result = MessageBox.Show(prompt, title, MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                List<IFavorite> selectedFavorites = this.StartBatchUpdate();
                Persistance.Instance.Favorites.Delete(selectedFavorites);
                this.GetMainForm().Cursor = Cursors.Default;
                MessageBox.Show("Delete all Favorites by group Complete.");
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

            IEnumerable<ICredentialSet> credentials = Persistance.Instance.Credentials;
            foreach (ICredentialSet credential in credentials)
            {
                var menuItem = new ToolStripMenuItem(credential.Name, null, new EventHandler(this.connectAsCred_Click));
                menuItem.Tag = credential.Id;
                this.connectAsToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }

        private void connectAsCred_Click(object sender, EventArgs e)
        {
            IFavorite favorite = this.favsTree.SelectedFavorite;
            if (favorite != null)
            {
                var menuItem = sender as ToolStripItem;
                var credential = Persistance.Instance.Credentials[(Guid)menuItem.Tag];
                this.GetMainForm().Connect(favorite.Name, this.consoleToolStripMenuItem.Checked,
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
            Settings.StartDelayedUpdate();
            Settings.ExpandedFavoriteNodes = GetExpandedFavoriteNodes(this.favsTree);
            Settings.ExpandedHistoryNodes = GetExpandedFavoriteNodes(this.historyTreeView);
            Settings.SaveAndFinishDelayedUpdate();
        }

        private static string GetExpandedFavoriteNodes(TreeView treeView)
        {
            List<string> expandedNodes = new List<string>();
            foreach (TreeNode treeNode in treeView.Nodes)
            {
                if (treeNode.IsExpanded)
                    expandedNodes.Add(treeNode.Text);
            }
            return string.Join("%%", expandedNodes.ToArray());
        }

        public void LoadState()
        {
            ExpandTreeView(Settings.ExpandedFavoriteNodes, this.favsTree);
            ExpandTreeView(Settings.ExpandedHistoryNodes, this.historyTreeView);
        }

        private static void ExpandTreeView(string savedNodesToExpand, TreeView treeView)
        {
            List<string> nodesToExpand = new List<string>();
            if (!string.IsNullOrEmpty(savedNodesToExpand))
                nodesToExpand.AddRange(Regex.Split(savedNodesToExpand, "%%"));

            if (nodesToExpand != null && nodesToExpand.Count > 0)
            {
                foreach (TreeNode treeNode in treeView.Nodes)
                {
                    if (nodesToExpand.Contains(treeNode.Text))
                        treeNode.Expand();
                }
            }
        }
    }
}
