namespace Terminals.Forms
{
    partial class UnhandledTerminationForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnhandledTerminationForm));
      this.closeButton = new System.Windows.Forms.Button();
      this.messageLabel = new System.Windows.Forms.Label();
      this.logoPictureBox = new System.Windows.Forms.PictureBox();
      this.logoGroupBox = new System.Windows.Forms.GroupBox();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // closeButton
      // 
      this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.closeButton.Location = new System.Drawing.Point(264, 100);
      this.closeButton.Name = "closeButton";
      this.closeButton.Size = new System.Drawing.Size(74, 23);
      this.closeButton.TabIndex = 1;
      this.closeButton.Text = "Close";
      this.closeButton.UseVisualStyleBackColor = true;
      // 
      // messageLabel
      // 
      this.messageLabel.Location = new System.Drawing.Point(12, 69);
      this.messageLabel.Name = "messageLabel";
      this.messageLabel.Size = new System.Drawing.Size(338, 38);
      this.messageLabel.TabIndex = 2;
      this.messageLabel.Text = "Terminals has encountered a problem and needs to close.\r\nForm more details see lo" +
    "g files.";
      // 
      // logoPictureBox
      // 
      this.logoPictureBox.BackColor = System.Drawing.Color.White;
      this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Top;
      this.logoPictureBox.Image = global::Terminals.Properties.Resources.terminalsbanner_small;
      this.logoPictureBox.Location = new System.Drawing.Point(0, 0);
      this.logoPictureBox.Name = "logoPictureBox";
      this.logoPictureBox.Size = new System.Drawing.Size(350, 64);
      this.logoPictureBox.TabIndex = 3;
      this.logoPictureBox.TabStop = false;
      // 
      // logoGroupBox
      // 
      this.logoGroupBox.Location = new System.Drawing.Point(0, 0);
      this.logoGroupBox.Name = "logoGroupBox";
      this.logoGroupBox.Size = new System.Drawing.Size(350, 66);
      this.logoGroupBox.TabIndex = 4;
      this.logoGroupBox.TabStop = false;
      // 
      // UnhandledTerminationForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.closeButton;
      this.ClientSize = new System.Drawing.Size(350, 135);
      this.Controls.Add(this.logoPictureBox);
      this.Controls.Add(this.logoGroupBox);
      this.Controls.Add(this.closeButton);
      this.Controls.Add(this.messageLabel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "UnhandledTerminationForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Terminals - problem occurred";
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.GroupBox logoGroupBox;
    }
}