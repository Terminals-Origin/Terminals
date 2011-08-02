using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal class InterfaceOptionPanel : OptionDialogCategoryPanel
    {
        private Panel panel1;
        private GroupBox groupBox10;
        private CheckBox chkEnableGroupsMenu;
        private GroupBox groupBox1;
        private RadioButton RenderBlackRadio;
        private RadioButton RenderBlueRadio;
        private RadioButton RenderNormalRadio;
        private GroupBox groupBox7;
        private CheckBox chkMinimizeToTrayCheckbox;
        private GroupBox groupBoxInformation;
        private CheckBox chkShowUserNameInTitle;
        private CheckBox chkShowInformationToolTips;
        private CheckBox chkShowFullInfo;

        public InterfaceOptionPanel()
        {
            InitializeComponent();
        }

        #region InitializeComponent
        
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.chkEnableGroupsMenu = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RenderBlackRadio = new System.Windows.Forms.RadioButton();
            this.RenderBlueRadio = new System.Windows.Forms.RadioButton();
            this.RenderNormalRadio = new System.Windows.Forms.RadioButton();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.chkMinimizeToTrayCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBoxInformation = new System.Windows.Forms.GroupBox();
            this.chkShowUserNameInTitle = new System.Windows.Forms.CheckBox();
            this.chkShowInformationToolTips = new System.Windows.Forms.CheckBox();
            this.chkShowFullInfo = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBoxInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox10);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox7);
            this.panel1.Controls.Add(this.groupBoxInformation);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(519, 331);
            this.panel1.TabIndex = 2;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.chkEnableGroupsMenu);
            this.groupBox10.Location = new System.Drawing.Point(6, 146);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(500, 44);
            this.groupBox10.TabIndex = 26;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Groups";
            // 
            // EnableGroupsMenu
            // 
            this.chkEnableGroupsMenu.AutoSize = true;
            this.chkEnableGroupsMenu.Checked = true;
            this.chkEnableGroupsMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableGroupsMenu.Location = new System.Drawing.Point(6, 20);
            this.chkEnableGroupsMenu.Name = "EnableGroupsMenu";
            this.chkEnableGroupsMenu.Size = new System.Drawing.Size(126, 17);
            this.chkEnableGroupsMenu.TabIndex = 23;
            this.chkEnableGroupsMenu.Text = "Enable Groups Menu";
            this.chkEnableGroupsMenu.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RenderBlackRadio);
            this.groupBox1.Controls.Add(this.RenderBlueRadio);
            this.groupBox1.Controls.Add(this.RenderNormalRadio);
            this.groupBox1.Location = new System.Drawing.Point(6, 196);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 90);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Theme";
            // 
            // RenderBlackRadio
            // 
            this.RenderBlackRadio.AutoSize = true;
            this.RenderBlackRadio.Location = new System.Drawing.Point(6, 66);
            this.RenderBlackRadio.Name = "RenderBlackRadio";
            this.RenderBlackRadio.Size = new System.Drawing.Size(83, 17);
            this.RenderBlackRadio.TabIndex = 2;
            this.RenderBlackRadio.TabStop = true;
            this.RenderBlackRadio.Text = "Office Black";
            this.RenderBlackRadio.UseVisualStyleBackColor = true;
            // 
            // RenderBlueRadio
            // 
            this.RenderBlueRadio.AutoSize = true;
            this.RenderBlueRadio.Location = new System.Drawing.Point(6, 43);
            this.RenderBlueRadio.Name = "RenderBlueRadio";
            this.RenderBlueRadio.Size = new System.Drawing.Size(77, 17);
            this.RenderBlueRadio.TabIndex = 1;
            this.RenderBlueRadio.TabStop = true;
            this.RenderBlueRadio.Text = "Office Blue";
            this.RenderBlueRadio.UseVisualStyleBackColor = true;
            // 
            // RenderNormalRadio
            // 
            this.RenderNormalRadio.AutoSize = true;
            this.RenderNormalRadio.Location = new System.Drawing.Point(6, 20);
            this.RenderNormalRadio.Name = "RenderNormalRadio";
            this.RenderNormalRadio.Size = new System.Drawing.Size(58, 17);
            this.RenderNormalRadio.TabIndex = 0;
            this.RenderNormalRadio.TabStop = true;
            this.RenderNormalRadio.Text = "Normal";
            this.RenderNormalRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.chkMinimizeToTrayCheckbox);
            this.groupBox7.Location = new System.Drawing.Point(6, 96);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(500, 44);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "System Tray";
            // 
            // MinimizeToTrayCheckbox
            // 
            this.chkMinimizeToTrayCheckbox.AutoSize = true;
            this.chkMinimizeToTrayCheckbox.Checked = true;
            this.chkMinimizeToTrayCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMinimizeToTrayCheckbox.Location = new System.Drawing.Point(6, 20);
            this.chkMinimizeToTrayCheckbox.Name = "MinimizeToTrayCheckbox";
            this.chkMinimizeToTrayCheckbox.Size = new System.Drawing.Size(106, 17);
            this.chkMinimizeToTrayCheckbox.TabIndex = 17;
            this.chkMinimizeToTrayCheckbox.Text = "Minimize To Tray";
            this.chkMinimizeToTrayCheckbox.UseVisualStyleBackColor = true;
            // 
            // groupBoxInformation
            // 
            this.groupBoxInformation.Controls.Add(this.chkShowUserNameInTitle);
            this.groupBoxInformation.Controls.Add(this.chkShowInformationToolTips);
            this.groupBoxInformation.Controls.Add(this.chkShowFullInfo);
            this.groupBoxInformation.Location = new System.Drawing.Point(6, 3);
            this.groupBoxInformation.Name = "groupBoxInformation";
            this.groupBoxInformation.Size = new System.Drawing.Size(500, 87);
            this.groupBoxInformation.TabIndex = 0;
            this.groupBoxInformation.TabStop = false;
            this.groupBoxInformation.Text = "Information";
            // 
            // chkShowUserNameInTitle
            // 
            this.chkShowUserNameInTitle.AutoSize = true;
            this.chkShowUserNameInTitle.Checked = true;
            this.chkShowUserNameInTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowUserNameInTitle.Location = new System.Drawing.Point(6, 20);
            this.chkShowUserNameInTitle.Name = "chkShowUserNameInTitle";
            this.chkShowUserNameInTitle.Size = new System.Drawing.Size(156, 17);
            this.chkShowUserNameInTitle.TabIndex = 0;
            this.chkShowUserNameInTitle.Text = "Show  user name in tab title";
            this.chkShowUserNameInTitle.UseVisualStyleBackColor = true;
            // 
            // chkShowInformationToolTips
            // 
            this.chkShowInformationToolTips.AutoSize = true;
            this.chkShowInformationToolTips.Checked = true;
            this.chkShowInformationToolTips.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowInformationToolTips.Location = new System.Drawing.Point(6, 43);
            this.chkShowInformationToolTips.Name = "chkShowInformationToolTips";
            this.chkShowInformationToolTips.Size = new System.Drawing.Size(213, 17);
            this.chkShowInformationToolTips.TabIndex = 1;
            this.chkShowInformationToolTips.Text = "Show connection information in tool tips";
            this.chkShowInformationToolTips.UseVisualStyleBackColor = true;
            this.chkShowInformationToolTips.CheckedChanged += new EventHandler(chkShowInformationToolTips_CheckedChanged);
            // 
            // chkShowFullInfo
            // 
            this.chkShowFullInfo.AutoSize = true;
            this.chkShowFullInfo.Location = new System.Drawing.Point(26, 66);
            this.chkShowFullInfo.Name = "chkShowFullInfo";
            this.chkShowFullInfo.Size = new System.Drawing.Size(123, 17);
            this.chkShowFullInfo.TabIndex = 2;
            this.chkShowFullInfo.Text = "Show full information";
            this.chkShowFullInfo.UseVisualStyleBackColor = true;
            // 
            // InterfacePanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "InterfacePanel";
            this.Size = new System.Drawing.Size(519, 331);
            this.panel1.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBoxInformation.ResumeLayout(false);
            this.groupBoxInformation.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        public override void Init()
        {
            this.chkEnableGroupsMenu.Checked = Settings.EnableGroupsMenu;
            this.chkMinimizeToTrayCheckbox.Checked = Settings.MinimizeToTray;
            this.chkShowUserNameInTitle.Checked = Settings.ShowUserNameInTitle;
            this.chkShowInformationToolTips.Checked = Settings.ShowInformationToolTips;
            this.chkShowFullInfo.Checked = Settings.ShowFullInformationToolTips;

            if (Settings.Office2007BlueFeel)
                this.RenderBlueRadio.Checked = true;
            else if (Settings.Office2007BlackFeel)
                this.RenderBlackRadio.Checked = true;
            else
                this.RenderNormalRadio.Checked = true;
        }

        public override Boolean Save()
        {
            try
            {
                Settings.DelayConfigurationSave = true;

                Settings.EnableGroupsMenu = this.chkEnableGroupsMenu.Checked;
                Settings.MinimizeToTray = this.chkMinimizeToTrayCheckbox.Checked;
                Settings.ShowUserNameInTitle = this.chkShowUserNameInTitle.Checked;
                Settings.ShowInformationToolTips = this.chkShowInformationToolTips.Checked;
                Settings.ShowFullInformationToolTips = this.chkShowFullInfo.Checked;

                Settings.Office2007BlackFeel = false;
                Settings.Office2007BlueFeel = false;

                if (this.RenderBlueRadio.Checked)
                    Settings.Office2007BlueFeel = true;
                else if (this.RenderBlackRadio.Checked)
                    Settings.Office2007BlackFeel = true;

                return true;
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Error(ex);
                return false;
            }
        }

        private void chkShowInformationToolTips_CheckedChanged(object sender, EventArgs e)
        {
            this.chkShowFullInfo.Enabled = this.chkShowInformationToolTips.Checked;
            if (!this.chkShowInformationToolTips.Checked)
            {
                this.chkShowFullInfo.Checked = false;
            }
        }
    }
}
