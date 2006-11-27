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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTerminals = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblEyal = new System.Windows.Forms.LinkLabel();
            this.lblDudu = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(352, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Terminals is a multi tab terminal services/remote desktop client. It uses Termina" +
                "l Services ActiveX Client (mstscax.dll).";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label2.Location = new System.Drawing.Point(16, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Terminals";
            // 
            // lblTerminals
            // 
            this.lblTerminals.AutoSize = true;
            this.lblTerminals.Location = new System.Drawing.Point(16, 72);
            this.lblTerminals.Name = "lblTerminals";
            this.lblTerminals.Size = new System.Drawing.Size(170, 13);
            this.lblTerminals.TabIndex = 2;
            this.lblTerminals.TabStop = true;
            this.lblTerminals.Text = "Terminals Home Page at CodePlex";
            this.lblTerminals.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblTerminals_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(280, 200);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Terminals is developed by:";
            // 
            // lblEyal
            // 
            this.lblEyal.AutoSize = true;
            this.lblEyal.Location = new System.Drawing.Point(16, 120);
            this.lblEyal.Name = "lblEyal";
            this.lblEyal.Size = new System.Drawing.Size(51, 13);
            this.lblEyal.TabIndex = 5;
            this.lblEyal.TabStop = true;
            this.lblEyal.Text = "Eayl Post";
            this.lblEyal.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblEyal_LinkClicked);
            // 
            // lblDudu
            // 
            this.lblDudu.AutoSize = true;
            this.lblDudu.Location = new System.Drawing.Point(16, 144);
            this.lblDudu.Name = "lblDudu";
            this.lblDudu.Size = new System.Drawing.Size(73, 13);
            this.lblDudu.TabIndex = 6;
            this.lblDudu.TabStop = true;
            this.lblDudu.Text = "Dudu Shmaya";
            this.lblDudu.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblDudu_LinkClicked);
            // 
            // AboutForm
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(378, 233);
            this.Controls.Add(this.lblDudu);
            this.Controls.Add(this.lblEyal);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblTerminals);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Terminals";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lblTerminals;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lblEyal;
        private System.Windows.Forms.LinkLabel lblDudu;
    }
}