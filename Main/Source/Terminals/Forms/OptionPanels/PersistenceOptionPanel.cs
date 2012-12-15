using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data.DB;
using System.Collections.Generic;

namespace Terminals.Forms
{
    internal partial class PersistenceOptionPanel : UserControl, IOptionPanel
    {
        private readonly bool filePersistence = Settings.PersistenceType == 0;

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
            this.rbtnSqlPersistence.Checked = !filePersistence;
            this.rbtnFilePersistence.Checked = filePersistence;
        }

        private void FillSqlUserControls()
        {
            if (String.IsNullOrEmpty(Settings.ConnectionString))
                this.ConnectionString = Database.DEFAULT_CONNECTION_STRING;
            else
                this.ConnectionString = Settings.ConnectionString;

            if (!string.IsNullOrEmpty(Settings.DatabaseMasterPassword))
                this.txtDbPassword.Text = NewTerminalForm.HIDDEN_PASSWORD;
        }

        public void SaveSettings()
        {
            Settings.PersistenceType = Convert.ToByte(this.rbtnSqlPersistence.Checked);
            if (this.rbtnSqlPersistence.Checked)
            {
                Settings.ConnectionString = ConnectionString;
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
            this.sqlServerOptionsPanel.Enabled = rbtnSqlPersistence.Checked;
            this.btnTestSqlConnection.Enabled = enableSqlControls;
            this.txtDbPassword.Enabled = enableSqlControls;
        }

        private void OnBntTestSqlConnectionClick(object sender, EventArgs e)
        {
            this.testLabel.Visible = true;
            var connectionProperties = new Tuple<string, string>(this.ConnectionString, this.txtDbPassword.Text);
            var t = Task<DatabaseValidataionResult>.Factory.StartNew(TryTestDatabaseConnection, connectionProperties);
            t.ContinueWith(this.ShowConnectionTestResult, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static DatabaseValidataionResult TryTestDatabaseConnection(object objectState)
        {
            var connectionParams = objectState as Tuple<string, string>;
            return Database.ValidateDatabaseConnection(connectionParams.Item1, connectionParams.Item2);
        }

        private void ShowConnectionTestResult(Task<DatabaseValidataionResult> antecedent)
        {
            DatabaseValidataionResult connectionResult = antecedent.Result;

            if (connectionResult.SuccessfulWithVersion)
            {
                string message = string.Format("Test connection succeeded. (Version: {0})", connectionResult.CurrentVersion);
                MessageBox.Show(message, MESSAGE_HEADER, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                ShowFailedConnectionTestMessage(connectionResult);
            }

            this.testLabel.Visible = false;
        }

        private static void ShowFailedConnectionTestMessage(DatabaseValidataionResult connectionResult)
        {
            string message = string.Format("Test database failed.\r\nReason:{0}", connectionResult.ErroMessage);
            if (connectionResult.IsMinimalVersion)
            {
                message += "\r\n\r\nThe specified database does include a versions table.\r\n" +
                           "Change the name of the database and click 'Create New' to create a new database on the server, " +
                           "or just hit 'Create New' to deploy into this existing database.";
            }
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
            var t = Task<DataTable>.Factory.StartNew(() => SqlDataSourceEnumerator.Instance.GetDataSources());

            t.ContinueWith((antecedent) =>
                {
                    this.FillServersComboboxItems(antecedent.Result);
                    this.queryLabel.Visible = false;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void FillServersComboboxItems(DataTable availableServers)
        {
            if (availableServers == null)
                return;

            this.serversComboBox.Items.Clear();
            foreach (DataRow row in availableServers.Rows)
            {
                string serverName = row["ServerName"].ToString();
                string instanceName = row["InstanceName"].ToString();
                string version = row["Version"].ToString();
                string display = FormatSqlInstanceDisplayName(serverName, instanceName, version);
                this.serversComboBox.Items.Add(display);
            }
        }

        private static string FormatSqlInstanceDisplayName(string serverName, string instanceName, string version)
        {
            string display = serverName;
            if (!string.IsNullOrEmpty(instanceName))
            {
                display = display + "\\" + instanceName;
            }
            if (!string.IsNullOrEmpty(version))
            {
                display = display + " (" + version + ")";
            }
            return display;
        }

        private void ButtonFindDatabasesClick(object sender, EventArgs e)
        {
            tableQueryLabel.Visible = true;
            string connectionString = FillNewGeneralConnectionString();
            var t = Task<List<string>>.Factory.StartNew((cs) => Database.FindDatabasesOnServer(cs.ToString()), connectionString);
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
            this.databaseCombobox.Text = connectionStringBuilder.InitialCatalog;
            this.IsSqlServerAuth = !connectionStringBuilder.IntegratedSecurity;
            if (!connectionStringBuilder.IntegratedSecurity)
            {
                this.sqlServerUserNameTextBox.Text = connectionStringBuilder.UserID;
                this.sqlServerPasswordTextBox.Text = connectionStringBuilder.Password;
            }
        }

        private void OnBtnSetPasswordClick(object sender, EventArgs e)
        {
            Tuple<bool, string, string> passwordPrompt = AskForDatabasePasswords();
            if (!passwordPrompt.Item1)
                return;

            this.Cursor = Cursors.WaitCursor;
            DatabasePasswordUpdate.UpdateMastrerPassord(this.ConnectionString, passwordPrompt.Item2, passwordPrompt.Item3);
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