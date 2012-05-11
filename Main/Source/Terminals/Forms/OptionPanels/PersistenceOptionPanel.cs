using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data.DB;

namespace Terminals.Forms
{
    internal partial class PersistenceOptionPanel : UserControl, IOptionPanel
    {
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
        }

        private void OnBntTestSqlConnectionClick(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            string connectionString = this.txtConnectionString.Text;
            string databasePassword = this.txtDbPassword.Text;
            Tuple<bool, string> result = Database.TestConnection(connectionString, databasePassword);
            this.Cursor = Cursors.Default;

            const string messageHeader = "Terminals - Sql connection test";
            if (result.Item1)
                MessageBox.Show("Test connection succeded.", messageHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(result.Item2, messageHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
