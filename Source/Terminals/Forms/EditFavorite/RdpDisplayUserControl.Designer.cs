namespace Terminals.Forms.EditFavorite
{
    partial class RdpDisplayUserControl
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
            this.chkConnectToConsole = new System.Windows.Forms.CheckBox();
            this.DisplaySettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.AllowDesktopCompositionCheckbox = new System.Windows.Forms.CheckBox();
            this.AllowFontSmoothingCheckbox = new System.Windows.Forms.CheckBox();
            this.customSizePanel = new System.Windows.Forms.Panel();
            this.widthUpDown = new System.Windows.Forms.NumericUpDown();
            this.heightUpDown = new System.Windows.Forms.NumericUpDown();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.chkDisableWallpaper = new System.Windows.Forms.CheckBox();
            this.chkDisableThemes = new System.Windows.Forms.CheckBox();
            this.chkDisableMenuAnimations = new System.Windows.Forms.CheckBox();
            this.chkDisableFullWindowDrag = new System.Windows.Forms.CheckBox();
            this.chkDisableCursorBlinking = new System.Windows.Forms.CheckBox();
            this.chkDisableCursorShadow = new System.Windows.Forms.CheckBox();
            this.cmbColors = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbResolution = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.DisplaySettingsGroupBox.SuspendLayout();
            this.customSizePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // chkConnectToConsole
            // 
            this.chkConnectToConsole.AutoSize = true;
            this.chkConnectToConsole.Location = new System.Drawing.Point(3, 211);
            this.chkConnectToConsole.Name = "chkConnectToConsole";
            this.chkConnectToConsole.Size = new System.Drawing.Size(119, 17);
            this.chkConnectToConsole.TabIndex = 20;
            this.chkConnectToConsole.Text = "Co&nnect to Console";
            this.chkConnectToConsole.UseVisualStyleBackColor = true;
            // 
            // DisplaySettingsGroupBox
            // 
            this.DisplaySettingsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisplaySettingsGroupBox.Controls.Add(this.AllowDesktopCompositionCheckbox);
            this.DisplaySettingsGroupBox.Controls.Add(this.AllowFontSmoothingCheckbox);
            this.DisplaySettingsGroupBox.Controls.Add(this.customSizePanel);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableWallpaper);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableThemes);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableMenuAnimations);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableFullWindowDrag);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableCursorBlinking);
            this.DisplaySettingsGroupBox.Controls.Add(this.chkDisableCursorShadow);
            this.DisplaySettingsGroupBox.Controls.Add(this.cmbColors);
            this.DisplaySettingsGroupBox.Controls.Add(this.label7);
            this.DisplaySettingsGroupBox.Controls.Add(this.cmbResolution);
            this.DisplaySettingsGroupBox.Controls.Add(this.label6);
            this.DisplaySettingsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.DisplaySettingsGroupBox.Name = "DisplaySettingsGroupBox";
            this.DisplaySettingsGroupBox.Size = new System.Drawing.Size(570, 206);
            this.DisplaySettingsGroupBox.TabIndex = 19;
            this.DisplaySettingsGroupBox.TabStop = false;
            // 
            // AllowDesktopCompositionCheckbox
            // 
            this.AllowDesktopCompositionCheckbox.AutoSize = true;
            this.AllowDesktopCompositionCheckbox.Location = new System.Drawing.Point(297, 182);
            this.AllowDesktopCompositionCheckbox.Name = "AllowDesktopCompositionCheckbox";
            this.AllowDesktopCompositionCheckbox.Size = new System.Drawing.Size(154, 17);
            this.AllowDesktopCompositionCheckbox.TabIndex = 16;
            this.AllowDesktopCompositionCheckbox.Text = "Allow Desktop Composition";
            this.AllowDesktopCompositionCheckbox.UseVisualStyleBackColor = true;
            // 
            // AllowFontSmoothingCheckbox
            // 
            this.AllowFontSmoothingCheckbox.AutoSize = true;
            this.AllowFontSmoothingCheckbox.Location = new System.Drawing.Point(297, 162);
            this.AllowFontSmoothingCheckbox.Name = "AllowFontSmoothingCheckbox";
            this.AllowFontSmoothingCheckbox.Size = new System.Drawing.Size(128, 17);
            this.AllowFontSmoothingCheckbox.TabIndex = 15;
            this.AllowFontSmoothingCheckbox.Text = "Allow Font Smoothing";
            this.AllowFontSmoothingCheckbox.UseVisualStyleBackColor = true;
            // 
            // customSizePanel
            // 
            this.customSizePanel.Controls.Add(this.widthUpDown);
            this.customSizePanel.Controls.Add(this.heightUpDown);
            this.customSizePanel.Controls.Add(this.label31);
            this.customSizePanel.Controls.Add(this.label32);
            this.customSizePanel.Location = new System.Drawing.Point(297, 14);
            this.customSizePanel.Name = "customSizePanel";
            this.customSizePanel.Size = new System.Drawing.Size(150, 73);
            this.customSizePanel.TabIndex = 14;
            this.customSizePanel.Visible = false;
            // 
            // widthUpDown
            // 
            this.widthUpDown.Location = new System.Drawing.Point(65, 4);
            this.widthUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.widthUpDown.Name = "widthUpDown";
            this.widthUpDown.Size = new System.Drawing.Size(72, 20);
            this.widthUpDown.TabIndex = 11;
            this.widthUpDown.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
            // 
            // heightUpDown
            // 
            this.heightUpDown.Location = new System.Drawing.Point(65, 37);
            this.heightUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.heightUpDown.Name = "heightUpDown";
            this.heightUpDown.Size = new System.Drawing.Size(72, 20);
            this.heightUpDown.TabIndex = 13;
            this.heightUpDown.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(9, 6);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(38, 13);
            this.label31.TabIndex = 10;
            this.label31.Text = "Width:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(9, 39);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(41, 13);
            this.label32.TabIndex = 12;
            this.label32.Text = "Height:";
            // 
            // chkDisableWallpaper
            // 
            this.chkDisableWallpaper.AutoSize = true;
            this.chkDisableWallpaper.Location = new System.Drawing.Point(102, 182);
            this.chkDisableWallpaper.Name = "chkDisableWallpaper";
            this.chkDisableWallpaper.Size = new System.Drawing.Size(155, 17);
            this.chkDisableWallpaper.TabIndex = 9;
            this.chkDisableWallpaper.Text = "Disable Desktop Wallpaper";
            this.chkDisableWallpaper.UseVisualStyleBackColor = true;
            // 
            // chkDisableThemes
            // 
            this.chkDisableThemes.AutoSize = true;
            this.chkDisableThemes.Location = new System.Drawing.Point(102, 162);
            this.chkDisableThemes.Name = "chkDisableThemes";
            this.chkDisableThemes.Size = new System.Drawing.Size(105, 17);
            this.chkDisableThemes.TabIndex = 8;
            this.chkDisableThemes.Text = "Disable Theming";
            this.chkDisableThemes.UseVisualStyleBackColor = true;
            // 
            // chkDisableMenuAnimations
            // 
            this.chkDisableMenuAnimations.AutoSize = true;
            this.chkDisableMenuAnimations.Location = new System.Drawing.Point(102, 142);
            this.chkDisableMenuAnimations.Name = "chkDisableMenuAnimations";
            this.chkDisableMenuAnimations.Size = new System.Drawing.Size(145, 17);
            this.chkDisableMenuAnimations.TabIndex = 7;
            this.chkDisableMenuAnimations.Text = "Disable Menu Animations";
            this.chkDisableMenuAnimations.UseVisualStyleBackColor = true;
            // 
            // chkDisableFullWindowDrag
            // 
            this.chkDisableFullWindowDrag.AutoSize = true;
            this.chkDisableFullWindowDrag.Location = new System.Drawing.Point(102, 122);
            this.chkDisableFullWindowDrag.Name = "chkDisableFullWindowDrag";
            this.chkDisableFullWindowDrag.Size = new System.Drawing.Size(146, 17);
            this.chkDisableFullWindowDrag.TabIndex = 6;
            this.chkDisableFullWindowDrag.Text = "Disable Full-Window drag";
            this.chkDisableFullWindowDrag.UseVisualStyleBackColor = true;
            // 
            // chkDisableCursorBlinking
            // 
            this.chkDisableCursorBlinking.AutoSize = true;
            this.chkDisableCursorBlinking.Location = new System.Drawing.Point(102, 102);
            this.chkDisableCursorBlinking.Name = "chkDisableCursorBlinking";
            this.chkDisableCursorBlinking.Size = new System.Drawing.Size(134, 17);
            this.chkDisableCursorBlinking.TabIndex = 5;
            this.chkDisableCursorBlinking.Text = "Disable Cursor Blinking";
            this.chkDisableCursorBlinking.UseVisualStyleBackColor = true;
            // 
            // chkDisableCursorShadow
            // 
            this.chkDisableCursorShadow.AutoSize = true;
            this.chkDisableCursorShadow.Location = new System.Drawing.Point(102, 82);
            this.chkDisableCursorShadow.Name = "chkDisableCursorShadow";
            this.chkDisableCursorShadow.Size = new System.Drawing.Size(136, 17);
            this.chkDisableCursorShadow.TabIndex = 4;
            this.chkDisableCursorShadow.Text = "Disable Cursor Shadow";
            this.chkDisableCursorShadow.UseVisualStyleBackColor = true;
            // 
            // cmbColors
            // 
            this.cmbColors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColors.FormattingEnabled = true;
            this.cmbColors.Items.AddRange(new object[] {
            "256 Colors",
            "High Color (16 Bit)",
            "True Color (24 Bit)",
            "Highest Quality (32 Bit)"});
            this.cmbColors.Location = new System.Drawing.Point(102, 51);
            this.cmbColors.Name = "cmbColors";
            this.cmbColors.Size = new System.Drawing.Size(171, 21);
            this.cmbColors.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Co&lors:";
            // 
            // cmbResolution
            // 
            this.cmbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResolution.FormattingEnabled = true;
            this.cmbResolution.Items.AddRange(new object[] {
            "640x480",
            "800x600",
            "1024x768",
            "1152x864",
            "1280x1024",
            "Fit to Window",
            "Full Screen",
            "Auto Scale",
            "Custom"});
            this.cmbResolution.Location = new System.Drawing.Point(102, 17);
            this.cmbResolution.Name = "cmbResolution";
            this.cmbResolution.Size = new System.Drawing.Size(171, 21);
            this.cmbResolution.TabIndex = 1;
            this.cmbResolution.SelectedIndexChanged += new System.EventHandler(this.CmbResolution_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "&Desktop size:";
            // 
            // RdpDisplayUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkConnectToConsole);
            this.Controls.Add(this.DisplaySettingsGroupBox);
            this.Name = "RdpDisplayUserControl";
            this.Size = new System.Drawing.Size(590, 365);
            this.DisplaySettingsGroupBox.ResumeLayout(false);
            this.DisplaySettingsGroupBox.PerformLayout();
            this.customSizePanel.ResumeLayout(false);
            this.customSizePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.widthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkConnectToConsole;
        private System.Windows.Forms.GroupBox DisplaySettingsGroupBox;
        private System.Windows.Forms.CheckBox AllowDesktopCompositionCheckbox;
        private System.Windows.Forms.CheckBox AllowFontSmoothingCheckbox;
        private System.Windows.Forms.Panel customSizePanel;
        private System.Windows.Forms.NumericUpDown widthUpDown;
        private System.Windows.Forms.NumericUpDown heightUpDown;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.CheckBox chkDisableWallpaper;
        private System.Windows.Forms.CheckBox chkDisableThemes;
        private System.Windows.Forms.CheckBox chkDisableMenuAnimations;
        private System.Windows.Forms.CheckBox chkDisableFullWindowDrag;
        private System.Windows.Forms.CheckBox chkDisableCursorBlinking;
        private System.Windows.Forms.CheckBox chkDisableCursorShadow;
        private System.Windows.Forms.ComboBox cmbColors;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbResolution;
        private System.Windows.Forms.Label label6;
    }
}
