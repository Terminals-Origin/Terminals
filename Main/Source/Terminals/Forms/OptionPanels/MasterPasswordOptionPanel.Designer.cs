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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterPasswordOptionPanel));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblPasswordsMatch = new System.Windows.Forms.Label();
            this.chkPasswordProtectTerminals = new System.Windows.Forms.CheckBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.PasswordTextbox = new System.Windows.Forms.TextBox();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.ConfirmPasswordTextBox = new System.Windows.Forms.TextBox();
            this.revealPwdButton = new System.Windows.Forms.Button();
            this.hideRevealButtonImages = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(511, 332);
            this.panel1.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.revealPwdButton);
            this.groupBox3.Controls.Add(this.lblPasswordsMatch);
            this.groupBox3.Controls.Add(this.chkPasswordProtectTerminals);
            this.groupBox3.Controls.Add(this.lblPassword);
            this.groupBox3.Controls.Add(this.PasswordTextbox);
            this.groupBox3.Controls.Add(this.lblConfirm);
            this.groupBox3.Controls.Add(this.ConfirmPasswordTextBox);
            this.groupBox3.Location = new System.Drawing.Point(6, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(500, 112);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            // 
            // lblPasswordsMatch
            // 
            this.lblPasswordsMatch.AutoSize = true;
            this.lblPasswordsMatch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPasswordsMatch.Location = new System.Drawing.Point(251, 81);
            this.lblPasswordsMatch.Name = "lblPasswordsMatch";
            this.lblPasswordsMatch.Size = new System.Drawing.Size(115, 13);
            this.lblPasswordsMatch.TabIndex = 6;
            this.lblPasswordsMatch.Text = "Password Match Label";
            // 
            // chkPasswordProtectTerminals
            // 
            this.chkPasswordProtectTerminals.AutoSize = true;
            this.chkPasswordProtectTerminals.Location = new System.Drawing.Point(13, 20);
            this.chkPasswordProtectTerminals.Name = "chkPasswordProtectTerminals";
            this.chkPasswordProtectTerminals.Size = new System.Drawing.Size(109, 17);
            this.chkPasswordProtectTerminals.TabIndex = 0;
            this.chkPasswordProtectTerminals.Text = "Password Protect";
            this.chkPasswordProtectTerminals.UseVisualStyleBackColor = true;
            this.chkPasswordProtectTerminals.CheckedChanged += new System.EventHandler(this.ChkPasswordProtectTerminals_CheckedChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(13, 53);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            // 
            // PasswordTextbox
            // 
            this.PasswordTextbox.Enabled = false;
            this.PasswordTextbox.Location = new System.Drawing.Point(98, 50);
            this.PasswordTextbox.Name = "PasswordTextbox";
            this.PasswordTextbox.PasswordChar = '*';
            this.PasswordTextbox.Size = new System.Drawing.Size(147, 20);
            this.PasswordTextbox.TabIndex = 1;
            this.PasswordTextbox.TextChanged += new System.EventHandler(this.PasswordTextbox_TextChanged);
            // 
            // lblConfirm
            // 
            this.lblConfirm.AutoSize = true;
            this.lblConfirm.Location = new System.Drawing.Point(13, 81);
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.Size = new System.Drawing.Size(45, 13);
            this.lblConfirm.TabIndex = 4;
            this.lblConfirm.Text = "Confirm:";
            // 
            // ConfirmPasswordTextBox
            // 
            this.ConfirmPasswordTextBox.Enabled = false;
            this.ConfirmPasswordTextBox.Location = new System.Drawing.Point(98, 78);
            this.ConfirmPasswordTextBox.Name = "ConfirmPasswordTextBox";
            this.ConfirmPasswordTextBox.PasswordChar = '*';
            this.ConfirmPasswordTextBox.Size = new System.Drawing.Size(147, 20);
            this.ConfirmPasswordTextBox.TabIndex = 2;
            this.ConfirmPasswordTextBox.TextChanged += new System.EventHandler(this.ConfirmPasswordTextBox_TextChanged);
            // 
            // revealPwdButton
            // 
            this.revealPwdButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.revealPwdButton.Enabled = false;
            this.revealPwdButton.FlatAppearance.BorderSize = 0;
            this.revealPwdButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.revealPwdButton.ImageIndex = 0;
            this.revealPwdButton.ImageList = this.hideRevealButtonImages;
            this.revealPwdButton.Location = new System.Drawing.Point(251, 44);
            this.revealPwdButton.Name = "revealPwdButton";
            this.revealPwdButton.Size = new System.Drawing.Size(25, 30);
            this.revealPwdButton.TabIndex = 7;
            this.revealPwdButton.UseVisualStyleBackColor = false;
            // 
            // hideRevealButtonImages
            // 
            this.hideRevealButtonImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("hideRevealButtonImages.ImageStream")));
            this.hideRevealButtonImages.TransparentColor = System.Drawing.Color.Transparent;
            this.hideRevealButtonImages.Images.SetKeyName(0, "eye_hide.png");
            this.hideRevealButtonImages.Images.SetKeyName(1, "eye_reveal.png");
            // 
            // MasterPasswordOptionPanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "MasterPasswordOptionPanel";
            this.Size = new System.Drawing.Size(511, 332);
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
        private Label lblPassword;
        private TextBox PasswordTextbox;
        private Label lblConfirm;
        private TextBox ConfirmPasswordTextBox;
        private Button revealPwdButton;
        private ImageList hideRevealButtonImages;
    }
}
