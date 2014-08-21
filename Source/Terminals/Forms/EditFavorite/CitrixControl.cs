using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    internal partial class CitrixControl : UserControl, IProtocolOptionsControl
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

        public void SaveTo(IFavorite favorite)
        {
            var icaOptions = favorite.ProtocolProperties as ICAOptions;
            if (icaOptions == null)
                return;

            icaOptions.ClientINI = this.ICAClientINI.Text;
            icaOptions.ServerINI = this.ICAServerINI.Text;
            icaOptions.EncryptionLevel = this.ICAEncryptionLevelCombobox.Text;
            icaOptions.EnableEncryption = this.ICAEnableEncryptionCheckbox.Checked;

            icaOptions.ApplicationName = this.ICAApplicationNameTextBox.Text;
            icaOptions.ApplicationPath = this.ICAApplicationPath.Text;
            icaOptions.ApplicationWorkingFolder = this.ICAWorkingFolder.Text;
        }

        public void LoadFrom(IFavorite favorite)
        {
            var icaOptions = favorite.ProtocolProperties as ICAOptions;
            if (icaOptions == null)
                return;

            this.ICAClientINI.Text = icaOptions.ClientINI;
            this.ICAServerINI.Text = icaOptions.ServerINI;
            this.ICAEncryptionLevelCombobox.Text = icaOptions.EncryptionLevel;
            this.ICAEnableEncryptionCheckbox.Checked = icaOptions.EnableEncryption;
            this.ICAEncryptionLevelCombobox.Enabled = icaOptions.EnableEncryption;

            this.ICAApplicationNameTextBox.Text = icaOptions.ApplicationName;
            this.ICAApplicationPath.Text = icaOptions.ApplicationPath;
            this.ICAWorkingFolder.Text = icaOptions.ApplicationWorkingFolder;
        }

    }
}
