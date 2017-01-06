using System.Windows.Forms;

namespace Terminals.Forms
{
    partial class StartShutdownOptionPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBoxShutdown = new System.Windows.Forms.GroupBox();
            this.chkSaveConnections = new System.Windows.Forms.CheckBox();
            this.chkShowConfirmDialog = new System.Windows.Forms.CheckBox();
            this.groupBoxStartup = new System.Windows.Forms.GroupBox();
            this.chkNeverShowTerminalsCheckbox = new System.Windows.Forms.CheckBox();
            this.chkSingleInstance = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBoxShutdown.SuspendLayout();
            this.groupBoxStartup.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBoxShutdown);
            this.panel1.Controls.Add(this.groupBoxStartup);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(512, 328);
            this.panel1.TabIndex = 1;
            // 
            // groupBoxShutdown
            // 
            this.groupBoxShutdown.Controls.Add(this.chkSaveConnections);
            this.groupBoxShutdown.Controls.Add(this.chkShowConfirmDialog);
            this.groupBoxShutdown.Location = new System.Drawing.Point(6, 89);
            this.groupBoxShutdown.Name = "groupBoxShutdown";
            this.groupBoxShutdown.Size = new System.Drawing.Size(500, 80);
            this.groupBoxShutdown.TabIndex = 1;
            this.groupBoxShutdown.TabStop = false;
            this.groupBoxShutdown.Text = "Shutdown";
            // 
            // chkSaveConnections
            // 
            this.chkSaveConnections.AutoSize = true;
            this.chkSaveConnections.Checked = true;
            this.chkSaveConnections.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveConnections.Location = new System.Drawing.Point(6, 43);
            this.chkSaveConnections.Name = "chkSaveConnections";
            this.chkSaveConnections.Size = new System.Drawing.Size(199, 21);
            this.chkSaveConnections.TabIndex = 5;
            this.chkSaveConnections.Text = "Save connections on close";
            this.chkSaveConnections.UseVisualStyleBackColor = true;
            // 
            // chkShowConfirmDialog
            // 
            this.chkShowConfirmDialog.AutoSize = true;
            this.chkShowConfirmDialog.Checked = true;
            this.chkShowConfirmDialog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowConfirmDialog.Location = new System.Drawing.Point(6, 20);
            this.chkShowConfirmDialog.Name = "chkShowConfirmDialog";
            this.chkShowConfirmDialog.Size = new System.Drawing.Size(224, 21);
            this.chkShowConfirmDialog.TabIndex = 4;
            this.chkShowConfirmDialog.Text = "Show close confirmation dialog";
            this.chkShowConfirmDialog.UseVisualStyleBackColor = true;
            // 
            // groupBoxStartup
            // 
            this.groupBoxStartup.Controls.Add(this.chkNeverShowTerminalsCheckbox);
            this.groupBoxStartup.Controls.Add(this.chkSingleInstance);
            this.groupBoxStartup.Location = new System.Drawing.Point(6, 3);
            this.groupBoxStartup.Name = "groupBoxStartup";
            this.groupBoxStartup.Size = new System.Drawing.Size(500, 80);
            this.groupBoxStartup.TabIndex = 0;
            this.groupBoxStartup.TabStop = false;
            this.groupBoxStartup.Text = "Startup";
            // 
            // chkNeverShowTerminalsCheckbox
            // 
            this.chkNeverShowTerminalsCheckbox.AutoSize = true;
            this.chkNeverShowTerminalsCheckbox.Location = new System.Drawing.Point(6, 42);
            this.chkNeverShowTerminalsCheckbox.Name = "chkNeverShowTerminalsCheckbox";
            this.chkNeverShowTerminalsCheckbox.Size = new System.Drawing.Size(333, 21);
            this.chkNeverShowTerminalsCheckbox.TabIndex = 4;
            this.chkNeverShowTerminalsCheckbox.Text = "Do not keep me up-to-date on Terminals project";
            this.chkNeverShowTerminalsCheckbox.UseVisualStyleBackColor = true;
            // 
            // chkSingleInstance
            // 
            this.chkSingleInstance.AutoSize = true;
            this.chkSingleInstance.Checked = true;
            this.chkSingleInstance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSingleInstance.Location = new System.Drawing.Point(6, 19);
            this.chkSingleInstance.Name = "chkSingleInstance";
            this.chkSingleInstance.Size = new System.Drawing.Size(382, 21);
            this.chkSingleInstance.TabIndex = 3;
            this.chkSingleInstance.Text = "Allow a single instance of the application (needs restart)";
            this.chkSingleInstance.UseVisualStyleBackColor = true;
            // 
            // StartShutdownOptionPanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "StartShutdownOptionPanel";
            this.Size = new System.Drawing.Size(512, 328);
            this.panel1.ResumeLayout(false);
            this.groupBoxShutdown.ResumeLayout(false);
            this.groupBoxShutdown.PerformLayout();
            this.groupBoxStartup.ResumeLayout(false);
            this.groupBoxStartup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private GroupBox groupBoxShutdown;
        private CheckBox chkSaveConnections;
        private CheckBox chkShowConfirmDialog;
        private GroupBox groupBoxStartup;
        private CheckBox chkNeverShowTerminalsCheckbox;
        private CheckBox chkSingleInstance;
    }
}
