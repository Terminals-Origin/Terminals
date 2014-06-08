using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class FavoritePropertiesControl : UserControl
    {
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

        public FavoritePropertiesControl()
        {
            this.InitializeComponent();

            // todo missing ras control this.generalPanel1.AssignRasControl(this.rasControl);
            // todo missing rdp local resources this.generalPanel1.AssignRdpLocalResources(this.rdpLocalResources);
        }

        internal void LoadContent()
        {
            this.DockAllPanels();
            this.HideAllPanels();

            this.protocolOptionsPanel1.Load();
            string[] availablePlugins = this.protocolOptionsPanel1.Available;
            this.generalPanel1.AssingAvailablePlugins(availablePlugins);
            this.generalPanel1.ProtocolChanged += GenearalPanel1ProtocolChanged;
            var firstPlugin = availablePlugins[0];
            this.GenearalPanel1ProtocolChanged(firstPlugin);
        }

        private void DockAllPanels()
        {
            this.generalPanel1.Dock = DockStyle.Fill;
            this.groupsPanel1.Dock = DockStyle.Fill;
            this.executePanel1.Dock = DockStyle.Fill;
        }

        private void GenearalPanel1ProtocolChanged(string newProtocol)
        {
            this.protocolOptionsPanel1.ReloadControls(newProtocol);
            this.ReloadProtocolTreeNodes();
        }

        private void ReloadProtocolTreeNodes()
        {
            TreeNodeCollection optionsNodes = this.ProtocolOptionsNode.Nodes;
            optionsNodes.Clear();

            foreach (Control pluginUserControl in this.protocolOptionsPanel1.Controls)
            {
                optionsNodes.Add(pluginUserControl.Name);
            }
        }

        private void HideAllPanels()
        {
            this.generalPanel1.Hide();
            this.groupsPanel1.Hide();
            this.executePanel1.Hide();
            this.protocolOptionsPanel1.Hide();
        }

        private void TreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            this.HideAllPanels();
            TreeNode node = e.Node;
            this.titleLabel.Text = this.ResolveTitle(node);

            if (node.Level > 0)
                this.FocusProtocolOptionControl(node);
            else
                this.ShowSelectedPanel(node);
        }

        private string ResolveTitle(TreeNode node)
        {
            if (node.Level > 0)
                return string.Format("{0} - {1}", this.ProtocolOptionsNode.Text, node.Text);

            return node.Text;
        }

        private void FocusProtocolOptionControl(TreeNode node)
        {
            this.protocolOptionsPanel1.Show();
            this.protocolOptionsPanel1.FocuControl(node.Text);
        }

        private void ShowSelectedPanel(TreeNode newNode)
        {
            switch (newNode.Name)
            {
                case "generalNode":
                    this.generalPanel1.Show();
                    break;
               case "groupsNode":
                    this.groupsPanel1.Show();
                    break;
                case "executeNode":
                    this.executePanel1.Show();
                    break;
                case "protocolOptionsNode":
                    this.protocolOptionsPanel1.Show();
                    this.protocolOptionsPanel1.FocuControl();
                    break;
            }
        }

        internal void InitializeValidations(NewTerminalFormValidator validator)
        {
            // todo this.rdpExtendedSettings.AssignValidatingEvents(validator);
            this.executePanel1.RegisterValiationControls(validator);
        }

        internal void AssignPersistence(IPersistence persistence)
        {
            this.generalPanel1.AssignPersistence(persistence);
            this.groupsPanel1.AssignPersistence(persistence);
        }

        internal void AssignValidationComponents(NewTerminalFormValidator validator)
        {
            this.generalPanel1.AssignValidationComponents(validator);
            this.groupsPanel1.AssignValidator(validator);
        }

        internal void SetErrorProviderIconsAlignment(ErrorProvider errorProvider)
        {
            this.generalPanel1.SetErrorProviderIconsAlignment(errorProvider);
        }

        internal void BindGroups()
        {
            this.groupsPanel1.BindGroups();
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

        public void ResetServerNameControls(string name)
        {
            this.generalPanel1.ResetServerNameControls(name);
        }

        public void FillCredentialsCombobox(Guid credentialGuid)
        {
            this.generalPanel1.FillCredentialsCombobox(credentialGuid);
        }

        public void FillServerName(string serverName)
        {
            this.generalPanel1.FillServerName(serverName);
        }

        public void AssingSelectedGroup(IGroup group)
        {
            this.groupsPanel1.AssingSelectedGroup(group);
        }

        public List<IGroup> GetNewlySelectedGroups()
        {
            return this.groupsPanel1.GetNewlySelectedGroups();
        }

        public Uri GetFullUrlFromHttpTextBox()
        {
            return this.generalPanel1.GetFullUrlFromHttpTextBox();
        }
    }
}
