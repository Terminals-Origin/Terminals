namespace Terminals.Forms.EditFavorite
{
    partial class RdpSecurityControl
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.SecurityStartFullScreenCheckbox = new System.Windows.Forms.CheckBox();
            this.label25 = new System.Windows.Forms.Label();
            this.SecurityWorkingFolderTextBox = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.SecuriytStartProgramTextbox = new System.Windows.Forms.TextBox();
            this.SecuritySettingsEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.SecurityStartFullScreenCheckbox);
            this.panel2.Controls.Add(this.label25);
            this.panel2.Controls.Add(this.SecurityWorkingFolderTextBox);
            this.panel2.Controls.Add(this.label24);
            this.panel2.Controls.Add(this.SecuriytStartProgramTextbox);
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(434, 99);
            this.panel2.TabIndex = 3;
            // 
            // SecurityStartFullScreenCheckbox
            // 
            this.SecurityStartFullScreenCheckbox.AutoSize = true;
            this.SecurityStartFullScreenCheckbox.Location = new System.Drawing.Point(6, 67);
            this.SecurityStartFullScreenCheckbox.Name = "SecurityStartFullScreenCheckbox";
            this.SecurityStartFullScreenCheckbox.Size = new System.Drawing.Size(104, 17);
            this.SecurityStartFullScreenCheckbox.TabIndex = 4;
            this.SecurityStartFullScreenCheckbox.Text = "Start Full Screen";
            this.SecurityStartFullScreenCheckbox.UseVisualStyleBackColor = true;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(3, 40);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(82, 13);
            this.label25.TabIndex = 3;
            this.label25.Text = "Working Folder:";
            // 
            // SecurityWorkingFolderTextBox
            // 
            this.SecurityWorkingFolderTextBox.Location = new System.Drawing.Point(116, 37);
            this.SecurityWorkingFolderTextBox.Name = "SecurityWorkingFolderTextBox";
            this.SecurityWorkingFolderTextBox.Size = new System.Drawing.Size(286, 20);
            this.SecurityWorkingFolderTextBox.TabIndex = 2;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(3, 10);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(74, 13);
            this.label24.TabIndex = 1;
            this.label24.Text = "Start Program:";
            // 
            // SecuriytStartProgramTextbox
            // 
            this.SecuriytStartProgramTextbox.Location = new System.Drawing.Point(116, 7);
            this.SecuriytStartProgramTextbox.Name = "SecuriytStartProgramTextbox";
            this.SecuriytStartProgramTextbox.Size = new System.Drawing.Size(286, 20);
            this.SecuriytStartProgramTextbox.TabIndex = 0;
            // 
            // SecuritySettingsEnabledCheckbox
            // 
            this.SecuritySettingsEnabledCheckbox.AutoSize = true;
            this.SecuritySettingsEnabledCheckbox.Location = new System.Drawing.Point(6, 23);
            this.SecuritySettingsEnabledCheckbox.Name = "SecuritySettingsEnabledCheckbox";
            this.SecuritySettingsEnabledCheckbox.Size = new System.Drawing.Size(65, 17);
            this.SecuritySettingsEnabledCheckbox.TabIndex = 2;
            this.SecuritySettingsEnabledCheckbox.Text = "Enabled";
            this.SecuritySettingsEnabledCheckbox.UseVisualStyleBackColor = true;
            this.SecuritySettingsEnabledCheckbox.CheckedChanged += new System.EventHandler(this.SecuritySettingsEnabledCheckbox_CheckedChanged);
            // 
            // RdpSecurityControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.SecuritySettingsEnabledCheckbox);
            this.Name = "RdpSecurityControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox SecurityStartFullScreenCheckbox;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox SecurityWorkingFolderTextBox;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox SecuriytStartProgramTextbox;
        private System.Windows.Forms.CheckBox SecuritySettingsEnabledCheckbox;
    }
}
