namespace Terminals.Connections
{
    partial class RASProperties
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
        private void InitializeComponent() {
            this.ras1 = new FalafelSoftware.TransPort.Ras();
            this.lbDetails1 = new System.Windows.Forms.ListBox();
            this.LogListBox = new System.Windows.Forms.ListBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.SuspendLayout();
            // 
            // ras1
            // 
            this.ras1.CallBackNumber = null;
            this.ras1.Domain = null;
            this.ras1.EntryName = null;
            this.ras1.Password = null;
            this.ras1.PhoneNumber = null;
            this.ras1.UserName = null;
            // 
            // lbDetails1
            // 
            this.lbDetails1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDetails1.FormattingEnabled = true;
            this.lbDetails1.Location = new System.Drawing.Point(0, 0);
            this.lbDetails1.Name = "lbDetails1";
            this.lbDetails1.Size = new System.Drawing.Size(470, 199);
            this.lbDetails1.TabIndex = 0;
            // 
            // LogListBox
            // 
            this.LogListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogListBox.FormattingEnabled = true;
            this.LogListBox.Location = new System.Drawing.Point(0, 199);
            this.LogListBox.Name = "LogListBox";
            this.LogListBox.Size = new System.Drawing.Size(470, 160);
            this.LogListBox.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 199);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(470, 3);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // RASProperties
            // 
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.LogListBox);
            this.Controls.Add(this.lbDetails1);
            this.Name = "RASProperties";
            this.Size = new System.Drawing.Size(470, 368);
            this.ResumeLayout(false);

        }

        #endregion

        private FalafelSoftware.TransPort.Ras ras1;
        private System.Windows.Forms.ListBox lbDetails1;
        private System.Windows.Forms.ListBox LogListBox;
        private System.Windows.Forms.Splitter splitter1;
    }
}
