using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal class CaptureOptionPanel : OptionDialogCategoryPanel
    {
        private Panel panel1;
        private GroupBox groupBox8;
        private CheckBox chkAutoSwitchToCaptureCheckbox;
        private CheckBox chkEnableCaptureToFolder;
        private CheckBox chkEnableCaptureToClipboard;
        private Button ButtonBrowseCaptureFolder;
        private Label label23;
        private TextBox txtScreenCaptureFolder;

        public CaptureOptionPanel()
        {
            InitializeComponent();
        }

        #region InitializeComponent
        
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.chkAutoSwitchToCaptureCheckbox = new System.Windows.Forms.CheckBox();
            this.chkEnableCaptureToFolder = new System.Windows.Forms.CheckBox();
            this.chkEnableCaptureToClipboard = new System.Windows.Forms.CheckBox();
            this.ButtonBrowseCaptureFolder = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.txtScreenCaptureFolder = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 333);
            this.panel1.TabIndex = 26;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.chkAutoSwitchToCaptureCheckbox);
            this.groupBox8.Controls.Add(this.chkEnableCaptureToFolder);
            this.groupBox8.Controls.Add(this.chkEnableCaptureToClipboard);
            this.groupBox8.Controls.Add(this.ButtonBrowseCaptureFolder);
            this.groupBox8.Controls.Add(this.label23);
            this.groupBox8.Controls.Add(this.txtScreenCaptureFolder);
            this.groupBox8.Location = new System.Drawing.Point(6, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(500, 138);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            // 
            // chkAutoSwitchToCaptureCheckbox
            // 
            this.chkAutoSwitchToCaptureCheckbox.AutoSize = true;
            this.chkAutoSwitchToCaptureCheckbox.Checked = true;
            this.chkAutoSwitchToCaptureCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoSwitchToCaptureCheckbox.Location = new System.Drawing.Point(26, 66);
            this.chkAutoSwitchToCaptureCheckbox.Name = "chkAutoSwitchToCaptureCheckbox";
            this.chkAutoSwitchToCaptureCheckbox.Size = new System.Drawing.Size(191, 17);
            this.chkAutoSwitchToCaptureCheckbox.TabIndex = 27;
            this.chkAutoSwitchToCaptureCheckbox.Text = "Auto switch to manager on capture";
            this.chkAutoSwitchToCaptureCheckbox.UseVisualStyleBackColor = true;
            // 
            // chkEnableCaptureToFolder
            // 
            this.chkEnableCaptureToFolder.AutoSize = true;
            this.chkEnableCaptureToFolder.Location = new System.Drawing.Point(6, 43);
            this.chkEnableCaptureToFolder.Name = "chkEnableCaptureToFolder";
            this.chkEnableCaptureToFolder.Size = new System.Drawing.Size(174, 17);
            this.chkEnableCaptureToFolder.TabIndex = 26;
            this.chkEnableCaptureToFolder.Text = "Enable screen capture to folder";
            this.chkEnableCaptureToFolder.UseVisualStyleBackColor = true;
            this.chkEnableCaptureToFolder.CheckedChanged += new EventHandler(chkEnableCaptureToFolder_CheckedChanged);
            // 
            // chkEnableCaptureToClipboard
            // 
            this.chkEnableCaptureToClipboard.AutoSize = true;
            this.chkEnableCaptureToClipboard.Checked = true;
            this.chkEnableCaptureToClipboard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableCaptureToClipboard.Location = new System.Drawing.Point(6, 20);
            this.chkEnableCaptureToClipboard.Name = "chkEnableCaptureToClipboard";
            this.chkEnableCaptureToClipboard.Size = new System.Drawing.Size(191, 17);
            this.chkEnableCaptureToClipboard.TabIndex = 25;
            this.chkEnableCaptureToClipboard.Text = "Enable screen capture to clipboard";
            this.chkEnableCaptureToClipboard.UseVisualStyleBackColor = true;
            // 
            // ButtonBrowseCaptureFolder
            // 
            this.ButtonBrowseCaptureFolder.Location = new System.Drawing.Point(383, 105);
            this.ButtonBrowseCaptureFolder.Name = "ButtonBrowseCaptureFolder";
            this.ButtonBrowseCaptureFolder.Size = new System.Drawing.Size(65, 23);
            this.ButtonBrowseCaptureFolder.TabIndex = 24;
            this.ButtonBrowseCaptureFolder.Text = "Browse...";
            this.ButtonBrowseCaptureFolder.UseVisualStyleBackColor = true;
            this.ButtonBrowseCaptureFolder.Click += new EventHandler(ButtonBrowseCaptureFolder_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 91);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(133, 13);
            this.label23.TabIndex = 23;
            this.label23.Text = "Screen capture root folder:";
            // 
            // txtScreenCaptureFolder
            // 
            this.txtScreenCaptureFolder.Location = new System.Drawing.Point(6, 107);
            this.txtScreenCaptureFolder.Name = "txtScreenCaptureFolder";
            this.txtScreenCaptureFolder.Size = new System.Drawing.Size(371, 20);
            this.txtScreenCaptureFolder.TabIndex = 22;
            // 
            // CaptureOptionPanel
            // 
            this.Controls.Add(this.panel1);
            this.Name = "CaptureOptionPanel";
            this.Size = new System.Drawing.Size(513, 333);
            this.panel1.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        public override void Init()
        {
            this.chkAutoSwitchToCaptureCheckbox.Enabled = this.chkEnableCaptureToFolder.Checked;
            this.chkEnableCaptureToClipboard.Checked = Settings.EnableCaptureToClipboard;
            this.chkEnableCaptureToFolder.Checked = Settings.EnableCaptureToFolder;

            this.txtScreenCaptureFolder.Text = Settings.CaptureRoot;
            this.txtScreenCaptureFolder.SelectionStart = this.txtScreenCaptureFolder.Text.Length;
            this.txtScreenCaptureFolder.Enabled = this.chkEnableCaptureToFolder.Checked;
            this.ButtonBrowseCaptureFolder.Enabled = this.chkEnableCaptureToFolder.Checked;
        }

        public override Boolean Save()
        {
            try
            {
                Settings.DelayConfigurationSave = true;

                Settings.AutoSwitchOnCapture = this.chkAutoSwitchToCaptureCheckbox.Checked;
                Settings.EnableCaptureToClipboard = this.chkEnableCaptureToClipboard.Checked;
                Settings.EnableCaptureToFolder = this.chkEnableCaptureToFolder.Checked;
                Settings.CaptureRoot = this.txtScreenCaptureFolder.Text;

                return true;
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Error(ex);
                return false;
            }
        }

        private void ButtonBrowseCaptureFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select the screen capture folder";
                dlg.RootFolder = Environment.SpecialFolder.MyComputer;
                String currentFld = this.txtScreenCaptureFolder.Text;
                if (!currentFld.Equals(String.Empty))
                    currentFld = (currentFld.EndsWith("\\")) ? currentFld : currentFld + "\\";

                dlg.SelectedPath = (currentFld.Equals(String.Empty)) ?
                    Environment.GetFolderPath(dlg.RootFolder) : System.IO.Path.GetDirectoryName(currentFld);

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    String selectedFld = dlg.SelectedPath;
                    if (!selectedFld.Equals(String.Empty))
                        selectedFld = (selectedFld.EndsWith("\\")) ? selectedFld : selectedFld + "\\";

                    this.txtScreenCaptureFolder.Text = selectedFld;
                    this.txtScreenCaptureFolder.SelectionStart = this.txtScreenCaptureFolder.Text.Length;
                }
            }
        }

        private void chkEnableCaptureToFolder_CheckedChanged(object sender, EventArgs e)
        {
            this.chkAutoSwitchToCaptureCheckbox.Enabled = this.chkEnableCaptureToFolder.Checked;
            this.txtScreenCaptureFolder.Enabled = this.chkEnableCaptureToFolder.Checked;
            this.ButtonBrowseCaptureFolder.Enabled = this.chkEnableCaptureToFolder.Checked;
        }
    }
}
