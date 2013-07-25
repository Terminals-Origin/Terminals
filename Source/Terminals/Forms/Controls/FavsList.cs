using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Credentials;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Forms.Controls;
using Terminals.Integration;
using Terminals.Network;

namespace Terminals
{
    internal partial class FavsList : UserControl
    {
        private static readonly string shutdownFailMessage = Program.Resources.GetString("UnableToRemoteShutdown");
        private MainForm mainForm;

        private static IFavorites PersistedFavorites
        {
            get { return Persistence.Instance.Favorites; }
        }

        public FavsList()
        {
            InitializeComponent();

            // Update the old treeview theme to the new theme from Win Vista and up
            Native.Methods.SetWindowTheme(this.favsTree.Handle, "Explorer", null);
            Native.Methods.SetWindowTheme(this.historyTreeView.Handle, "Explorer", null);

            this.historyTreeView.DoubleClick += new EventHandler(this.HistoryTreeView_DoubleClick);
        }

        #region Private methods

        private MainForm GetMainForm()
        {
            if (this.mainForm == null)
                this.mainForm = MainForm.GetMainForm();

            return this.mainForm;
        }

        private void CloseMenuStrips()
        {
            this.favoritesContextMenu.Close();
            this.groupsContextMenu.Close();
        }

        private void ConnectToFavorite(IFavorite favorite, bool console, bool newWindow)
        {
            if (favorite != null)
                this.GetMainForm().Connect(favorite.Name, console, newWindow);
        }

        #endregion

        #region Private event handler methods

        private void FavsList_Load(object sender, EventArgs e)
        {
            this.favsTree.Load();
            this.historyTreeView.Load();
            this.LoadState();
            this.favsTree.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.FavsTree_NodeMouseClick);
        }

        private void HistoryTreeView_DoubleClick(object sender, EventArgs e)
        {
            this.StartConnection(this.historyTreeView);
        }

