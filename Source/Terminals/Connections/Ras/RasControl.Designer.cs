namespace Terminals.Forms.EditFavorite
{
    partial class RasControl
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
            this.RASGroupBox = new System.Windows.Forms.GroupBox();
            this.RASDetailsListBox = new System.Windows.Forms.ListBox();
            this.ras1 = new FalafelSoftware.TransPort.Ras();
            this.RASGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // RASGroupBox
            // 
            this.RASGroupBox.Controls.Add(this.RASDetailsListBox);
            this.RASGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RASGroupBox.Location = new System.Drawing.Point(0, 0);
            this.RASGroupBox.Name = "RASGroupBox";
            this.RASGroupBox.Size = new System.Drawing.Size(590, 365);
            this.RASGroupBox.TabIndex = 1;
            this.RASGroupBox.TabStop = false;
            // 
            // RASDetailsListBox
            // 
            this.RASDetailsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RASDetailsListBox.FormattingEnabled = true;
            this.RASDetailsListBox.Location = new System.Drawing.Point(3, 16);
            this.RASDetailsListBox.Name = "RASDetailsListBox";
            this.RASDetailsListBox.Size = new System.Drawing.Size(584, 346);
            this.RASDetailsListBox.TabIndex = 0;
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
            // RasControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.RASGroupBox);
            this.Name = "RasControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.RASGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox RASGroupBox;
        private System.Windows.Forms.ListBox RASDetailsListBox;
        private FalafelSoftware.TransPort.Ras ras1;
    }
}
