using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Common.Connections;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Data.Credentials;
using Terminals.Forms.Controls;

namespace Terminals.Forms.EditFavorite
{
    internal partial class FavoritePropertiesControl : UserControl
    {
        private const string GENERAL_NODE = "generalNode";
        private const string GROUPS_NODE = "groupsNode";
        private const string EXECUTE_NODE = "executeNode";
        private const string NOTES_NODE = "notesNode";

        private readonly ConnectionManager connectionManager = ConnectionManager.Instance;

        public event EventHandler SetOkButtonRequested
        {
            add { this.generalPanel1.SetOkButtonRequested += value; }
            remove { this.generalPanel1.SetOkButtonRequested -= value; }
        }

        private TreeNode ProtocolOptionsNode
        {
            get
            {
                return this.treeView.Nodes["protocolOptionsNode"];
            }
        }

        public string ProtocolText { get { return this.generalPanel1.ProtocolText; } }

        public string ServerNameText { get { return this.generalPanel1.ServerNameText; } }

        public string PortText { get { return this.generalPanel1.PortText; } }

        public bool UrlVisible { get { return this.generalPanel1.UrlVisible; } }

        public bool ShowOnToolbar { get { return this.generalPanel1.ShowOnToolbar; } }

        public FavoritePropertiesControl()
        {
            this.InitializeComponent();

            var iconsBuilder = new ProtocolImageListBuilder(FavoriteIcons.GetProtocolIcons);
            iconsBuilder.Build(this.treeIcons);
            this.generalPanel1.AssignRasControl(this.rasControl1);
        }

        internal void LoadContent()
        {
            this.DockAllPanels();
            this.HideAllPanels();

            string[] availablePlugins = connectionManager.GetAvailableProtocols();
            this.generalPanel1.AssingAvailablePlugins(availablePlugins);
            this.generalPanel1.ProtocolChanged += GenearalPanel1ProtocolChanged;
            string firstPlugin = KnownConnectionConstants.RDP;
            this.GenearalPanel1ProtocolChanged(firstPlugin);
            this.groupsPanel1.BindGroups();
            this.generalPanel1.ServerNameChanged += this.protocolOptionsPanel1.OnServerNameChanged;
            this.treeView.SelectedNode = this.treeView.Nodes[0];
        }

        private void DockAllPanels()
        {
            this.generalPanel1.Dock = DockStyle.Fill;
            this.groupsPanel1.Dock = DockStyle.Fill;
            this.executePanel1.Dock = DockStyle.Fill;
            this.rasControl1.Dock = DockStyle.Fill;
            this.protocolOptionsPanel1.Dock = DockStyle.Fill;
            this.notesControl1.Dock = DockStyle.Fill;
        }

        private void GenearalPanel1ProtocolChanged(string newProtocol)
        {
            this.ProtocolOptionsNode.Text = string.Format("{0} Options", newProtocol);
            Control[] newControls = this.connectionManager.CreateControls(newProtocol);
            this.protocolOptionsPanel1.ReloadControls(newControls);
            this.UpdateProtocolOptionsNodeIcons(newProtocol);
            this.ReloadProtocolTreeNodes();
        }

        private void UpdateProtocolOptionsNodeIcons(string newProtocol)
        {
            string imageKey = FavoriteIcons.GetTreeviewImageListKey(newProtocol);
            UpdateNodeIcon(this.ProtocolOptionsNode, imageKey);
        }

        private void ReloadProtocolTreeNodes()
        {
            TreeNodeCollection optionsNodes = this.ProtocolOptionsNode.Nodes;
            optionsNodes.Clear();

            if (this.protocolOptionsPanel1.Controls.Count <= 1)
                return;

            this.CreateChildProtocolNodes(optionsNodes);
        }

        private void CreateChildProtocolNodes(TreeNodeCollection optionsNodes)
        {
            foreach (Control pluginUserControl in this.protocolOptionsPanel1.Controls)
            {
                TreeNode newNode = this.CreateChildNode(pluginUserControl);
                optionsNodes.Add(newNode);
            }
        }

        private TreeNode CreateChildNode(Control pluginUserControl)
        {
            var newNode = new TreeNode(pluginUserControl.Name);
            UpdateNodeIcon(newNode, this.ProtocolOptionsNode.ImageKey);
            return newNode;
        }

        private static void UpdateNodeIcon(TreeNode newNode, string protocolImageKey)
        {
            newNode.ImageKey = protocolImageKey;
            newNode.SelectedImageKey = protocolImageKey;
        }

