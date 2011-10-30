using System;
using System.Drawing;
using System.Windows.Forms;

namespace Terminals.Forms
{
    partial class MasterPasswordOptionPanel
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
            this.panel1 = new Panel();
            this.groupBox3 = new GroupBox();
            this.lblPasswordsMatch = new Label();
            this.chkPasswordProtectTerminals = new CheckBox();
            this.ClearMasterButton = new Button();
            this.lblPassword = new Label();
            this.PasswordTextbox = new TextBox();
            this.lblConfirm = new Label();
            this.ConfirmPasswordTextBox = new TextBox();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(511, 332);
            this.panel1.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblPasswordsMatch);
            this.groupBox3.Controls.Add(this.chkPasswordProtectTerminals);
            this.groupBox3.Controls.Add(this.ClearMasterButton);
            this.groupBox3.Controls.Add(this.lblPassword);
            this.groupBox3.Controls.Add(this.PasswordTextbox);
            this.groupBox3.Controls.Add(this.lblConfirm);
            this.groupBox3.Controls.Add(this.ConfirmPasswordTextBox);
            this.groupBox3.Location = new Point(6, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(500, 133);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // PasswordsMatchLabel
            // 
            this.lblPasswordsMatch.AutoSize = true;
            this.lblPasswordsMatch.ForeColor = SystemColors.ControlText;
            this.lblPasswordsMatch.Location = new Point(223, 73);
            this.lblPasswordsMatch.Name = "PasswordsMatchLabel";
            this.lblPasswordsMatch.Size = new Size(115, 13);
            this.lblPasswordsMatch.TabIndex = 6;
            this.lblPasswordsMatch.Text = "Password Match Label";
            // 
            // PasswordProtectTerminalsCheckbox
            // 
            this.chkPasswordProtectTerminals.AutoSize = true;
            this.chkPasswordProtectTerminals.Location = new Point(6, 20);
            this.chkPasswordProtectTerminals.Name = "PasswordProtectTerminalsCheckbox";
            this.chkPasswordProtectTerminals.Size = new Size(109, 17);
            this.chkPasswordProtectTerminals.TabIndex = 0;
            this.chkPasswordProtectTerminals.Text = "Password Protect";
            this.chkPasswordProtectTerminals.UseVisualStyleBackColor = true;
            this.chkPasswordProtectTerminals.CheckedChanged += new EventHandler(chkPasswordProtectTerminals_CheckedChanged);
            // 
            // ClearMasterButton
            // 
            this.ClearMasterButton.Location = new Point(79, 101);
            this.ClearMasterButton.Name = "ClearMasterButton";
            this.ClearMasterButton.Size = new Size(138, 23);
            this.ClearMasterButton.TabIndex = 6;
            this.ClearMasterButton.Text = "Clear Master Password";
            this.ClearMasterButton.UseVisualStyleBackColor = true;
            this.ClearMasterButton.Click += new EventHandler(ClearMasterButton_Click);
            // 
            // label6
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new Point(6, 45);
            this.lblPassword.Name = "label6";
            this.lblPassword.Size = new Size(56, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            // 
            // PasswordTextbox
            // 
            this.PasswordTextbox.Enabled = false;
            this.PasswordTextbox.Location = new Point(70, 42);
            this.PasswordTextbox.Name = "PasswordTextbox";
            this.PasswordTextbox.PasswordChar = '*';
            this.PasswordTextbox.Size = new Size(147, 20);
            this.PasswordTextbox.TabIndex = 1;
            this.PasswordTextbox.TextChanged += new EventHandler(PasswordTextbox_TextChanged);
            // 
            // label7
            // 
            this.lblConfirm.AutoSize = true;
            this.lblConfirm.Location = new Point(6, 73);
            this.lblConfirm.Name = "label7";
            this.lblConfirm.Size = new Size(45, 13);
            this.lblConfirm.TabIndex = 4;
            this.lblConfirm.Text = "Confirm:";
            // 
            // ConfirmPasswordTextBox
            // 
            this.ConfirmPasswordTextBox.Enabled = false;
            this.ConfirmPasswordTextBox.Location = new Point(70, 70);
            this.ConfirmPasswordTextBox.Name = "ConfirmPasswordTextBox";
            this.ConfirmPasswordTextBox.PasswordChar = '*';
            this.ConfirmPasswordTextBox.Size = new Size(147, 20);
            this.ConfirmPasswordTextBox.TabIndex = 2;
            this.ConfirmPasswordTextBox.TextChanged += new EventHandler(ConfirmPasswordTextBox_TextChanged);
            // 
            // MasterPasswordOptionpanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "MasterPasswordOptionpanel";
            this.Size = new Size(511, 332);
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private GroupBox groupBox3;
        private Label lblPasswordsMatch;
        private CheckBox chkPasswordProtectTerminals;
        private Button ClearMasterButton;
        private Label lblPassword;
        private TextBox PasswordTextbox;
        private Label lblConfirm;
        private TextBox ConfirmPasswordTextBox;
    }
}