        private void PingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenNetworkingTool(NettworkingTools.Ping);
        }

        private void DNsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenNetworkingTool(NettworkingTools.Dns);
        }

        private void TraceRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenNetworkingTool(NettworkingTools.Trace);
        }

        private void TsAdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenNetworkingTool(NettworkingTools.TsAdmin);
        }

        private void OpenNetworkingTool(NettworkingTools toolName)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
                this.GetMainForm().OpenNetworkingTools(toolName, fav.ServerName);
        }

        private void PropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
                this.GetMainForm().ShowManageTerminalForm(fav);
        }

        private void RebootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ProcessRemoteShutdownOpearation("reboot", ShutdownCommands.ForcedReboot);
        }

        private void ShutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ProcessRemoteShutdownOpearation("shutdown", ShutdownCommands.ForcedShutdown);
        }

        private void ProcessRemoteShutdownOpearation(string operationName, ShutdownCommands shutdownStyle)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav == null)
                return;

            const string QUESTION_TEMPLATE = "Are you sure you want to {0} this machine: {1}\r\n" +
                                             "Operation requires administrator priviledges and can take a while.";
            var question = String.Format(QUESTION_TEMPLATE, operationName, fav.ServerName);
            string title = Program.Resources.GetString("Confirmation");
            var confirmResult = MessageBox.Show(question, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult != DialogResult.Yes)
                return;

            var options = new Tuple<ShutdownCommands, IFavorite>(shutdownStyle, fav);
            var shutdownTask = Task.Factory.StartNew(new Func<object, string>(TryPerformRemoteShutdown), options);
            shutdownTask.ContinueWith(new Action<Task<string>>(ShowRebootResult), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ShowRebootResult(Task<string> shutdownTask)
        {
            MessageBox.Show(shutdownTask.Result, "Remote action result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string TryPerformRemoteShutdown(object state)
        {
            try
            {
                var options = state as Tuple<ShutdownCommands, IFavorite>;
                if (options != null && RemoteManagement.ForceShutdown(options.Item2, options.Item1))
                    return "Terminals successfully sent the shutdown command.";

                return shutdownFailMessage;
            }
            catch (ManagementException ex)
            {
                Logging.Log.Info(ex.ToString(), ex);
                return shutdownFailMessage + "\r\nPlease check the log file for more details.";
            }
            catch (UnauthorizedAccessException)
            {
                return shutdownFailMessage + "\r\n\r\nAccess is Denied.";
            }
        }

        private void EnableRDPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo needs admin priviledges
            // todo needs exception handling
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

        private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            this.ConnectToFavorite(fav, this.consoleToolStripMenuItem.Checked, this.newWindowToolStripMenuItem.Checked);
            this.CloseMenuStrips();
        }

        private void ConnectToAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in this.favsTree.SelectedNode.Nodes)
            {
                var fav = node.Tag as IFavorite;
                this.ConnectToFavorite(fav, this.consoleAllToolStripMenuItem.Checked, this.newWindowAllToolStripMenuItem.Checked);
            }

            this.CloseMenuStrips();
        }

        private void ComputerManagementMmcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IFavorite fav = this.favsTree.SelectedFavorite;
            if (fav != null)
            {
                Process.Start("mmc.exe", "compmgmt.msc /a /computer=" + fav.ServerName);
            }

        }

        private void SystemInformationToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void SetCredentialByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string VARIABLE = "Credential";
            InputBoxResult result = this.PromptForVariableChange(VARIABLE);
            if (result.ReturnCode == DialogResult.OK)
            {
                ICredentialSet credential = Persistence.Instance.Credentials[result.Text];
                if (credential == null)
                {
                    MessageBox.Show("The credential you specified does not exist.");
                    return;
                }

                List<IFavorite> selectedFavorites = this.StartBatchUpdate();
                PersistedFavorites.ApplyCredentialsToAllFavorites(selectedFavorites, credential);
                this.FinishBatchUpdate(VARIABLE);
            }
        }

        private void SetPasswordByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string VARIABLE = "Password";
            InputBoxResult result = this.PromptForVariableChange(VARIABLE, '*');
            if (result.ReturnCode == DialogResult.OK)
            {
                List<IFavorite> selectedFavorites = this.StartBatchUpdate();
                PersistedFavorites.SetPasswordToAllFavorites(selectedFavorites, result.Text);
                this.FinishBatchUpdate(VARIABLE);
            }
        }

        private void SetDomainByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string VARIABLE = "Domain name";
            InputBoxResult result = this.PromptForVariableChange(VARIABLE);
            if (result.ReturnCode == DialogResult.OK)
            {
                List<IFavorite> selectedFavorites = this.StartBatchUpdate();
                PersistedFavorites.ApplyDomainNameToAllFavorites(selectedFavorites, result.Text);
                this.FinishBatchUpdate(VARIABLE);
            }
        }

        private void SetUsernameByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string VARIABLE = "User name";
            InputBoxResult result = this.PromptForVariableChange(VARIABLE);
            if (result.ReturnCode == DialogResult.OK)
            {
                List<IFavorite> selectedFavorites = this.StartBatchUpdate();
                PersistedFavorites.ApplyUserNameToAllFavorites(selectedFavorites, result.Text);
                this.FinishBatchUpdate(VARIABLE);
            }
        }

        private List<IFavorite> StartBatchUpdate()
        {
            this.GetMainForm().Cursor = Cursors.WaitCursor;
            return this.GetSelectedFavorites();
        }

        private List<IFavorite> GetSelectedFavorites()
        {
            var groupNode = this.favsTree.SelectedNode as GroupTreeNode;
            if (groupNode == null)
                return new List<IFavorite>();
            
            return groupNode.Favorites;
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

        private void DeleteAllFavoritesByTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String groupName = this.favsTree.SelectedNode.Text;
            string title = "Delete all Favorites by group - " + groupName;
            const string PROMPT = "This will DELETE all Favorites within this group.\r\nUse at your own risk!\r\n\r\nDo you realy want to delete them?";
            DialogResult result = MessageBox.Show(PROMPT, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                List<IFavorite> selectedFavorites = this.StartBatchUpdate();
                Persistence.Instance.Favorites.Delete(selectedFavorites);
                this.GetMainForm().Cursor = Cursors.Default;
                MessageBox.Show("Delete all Favorites by group Complete.");
            }
        }

        private void RemoveSelectedToolStripMenuItem_Click(object sender, EventArgs e)
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
                    this.favsTree.ContextMenuStrip = this.favoritesContextMenu;
                else
                    this.favsTree.ContextMenuStrip = this.groupsContextMenu;
            }
        }

        private void FavsTree_DoubleClick(object sender, EventArgs e)
        {
            this.StartConnection(favsTree);
        }

        private void FavsTree_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
        }

        private void FavsTree_DragDrop(object sender, DragEventArgs e)
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
                MainForm main = this.GetMainForm();
                main.Connect(tv.SelectedNode.Text, false, false);
            }
        }

        private void HistoryTreeView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.StartConnection(historyTreeView);
        }

        private void ConnectAsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            this.ReleaseOldContextMenu();
            this.connectAsToolStripMenuItem.DropDownItems.Add(this.userConnectToolStripMenuItem);

            IEnumerable<ICredentialSet> credentials = Persistence.Instance.Credentials;
            foreach (ICredentialSet credential in credentials)
            {
                var menuItem = new ToolStripMenuItem(credential.Name, null, new EventHandler(this.ConnectAsCred_Click));
                menuItem.Tag = credential.Id;
                this.connectAsToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }

        private void ReleaseOldContextMenu()
        {
            foreach (ToolStripMenuItem dropDownItem in this.connectAsToolStripMenuItem.DropDownItems)
            {
                dropDownItem.Click -= this.ConnectAsCred_Click;
            }
            this.connectAsToolStripMenuItem.DropDownItems.Clear();
        }

        private void ConnectAsCred_Click(object sender, EventArgs e)
        {
            IFavorite favorite = this.favsTree.SelectedFavorite;
            if (favorite != null)
            {
                var menuItem = sender as ToolStripItem;
                var credential = Persistence.Instance.Credentials[(Guid)menuItem.Tag];
                this.GetMainForm().Connect(favorite.Name, this.consoleToolStripMenuItem.Checked,
                                           this.newWindowToolStripMenuItem.Checked, credential);
            }
        }

        private void UserConnectToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void DisplayWindow_Click(object sender, EventArgs e)
        {
            this.favoritesContextMenu.Show();
        }

        private void DisplayAllWindow_Click(object sender, EventArgs e)
        {
            this.groupsContextMenu.Show();
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

        private void LoadState()
        {
            ExpandTreeView(Settings.ExpandedFavoriteNodes, this.favsTree);
            ExpandTreeView(Settings.ExpandedHistoryNodes, this.historyTreeView);
        }

        private static void ExpandTreeView(string savedNodesToExpand, TreeView treeView)
        {
            var nodesToExpand = new List<string>();
            if (!string.IsNullOrEmpty(savedNodesToExpand))
                nodesToExpand.AddRange(Regex.Split(savedNodesToExpand, "%%"));

            if (nodesToExpand.Count > 0)
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
