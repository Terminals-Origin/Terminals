namespace Terminals.Forms
{
    partial class PersistenceErrorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PersistenceErrorForm));
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.reTryButton = new System.Windows.Forms.Button();
            this.messageLabel = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.detailLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Image = global::Terminals.Properties.Resources.terminalsicon;
            this.iconPictureBox.Location = new System.Drawing.Point(12, 11);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(106, 108);
            this.iconPictureBox.TabIndex = 0;
            this.iconPictureBox.TabStop = false;
            // 
            // reTryButton
            // 
            this.reTryButton.Location = new System.Drawing.Point(198, 124);
            this.reTryButton.Name = "reTryButton";
            this.reTryButton.Size = new System.Drawing.Size(113, 23);
            this.reTryButton.TabIndex = 1;
            this.reTryButton.Text = "Try again";
            this.reTryButton.UseVisualStyleBackColor = true;
            this.reTryButton.Click += new System.EventHandler(this.ReTryButtonClick);
            // 
            // messageLabel
            // 
            this.messageLabel.Location = new System.Drawing.Point(140, 13);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(300, 71);
            this.messageLabel.TabIndex = 2;
            this.messageLabel.Text = "Problem occurred, when Terminals tried to work with data.\r\n- Try again to repeat " +
    "the operation\r\n- Exit application to close all opened connections and exit\r\n\r\nDe" +
    "tail:";
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(317, 124);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(123, 23);
            this.exitButton.TabIndex = 3;
            this.exitButton.Text = "Exit application";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButtonClick);
            // 
            // detailLabel
            // 
            this.detailLabel.Location = new System.Drawing.Point(140, 84);
            this.detailLabel.Name = "detailLabel";
            this.detailLabel.Size = new System.Drawing.Size(300, 31);
            this.detailLabel.TabIndex = 4;
            // 
            // PersistenceErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 159);
            this.ControlBox = false;
            this.Controls.Add(this.detailLabel);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.reTryButton);
            this.Controls.Add(this.iconPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PersistenceErrorForm";
            this.ShowInTaskbar = false;
            this.Text = "Terminals - data problem occurred";
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.Button reTryButton;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Label detailLabel;
    }
}