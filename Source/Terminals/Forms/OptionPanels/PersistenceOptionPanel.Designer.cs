using System.Windows.Forms;

namespace Terminals.Forms
{
    partial class PersistenceOptionPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainPanel = new System.Windows.Forms.Panel();
            this.groupBoxPersistenceType = new System.Windows.Forms.GroupBox();
            this.sqlServerOptionsPanel = new System.Windows.Forms.Panel();
            this.lblDbMasterPassword = new System.Windows.Forms.Label();
            this.txtDbPassword = new System.Windows.Forms.TextBox();
            this.btnSetDatabasePassword = new System.Windows.Forms.Button();
            this.btnAdvanced = new System.Windows.Forms.Button();
            this.databaseCombobox = new System.Windows.Forms.ComboBox();
            this.queryLabel = new System.Windows.Forms.Label();
            this.tableQueryLabel = new System.Windows.Forms.Label();
            this.serversComboBox = new System.Windows.Forms.ComboBox();
            this.btnDatabases = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.testLabel = new System.Windows.Forms.Label();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.btnCreateNew = new System.Windows.Forms.Button();
            this.lblServerName = new System.Windows.Forms.Label();
            this.sqlServerAuthPanel = new System.Windows.Forms.Panel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.sqlServerPasswordTextBox = new System.Windows.Forms.TextBox();
            this.sqlServerUserNameTextBox = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblAuthentication = new System.Windows.Forms.Label();
            this.btnTestSqlConnection = new System.Windows.Forms.Button();
            this.sqlServerAuthenticationComboBox = new System.Windows.Forms.ComboBox();
            this.lblRestart = new System.Windows.Forms.Label();
            this.rbtnSqlPersistence = new System.Windows.Forms.RadioButton();
            this.rbtnFilePersistence = new System.Windows.Forms.RadioButton();
            this.mainPanel.SuspendLayout();
            this.groupBoxPersistenceType.SuspendLayout();
            this.sqlServerOptionsPanel.SuspendLayout();
            this.sqlServerAuthPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.AutoScroll = true;
            this.mainPanel.Controls.Add(this.groupBoxPersistenceType);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(512, 328);
            this.mainPanel.TabIndex = 1;
            // 
            // groupBoxPersistenceType
            // 
            this.groupBoxPersistenceType.Controls.Add(this.sqlServerOptionsPanel);
            this.groupBoxPersistenceType.Controls.Add(this.lblRestart);
            this.groupBoxPersistenceType.Controls.Add(this.rbtnSqlPersistence);
            this.groupBoxPersistenceType.Controls.Add(this.rbtnFilePersistence);
            this.groupBoxPersistenceType.Location = new System.Drawing.Point(6, 3);
            this.groupBoxPersistenceType.Name = "groupBoxPersistenceType";
            this.groupBoxPersistenceType.Size = new System.Drawing.Size(500, 322);
            this.groupBoxPersistenceType.TabIndex = 0;
            this.groupBoxPersistenceType.TabStop = false;
            this.groupBoxPersistenceType.Text = "Select where to store application data";
            // 
            // sqlServerOptionsPanel
            // 
            this.sqlServerOptionsPanel.Controls.Add(this.lblDbMasterPassword);
            this.sqlServerOptionsPanel.Controls.Add(this.txtDbPassword);
            this.sqlServerOptionsPanel.Controls.Add(this.btnSetDatabasePassword);
            this.sqlServerOptionsPanel.Controls.Add(this.btnAdvanced);
            this.sqlServerOptionsPanel.Controls.Add(this.databaseCombobox);
            this.sqlServerOptionsPanel.Controls.Add(this.queryLabel);
            this.sqlServerOptionsPanel.Controls.Add(this.tableQueryLabel);
            this.sqlServerOptionsPanel.Controls.Add(this.serversComboBox);
            this.sqlServerOptionsPanel.Controls.Add(this.btnDatabases);
            this.sqlServerOptionsPanel.Controls.Add(this.btnSearch);
            this.sqlServerOptionsPanel.Controls.Add(this.testLabel);
            this.sqlServerOptionsPanel.Controls.Add(this.lblDatabase);
            this.sqlServerOptionsPanel.Controls.Add(this.btnCreateNew);
            this.sqlServerOptionsPanel.Controls.Add(this.lblServerName);
            this.sqlServerOptionsPanel.Controls.Add(this.sqlServerAuthPanel);
            this.sqlServerOptionsPanel.Controls.Add(this.lblAuthentication);
            this.sqlServerOptionsPanel.Controls.Add(this.btnTestSqlConnection);
            this.sqlServerOptionsPanel.Controls.Add(this.sqlServerAuthenticationComboBox);
            this.sqlServerOptionsPanel.Enabled = false;
            this.sqlServerOptionsPanel.Location = new System.Drawing.Point(25, 74);
            this.sqlServerOptionsPanel.Name = "sqlServerOptionsPanel";
            this.sqlServerOptionsPanel.Size = new System.Drawing.Size(469, 209);
            this.sqlServerOptionsPanel.TabIndex = 17;
            // 
            // lblDbMasterPassword
            // 
            this.lblDbMasterPassword.AutoSize = true;
            this.lblDbMasterPassword.Location = new System.Drawing.Point(29, 155);
            this.lblDbMasterPassword.Name = "lblDbMasterPassword";
            this.lblDbMasterPassword.Size = new System.Drawing.Size(104, 13);
            this.lblDbMasterPassword.TabIndex = 27;
            this.lblDbMasterPassword.Text = "Database password:";
            this.lblDbMasterPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDbPassword
            // 
            this.txtDbPassword.Location = new System.Drawing.Point(148, 152);
            this.txtDbPassword.Name = "txtDbPassword";
            this.txtDbPassword.PasswordChar = '*';
            this.txtDbPassword.Size = new System.Drawing.Size(173, 20);
            this.txtDbPassword.TabIndex = 26;
            // 
            // btnSetDatabasePassword
            // 
            this.btnSetDatabasePassword.Location = new System.Drawing.Point(327, 152);
            this.btnSetDatabasePassword.Name = "btnSetDatabasePassword";
            this.btnSetDatabasePassword.Size = new System.Drawing.Size(104, 23);
            this.btnSetDatabasePassword.TabIndex = 25;
            this.btnSetDatabasePassword.Text = "Set password";
            this.btnSetDatabasePassword.UseVisualStyleBackColor = true;
            this.btnSetDatabasePassword.Click += new System.EventHandler(this.OnBtnSetPasswordClick);
            // 
            // btnAdvanced
            // 
            this.btnAdvanced.Location = new System.Drawing.Point(205, 181);
            this.btnAdvanced.Name = "btnAdvanced";
            this.btnAdvanced.Size = new System.Drawing.Size(75, 23);
            this.btnAdvanced.TabIndex = 24;
            this.btnAdvanced.Text = "Advanced";
            this.btnAdvanced.UseVisualStyleBackColor = true;
            this.btnAdvanced.Click += new System.EventHandler(this.ButtonAdvancedClick);
            // 
            // databaseCombobox
            // 
            this.databaseCombobox.DropDownWidth = 200;
            this.databaseCombobox.Location = new System.Drawing.Point(148, 125);
            this.databaseCombobox.Name = "databaseCombobox";
            this.databaseCombobox.Size = new System.Drawing.Size(173, 21);
            this.databaseCombobox.TabIndex = 23;
            // 
            // queryLabel
            // 
            this.queryLabel.AutoSize = true;
            this.queryLabel.Location = new System.Drawing.Point(362, 17);
            this.queryLabel.Name = "queryLabel";
            this.queryLabel.Size = new System.Drawing.Size(58, 13);
            this.queryLabel.TabIndex = 20;
            this.queryLabel.Text = "Querying...";
            this.queryLabel.Visible = false;
            // 
            // tableQueryLabel
            // 
            this.tableQueryLabel.AutoSize = true;
            this.tableQueryLabel.Location = new System.Drawing.Point(362, 128);
            this.tableQueryLabel.Name = "tableQueryLabel";
            this.tableQueryLabel.Size = new System.Drawing.Size(58, 13);
            this.tableQueryLabel.TabIndex = 21;
            this.tableQueryLabel.Text = "Querying...";
            this.tableQueryLabel.Visible = false;
            // 
            // serversComboBox
            // 
            this.serversComboBox.FormattingEnabled = true;
            this.serversComboBox.Location = new System.Drawing.Point(148, 12);
            this.serversComboBox.Name = "serversComboBox";
            this.serversComboBox.Size = new System.Drawing.Size(173, 21);
            this.serversComboBox.TabIndex = 2;
            // 
            // btnDatabases
            // 
            this.btnDatabases.Location = new System.Drawing.Point(327, 123);
            this.btnDatabases.Name = "btnDatabases";
            this.btnDatabases.Size = new System.Drawing.Size(29, 23);
            this.btnDatabases.TabIndex = 22;
            this.btnDatabases.Text = "...";
            this.btnDatabases.UseVisualStyleBackColor = true;
            this.btnDatabases.Click += new System.EventHandler(this.ButtonFindDatabasesClick);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(327, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(29, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "...";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.SearchServersButtonClick);
            // 
            // testLabel
            // 
            this.testLabel.AutoSize = true;
            this.testLabel.Location = new System.Drawing.Point(393, 186);
            this.testLabel.Name = "testLabel";
            this.testLabel.Size = new System.Drawing.Size(51, 13);
            this.testLabel.TabIndex = 21;
            this.testLabel.Text = "Testing...";
            this.testLabel.Visible = false;
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(77, 128);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(56, 13);
            this.lblDatabase.TabIndex = 16;
            this.lblDatabase.Text = "Database:";
            this.lblDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCreateNew
            // 
            this.btnCreateNew.Location = new System.Drawing.Point(67, 181);
            this.btnCreateNew.Name = "btnCreateNew";
            this.btnCreateNew.Size = new System.Drawing.Size(132, 23);
            this.btnCreateNew.TabIndex = 9;
            this.btnCreateNew.Text = "Create new database";
            this.btnCreateNew.UseVisualStyleBackColor = true;
            this.btnCreateNew.Visible = false;
            this.btnCreateNew.Click += new System.EventHandler(this.ButtonCreateNewDatabaseClick);
            // 
            // lblServerName
            // 
            this.lblServerName.AutoSize = true;
            this.lblServerName.Location = new System.Drawing.Point(61, 15);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(72, 13);
            this.lblServerName.TabIndex = 8;
            this.lblServerName.Text = "Server Name:";
            this.lblServerName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sqlServerAuthPanel
            // 
            this.sqlServerAuthPanel.Controls.Add(this.lblUserName);
            this.sqlServerAuthPanel.Controls.Add(this.sqlServerPasswordTextBox);
            this.sqlServerAuthPanel.Controls.Add(this.sqlServerUserNameTextBox);
            this.sqlServerAuthPanel.Controls.Add(this.lblPassword);
            this.sqlServerAuthPanel.Enabled = false;
            this.sqlServerAuthPanel.Location = new System.Drawing.Point(3, 66);
            this.sqlServerAuthPanel.Name = "sqlServerAuthPanel";
            this.sqlServerAuthPanel.Size = new System.Drawing.Size(327, 55);
            this.sqlServerAuthPanel.TabIndex = 16;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(69, 7);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(61, 13);
            this.lblUserName.TabIndex = 12;
            this.lblUserName.Text = "User name:";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sqlServerPasswordTextBox
            // 
            this.sqlServerPasswordTextBox.Location = new System.Drawing.Point(145, 30);
            this.sqlServerPasswordTextBox.Name = "sqlServerPasswordTextBox";
            this.sqlServerPasswordTextBox.PasswordChar = '*';
            this.sqlServerPasswordTextBox.Size = new System.Drawing.Size(173, 20);
            this.sqlServerPasswordTextBox.TabIndex = 6;
            // 
            // sqlServerUserNameTextBox
            // 
            this.sqlServerUserNameTextBox.Location = new System.Drawing.Point(145, 4);
            this.sqlServerUserNameTextBox.Name = "sqlServerUserNameTextBox";
            this.sqlServerUserNameTextBox.Size = new System.Drawing.Size(173, 20);
            this.sqlServerUserNameTextBox.TabIndex = 5;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(74, 33);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(56, 13);
            this.lblPassword.TabIndex = 14;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAuthentication
            // 
            this.lblAuthentication.AutoSize = true;
            this.lblAuthentication.Location = new System.Drawing.Point(55, 42);
            this.lblAuthentication.Name = "lblAuthentication";
            this.lblAuthentication.Size = new System.Drawing.Size(78, 13);
            this.lblAuthentication.TabIndex = 9;
            this.lblAuthentication.Text = "Authentication:";
            this.lblAuthentication.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTestSqlConnection
            // 
            this.btnTestSqlConnection.Enabled = false;
            this.btnTestSqlConnection.Location = new System.Drawing.Point(286, 181);
            this.btnTestSqlConnection.Name = "btnTestSqlConnection";
            this.btnTestSqlConnection.Size = new System.Drawing.Size(101, 23);
            this.btnTestSqlConnection.TabIndex = 7;
            this.btnTestSqlConnection.Text = "Test connection";
            this.btnTestSqlConnection.UseVisualStyleBackColor = true;
            this.btnTestSqlConnection.Click += new System.EventHandler(this.OnBntTestSqlConnectionClick);
            // 
            // sqlServerAuthenticationComboBox
            // 
            this.sqlServerAuthenticationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sqlServerAuthenticationComboBox.FormattingEnabled = true;
            this.sqlServerAuthenticationComboBox.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this.sqlServerAuthenticationComboBox.Location = new System.Drawing.Point(148, 39);
            this.sqlServerAuthenticationComboBox.Name = "sqlServerAuthenticationComboBox";
            this.sqlServerAuthenticationComboBox.Size = new System.Drawing.Size(173, 21);
            this.sqlServerAuthenticationComboBox.TabIndex = 4;
            this.sqlServerAuthenticationComboBox.SelectedIndexChanged += new System.EventHandler(this.SqlServerAuthenticationComboBoxSelectedIndexChanged);
            // 
            // lblRestart
            // 
            this.lblRestart.AutoSize = true;
            this.lblRestart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRestart.Location = new System.Drawing.Point(22, 297);
            this.lblRestart.Name = "lblRestart";
            this.lblRestart.Size = new System.Drawing.Size(328, 13);
            this.lblRestart.TabIndex = 6;
            this.lblRestart.Text = "To apply this options you have to restart the application!";
            // 
            // rbtnSqlPersistence
            // 
            this.rbtnSqlPersistence.AutoSize = true;
            this.rbtnSqlPersistence.Location = new System.Drawing.Point(25, 51);
            this.rbtnSqlPersistence.Name = "rbtnSqlPersistence";
            this.rbtnSqlPersistence.Size = new System.Drawing.Size(174, 17);
            this.rbtnSqlPersistence.TabIndex = 1;
            this.rbtnSqlPersistence.Text = "Microsoft SQL database server:";
            this.rbtnSqlPersistence.UseVisualStyleBackColor = true;
            this.rbtnSqlPersistence.CheckedChanged += new System.EventHandler(this.RbtnSqlPersistenceCheckedChanged);
            // 
            // rbtnFilePersistence
            // 
            this.rbtnFilePersistence.AutoSize = true;
            this.rbtnFilePersistence.Checked = true;
            this.rbtnFilePersistence.Location = new System.Drawing.Point(25, 28);
            this.rbtnFilePersistence.Name = "rbtnFilePersistence";
            this.rbtnFilePersistence.Size = new System.Drawing.Size(113, 17);
            this.rbtnFilePersistence.TabIndex = 0;
            this.rbtnFilePersistence.TabStop = true;
            this.rbtnFilePersistence.Text = "Files in local profile";
            this.rbtnFilePersistence.UseVisualStyleBackColor = true;
            this.rbtnFilePersistence.CheckedChanged += new System.EventHandler(this.OnRbtnFilePersistenceCheckedChanged);
            // 
            // PersistenceOptionPanel
            // 
            this.Controls.Add(this.mainPanel);
            this.Name = "PersistenceOptionPanel";
            this.Size = new System.Drawing.Size(512, 328);
            this.mainPanel.ResumeLayout(false);
            this.groupBoxPersistenceType.ResumeLayout(false);
            this.groupBoxPersistenceType.PerformLayout();
            this.sqlServerOptionsPanel.ResumeLayout(false);
            this.sqlServerOptionsPanel.PerformLayout();
            this.sqlServerAuthPanel.ResumeLayout(false);
            this.sqlServerAuthPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel mainPanel;
        private GroupBox groupBoxPersistenceType;
        private RadioButton rbtnSqlPersistence;
        private RadioButton rbtnFilePersistence;
        private Label lblRestart;
        private Button btnTestSqlConnection;
        private Panel sqlServerAuthPanel;
        private Label lblUserName;
        private TextBox sqlServerPasswordTextBox;
        private TextBox sqlServerUserNameTextBox;
        private Label lblPassword;
        private ComboBox sqlServerAuthenticationComboBox;
        private Label lblAuthentication;
        private Label lblServerName;
        private Panel sqlServerOptionsPanel;
        private Button btnCreateNew;
        private Label lblDatabase;
        private Button btnSearch;
        private ComboBox serversComboBox;
        private Label queryLabel;
        private Label testLabel;
        private Label tableQueryLabel;
        private Button btnDatabases;
        private ComboBox databaseCombobox;
        private Button btnAdvanced;
        private Label lblDbMasterPassword;
        private TextBox txtDbPassword;
        private Button btnSetDatabasePassword;
    }
}
