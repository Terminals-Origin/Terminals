using System;
using System.Windows.Forms;

namespace Terminals.Forms.EditFavorite
{
    public partial class CitrixControl : UserControl
    {
        public CitrixControl()
        {
            InitializeComponent();
        }

        private void ICAEnableEncryptionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            this.ICAEncryptionLevelCombobox.Enabled = this.ICAEnableEncryptionCheckbox.Checked;
        }

        private void AppPathBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();

            DialogResult result = d.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ICAApplicationPath.Text = d.SelectedPath;
            }
        }

        private void AppWorkingFolderBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();

            DialogResult result = d.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ICAWorkingFolder.Text = d.SelectedPath;
            }
        }

        private void ServerINIBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.DefaultExt = "*.ini";
            d.CheckFileExists = true;
            DialogResult result = d.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ICAServerINI.Text = d.FileName;
            }
        }

        private void ClientINIBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.DefaultExt = "*.ini";
            d.CheckFileExists = true;

            DialogResult result = d.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                ICAClientINI.Text = d.FileName;
            }
        }
    }
}
