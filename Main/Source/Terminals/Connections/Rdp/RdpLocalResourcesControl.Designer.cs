namespace Terminals.Forms.EditFavorite
{
    partial class RdpLocalResourcesControl
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
            this.btnDrives = new System.Windows.Forms.Button();
            this.cmbSounds = new System.Windows.Forms.ComboBox();
            this.chkRedirectSmartcards = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkRedirectClipboard = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnBrowseShare = new System.Windows.Forms.Button();
            this.chkPrinters = new System.Windows.Forms.CheckBox();
            this.txtDesktopShare = new System.Windows.Forms.TextBox();
            this.chkSerialPorts = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnDrives
            // 
            this.btnDrives.Location = new System.Drawing.Point(121, 74);
            this.btnDrives.Name = "btnDrives";
            this.btnDrives.Size = new System.Drawing.Size(236, 25);
            this.btnDrives.TabIndex = 12;
            this.btnDrives.Text = "&Disk Drives && Plug and Play Devices...";
            this.btnDrives.UseVisualStyleBackColor = true;
            this.btnDrives.Click += new System.EventHandler(this.BtnDrives_Click);
            // 
            // cmbSounds
            // 
            this.cmbSounds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSounds.FormattingEnabled = true;
            this.cmbSounds.Items.AddRange(new object[] {
            "Redirect sounds to the client",
            "Play sounds at the remote computer",
            "Disable sound redirection; do not play sounds at the server"});
            this.cmbSounds.Location = new System.Drawing.Point(121, 17);
            this.cmbSounds.Name = "cmbSounds";
            this.cmbSounds.Size = new System.Drawing.Size(329, 21);
            this.cmbSounds.TabIndex = 1;
            // 
            // chkRedirectSmartcards
            // 
            this.chkRedirectSmartcards.AutoSize = true;
            this.chkRedirectSmartcards.Location = new System.Drawing.Point(229, 123);
            this.chkRedirectSmartcards.Name = "chkRedirectSmartcards";
            this.chkRedirectSmartcards.Size = new System.Drawing.Size(125, 17);
            this.chkRedirectSmartcards.TabIndex = 8;
            this.chkRedirectSmartcards.Text = "Redirect Smart ca&rds";
            this.chkRedirectSmartcards.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Remote &sounds:";
            // 
            // chkRedirectClipboard
            // 
            this.chkRedirectClipboard.AutoSize = true;
            this.chkRedirectClipboard.Checked = true;
            this.chkRedirectClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRedirectClipboard.Location = new System.Drawing.Point(229, 103);
            this.chkRedirectClipboard.Name = "chkRedirectClipboard";
            this.chkRedirectClipboard.Size = new System.Drawing.Size(113, 17);
            this.chkRedirectClipboard.TabIndex = 7;
            this.chkRedirectClipboard.Text = "Redirect &Clipboard";
            this.chkRedirectClipboard.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(118, 54);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(223, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Automatically connect to these local devices :";
            // 
            // btnBrowseShare
            // 
            this.btnBrowseShare.Image = global::Terminals.Properties.Resources.folder;
            this.btnBrowseShare.Location = new System.Drawing.Point(420, 157);
            this.btnBrowseShare.Name = "btnBrowseShare";
            this.btnBrowseShare.Size = new System.Drawing.Size(30, 24);
            this.btnBrowseShare.TabIndex = 11;
            this.btnBrowseShare.UseVisualStyleBackColor = true;
            this.btnBrowseShare.Click += new System.EventHandler(this.BtnBrowseShare_Click);
            // 
            // chkPrinters
            // 
            this.chkPrinters.AutoSize = true;
            this.chkPrinters.Location = new System.Drawing.Point(121, 103);
            this.chkPrinters.Name = "chkPrinters";
            this.chkPrinters.Size = new System.Drawing.Size(61, 17);
            this.chkPrinters.TabIndex = 4;
            this.chkPrinters.Text = "&Printers";
            this.chkPrinters.UseVisualStyleBackColor = true;
            // 
            // txtDesktopShare
            // 
            this.txtDesktopShare.Location = new System.Drawing.Point(121, 157);
            this.txtDesktopShare.Name = "txtDesktopShare";
            this.txtDesktopShare.Size = new System.Drawing.Size(293, 20);
            this.txtDesktopShare.TabIndex = 10;
            // 
            // chkSerialPorts
            // 
            this.chkSerialPorts.AutoSize = true;
            this.chkSerialPorts.Location = new System.Drawing.Point(121, 123);
            this.chkSerialPorts.Name = "chkSerialPorts";
            this.chkSerialPorts.Size = new System.Drawing.Size(78, 17);
            this.chkSerialPorts.TabIndex = 5;
            this.chkSerialPorts.Text = "Seria&l ports";
            this.chkSerialPorts.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 159);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Desktop S&hare:";
            // 
            // RdpLocalResourcesUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDrives);
            this.Controls.Add(this.cmbSounds);
            this.Controls.Add(this.chkRedirectSmartcards);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkRedirectClipboard);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnBrowseShare);
            this.Controls.Add(this.chkPrinters);
            this.Controls.Add(this.txtDesktopShare);
            this.Controls.Add(this.chkSerialPorts);
            this.Controls.Add(this.label10);
            
            this.Name = "RdpLocalResourcesUserControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDrives;
        private System.Windows.Forms.ComboBox cmbSounds;
        private System.Windows.Forms.CheckBox chkRedirectSmartcards;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkRedirectClipboard;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnBrowseShare;
        private System.Windows.Forms.CheckBox chkPrinters;
        private System.Windows.Forms.TextBox txtDesktopShare;
        private System.Windows.Forms.CheckBox chkSerialPorts;
        private System.Windows.Forms.Label label10;
    }
}
