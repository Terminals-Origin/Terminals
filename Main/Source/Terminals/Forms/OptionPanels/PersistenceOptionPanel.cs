using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.DB;
using System.Collections.Generic;
using Terminals.Forms.Controls;
using Terminals.Forms.EditFavorite;

namespace Terminals.Forms
{
    internal partial class PersistenceOptionPanel : UserControl, IOptionPanel
    {
        /// <summary>
        /// don't depend only on the user control properties.
        /// Using this instance we are able to update all connections string properties,
        /// because some of them are edited only in "advanced properties" window.
        /// </summary>
        private SqlConnectionStringBuilder connectionStringBuilder;

        private string ConnectionString
        {
            get
            {
                this.FillSqlConnectionStringBuilder();
                return this.connectionStringBuilder.ToString();
            }
            set
            {
                var newConnectionStringBuilder = new SqlConnectionStringBuilder(value);
                this.FillSqlControlsFromConnecitonBuilder(newConnectionStringBuilder);
            }
        }

        private bool IsSqlServerAuth
        {
            get
            {
                return this.sqlServerAuthenticationComboBox.Text == "SQL Server Authentication";
            }
            set
            {
                if (value)
                    this.sqlServerAuthenticationComboBox.SelectedIndex = 1;
                else
                    this.sqlServerAuthenticationComboBox.SelectedIndex = 0;
            }
        }

        private const string MESSAGE_HEADER = "Terminals - Sql connection test";

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
            // we have to check both, because when false, the user control only uncheck
            bool filePersistence = Settings.PersistenceType == FilePersistence.TYPE_ID;
            this.rbtnSqlPersistence.Checked = !filePersistence;
            this.rbtnFilePersistence.Checked = filePersistence;
        }

        private void FillSqlUserControls()
        {
            if (String.IsNullOrEmpty(Settings.ConnectionString))
                this.ConnectionString = DatabaseConnections.DEFAULT_CONNECTION_STRING;
            else
                this.ConnectionString = Settings.ConnectionString;

            if (!string.IsNullOrEmpty(Settings.DatabaseMasterPassword))
                this.txtDbPassword.Text = CredentialsPanel.HIDDEN_PASSWORD;
        }

        public void SaveSettings()
        {
            if (this.rbtnSqlPersistence.Checked)
            {
                Settings.PersistenceType = SqlPersistence.TYPE_ID;
                Settings.ConnectionString = this.ConnectionString;
                if (this.txtDbPassword.Text != CredentialsPanel.HIDDEN_PASSWORD)
                    Settings.DatabaseMasterPassword = this.txtDbPassword.Text;
            }
            else
            {
                Settings.PersistenceType = FilePersistence.TYPE_ID;
                Settings.ConnectionString = string.Empty;
                Settings.DatabaseMasterPassword = string.Empty;
            }
        }

        private void OnRbtnFilePersistenceCheckedChanged(object sender, EventArgs e)
        {
            bool enableSqlControls = !this.rbtnFilePersistence.Checked;
            this.sqlServerOptionsPanel.Enabled = rbtnSqlPersistence.Checked;
            this.btnTestSqlConnection.Enabled = enableSqlControls;
            this.txtDbPassword.Enabled = enableSqlControls;
        }

