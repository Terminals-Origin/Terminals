namespace Terminals.Forms.EditFavorite
{
    partial class ExecuteControl
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
            this.txtInitialDirectory = new System.Windows.Forms.TextBox();
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.chkExecuteBeforeConnect = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.chkWaitForExit = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtInitialDirectory
            // 
            this.txtInitialDirectory.Location = new System.Drawing.Point(118, 110);
            this.txtInitialDirectory.Name = "txtInitialDirectory";
            this.txtInitialDirectory.Size = new System.Drawing.Size(265, 20);
            this.txtInitialDirectory.TabIndex = 14;
            // 
            // txtArguments
            // 
            this.txtArguments.Location = new System.Drawing.Point(118, 80);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(265, 20);
            this.txtArguments.TabIndex = 13;
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(118, 50);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(265, 20);
            this.txtCommand.TabIndex = 12;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 113);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(79, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "Initial Directory:";
            // 
            // chkExecuteBeforeConnect
            // 
            this.chkExecuteBeforeConnect.AutoSize = true;
            this.chkExecuteBeforeConnect.Location = new System.Drawing.Point(6, 23);
            this.chkExecuteBeforeConnect.Name = "chkExecuteBeforeConnect";
            this.chkExecuteBeforeConnect.Size = new System.Drawing.Size(140, 17);
            this.chkExecuteBeforeConnect.TabIndex = 11;
            this.chkExecuteBeforeConnect.Text = "&Execute before connect";
            this.chkExecuteBeforeConnect.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 83);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 13);
            this.label12.TabIndex = 17;
            this.label12.Text = "Arguments";
            // 
            // chkWaitForExit
            // 
            this.chkWaitForExit.AutoSize = true;
            this.chkWaitForExit.Location = new System.Drawing.Point(6, 139);
            this.chkWaitForExit.Name = "chkWaitForExit";
            this.chkWaitForExit.Size = new System.Drawing.Size(82, 17);
            this.chkWaitForExit.TabIndex = 15;
            this.chkWaitForExit.Text = "&Wait for exit";
            this.chkWaitForExit.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 53);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "Command:";
            // 
            // ExecuteControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtInitialDirectory);
            this.Controls.Add(this.txtArguments);
            this.Controls.Add(this.txtCommand);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.chkExecuteBeforeConnect);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.chkWaitForExit);
            this.Controls.Add(this.label11);

            this.Name = "ExecuteControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtInitialDirectory;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkExecuteBeforeConnect;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkWaitForExit;
        private System.Windows.Forms.Label label11;
    }
}
