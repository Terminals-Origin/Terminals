namespace Terminals.Forms.EditFavorite
{
    partial class VncControl
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
            this.VncViewOnlyCheckbox = new System.Windows.Forms.CheckBox();
            this.vncDisplayNumberInput = new System.Windows.Forms.NumericUpDown();
            this.label37 = new System.Windows.Forms.Label();
            this.vncAutoScaleCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.vncDisplayNumberInput)).BeginInit();
            this.SuspendLayout();
            // 
            // VncViewOnlyCheckbox
            // 
            this.VncViewOnlyCheckbox.AutoSize = true;
            this.VncViewOnlyCheckbox.Location = new System.Drawing.Point(6, 50);
            this.VncViewOnlyCheckbox.Name = "VncViewOnlyCheckbox";
            this.VncViewOnlyCheckbox.Size = new System.Drawing.Size(73, 17);
            this.VncViewOnlyCheckbox.TabIndex = 9;
            this.VncViewOnlyCheckbox.Text = "View Only";
            this.VncViewOnlyCheckbox.UseVisualStyleBackColor = true;
            // 
            // vncDisplayNumberInput
            // 
            this.vncDisplayNumberInput.Location = new System.Drawing.Point(115, 79);
            this.vncDisplayNumberInput.Name = "vncDisplayNumberInput";
            this.vncDisplayNumberInput.Size = new System.Drawing.Size(47, 20);
            this.vncDisplayNumberInput.TabIndex = 8;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(6, 81);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(84, 13);
            this.label37.TabIndex = 7;
            this.label37.Text = "Display Number:";
            // 
            // vncAutoScaleCheckbox
            // 
            this.vncAutoScaleCheckbox.AutoSize = true;
            this.vncAutoScaleCheckbox.Location = new System.Drawing.Point(6, 23);
            this.vncAutoScaleCheckbox.Name = "vncAutoScaleCheckbox";
            this.vncAutoScaleCheckbox.Size = new System.Drawing.Size(121, 17);
            this.vncAutoScaleCheckbox.TabIndex = 6;
            this.vncAutoScaleCheckbox.Text = "Auto Scale Desktop";
            this.vncAutoScaleCheckbox.UseVisualStyleBackColor = true;
            // 
            // VncControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VncViewOnlyCheckbox);
            this.Controls.Add(this.vncDisplayNumberInput);
            this.Controls.Add(this.label37);
            this.Controls.Add(this.vncAutoScaleCheckbox);
            this.Name = "VncControl";
            this.Size = new System.Drawing.Size(590, 365);
            ((System.ComponentModel.ISupportInitialize)(this.vncDisplayNumberInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox VncViewOnlyCheckbox;
        private System.Windows.Forms.NumericUpDown vncDisplayNumberInput;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.CheckBox vncAutoScaleCheckbox;
    }
}
