namespace Terminals.Wizard
{
    partial class MasterPassword
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.enterPassword1 = new Terminals.Wizard.EnterPassword();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(360, 62);
            this.label2.TabIndex = 4;
            this.label2.Text = "By setting your Master Password allows Terminals to store your connection informa" +
                "tion in a much more secure manner.  Although it is not required, it is highly re" +
                "commended that you do set it now.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "Welcome to Terminals";
            // 
            // enterPassword1
            // 
            this.enterPassword1.Location = new System.Drawing.Point(33, 120);
            this.enterPassword1.Name = "enterPassword1";
            this.enterPassword1.Size = new System.Drawing.Size(317, 115);
            this.enterPassword1.TabIndex = 5;
            // 
            // MasterPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.enterPassword1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MasterPassword";
            this.Size = new System.Drawing.Size(379, 283);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private EnterPassword enterPassword1;
    }
}
