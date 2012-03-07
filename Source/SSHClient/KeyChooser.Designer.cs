namespace SSHClient
{
    partial class KeyChooser
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
        	this.box = new System.Windows.Forms.ComboBox();
        	this.label = new System.Windows.Forms.Label();
        	this.SuspendLayout();
        	// 
        	// box
        	// 
        	this.box.FormattingEnabled = true;
        	this.box.Location = new System.Drawing.Point(67, 13);
        	this.box.Name = "box";
        	this.box.Size = new System.Drawing.Size(148, 21);
        	this.box.TabIndex = 0;
        	// 
        	// label
        	// 
        	this.label.AutoSize = true;
        	this.label.Location = new System.Drawing.Point(18, 16);
        	this.label.Name = "label";
        	this.label.Size = new System.Drawing.Size(43, 13);
        	this.label.TabIndex = 1;
        	this.label.Text = "Key tag";
        	// 
        	// KeyChooser
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.Controls.Add(this.label);
        	this.Controls.Add(this.box);
        	this.Name = "KeyChooser";
        	this.Size = new System.Drawing.Size(238, 51);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox box;
        private System.Windows.Forms.Label label;
    }
}
