namespace Terminals.Forms.EditFavorite
{
    partial class VmrcControl
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
            this.VmrcGroupBox = new System.Windows.Forms.GroupBox();
            this.VMRCReducedColorsCheckbox = new System.Windows.Forms.CheckBox();
            this.VMRCAdminModeCheckbox = new System.Windows.Forms.CheckBox();
            this.VmrcGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // VmrcGroupBox
            // 
            this.VmrcGroupBox.Controls.Add(this.VMRCReducedColorsCheckbox);
            this.VmrcGroupBox.Controls.Add(this.VMRCAdminModeCheckbox);
            this.VmrcGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VmrcGroupBox.Location = new System.Drawing.Point(0, 0);
            this.VmrcGroupBox.Name = "VmrcGroupBox";
            this.VmrcGroupBox.Size = new System.Drawing.Size(590, 365);
            this.VmrcGroupBox.TabIndex = 1;
            this.VmrcGroupBox.TabStop = false;
            // 
            // VMRCReducedColorsCheckbox
            // 
            this.VMRCReducedColorsCheckbox.AutoSize = true;
            this.VMRCReducedColorsCheckbox.Location = new System.Drawing.Point(6, 50);
            this.VMRCReducedColorsCheckbox.Name = "VMRCReducedColorsCheckbox";
            this.VMRCReducedColorsCheckbox.Size = new System.Drawing.Size(102, 17);
            this.VMRCReducedColorsCheckbox.TabIndex = 3;
            this.VMRCReducedColorsCheckbox.Text = "Reduced Colors";
            this.VMRCReducedColorsCheckbox.UseVisualStyleBackColor = true;
            // 
            // VMRCAdminModeCheckbox
            // 
            this.VMRCAdminModeCheckbox.AutoSize = true;
            this.VMRCAdminModeCheckbox.Location = new System.Drawing.Point(6, 23);
            this.VMRCAdminModeCheckbox.Name = "VMRCAdminModeCheckbox";
            this.VMRCAdminModeCheckbox.Size = new System.Drawing.Size(116, 17);
            this.VMRCAdminModeCheckbox.TabIndex = 2;
            this.VMRCAdminModeCheckbox.Text = "Administrator Mode";
            this.VMRCAdminModeCheckbox.UseVisualStyleBackColor = true;
            // 
            // VmrcControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VmrcGroupBox);
            this.Name = "VmrcControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.VmrcGroupBox.ResumeLayout(false);
            this.VmrcGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox VmrcGroupBox;
        private System.Windows.Forms.CheckBox VMRCReducedColorsCheckbox;
        private System.Windows.Forms.CheckBox VMRCAdminModeCheckbox;
    }
}
