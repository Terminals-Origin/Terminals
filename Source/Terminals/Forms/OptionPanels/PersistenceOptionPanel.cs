using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data.DB;
using System.Collections.Generic;
using Versioning = SqlScriptRunner.Versioning;

namespace Terminals.Forms
{
    internal partial class PersistenceOptionPanel : UserControl, IOptionPanel
    {
        private readonly bool filePersistence = Settings.PersistenceType == 0;

        private string ConnectionString
        {
            get
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder();
                connectionStringBuilder.DataSource = this.serversComboBox.Text;
                connectionStringBuilder.InitialCatalog = this.databaseCombobox.Text;

                if (string.IsNullOrEmpty(connectionStringBuilder.InitialCatalog))
                    connectionStringBuilder.InitialCatalog = "master";

                connectionStringBuilder.IntegratedSecurity = !this.IsSqlServerAuth;
                if (!connectionStringBuilder.IntegratedSecurity)
                {
                    connectionStringBuilder.UserID = this.SqlServerUserNameTextBox.Text;
                    connectionStringBuilder.Password = this.SqlServerPasswordTextBox.Text;
                }
                return connectionStringBuilder.ToString();
            }
            set
            {
                var connectionStringBuilder = new SqlConnectionStringBuilder(value);
                this.serversComboBox.Text = connectionStringBuilder.DataSource;
                this.databaseCombobox.Text = connectionStringBuilder.InitialCatalog;
                this.IsSqlServerAuth = !connectionStringBuilder.IntegratedSecurity;
                if (!connectionStringBuilder.IntegratedSecurity)
                {
                    this.SqlServerUserNameTextBox.Text = connectionStringBuilder.UserID;
                    this.SqlServerPasswordTextBox.Text = connectionStringBuilder.Password;
                }
            }
        }

        private bool IsSqlServerAuth
        {
            get
            {
                return this.SqlServerAuthenticationComboBox.Text == "SQL Server Authentication";
            }
            set
            {
                if (value)
                    this.SqlServerAuthenticationComboBox.SelectedIndex = 0;
                else
                {
                    this.SqlServerAuthenticationComboBox.SelectedIndex = 1;
                }
            }
        }

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
        }

        public void SaveSettings()
        {
            Settings.PersistenceType = Convert.ToByte(this.rbtnSqlPersistence.Checked);
            if (this.rbtnSqlPersistence.Checked)
            {
                Settings.ConnectionString = ConnectionString;
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
            SqlServerOptionsPanel.Enabled = rbtnSqlPersistence.Checked;
            //this.txtConnectionString.Enabled = enableSqlControls;
            this.bntTestSqlConnection.Enabled = enableSqlControls;
            //this.txtDbPassword.Enabled = enableSqlControls;
            this.bntTestSqlConnection.Enabled = enableSqlControls;
        }

        private void OnBntTestSqlConnectionClick(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.TestLabel.Visible = true;
            var version = Versioning.Version.Min;
            string connectionString = this.ConnectionString;
            Exception exc= null;

            Task t = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        version = Database.DatabaseVersion(connectionString);
                    }
                    catch (Exception exception)
                    {
                        exc = exception;
                    }
                });

            t.ContinueWith((antecedant) =>
                {
                    ShowConnectionTestResult(version, exc);
                    this.Cursor = Cursors.Default;
                    this.TestLabel.Visible = false;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static void ShowConnectionTestResult(Versioning.Version version, Exception exc)
        {
            const string messageHeader = "Terminals - Sql connection test";
            if (exc != null &&  version != Versioning.Version.Min)
            {
                string message = string.Format("Test connection succeeded. (Version: {0})", version);
                MessageBox.Show(message, messageHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string message = "Test database failed.\r\nReason:";
                if (exc != null)
                {
                    message += exc.Message;
                }
                else
                {
                    message += "The specified database does include a versions table. " +
                               "Change the name of the database and click 'Create New' to create a new database on the server, " +
                               "or just hit 'Create New' to deploy into this existing database.";
                }
                MessageBox.Show(message, messageHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SqlServerAuthenticationComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            SqlServerAuthPanel.Enabled = IsSqlServerAuth;
        }

        private void RbtnSqlPersistenceCheckedChanged(object sender, EventArgs e)
        {
            SqlServerOptionsPanel.Enabled = rbtnSqlPersistence.Checked;
        }

        private void SearchButtonClick(object sender, EventArgs e)
        {
            QueryLabel.Visible = true;
            Cursor = Cursors.WaitCursor;
            DataTable instances = null;
            Task t = Task.Factory.StartNew(() => instances = SqlDataSourceEnumerator.Instance.GetDataSources());

            t.ContinueWith((antecedant) =>
                {
                    this.FillServersComboboxItems(instances);
                    Cursor = Cursors.Default;
                    QueryLabel.Visible = false;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void FillServersComboboxItems(DataTable instances)
        {
            if (instances != null)
            {
                this.serversComboBox.Items.Clear();
                foreach (DataRow row in instances.Rows)
                {
                    string serverName = row["ServerName"].ToString();
                    string instanceName = row["InstanceName"].ToString();
                    string version = row["Version"].ToString();
                    string display = FormatSqlInstanceDisplayName(serverName, instanceName, version);
                    this.serversComboBox.Items.Add(display);
                }
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

        private void ButtonDatabasesClick(object sender, EventArgs e)
        {
            TableQueryLabel.Visible = true;
            Cursor = Cursors.WaitCursor;
            List<string> dbs = null;
            string connectionString = this.ConnectionString;
            Task t = Task.Factory.StartNew(() => dbs = Database.Databases(connectionString));

            t.ContinueWith((antecedant) =>
            {
                if (dbs != null)
                {
                    this.databaseCombobox.Items.Clear();
                    this.databaseCombobox.Items.AddRange(dbs.ToArray());
                    Cursor = Cursors.Default;
                    TableQueryLabel.Visible = false;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}