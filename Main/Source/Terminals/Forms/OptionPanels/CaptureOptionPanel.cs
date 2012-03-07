using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class CaptureOptionPanel : UserControl, IOptionPanel
    {
        public CaptureOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.chkEnableCaptureToClipboard.Checked = Settings.EnableCaptureToClipboard;
            this.chkEnableCaptureToFolder.Checked = Settings.EnableCaptureToFolder;
            this.chkAutoSwitchToCaptureCheckbox.Enabled = Settings.AutoSwitchOnCapture;
            this.txtScreenCaptureFolder.Text = Settings.CaptureRoot;

            this.txtScreenCaptureFolder.SelectionStart = this.txtScreenCaptureFolder.Text.Length;
            UpdateCaptureToFolderControls();
        }

        public void SaveSettings()
        {
                Settings.AutoSwitchOnCapture = this.chkAutoSwitchToCaptureCheckbox.Checked;
                Settings.EnableCaptureToClipboard = this.chkEnableCaptureToClipboard.Checked;
                Settings.EnableCaptureToFolder = this.chkEnableCaptureToFolder.Checked;
                Settings.CaptureRoot = this.txtScreenCaptureFolder.Text;
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

                if (dlg.ShowDialog() == DialogResult.OK)
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
            UpdateCaptureToFolderControls();
        }

        private void UpdateCaptureToFolderControls()
        {
            this.chkAutoSwitchToCaptureCheckbox.Enabled = this.chkEnableCaptureToFolder.Checked;
            this.txtScreenCaptureFolder.Enabled = this.chkEnableCaptureToFolder.Checked;
            this.ButtonBrowseCaptureFolder.Enabled = this.chkEnableCaptureToFolder.Checked;
        }
    }
}
