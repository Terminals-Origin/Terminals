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
      this.reTryButton = new System.Windows.Forms.Button();
      this.messageLabel = new System.Windows.Forms.Label();
      this.exitButton = new System.Windows.Forms.Button();
      this.textBoxDetail = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // reTryButton
      // 
      this.reTryButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.reTryButton.Location = new System.Drawing.Point(222, 133);
      this.reTryButton.Name = "reTryButton";
      this.reTryButton.Size = new System.Drawing.Size(90, 23);
      this.reTryButton.TabIndex = 1;
      this.reTryButton.Text = "Try again";
      this.reTryButton.UseVisualStyleBackColor = true;
      // 
      // messageLabel
      // 
      this.messageLabel.Location = new System.Drawing.Point(12, 9);
      this.messageLabel.Name = "messageLabel";
      this.messageLabel.Size = new System.Drawing.Size(396, 66);
      this.messageLabel.TabIndex = 2;
      this.messageLabel.Text = "Problem occurred, when Terminals tried to work with data.\r\n- Try again to repeat " +
    "the operation\r\n- Exit to close all opened connections and exit the application\r\n" +
    "\r\nDetail:";
      // 
      // exitButton
      // 
      this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.exitButton.Location = new System.Drawing.Point(318, 133);
      this.exitButton.Name = "exitButton";
      this.exitButton.Size = new System.Drawing.Size(90, 23);
      this.exitButton.TabIndex = 3;
      this.exitButton.Text = "Exit";
      this.exitButton.UseVisualStyleBackColor = true;
      // 
      // textBoxDetail
      // 
      this.textBoxDetail.Location = new System.Drawing.Point(15, 78);
      this.textBoxDetail.Multiline = true;
      this.textBoxDetail.Name = "textBoxDetail";
      this.textBoxDetail.ReadOnly = true;
      this.textBoxDetail.Size = new System.Drawing.Size(393, 42);
      this.textBoxDetail.TabIndex = 6;
      // 
      // PersistenceErrorForm
      // 
      this.AcceptButton = this.reTryButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.exitButton;
      this.ClientSize = new System.Drawing.Size(417, 168);
      this.Controls.Add(this.textBoxDetail);
      this.Controls.Add(this.exitButton);
      this.Controls.Add(this.messageLabel);
      this.Controls.Add(this.reTryButton);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PersistenceErrorForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Terminals - data problem occurred";
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button reTryButton;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.TextBox textBoxDetail;
    }
}