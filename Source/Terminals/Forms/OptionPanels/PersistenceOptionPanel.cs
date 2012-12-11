using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data.DB;
using System.Collections.Generic;

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

        private bool filePersistence = Settings.PersistenceType == 0;

        private void ActivateCheckBoxes()
        {
            // we have to check both, because when false, the user control only unchecks
            this.rbtnSqlPersistence.Checked = !filePersistence;
            this.rbtnFilePersistence.Checked = filePersistence;
        }

        private void FillSqlUserControls()
        {
            if (String.IsNullOrEmpty(Settings.ConnectionString))
                this.ConnectionString = Database.DEFAULT_CONNECTION_STRING;
            else
                this.ConnectionString = Settings.ConnectionString;

            //if (!string.IsNullOrEmpty(Settings.DatabaseMasterPassword))
            //    this.txtDbPassword.Text = NewTerminalForm.HIDDEN_PASSWORD;
        }

        public void SaveSettings()
        {
            
            Settings.PersistenceType = Convert.ToByte(this.rbtnSqlPersistence.Checked);
            if (this.rbtnSqlPersistence.Checked)
            {
                Settings.ConnectionString = ConnectionString;
                //if (this.txtDbPassword.Text != NewTerminalForm.HIDDEN_PASSWORD)
                //    Settings.DatabaseMasterPassword = this.txtDbPassword.Text;
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

        private string ConnectionString
        {
            get
            {
                System.Data.SqlClient.SqlConnectionStringBuilder b = new SqlConnectionStringBuilder();
                b.DataSource = serversComboBox.Text;
                b.InitialCatalog = DatabaseCombobox.Text;

                if (string.IsNullOrEmpty(b.InitialCatalog))
                    b.InitialCatalog = "master";

                b.IntegratedSecurity = !IsSqlServerAuth;
                if (!b.IntegratedSecurity)
                {
                    b.UserID = SqlServerUserNameTextBox.Text;
                    b.Password = SqlServerPasswordTextBox.Text;
                }
                return b.ToString();
            }
            set
            {
                System.Data.SqlClient.SqlConnectionStringBuilder b = new SqlConnectionStringBuilder(value);
                serversComboBox.Text = b.DataSource;
                DatabaseCombobox.Text = b.InitialCatalog;
                IsSqlServerAuth = !b.IntegratedSecurity;
                if (!b.IntegratedSecurity)
                {
                    SqlServerUserNameTextBox.Text = b.UserID;
                    SqlServerPasswordTextBox.Text = b.Password;
                }

            }
        }

        private void OnBntTestSqlConnectionClick(object sender, EventArgs e)
        {


            this.Cursor = Cursors.WaitCursor;
            this.TestLabel.Visible = true;
            var version = SqlScriptRunner.Versioning.Version.Min;
            var cstr = this.ConnectionString;
            System.Exception exc= null;
            Task t = Task.Factory.StartNew(() =>
                {
                    ////string databasePassword = this.txtDbPassword.Text;
                    try
                    {
                        version = Database.DatabaseVersion(cstr, null);
                    }
                    catch (Exception exception)
                    {
                        exc = exception;
                    }
                });
            t.ContinueWith((antecedant) =>
                {


                    const string messageHeader = "Terminals - Sql connection test";
                    if (version != SqlScriptRunner.Versioning.Version.Min && exc != null)
                    {
                        MessageBox.Show("Test connection succeded. (Version:" + version.ToString() + ")", messageHeader,
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string msg = "Test database failed.";
                        if (exc != null)
                            msg += "\nReason:" + exc.Message;
                        else
                        {
                            msg += "\nReason:The specified database does include a versions table. Change the name of the database and click 'Create New' to create a new database on the server, or just hit 'Create New' to deploy into this existing database.";
                        }
                        MessageBox.Show(msg, messageHeader, MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }

                    this.Cursor = Cursors.Default;
                    this.TestLabel.Visible = false;
                }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        //private void OnBtnSetPasswordClick(object sender, EventArgs e)
        //{
        //    Tuple<bool, string, string> passwordPrompt = AskForDatabasePasswords();
        //    if (!passwordPrompt.Item1)
        //        return;

        //    this.Cursor = Cursors.WaitCursor;
        //    DatabasePasswordUpdate.UpdateMastrerPassord(this.EnteredConnectionString, passwordPrompt.Item2, passwordPrompt.Item3);
        //    this.Cursor = Cursors.Default;
        //}

        //private Tuple<bool, string, string> AskForDatabasePasswords()
        //{
        //    using (var passwordPrompt = new ChangePassword())
        //    {
        //        if (passwordPrompt.ShowDialog() == DialogResult.OK)
        //            return new Tuple<bool, string, string>(true, passwordPrompt.OldPassword, passwordPrompt.NewPassword);
        //    }

        //    return new Tuple<bool, string, string>(false, string.Empty, string.Empty);
        //}

        private void SqlServerAuthenticationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlServerAuthPanel.Enabled = IsSqlServerAuth;
        }

        public bool IsSqlServerAuth
        {
            get { return (SqlServerAuthenticationComboBox.Text == "SQL Server Authentication"); }
            set
            {
                if (value)
                    SqlServerAuthenticationComboBox.SelectedIndex = 0;
                else
                {
                    SqlServerAuthenticationComboBox.SelectedIndex = 1;
                }
            }
        }


        private void rbtnSqlPersistence_CheckedChanged(object sender, EventArgs e)
        {
            SqlServerOptionsPanel.Enabled = rbtnSqlPersistence.Checked;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            QueryLabel.Visible = true;
            Cursor = Cursors.WaitCursor;
            System.Data.DataTable instances = null;
            Task t = Task.Factory.StartNew(() =>
                {
                    instances = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();

                });

            t.ContinueWith((antecedant) =>
                {
                    if (instances != null)
                    {
                        serversComboBox.Items.Clear();
                        foreach (System.Data.DataRow row in instances.Rows)
                        {
                            var serverName = row["ServerName"].ToString();
                            var instanceName = row["InstanceName"].ToString();
                            var isClustered = row["IsClustered"].ToString();
                            var version = row["Version"].ToString();

                            string display = serverName;
                            if (!string.IsNullOrEmpty(instanceName))
                                display = display + "\\" + instanceName;
                            if (!string.IsNullOrEmpty(version))
                                display = display + " (" + version + ")";

                            serversComboBox.Items.Add(display);
                        }
                        Cursor = Cursors.Default;
                        QueryLabel.Visible = false;
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TableQueryLabel.Visible = true;
            Cursor = Cursors.WaitCursor;
            List<string> dbs = null;
            var cstr = this.ConnectionString;
            Task t = Task.Factory.StartNew(() =>
                {
                    dbs = Database.Databases(cstr);

                });

            t.ContinueWith((antecedant) =>
            {
                if (dbs != null)
                {
                    serversComboBox.Items.Clear();
                    DatabaseCombobox.Items.AddRange(dbs.ToArray());
                    Cursor = Cursors.Default;
                    TableQueryLabel.Visible = false;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

    }
}