        private void HideAllPanels()
        {
            this.generalPanel1.Hide();
            this.groupsPanel1.Hide();
            this.executePanel1.Hide();
            this.rasControl1.Hide();
            this.notesControl1.Hide();
            this.protocolOptionsPanel1.Hide();
        }

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            this.HideAllPanels();
            PanelSwitch protocolSwitch = this.ResolveSwitch(e.Node);
            this.titleLabel.Text = protocolSwitch.Title;
            protocolSwitch.ShowPanel();
        }

        private string ResolveProtocolPanelTitle(TreeNode node)
        {
            Control childControl = this.protocolOptionsPanel1.ResolveChildByNameOrFirst(node.Text);
            if (childControl == null)
                return this.ProtocolOptionsNode.Text;

            return string.Format("{0} - {1}", this.ProtocolOptionsNode.Text, childControl.Name);
        }

        private PanelSwitch ResolveSwitch(TreeNode newNode)
        {
            switch (newNode.Name)
            {
               case GENERAL_NODE:
                    return new PanelSwitch(newNode.Text, this.generalPanel1.Show);
               case GROUPS_NODE:
                    return new PanelSwitch(newNode.Text, this.groupsPanel1.Show);
               case EXECUTE_NODE:
                    return new PanelSwitch(newNode.Text, this.executePanel1.Show);
               case NOTES_NODE:
                    return new PanelSwitch(newNode.Text, this.notesControl1.Show);
                default:
                    string title = this.ResolveProtocolPanelTitle(newNode);
                    Action showAction = () => this.FocusProtocolOptionsChild(newNode);
                    return new PanelSwitch(title, showAction);
            }
        }

        private void FocusProtocolOptionsChild(TreeNode newNode)
        {
            this.protocolOptionsPanel1.Show();
            this.protocolOptionsPanel1.FocusControl(newNode.Text);
        }

        internal void RegisterValidations(NewTerminalFormValidator validator)
        {
            this.generalPanel1.RegisterValidations(validator);
            this.groupsPanel1.RegisterValidations(validator);
            this.executePanel1.RegisterValidations(validator);
            this.protocolOptionsPanel1.RegisterValidations(validator);
            this.notesControl1.RegisterValidations(validator);
        }

        internal void AssignPersistence(IPersistence persistence)
        {
            this.generalPanel1.AssignPersistence(persistence);
            this.groupsPanel1.AssignPersistence(persistence);
            this.protocolOptionsPanel1.CredentialsFactory = new GuardedCredentialFactory(persistence.Security);
        }

        internal void SetErrorProviderIconsAlignment(ErrorProvider errorProvider)
        {
            this.generalPanel1.SetErrorProviderIconsAlignment(errorProvider);
            this.notesControl1.SettErrorProviderIconsAlignment(errorProvider);
        }

        internal void FocusServers()
        {
            this.generalPanel1.FocusServers();
        }

        internal void SaveMRUs()
        {
            this.generalPanel1.SaveMRUs();
        }

        internal void LoadMRUs()
        {
            this.generalPanel1.LoadMRUs();
            this.groupsPanel1.LoadMRUs();
        }

        internal void ResetServerNameControls(string name)
        {
            this.generalPanel1.ResetServerNameControls(name);
        }

        internal void FillCredentialsCombobox(Guid credentialGuid)
        {
            this.generalPanel1.FillCredentialsCombobox(credentialGuid);
        }

        internal void FillServerName(string serverName)
        {
            this.generalPanel1.FillServerName(serverName);
        }

        internal void AssingSelectedGroup(IGroup group)
        {
            this.groupsPanel1.AssingSelectedGroup(group);
        }

        internal List<IGroup> GetNewlySelectedGroups()
        {
            return this.groupsPanel1.GetNewlySelectedGroups();
        }

        internal Uri GetFullUrlFromHttpTextBox()
        {
            return this.generalPanel1.GetFullUrlFromHttpTextBox();
        }

        internal void LoadFrom(IFavorite favorite)
        {
            this.generalPanel1.LoadFrom(favorite);
            this.executePanel1.LoadFrom(favorite);
            this.groupsPanel1.LoadFrom(favorite);
            this.notesControl1.LoadFrom(favorite);
            this.protocolOptionsPanel1.LoadFrom(favorite);
        }

        internal void SaveTo(IFavorite favorite)
        {
            this.generalPanel1.SaveTo(favorite);
            this.executePanel1.SaveTo(favorite);
            this.protocolOptionsPanel1.SaveTo(favorite);
            this.notesControl1.SaveTo(favorite);
            // save of groups is done in the form
        }
    }
}
