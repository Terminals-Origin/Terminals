namespace Terminals
{
    partial class AboutForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.label1 = new System.Windows.Forms.Label();
            this.lblTerminals = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblEyal = new System.Windows.Forms.LinkLabel();
            this.lblDudu = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lblHiro = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(408, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Terminals is a multi tab terminal services/remote desktop client. It uses the Ter" +
                "minal Services ActiveX Client (mstscax.dll).";
            // 
            // lblTerminals
            // 
            this.lblTerminals.AutoSize = true;
            this.lblTerminals.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblTerminals.Location = new System.Drawing.Point(131, 159);
            this.lblTerminals.Name = "lblTerminals";
            this.lblTerminals.Size = new System.Drawing.Size(170, 13);
            this.lblTerminals.TabIndex = 1;
            this.lblTerminals.TabStop = true;
            this.lblTerminals.Text = "Terminals Home Page at CodePlex";
            this.lblTerminals.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblTerminals_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.SystemColors.Control;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(340, 258);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 26);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "OK";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 195);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Terminals is developed by:";
            // 
            // lblEyal
            // 
            this.lblEyal.AutoSize = true;
            this.lblEyal.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblEyal.LinkColor = System.Drawing.Color.Blue;
            this.lblEyal.Location = new System.Drawing.Point(32, 217);
            this.lblEyal.Name = "lblEyal";
            this.lblEyal.Size = new System.Drawing.Size(27, 13);
            this.lblEyal.TabIndex = 2;
            this.lblEyal.TabStop = true;
            this.lblEyal.Text = "Eyal";
            this.lblEyal.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblEyal.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblEyal_LinkClicked);
            // 
            // lblDudu
            // 
            this.lblDudu.AutoSize = true;
            this.lblDudu.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblDudu.LinkColor = System.Drawing.Color.Blue;
            this.lblDudu.Location = new System.Drawing.Point(32, 241);
            this.lblDudu.Name = "lblDudu";
            this.lblDudu.Size = new System.Drawing.Size(32, 13);
            this.lblDudu.TabIndex = 3;
            this.lblDudu.TabStop = true;
            this.lblDudu.Text = "Dudu";
            this.lblDudu.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblDudu.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblDudu_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.Blue;
            this.linkLabel1.Location = new System.Drawing.Point(144, 241);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(45, 13);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Shmaya";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblDudu_LinkClicked);
            // 
            // lblHiro
            // 
            this.lblHiro.AutoSize = true;
            this.lblHiro.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lblHiro.LinkColor = System.Drawing.Color.Blue;
            this.lblHiro.Location = new System.Drawing.Point(61, 241);
            this.lblHiro.Name = "lblHiro";
            this.lblHiro.Size = new System.Drawing.Size(85, 13);
            this.lblHiro.TabIndex = 4;
            this.lblHiro.TabStop = true;
            this.lblHiro.Text = "\"Hiro Nakamura\"";
            this.lblHiro.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblHiro.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHiro_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Terminals.Properties.Resources.rdp;
            this.pictureBox1.Location = new System.Drawing.Point(15, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(405, 67);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel2.LinkColor = System.Drawing.Color.Blue;
            this.linkLabel2.Location = new System.Drawing.Point(131, 217);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(28, 13);
            this.linkLabel2.TabIndex = 7;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Post";
            this.linkLabel2.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel3.LinkColor = System.Drawing.Color.Blue;
            this.linkLabel3.Location = new System.Drawing.Point(56, 217);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(76, 13);
            this.linkLabel3.TabIndex = 8;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "\"Peter Petrelli\"";
            this.linkLabel3.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblVersion.Location = new System.Drawing.Point(12, 91);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(327, 13);
            this.lblVersion.TabIndex = 9;
            this.lblVersion.Text = "Terminals 1.6  (RDP, VNC, VMRC, RAS, Telnet and SSH Supported)";
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel4.LinkColor = System.Drawing.Color.Blue;
            this.linkLabel4.Location = new System.Drawing.Point(32, 262);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(68, 13);
            this.linkLabel4.TabIndex = 10;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "Rob Chartier";
            this.linkLabel4.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // AboutForm
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(432, 296);
            this.Controls.Add(this.linkLabel4);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblHiro);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.lblDudu);
            this.Controls.Add(this.lblEyal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblTerminals);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Terminals";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel lblTerminals;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lblEyal;
        private System.Windows.Forms.LinkLabel lblDudu;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel lblHiro;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.LinkLabel linkLabel4;
    }
}