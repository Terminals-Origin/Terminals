using System;
using System.Drawing;
using System.Windows.Forms;

namespace Terminals.Connections
{
    internal class VmrcMenuVisitor : IToolbarExtender
    {
        internal const string VMRCADMINSWITCHBUTTON = "VMRCAdminSwitchButton";
        internal const string VMRCVIEWONLYBUTTON = "VMRCViewOnlyButton";

        private readonly ICurrenctConnectionProvider connectionProvider;
        private ToolStripButton viewOnlyButton;
        private ToolStripButton adminSwitchButton;
        
        public VmrcMenuVisitor(ICurrenctConnectionProvider connectionProvider)
        {
            this.connectionProvider = connectionProvider;
        }

        public void Visit(ToolStrip standardToolbar)
        {
            this.EnusereMenuCreated(standardToolbar);

            bool commandsAvailable = this.connectionProvider.CurrentConnection is VMRCConnection;
            adminSwitchButton.Visible = commandsAvailable;
            viewOnlyButton.Visible = commandsAvailable;
        }

        private void EnusereMenuCreated(ToolStrip standardToolbar)
        {
            if (standardToolbar.Items[VMRCADMINSWITCHBUTTON] == null)
                this.CreateAdminSwitchButton(standardToolbar);

            if (standardToolbar.Items[VMRCVIEWONLYBUTTON] == null)
                this.CreateViewOnlyButton(standardToolbar);
        }

        private void CreateAdminSwitchButton(ToolStrip standardToolbar)
        {
            this.adminSwitchButton = new ToolStripButton();
            this.adminSwitchButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.adminSwitchButton.Image = Properties.Resources.server_administrator_icon;
            this.adminSwitchButton.ImageTransparentColor = Color.Magenta;
            this.adminSwitchButton.Name = VMRCADMINSWITCHBUTTON;
            this.adminSwitchButton.Size = new Size(23, 22);
            this.adminSwitchButton.Text = "VMRC: Switch to Administrator View";
            this.adminSwitchButton.Click += new EventHandler(this.AdminSwitchButton_Click);
            standardToolbar.Items.Add(this.adminSwitchButton);
        }

        private void CreateViewOnlyButton(ToolStrip standardToolbar)
        {
            this.viewOnlyButton = new ToolStripButton();
            this.viewOnlyButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.viewOnlyButton.Image = Properties.Resources.polarized_glasses;
            this.viewOnlyButton.ImageTransparentColor = Color.White;
            this.viewOnlyButton.Name = VMRCVIEWONLYBUTTON;
            this.viewOnlyButton.Size = new Size(23, 22);
            this.viewOnlyButton.Text = "VMRC: View Only Mode";
            this.viewOnlyButton.Click += new EventHandler(this.ViewOnlyButton_Click);
            standardToolbar.Items.Add(this.viewOnlyButton);
        }

        private void AdminSwitchButton_Click(object sender, EventArgs e)
        {
            var vmrc = this.connectionProvider.CurrentConnection as VMRCConnection;
            if (vmrc != null)
            {
                vmrc.AdminDisplay();
            }
        }

        private void ViewOnlyButton_Click(object sender, EventArgs e)
        {
            var vmrc = this.connectionProvider.CurrentConnection as VMRCConnection;
            if (vmrc != null)
            {
                vmrc.ViewOnlyMode = !vmrc.ViewOnlyMode;
            }
        }
    }
}