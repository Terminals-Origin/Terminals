namespace Terminals {
    partial class QuickConnect {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.cmbServerList = new System.Windows.Forms.ComboBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ButtonConnect.Location = new System.Drawing.Point(165, 2);
            this.ButtonConnect.Name = "ConnectButton";
            this.ButtonConnect.Size = new System.Drawing.Size(60, 23);
            this.ButtonConnect.TabIndex = 0;
            this.ButtonConnect.Text = "&Connect";
            this.ButtonConnect.UseVisualStyleBackColor = true;
            this.ButtonConnect.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // cmbServerList
            // 
            this.cmbServerList.FormattingEnabled = true;
            this.cmbServerList.Location = new System.Drawing.Point(1, 4);
            this.cmbServerList.Name = "cmbServerList";
            this.cmbServerList.Size = new System.Drawing.Size(158, 21);
            this.cmbServerList.TabIndex = 1;
            // 
            // CancelButton
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(227, 2);
            this.ButtonCancel.Name = "CancelButton";
            this.ButtonCancel.Size = new System.Drawing.Size(60, 23);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // QuickConnect
            // 
            this.AcceptButton = this.ButtonConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 28);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.cmbServerList);
            this.Controls.Add(this.ButtonConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuickConnect";
            this.Text = "Quick Connect...";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonConnect;
        private System.Windows.Forms.ComboBox cmbServerList;
        private System.Windows.Forms.Button ButtonCancel;
    }
}