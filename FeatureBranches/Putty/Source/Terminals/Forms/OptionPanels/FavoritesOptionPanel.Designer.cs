using System.Windows.Forms;

namespace Terminals.Forms
{
    partial class FavoritesOptionPanel
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.FavSortGroupBox = new System.Windows.Forms.GroupBox();
            this.NoneRadioButton = new System.Windows.Forms.RadioButton();
            this.ProtocolRadionButton = new System.Windows.Forms.RadioButton();
            this.ConnectionNameRadioButton = new System.Windows.Forms.RadioButton();
            this.ServerNameRadio = new System.Windows.Forms.RadioButton();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.chkAutoCaseTags = new System.Windows.Forms.CheckBox();
            this.chkAutoExapandTagsPanel = new System.Windows.Forms.CheckBox();
            this.chkEnableFavoritesPanel = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.FavSortGroupBox.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.FavSortGroupBox);
            this.panel1.Controls.Add(this.groupBox11);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 335);
            this.panel1.TabIndex = 2;
            // 
            // FavSortGroupBox
            // 
            this.FavSortGroupBox.Controls.Add(this.NoneRadioButton);
            this.FavSortGroupBox.Controls.Add(this.ProtocolRadionButton);
            this.FavSortGroupBox.Controls.Add(this.ConnectionNameRadioButton);
            this.FavSortGroupBox.Controls.Add(this.ServerNameRadio);
            this.FavSortGroupBox.Location = new System.Drawing.Point(6, 97);
            this.FavSortGroupBox.Name = "FavSortGroupBox";
            this.FavSortGroupBox.Size = new System.Drawing.Size(500, 114);
            this.FavSortGroupBox.TabIndex = 26;
            this.FavSortGroupBox.TabStop = false;
            this.FavSortGroupBox.Text = "Favorites Sort";
            // 
            // NoneRadioButton
            // 
            this.NoneRadioButton.AutoSize = true;
            this.NoneRadioButton.Location = new System.Drawing.Point(7, 89);
            this.NoneRadioButton.Name = "NoneRadioButton";
            this.NoneRadioButton.Size = new System.Drawing.Size(51, 17);
            this.NoneRadioButton.TabIndex = 3;
            this.NoneRadioButton.TabStop = true;
            this.NoneRadioButton.Text = "None";
            this.NoneRadioButton.UseVisualStyleBackColor = true;
            // 
            // ProtocolRadionButton
            // 
            this.ProtocolRadionButton.AutoSize = true;
            this.ProtocolRadionButton.Location = new System.Drawing.Point(6, 66);
            this.ProtocolRadionButton.Name = "ProtocolRadionButton";
            this.ProtocolRadionButton.Size = new System.Drawing.Size(64, 17);
            this.ProtocolRadionButton.TabIndex = 2;
            this.ProtocolRadionButton.TabStop = true;
            this.ProtocolRadionButton.Text = "Protocol";
            this.ProtocolRadionButton.UseVisualStyleBackColor = true;
            // 
            // ConnectionNameRadioButton
            // 
            this.ConnectionNameRadioButton.AutoSize = true;
            this.ConnectionNameRadioButton.Location = new System.Drawing.Point(6, 43);
            this.ConnectionNameRadioButton.Name = "ConnectionNameRadioButton";
            this.ConnectionNameRadioButton.Size = new System.Drawing.Size(110, 17);
            this.ConnectionNameRadioButton.TabIndex = 1;
            this.ConnectionNameRadioButton.TabStop = true;
            this.ConnectionNameRadioButton.Text = "Connection Name";
            this.ConnectionNameRadioButton.UseVisualStyleBackColor = true;
            // 
            // ServerNameRadio
            // 
            this.ServerNameRadio.AutoSize = true;
            this.ServerNameRadio.Location = new System.Drawing.Point(7, 20);
            this.ServerNameRadio.Name = "ServerNameRadio";
            this.ServerNameRadio.Size = new System.Drawing.Size(87, 17);
            this.ServerNameRadio.TabIndex = 0;
            this.ServerNameRadio.TabStop = true;
            this.ServerNameRadio.Text = "Server Name";
            this.ServerNameRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.chkAutoCaseTags);
            this.groupBox11.Controls.Add(this.chkAutoExapandTagsPanel);
            this.groupBox11.Controls.Add(this.chkEnableFavoritesPanel);
            this.groupBox11.Location = new System.Drawing.Point(6, 3);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(500, 88);
            this.groupBox11.TabIndex = 0;
            this.groupBox11.TabStop = false;
            // 
            // autoCaseTagsCheckbox
            // 
            this.chkAutoCaseTags.AutoSize = true;
            this.chkAutoCaseTags.Location = new System.Drawing.Point(6, 66);
            this.chkAutoCaseTags.Name = "autoCaseTagsCheckbox";
            this.chkAutoCaseTags.Size = new System.Drawing.Size(143, 17);
            this.chkAutoCaseTags.TabIndex = 23;
            this.chkAutoCaseTags.Text = "Auto-Case Favorite Groups";
            this.chkAutoCaseTags.UseVisualStyleBackColor = true;
            // 
            // AutoExapandTagsPanelCheckBox
            // 
            this.chkAutoExapandTagsPanel.AutoSize = true;
            this.chkAutoExapandTagsPanel.Location = new System.Drawing.Point(6, 43);
            this.chkAutoExapandTagsPanel.Name = "AutoExapandTagsPanelCheckBox";
            this.chkAutoExapandTagsPanel.Size = new System.Drawing.Size(133, 17);
            this.chkAutoExapandTagsPanel.TabIndex = 27;
            this.chkAutoExapandTagsPanel.Text = "Auto Expand (show) Favorites Panel, when hidden";
            this.chkAutoExapandTagsPanel.UseVisualStyleBackColor = true;
            // 
            // EnableFavoritesPanel
            // 
            this.chkEnableFavoritesPanel.AutoSize = true;
            this.chkEnableFavoritesPanel.Checked = true;
            this.chkEnableFavoritesPanel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableFavoritesPanel.Location = new System.Drawing.Point(6, 20);
            this.chkEnableFavoritesPanel.Name = "EnableFavoritesPanel";
            this.chkEnableFavoritesPanel.Size = new System.Drawing.Size(135, 17);
            this.chkEnableFavoritesPanel.TabIndex = 24;
            this.chkEnableFavoritesPanel.Text = "Enable Favorites Panel";
            this.chkEnableFavoritesPanel.UseVisualStyleBackColor = true;
            // 
            // FavoritesOptionPanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "FavoritesOptionPanel";
            this.Size = new System.Drawing.Size(519, 335);
            this.panel1.ResumeLayout(false);
            this.FavSortGroupBox.ResumeLayout(false);
            this.FavSortGroupBox.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private GroupBox FavSortGroupBox;
        private RadioButton NoneRadioButton;
        private RadioButton ProtocolRadionButton;
        private RadioButton ConnectionNameRadioButton;
        private RadioButton ServerNameRadio;
        private GroupBox groupBox11;
        private CheckBox chkAutoCaseTags;
        private CheckBox chkAutoExapandTagsPanel;
        private CheckBox chkEnableFavoritesPanel;
    }
}