        private void OnBntTestSqlConnectionClick(object sender, EventArgs e)
        {
            this.testLabel.Visible = true;
            string databasePassword = this.GetFilledDatabasePassword();
            var connectionProperties = new Tuple<string, string>(this.ConnectionString, databasePassword);
            var t = Task<DatabaseValidationResult>.Factory.StartNew(TryTestDatabaseConnection, connectionProperties);
            t.ContinueWith(this.ShowConnectionTestResult, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private string GetFilledDatabasePassword()
        {
            if (this.txtDbPassword.Text != CredentialsPanel.HIDDEN_PASSWORD)
                return this.txtDbPassword.Text;

            return Settings.DatabaseMasterPassword;
        }

        private static DatabaseValidationResult TryTestDatabaseConnection(object objectState)
        {
            var connectionParams = objectState as Tuple<string, string>;
            return DatabaseConnections.ValidateDatabaseConnection(connectionParams.Item1, connectionParams.Item2);
        }

        private void ShowConnectionTestResult(Task<DatabaseValidationResult> antecedent)
        {
            DatabaseValidationResult connectionResult = antecedent.Result;

            if (connectionResult.SuccessfulWithVersion)
            {
                string message = string.Format("Test connection succeeded.");
                // todo enable database versioning
                // string message = string.Format("Test connection succeeded. (Version: {0})", connectionResult.CurrentVersion);
                MessageBox.Show(message, MESSAGE_HEADER, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ShowFailedConnectionTestMessage(connectionResult);
            }

            this.testLabel.Visible = false;
        }

        private static void ShowFailedConnectionTestMessage(DatabaseValidationResult connectionResult)
        {
            string message = string.Format("Test database failed.\r\nReason:{0}", connectionResult.ErroMessage);
            // todo enable database versioning
            //if (connectionResult.IsMinimalVersion)
            //{
            //    message += "\r\n\r\nThe specified database does include a versions table.\r\n" +
            //               "Change the name of the database and click 'Create New' to create a new database on the server, " +
            //               "or just hit 'Create New' to deploy into this existing database.";
            //}
            MessageBox.Show(message, MESSAGE_HEADER, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void SqlServerAuthenticationComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            sqlServerAuthPanel.Enabled = IsSqlServerAuth;
        }

        private void RbtnSqlPersistenceCheckedChanged(object sender, EventArgs e)
        {
            sqlServerOptionsPanel.Enabled = rbtnSqlPersistence.Checked;
        }

        private void SearchServersButtonClick(object sender, EventArgs e)
        {
            this.queryLabel.Visible = true;
            var searcher = new ServerInstancesSearcher();
            var t = searcher.FindSqlServerInstancesAsync();

            t.ContinueWith((antecedent) =>
                {
                    this.FillServersComboboxItems(antecedent.Result);
                    this.queryLabel.Visible = false;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void FillServersComboboxItems(List<string> availableServers)
        {
            this.serversComboBox.Items.Clear();
            foreach (string instance in availableServers)
            {
                this.serversComboBox.Items.Add(instance);
            }
        }

        private void ButtonFindDatabasesClick(object sender, EventArgs e)
        {
            tableQueryLabel.Visible = true;
            string connectionString = FillNewGeneralConnectionString();
            var t = Task<List<string>>.Factory.StartNew((cs) => DatabaseConnections.FindDatabasesOnServer(cs.ToString()), connectionString);
            t.ContinueWith(this.FinishDatabasesReload, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void FinishDatabasesReload(Task<List<string>> antecedent)
        {
            object[] databases = antecedent.Result.Cast<object>().ToArray();
            this.databaseCombobox.Items.Clear();
            this.databaseCombobox.Items.AddRange(databases);
            tableQueryLabel.Visible = false;
        }

        private void ButtonAdvancedClick(object sender, EventArgs e)
        {
            using (var advancedForm = new SqlConnectionForm())
            {
                this.FillSqlConnectionStringBuilder();
                // create copy to be able cancel the edit
                var currentConnectionCopy = new SqlConnectionStringBuilder(this.connectionStringBuilder.ToString());
                advancedForm.DataSource = currentConnectionCopy;
                if (advancedForm.ShowDialog() == DialogResult.OK)
                {
                  this.FillSqlControlsFromConnecitonBuilder(currentConnectionCopy);
                }
            }
        }

        private void FillSqlConnectionStringBuilder()
        {
            if (this.connectionStringBuilder == null)
                this.connectionStringBuilder = new SqlConnectionStringBuilder();

            this.FillConnectionStringBuilder(this.connectionStringBuilder);
            this.connectionStringBuilder.InitialCatalog = this.databaseCombobox.Text;
            if (!string.IsNullOrEmpty(this.connectionStringBuilder.InitialCatalog))
              this.connectionStringBuilder.AttachDBFilename = string.Empty;
        }

        private void FillConnectionStringBuilder(SqlConnectionStringBuilder connectionBuilder)
        {
            connectionBuilder.IntegratedSecurity = !this.IsSqlServerAuth;
            if (!connectionBuilder.IntegratedSecurity)
            {
                connectionBuilder.UserID = this.sqlServerUserNameTextBox.Text;
                connectionBuilder.Password = this.sqlServerPasswordTextBox.Text;
            }

            connectionBuilder.DataSource = this.serversComboBox.Text;
        }

        private string FillNewGeneralConnectionString()
        {
            var connectionBuilder = new SqlConnectionStringBuilder(this.connectionStringBuilder.ToString());
            // fill as much as from the current, because advanced properties may be set
            this.FillConnectionStringBuilder(connectionBuilder);
            connectionBuilder.InitialCatalog = string.Empty;
            connectionBuilder.AttachDBFilename = string.Empty;
            return connectionBuilder.ToString();
        }

        private void FillSqlControlsFromConnecitonBuilder(SqlConnectionStringBuilder connectionStringBuilder)
        {
            this.connectionStringBuilder = connectionStringBuilder;
            this.serversComboBox.Text = connectionStringBuilder.DataSource;
            
            this.IsSqlServerAuth = !connectionStringBuilder.IntegratedSecurity;
            if (!connectionStringBuilder.IntegratedSecurity)
            {
                this.sqlServerUserNameTextBox.Text = connectionStringBuilder.UserID;
                this.sqlServerPasswordTextBox.Text = connectionStringBuilder.Password;
            }

            // advanced properties override possible inconsistent values.
            if (!string.IsNullOrEmpty(connectionStringBuilder.AttachDBFilename))
                connectionStringBuilder.InitialCatalog = string.Empty;
            
            this.databaseCombobox.Text = connectionStringBuilder.InitialCatalog;
        }

        private void OnBtnSetPasswordClick(object sender, EventArgs e)
        {
            Tuple<bool, string, string> passwordPrompt = AskForDatabasePasswords();
            if (!passwordPrompt.Item1)
                return;

            this.Cursor = Cursors.WaitCursor;
            var connectionProperties = new Tuple<string, string, string>(this.ConnectionString, passwordPrompt.Item2, passwordPrompt.Item3);
            Task<TestConnectionResult> t = Task<TestConnectionResult>.Factory.StartNew(TrySetNewDatabasePassword, connectionProperties);
            t.ContinueWith(this.ShowPasswordSetResult, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private TestConnectionResult TrySetNewDatabasePassword(object state)
        {
            var connectionProperties = state as Tuple<string, string, string>;
            return DatabasePasswordUpdate.UpdateMastrerPassord(connectionProperties.Item1,
                connectionProperties.Item2, connectionProperties.Item3);
        }

        private void ShowPasswordSetResult(Task<TestConnectionResult> task)
        {
            const string header = "Set new database password";
            var result = task.Result;
            if (result.Successful)
            {
                MessageBox.Show("Password was applied successfuly.", header, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string messsage = string.Format("There was an error, when setting new password:\r\n{0}", result.ErroMessage);
                MessageBox.Show(messsage, header, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void ButtonCreateNewDatabaseClick(object sender, EventArgs e)
        {
            // todo create new database, when configuring connection
        }
    }
}