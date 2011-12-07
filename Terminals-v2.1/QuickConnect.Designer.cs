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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickConnect));
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.cmbServerList = new System.Windows.Forms.ComboBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.Location = new System.Drawing.Point(220, 2);
            this.ButtonConnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(80, 28);
            this.ButtonConnect.TabIndex = 0;
            this.ButtonConnect.Text = "&Connect";
            this.ButtonConnect.UseVisualStyleBackColor = true;
            this.ButtonConnect.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // cmbServerList
            // 
            this.cmbServerList.FormattingEnabled = true;
            this.cmbServerList.Location = new System.Drawing.Point(1, 5);
            this.cmbServerList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbServerList.Name = "cmbServerList";
            this.cmbServerList.Size = new System.Drawing.Size(209, 24);
            this.cmbServerList.TabIndex = 1;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(303, 2);
            this.ButtonCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(80, 28);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // QuickConnect
            // 
            this.AcceptButton = this.ButtonConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 34);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.cmbServerList);
            this.Controls.Add(this.ButtonConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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