using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data.DB;

namespace Terminals.Forms
{
    internal partial class PersistenceOptionPanel : UserControl, IOptionPanel
    {
        private string EnteredConnectionString { get { return this.txtConnectionString.Text; } }

        public PersistenceOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.ActivateCheckBoxes();
            this.FillSqlUserControls();
        }

        private void ActivateCheckBoxes()
        {
            bool filePersistence = Settings.PersistenceType == 0;
            // we have to check both, because when false, the user control only unchecks
            this.rbtnSqlPersistence.Checked = !filePersistence;
            this.rbtnFilePersistence.Checked = filePersistence;
        }

        private void FillSqlUserControls()
        {
            if (String.IsNullOrEmpty(Settings.ConnectionString))
                this.txtConnectionString.Text = Database.DEFAULT_CONNECTION_STRING;
            else
                this.txtConnectionString.Text = Settings.ConnectionString;

            if (!string.IsNullOrEmpty(Settings.DatabaseMasterPassword))
                this.txtDbPassword.Text = NewTerminalForm.HIDDEN_PASSWORD;
        }

        public void SaveSettings()
        {
            Settings.PersistenceType = Convert.ToByte(this.rbtnSqlPersistence.Checked);
            if (this.txtConnectionString.Enabled)
            {
                Settings.ConnectionString = this.txtConnectionString.Text;
                if (this.txtDbPassword.Text != NewTerminalForm.HIDDEN_PASSWORD)
                    Settings.DatabaseMasterPassword = this.txtDbPassword.Text;
            }
            else
            {
                Settings.ConnectionString = string.Empty;
                Settings.DatabaseMasterPassword = string.Empty;
            }
        }

        private void OnRbtnFilePersistenceCheckedChanged(object sender, EventArgs e)
        {
            bool enableSqlControls = !this.rbtnFilePersistence.Checked;
            this.txtConnectionString.Enabled = enableSqlControls;
            this.bntTestSqlConnection.Enabled = enableSqlControls;
            this.txtDbPassword.Enabled = enableSqlControls;
            this.bntTestSqlConnection.Enabled = enableSqlControls;
        }

        private void OnBntTestSqlConnectionClick(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string databasePassword = this.txtDbPassword.Text;
            Tuple<bool, string> result = Database.TestConnection(this.EnteredConnectionString, databasePassword);
            this.Cursor = Cursors.Default;

            const string messageHeader = "Terminals - Sql connection test";
            if (result.Item1)
                MessageBox.Show("Test connection succeded.", messageHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(result.Item2, messageHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void OnBtnSetPasswordClick(object sender, EventArgs e)
        {
            Tuple<bool, string, string> passwordPrompt = AskForDatabasePasswords();
            if (!passwordPrompt.Item1)
                return;

            this.Cursor = Cursors.WaitCursor;
            DatabasePasswordUpdate.UpdateMastrerPassord(this.EnteredConnectionString, passwordPrompt.Item2, passwordPrompt.Item3);
            this.Cursor = Cursors.Default;
        }

        private Tuple<bool, string, string> AskForDatabasePasswords()
        {
            using (var passwordPrompt = new ChangePassword())
            {
                if (passwordPrompt.ShowDialog() == DialogResult.OK)
                    return new Tuple<bool, string, string>(true, passwordPrompt.OldPassword, passwordPrompt.NewPassword);
            }

            return new Tuple<bool, string, string>(false, string.Empty, string.Empty);
        }
    }
}
