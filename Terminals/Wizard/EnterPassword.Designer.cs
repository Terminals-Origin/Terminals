namespace Terminals.Wizard
{
    partial class EnterPassword
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
            if(disposing && (components != null))
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
            this.confirmTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.masterPasswordTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // confirmTextBox
            // 
            this.confirmTextBox.Location = new System.Drawing.Point(109, 41);
            this.confirmTextBox.Name = "confirmTextBox";
            this.confirmTextBox.PasswordChar = '*';
            this.confirmTextBox.Size = new System.Drawing.Size(182, 20);
            this.confirmTextBox.TabIndex = 12;
            this.confirmTextBox.TextChanged += new System.EventHandler(this.confirmTextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Confirmation:";
            // 
            // masterPasswordTextbox
            // 
            this.masterPasswordTextbox.Location = new System.Drawing.Point(109, 15);
            this.masterPasswordTextbox.Name = "masterPasswordTextbox";
            this.masterPasswordTextbox.PasswordChar = '*';
            this.masterPasswordTextbox.Size = new System.Drawing.Size(182, 20);
            this.masterPasswordTextbox.TabIndex = 10;
            this.masterPasswordTextbox.TextChanged += new System.EventHandler(this.confirmTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Master Password:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(109, 90);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(182, 14);
            this.progressBar1.TabIndex = 13;
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Location = new System.Drawing.Point(115, 70);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(0, 13);
            this.ErrorLabel.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Strength:";
            // 
            // EnterPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ErrorLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.confirmTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.masterPasswordTextbox);
            this.Controls.Add(this.label3);
            this.Name = "EnterPassword";
            this.Size = new System.Drawing.Size(317, 115);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox confirmTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox masterPasswordTextbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.Label label1;
    }
}
