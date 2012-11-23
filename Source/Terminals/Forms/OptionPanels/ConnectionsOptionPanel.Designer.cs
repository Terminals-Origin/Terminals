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
            this.groupBoxConnections = new System.Windows.Forms.GroupBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.EvaluatedDesktopShareLabel = new System.Windows.Forms.Label();
            this.PortscanTimeoutTextBox = new System.Windows.Forms.TextBox();
            this.lblDefaultDesktopShare = new System.Windows.Forms.Label();
            this.txtDefaultDesktopShare = new System.Windows.Forms.TextBox();
            this.lblEvaluatedDesktopShare = new System.Windows.Forms.Label();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.lblPortScannerTimeout = new System.Windows.Forms.Label();
            this.restoreWindowCheckbox = new System.Windows.Forms.CheckBox();
            this.validateServerNamesCheckbox = new System.Windows.Forms.CheckBox();
            this.warnDisconnectCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBoxConnections.SuspendLayout();
            this.groupBox12.SuspendLayout();
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
            // groupBoxConnections
            // 
            this.groupBoxConnections.Controls.Add(this.restoreWindowCheckbox);
            this.groupBoxConnections.Controls.Add(this.validateServerNamesCheckbox);
            this.groupBoxConnections.Controls.Add(this.warnDisconnectCheckBox);
            this.groupBoxConnections.Location = new System.Drawing.Point(6, 3);
            this.groupBoxConnections.Name = "groupBoxConnections";
            this.groupBoxConnections.Size = new System.Drawing.Size(500, 93);
            this.groupBoxConnections.TabIndex = 4;
            this.groupBoxConnections.TabStop = false;
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.EvaluatedDesktopShareLabel);
            this.groupBox12.Controls.Add(this.PortscanTimeoutTextBox);
            this.groupBox12.Controls.Add(this.lblDefaultDesktopShare);
            this.groupBox12.Controls.Add(this.txtDefaultDesktopShare);
            this.groupBox12.Controls.Add(this.lblEvaluatedDesktopShare);
            this.groupBox12.Controls.Add(this.lblSeconds);
            this.groupBox12.Controls.Add(this.lblPortScannerTimeout);
            this.groupBox12.Location = new System.Drawing.Point(6, 102);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(500, 151);
            this.groupBox12.TabIndex = 20;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "RDP Desktop Share";
            // 
            // EvaluatedDesktopShareLabel
            // 
            this.EvaluatedDesktopShareLabel.AutoSize = true;
            this.EvaluatedDesktopShareLabel.ForeColor = System.Drawing.Color.Blue;
            this.EvaluatedDesktopShareLabel.Location = new System.Drawing.Point(350, 17);
            this.EvaluatedDesktopShareLabel.Name = "EvaluatedDesktopShareLabel";
            this.EvaluatedDesktopShareLabel.Size = new System.Drawing.Size(146, 13);
            this.EvaluatedDesktopShareLabel.TabIndex = 23;
            this.EvaluatedDesktopShareLabel.Text = "Hidden Label Evaluate Share";
            // 
            // PortscanTimeoutTextBox
            // 
            this.PortscanTimeoutTextBox.Location = new System.Drawing.Point(123, 84);
            this.PortscanTimeoutTextBox.Name = "PortscanTimeoutTextBox";
            this.PortscanTimeoutTextBox.Size = new System.Drawing.Size(58, 20);
            this.PortscanTimeoutTextBox.TabIndex = 22;
            // 
            // lblDefaultDesktopShare
            // 
            this.lblDefaultDesktopShare.AutoSize = true;
            this.lblDefaultDesktopShare.Location = new System.Drawing.Point(3, 17);
            this.lblDefaultDesktopShare.Name = "lblDefaultDesktopShare";
            this.lblDefaultDesktopShare.Size = new System.Drawing.Size(279, 13);
            this.lblDefaultDesktopShare.TabIndex = 17;
            this.lblDefaultDesktopShare.Text = "Default Desktop Share (Use %SERVER% and %USER%):";
            // 
            // txtDefaultDesktopShare
            // 
            this.txtDefaultDesktopShare.Location = new System.Drawing.Point(6, 33);
            this.txtDefaultDesktopShare.Name = "txtDefaultDesktopShare";
            this.txtDefaultDesktopShare.Size = new System.Drawing.Size(360, 20);
            this.txtDefaultDesktopShare.TabIndex = 18;
            this.txtDefaultDesktopShare.TextChanged += new System.EventHandler(this.txtDefaultDesktopShare_TextChanged);
            // 
            // lblEvaluatedDesktopShare
            // 
            this.lblEvaluatedDesktopShare.AutoSize = true;
            this.lblEvaluatedDesktopShare.Location = new System.Drawing.Point(3, 68);
            this.lblEvaluatedDesktopShare.Name = "lblEvaluatedDesktopShare";
            this.lblEvaluatedDesktopShare.Size = new System.Drawing.Size(317, 13);
            this.lblEvaluatedDesktopShare.TabIndex = 19;
            this.lblEvaluatedDesktopShare.Text = "Evaluated Desktop Share (according to selected connection tab):";
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(187, 87);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(49, 13);
            this.lblSeconds.TabIndex = 21;
            this.lblSeconds.Text = "Seconds";
            // 
            // lblPortScannerTimeout
            // 
            this.lblPortScannerTimeout.AutoSize = true;
            this.lblPortScannerTimeout.Location = new System.Drawing.Point(3, 87);
            this.lblPortScannerTimeout.Name = "lblPortScannerTimeout";
            this.lblPortScannerTimeout.Size = new System.Drawing.Size(113, 13);
            this.lblPortScannerTimeout.TabIndex = 20;
            this.lblPortScannerTimeout.Text = "Port Scanner Timeout:";
            // 
            // restoreWindowCheckbox
            // 
            this.restoreWindowCheckbox.AutoSize = true;
            this.restoreWindowCheckbox.Location = new System.Drawing.Point(6, 66);
            this.restoreWindowCheckbox.Name = "restoreWindowCheckbox";
            this.restoreWindowCheckbox.Size = new System.Drawing.Size(353, 17);
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
            this.validateServerNamesCheckbox.Size = new System.Drawing.Size(134, 17);
            this.validateServerNamesCheckbox.TabIndex = 18;
            this.validateServerNamesCheckbox.Text = "Validate Server Names";
            this.validateServerNamesCheckbox.UseVisualStyleBackColor = true;
            // 
            // warnDisconnectCheckBox
            // 
            this.warnDisconnectCheckBox.AutoSize = true;
            this.warnDisconnectCheckBox.Checked = true;
            this.warnDisconnectCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.warnDisconnectCheckBox.Location = new System.Drawing.Point(6, 43);
            this.warnDisconnectCheckBox.Name = "warnDisconnectCheckBox";
            this.warnDisconnectCheckBox.Size = new System.Drawing.Size(272, 17);
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
            this.groupBoxConnections.ResumeLayout(false);
            this.groupBoxConnections.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
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
        private Label lblSeconds;
        private Label lblPortScannerTimeout;
        private GroupBox groupBoxConnections;
        private CheckBox validateServerNamesCheckbox;
        private CheckBox warnDisconnectCheckBox;
        private CheckBox restoreWindowCheckbox;
    }
}
