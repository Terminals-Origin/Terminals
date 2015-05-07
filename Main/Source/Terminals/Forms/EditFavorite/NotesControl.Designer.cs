namespace Terminals.Forms.EditFavorite
{
    partial class NotesControl
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
            this.NotesTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // NotesTextbox
            // 
            this.NotesTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NotesTextbox.Location = new System.Drawing.Point(3, 3);
            this.NotesTextbox.Multiline = true;
            this.NotesTextbox.Name = "NotesTextbox";
            this.NotesTextbox.Size = new System.Drawing.Size(284, 244);
            this.NotesTextbox.TabIndex = 34;
            // 
            // NotesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NotesTextbox);
            this.Name = "NotesControl";
            this.Size = new System.Drawing.Size(294, 250);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NotesTextbox;
    }
}
