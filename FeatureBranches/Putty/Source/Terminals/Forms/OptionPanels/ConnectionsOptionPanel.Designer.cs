using System;
using System.Windows.Forms;

namespace Terminals.Forms
{
    partial class ConnectionsOptionPanel
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
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.EvaluatedDesktopShareLabel = new System.Windows.Forms.Label();
            this.PortscanTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.lblDefaultDesktopShare = new System.Windows.Forms.Label();
            this.txtDefaultDesktopShare = new System.Windows.Forms.TextBox();
            this.lblEvaluatedDesktopShare = new System.Windows.Forms.Label();
            this.lblPortScannerTimeout = new System.Windows.Forms.Label();
            this.groupBoxConnections = new System.Windows.Forms.GroupBox();
            this.tryReconnectCheckBox = new System.Windows.Forms.CheckBox();
            this.restoreWindowCheckbox = new System.Windows.Forms.CheckBox();
            this.validateServerNamesCheckbox = new System.Windows.Forms.CheckBox();
            this.warnDisconnectCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBoxConnections.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox12);
            this.panel1.Controls.Add(this.groupBoxConnections);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(514, 332);
            this.panel1.TabIndex = 25;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.EvaluatedDesktopShareLabel);
            this.groupBox12.Controls.Add(this.PortscanTimeoutTextBox);
            this.groupBox12.Controls.Add(this.lblDefaultDesktopShare);
            this.groupBox12.Controls.Add(this.txtDefaultDesktopShare);
            this.groupBox12.Controls.Add(this.lblEvaluatedDesktopShare);
            this.groupBox12.Controls.Add(this.lblPortScannerTimeout);
            this.groupBox12.Location = new System.Drawing.Point(6, 143);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(500, 166);
            this.groupBox12.TabIndex = 20;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "RDP Desktop Share";
            // 
            // EvaluatedDesktopShareLabel
            // 
            this.EvaluatedDesktopShareLabel.AutoSize = true;
            this.EvaluatedDesktopShareLabel.ForeColor = System.Drawing.Color.Blue;
            this.EvaluatedDesktopShareLabel.Location = new System.Drawing.Point(3, 90);
            this.EvaluatedDesktopShareLabel.Name = "EvaluatedDesktopShareLabel";
            this.EvaluatedDesktopShareLabel.Size = new System.Drawing.Size(193, 17);
            this.EvaluatedDesktopShareLabel.TabIndex = 23;
            this.EvaluatedDesktopShareLabel.Text = "Hidden Label Evaluate Share";
            // 
            // PortscanTimeoutTextBox
            // 
            this.PortscanTimeoutTextBox.Location = new System.Drawing.Point(259, 121);
            this.PortscanTimeoutTextBox.Name = "PortscanTimeoutTextBox";
            this.PortscanTimeoutTextBox.Size = new System.Drawing.Size(58, 22);
            this.PortscanTimeoutTextBox.TabIndex = 22;
            // 
            // lblDefaultDesktopShare
            // 
            this.lblDefaultDesktopShare.AutoSize = true;
            this.lblDefaultDesktopShare.Location = new System.Drawing.Point(3, 25);
            this.lblDefaultDesktopShare.Name = "lblDefaultDesktopShare";
            this.lblDefaultDesktopShare.Size = new System.Drawing.Size(372, 17);
            this.lblDefaultDesktopShare.TabIndex = 17;
            this.lblDefaultDesktopShare.Text = "Default Desktop Share (Use %SERVER% and %USER%):";
            // 
            // txtDefaultDesktopShare
            // 
            this.txtDefaultDesktopShare.Location = new System.Drawing.Point(6, 46);
            this.txtDefaultDesktopShare.Name = "txtDefaultDesktopShare";
            this.txtDefaultDesktopShare.Size = new System.Drawing.Size(360, 22);
            this.txtDefaultDesktopShare.TabIndex = 18;
            this.txtDefaultDesktopShare.TextChanged += new System.EventHandler(this.txtDefaultDesktopShare_TextChanged);
            // 
            // lblEvaluatedDesktopShare
            // 
            this.lblEvaluatedDesktopShare.AutoSize = true;
            this.lblEvaluatedDesktopShare.Location = new System.Drawing.Point(3, 73);
            this.lblEvaluatedDesktopShare.Name = "lblEvaluatedDesktopShare";
            this.lblEvaluatedDesktopShare.Size = new System.Drawing.Size(419, 17);
            this.lblEvaluatedDesktopShare.TabIndex = 19;
            this.lblEvaluatedDesktopShare.Text = "Evaluated Desktop Share (according to selected connection tab):";
            // 
            // lblPortScannerTimeout
            // 
            this.lblPortScannerTimeout.AutoSize = true;
            this.lblPortScannerTimeout.Location = new System.Drawing.Point(3, 124);
            this.lblPortScannerTimeout.Name = "lblPortScannerTimeout";
            this.lblPortScannerTimeout.Size = new System.Drawing.Size(250, 17);
            this.lblPortScannerTimeout.TabIndex = 20;
            this.lblPortScannerTimeout.Text = "Port Scanner Timeout (0-60 seconds):";
            // 
            // groupBoxConnections
            // 
            this.groupBoxConnections.Controls.Add(this.tryReconnectCheckBox);
            this.groupBoxConnections.Controls.Add(this.restoreWindowCheckbox);
            this.groupBoxConnections.Controls.Add(this.validateServerNamesCheckbox);
            this.groupBoxConnections.Controls.Add(this.warnDisconnectCheckBox);
            this.groupBoxConnections.Location = new System.Drawing.Point(6, 3);
            this.groupBoxConnections.Name = "groupBoxConnections";
            this.groupBoxConnections.Size = new System.Drawing.Size(500, 134);
            this.groupBoxConnections.TabIndex = 4;
            this.groupBoxConnections.TabStop = false;
            // 
            // tryReconnectCheckBox
            // 
            this.tryReconnectCheckBox.AutoSize = true;
            this.tryReconnectCheckBox.Location = new System.Drawing.Point(6, 74);
            this.tryReconnectCheckBox.Name = "tryReconnectCheckBox";
            this.tryReconnectCheckBox.Size = new System.Drawing.Size(515, 21);
            this.tryReconnectCheckBox.TabIndex = 21;
            this.tryReconnectCheckBox.Text = "Ask to reconnect when connection is lost due Shutdown or reboot (RDP only)";
            this.tryReconnectCheckBox.UseVisualStyleBackColor = true;
            // 
            // restoreWindowCheckbox
            // 
            this.restoreWindowCheckbox.AutoSize = true;
            this.restoreWindowCheckbox.Location = new System.Drawing.Point(6, 101);
            this.restoreWindowCheckbox.Name = "restoreWindowCheckbox";
            this.restoreWindowCheckbox.Size = new System.Drawing.Size(464, 21);
            this.restoreWindowCheckbox.TabIndex = 20;
            this.restoreWindowCheckbox.Text = "Automatically restore main window when the last connection is closed";
            this.restoreWindowCheckbox.UseVisualStyleBackColor = true;
            // 
            // validateServerNamesCheckbox
            // 
            this.validateServerNamesCheckbox.AutoSize = true;
            this.validateServerNamesCheckbox.Checked = true;
            this.validateServerNamesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.validateServerNamesCheckbox.Location = new System.Drawing.Point(6, 20);
            this.validateServerNamesCheckbox.Name = "validateServerNamesCheckbox";
            this.validateServerNamesCheckbox.Size = new System.Drawing.Size(175, 21);
            this.validateServerNamesCheckbox.TabIndex = 18;
            this.validateServerNamesCheckbox.Text = "Validate Server Names";
            this.validateServerNamesCheckbox.UseVisualStyleBackColor = true;
            // 
            // warnDisconnectCheckBox
            // 
            this.warnDisconnectCheckBox.AutoSize = true;
            this.warnDisconnectCheckBox.Checked = true;
            this.warnDisconnectCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.warnDisconnectCheckBox.Location = new System.Drawing.Point(6, 47);
            this.warnDisconnectCheckBox.Name = "warnDisconnectCheckBox";
            this.warnDisconnectCheckBox.Size = new System.Drawing.Size(356, 21);
            this.warnDisconnectCheckBox.TabIndex = 19;
            this.warnDisconnectCheckBox.Text = "Show confirm dialog on close or warn on disconnect";
            this.warnDisconnectCheckBox.UseVisualStyleBackColor = true;
            // 
            // ConnectionsOptionPanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "ConnectionsOptionPanel";
            this.Size = new System.Drawing.Size(514, 332);
            this.panel1.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBoxConnections.ResumeLayout(false);
            this.groupBoxConnections.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private GroupBox groupBox12;
        private Label EvaluatedDesktopShareLabel;
        private TextBox PortscanTimeoutTextBox;
        private Label lblDefaultDesktopShare;
        private TextBox txtDefaultDesktopShare;
        private Label lblEvaluatedDesktopShare;
        private Label lblPortScannerTimeout;
        private GroupBox groupBoxConnections;
        private CheckBox validateServerNamesCheckbox;
        private CheckBox warnDisconnectCheckBox;
        private CheckBox restoreWindowCheckbox;
        private CheckBox tryReconnectCheckBox;
    }
}
