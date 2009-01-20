/*
 * Created by SharpDevelop.
 * User: cablej01
 * Date: 03/11/2008
 * Time: 18:30
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace SSHClient
{
	partial class Preferences
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonGenerateKey = new System.Windows.Forms.Button();
			this.comboBoxKey = new System.Windows.Forms.ComboBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.buttonKbd = new System.Windows.Forms.RadioButton();
			this.buttonPublicKey = new System.Windows.Forms.RadioButton();
			this.buttonPassword = new System.Windows.Forms.RadioButton();
			this.buttonHost = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonSSH2 = new System.Windows.Forms.RadioButton();
			this.buttonSSH1 = new System.Windows.Forms.RadioButton();
			this.openSSHTextBox = new System.Windows.Forms.TextBox();
			this.buttonCopy = new System.Windows.Forms.Button();
			this.groupBox4.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonGenerateKey
			// 
			this.buttonGenerateKey.Location = new System.Drawing.Point(318, 59);
			this.buttonGenerateKey.Name = "buttonGenerateKey";
			this.buttonGenerateKey.Size = new System.Drawing.Size(48, 23);
			this.buttonGenerateKey.TabIndex = 4;
			this.buttonGenerateKey.Text = "New";
			this.buttonGenerateKey.UseVisualStyleBackColor = true;
			this.buttonGenerateKey.Click += new System.EventHandler(this.ButtonGenerateKeyClick);
			// 
			// comboBoxKey
			// 
			this.comboBoxKey.FormattingEnabled = true;
			this.comboBoxKey.Location = new System.Drawing.Point(88, 59);
			this.comboBoxKey.Name = "comboBoxKey";
			this.comboBoxKey.Size = new System.Drawing.Size(169, 21);
			this.comboBoxKey.TabIndex = 3;
			this.comboBoxKey.SelectedIndexChanged += new System.EventHandler(this.comboBoxKey_SelectedIndexChanged);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.buttonKbd);
			this.groupBox4.Controls.Add(this.buttonPublicKey);
			this.groupBox4.Controls.Add(this.buttonPassword);
			this.groupBox4.Controls.Add(this.buttonHost);
			this.groupBox4.Location = new System.Drawing.Point(9, 7);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(357, 49);
			this.groupBox4.TabIndex = 2;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Authentication Method";
			// 
			// buttonKbd
			// 
			this.buttonKbd.Location = new System.Drawing.Point(247, 19);
			this.buttonKbd.Name = "buttonKbd";
			this.buttonKbd.Size = new System.Drawing.Size(104, 24);
			this.buttonKbd.TabIndex = 3;
			this.buttonKbd.TabStop = true;
			this.buttonKbd.Text = "Kbd Interactive";
			this.buttonKbd.UseVisualStyleBackColor = true;
			// 
			// buttonPublicKey
			// 
			this.buttonPublicKey.Location = new System.Drawing.Point(163, 19);
			this.buttonPublicKey.Name = "buttonPublicKey";
			this.buttonPublicKey.Size = new System.Drawing.Size(82, 24);
			this.buttonPublicKey.TabIndex = 2;
			this.buttonPublicKey.TabStop = true;
			this.buttonPublicKey.Text = "Public Key";
			this.buttonPublicKey.UseVisualStyleBackColor = true;
			// 
			// buttonPassword
			// 
			this.buttonPassword.Location = new System.Drawing.Point(79, 19);
			this.buttonPassword.Name = "buttonPassword";
			this.buttonPassword.Size = new System.Drawing.Size(77, 24);
			this.buttonPassword.TabIndex = 1;
			this.buttonPassword.TabStop = true;
			this.buttonPassword.Text = "Password";
			this.buttonPassword.UseVisualStyleBackColor = true;
			// 
			// buttonHost
			// 
			this.buttonHost.Location = new System.Drawing.Point(6, 19);
			this.buttonHost.Name = "buttonHost";
			this.buttonHost.Size = new System.Drawing.Size(66, 24);
			this.buttonHost.TabIndex = 0;
			this.buttonHost.TabStop = true;
			this.buttonHost.Text = "Host";
			this.buttonHost.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(45, 64);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Key";
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(3, 204);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(224, 60);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Local Ports Forwarded";
			// 
			// groupBox3
			// 
			this.groupBox3.Location = new System.Drawing.Point(238, 204);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(222, 60);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Remote Ports Forwarded";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonSSH2);
			this.panel1.Controls.Add(this.buttonSSH1);
			this.panel1.Location = new System.Drawing.Point(377, 7);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(83, 78);
			this.panel1.TabIndex = 0;
			// 
			// buttonSSH2
			// 
			this.buttonSSH2.Location = new System.Drawing.Point(16, 44);
			this.buttonSSH2.Name = "buttonSSH2";
			this.buttonSSH2.Size = new System.Drawing.Size(62, 24);
			this.buttonSSH2.TabIndex = 1;
			this.buttonSSH2.TabStop = true;
			this.buttonSSH2.Text = "SSH2";
			this.buttonSSH2.UseVisualStyleBackColor = true;
			// 
			// buttonSSH1
			// 
			this.buttonSSH1.Location = new System.Drawing.Point(16, 16);
			this.buttonSSH1.Name = "buttonSSH1";
			this.buttonSSH1.Size = new System.Drawing.Size(62, 24);
			this.buttonSSH1.TabIndex = 0;
			this.buttonSSH1.TabStop = true;
			this.buttonSSH1.Text = "SSH1";
			this.buttonSSH1.UseVisualStyleBackColor = true;
			this.buttonSSH1.CheckedChanged += new System.EventHandler(this.ButtonSSH1CheckedChanged);
			// 
			// openSSHTextBox
			// 
			this.openSSHTextBox.Location = new System.Drawing.Point(9, 86);
			this.openSSHTextBox.Multiline = true;
			this.openSSHTextBox.Name = "openSSHTextBox";
			this.openSSHTextBox.ReadOnly = true;
			this.openSSHTextBox.Size = new System.Drawing.Size(357, 112);
			this.openSSHTextBox.TabIndex = 5;
			// 
			// buttonCopy
			// 
			this.buttonCopy.Location = new System.Drawing.Point(372, 134);
			this.buttonCopy.Name = "buttonCopy";
			this.buttonCopy.Size = new System.Drawing.Size(88, 23);
			this.buttonCopy.TabIndex = 6;
			this.buttonCopy.Text = "Copy";
			this.buttonCopy.UseVisualStyleBackColor = true;
			this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
			// 
			// Preferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.buttonCopy);
			this.Controls.Add(this.openSSHTextBox);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.buttonGenerateKey);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.comboBoxKey);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label2);
			this.Name = "Preferences";
			this.Size = new System.Drawing.Size(466, 267);
			this.groupBox4.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.RadioButton buttonSSH2;
		private System.Windows.Forms.Button buttonGenerateKey;
		private System.Windows.Forms.ComboBox comboBoxKey;
		private System.Windows.Forms.RadioButton buttonKbd;
		private System.Windows.Forms.RadioButton buttonPublicKey;
		private System.Windows.Forms.RadioButton buttonPassword;
		private System.Windows.Forms.RadioButton buttonHost;
		private System.Windows.Forms.RadioButton buttonSSH1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox openSSHTextBox;
        private System.Windows.Forms.Button buttonCopy;
	}
}
