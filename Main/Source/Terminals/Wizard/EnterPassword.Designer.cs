namespace Terminals.Wizard
{
    partial class EnterPassword
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
            if(disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnterPassword));
            this.confirmTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.masterPasswordTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.revealPwdButton = new System.Windows.Forms.Button();
            this.hideRevealButtonImages = new System.Windows.Forms.ImageList(this.components);
            this.EnableMasterPassword = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // confirmTextBox
            // 
            this.confirmTextBox.Location = new System.Drawing.Point(101, 29);
            this.confirmTextBox.Name = "confirmTextBox";
            this.confirmTextBox.PasswordChar = '*';
            this.confirmTextBox.Size = new System.Drawing.Size(182, 20);
            this.confirmTextBox.TabIndex = 12;
            this.confirmTextBox.TextChanged += new System.EventHandler(this.ConfirmTextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Confirmation:";
            // 
            // masterPasswordTextbox
            // 
            this.masterPasswordTextbox.Location = new System.Drawing.Point(101, 3);
            this.masterPasswordTextbox.Name = "masterPasswordTextbox";
            this.masterPasswordTextbox.PasswordChar = '*';
            this.masterPasswordTextbox.Size = new System.Drawing.Size(182, 20);
            this.masterPasswordTextbox.TabIndex = 10;
            this.masterPasswordTextbox.TextChanged += new System.EventHandler(this.ConfirmTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Master Password:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(101, 73);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(182, 14);
            this.progressBar1.TabIndex = 13;
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Location = new System.Drawing.Point(104, 55);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(0, 13);
            this.ErrorLabel.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Strength:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.revealPwdButton);
            this.panel1.Controls.Add(this.masterPasswordTextbox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.ErrorLabel);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.confirmTextBox);
            this.panel1.Location = new System.Drawing.Point(3, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 92);
            this.panel1.TabIndex = 16;
            // 
            // revealPwdButton
            // 
            this.revealPwdButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.revealPwdButton.BackColor = System.Drawing.Color.Transparent;
            this.revealPwdButton.FlatAppearance.BorderSize = 0;
            this.revealPwdButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.revealPwdButton.ImageIndex = 0;
            this.revealPwdButton.ImageList = this.hideRevealButtonImages;
            this.revealPwdButton.Location = new System.Drawing.Point(285, -3);
            this.revealPwdButton.Name = "revealPwdButton";
            this.revealPwdButton.Size = new System.Drawing.Size(25, 30);
            this.revealPwdButton.TabIndex = 16;
            this.revealPwdButton.UseVisualStyleBackColor = false;
            // 
            // hideRevealButtonImages
            // 
            this.hideRevealButtonImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("hideRevealButtonImages.ImageStream")));
            this.hideRevealButtonImages.TransparentColor = System.Drawing.Color.Transparent;
            this.hideRevealButtonImages.Images.SetKeyName(0, "eye_hide.png");
            this.hideRevealButtonImages.Images.SetKeyName(1, "eye_reveal.png");
            // 
            // EnableMasterPassword
            // 
            this.EnableMasterPassword.AutoSize = true;
            this.EnableMasterPassword.Location = new System.Drawing.Point(10, 5);
            this.EnableMasterPassword.Name = "EnableMasterPassword";
            this.EnableMasterPassword.Size = new System.Drawing.Size(143, 17);
            this.EnableMasterPassword.TabIndex = 17;
            this.EnableMasterPassword.Text = "Enable Master Password";
            this.EnableMasterPassword.UseVisualStyleBackColor = true;
            this.EnableMasterPassword.CheckedChanged += new System.EventHandler(this.EnableMasterPassword_CheckedChanged);
            // 
            // EnterPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EnableMasterPassword);
            this.Controls.Add(this.panel1);
            this.Name = "EnterPassword";
            this.Size = new System.Drawing.Size(323, 120);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox confirmTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox masterPasswordTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox EnableMasterPassword;
        private System.Windows.Forms.Button revealPwdButton;
        private System.Windows.Forms.ImageList hideRevealButtonImages;
    }
}
