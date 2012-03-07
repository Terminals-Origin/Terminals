namespace TabControl
{
    partial class TabPreview
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
            this.imageDetach = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.imageDetach)).BeginInit();
            this.SuspendLayout();
            // 
            // imageDetach
            // 
            this.imageDetach.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.imageDetach.Image = global::TabControl.Properties.Resources.application_put;
            this.imageDetach.Location = new System.Drawing.Point(143, 1);
            this.imageDetach.Name = "imageDetach";
            this.imageDetach.Size = new System.Drawing.Size(17, 17);
            this.imageDetach.TabIndex = 0;
            this.imageDetach.TabStop = false;
            this.imageDetach.Visible = false;
            // 
            // TabPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(161, 27);
            this.Controls.Add(this.imageDetach);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TabPreview";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "TabPreview";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Gray;
            ((System.ComponentModel.ISupportInitialize)(this.imageDetach)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imageDetach;

    }
}