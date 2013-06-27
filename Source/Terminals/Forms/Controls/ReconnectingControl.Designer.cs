namespace Terminals.Forms.Controls
{
    partial class ReconnectingControl
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
            this.abortButton = new System.Windows.Forms.Button();
            this.progressPictureBox = new System.Windows.Forms.PictureBox();
            this.messageLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.progressPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // abortButton
            // 
            this.abortButton.Location = new System.Drawing.Point(87, 52);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(75, 23);
            this.abortButton.TabIndex = 1;
            this.abortButton.Text = "Abort";
            this.abortButton.UseVisualStyleBackColor = true;
            this.abortButton.Click += new System.EventHandler(this.AbortButtonClick);
            // 
            // progressPictureBox
            // 
            this.progressPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.progressPictureBox.Image = global::Terminals.Properties.Resources.Progress;
            this.progressPictureBox.Location = new System.Drawing.Point(18, 12);
            this.progressPictureBox.Name = "progressPictureBox";
            this.progressPictureBox.Size = new System.Drawing.Size(31, 31);
            this.progressPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.progressPictureBox.TabIndex = 2;
            this.progressPictureBox.TabStop = false;
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(65, 12);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(167, 26);
            this.messageLabel.TabIndex = 3;
            this.messageLabel.Text = "Connection to the server was lost.\r\nTrying to reconnect...";
            // 
            // ReconnectingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.progressPictureBox);
            this.Controls.Add(this.abortButton);
            this.Name = "ReconnectingControl";
            this.Size = new System.Drawing.Size(250, 83);
            ((System.ComponentModel.ISupportInitialize)(this.progressPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button abortButton;
        private System.Windows.Forms.PictureBox progressPictureBox;
        private System.Windows.Forms.Label messageLabel;
    }
}
