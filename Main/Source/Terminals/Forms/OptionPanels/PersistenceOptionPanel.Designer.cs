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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBoxPersistenceType = new System.Windows.Forms.GroupBox();
            this.SqlServerOptionsPanel = new System.Windows.Forms.Panel();
            this.QueryLabel = new System.Windows.Forms.Label();
            this.serversComboBox = new System.Windows.Forms.ComboBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SqlServerAuthPanel = new System.Windows.Forms.Panel();
            this.databaseCombobox = new System.Windows.Forms.ComboBox();
            this.TableQueryLabel = new System.Windows.Forms.Label();
            this.buttonDatabases = new System.Windows.Forms.Button();
            this.TestLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SqlServerPasswordTextBox = new System.Windows.Forms.TextBox();
            this.SqlServerUserNameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bntTestSqlConnection = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SqlServerAuthenticationComboBox = new System.Windows.Forms.ComboBox();
            this.lblRestart = new System.Windows.Forms.Label();
            this.rbtnSqlPersistence = new System.Windows.Forms.RadioButton();
            this.rbtnFilePersistence = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.groupBoxPersistenceType.SuspendLayout();
            this.SqlServerOptionsPanel.SuspendLayout();
            this.SqlServerAuthPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBoxPersistenceType);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(512, 328);
            this.panel1.TabIndex = 1;
            // 
            // groupBoxPersistenceType
            // 
            this.groupBoxPersistenceType.Controls.Add(this.SqlServerOptionsPanel);
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
            // SqlServerOptionsPanel
            // 
            this.SqlServerOptionsPanel.Controls.Add(this.databaseCombobox);
            this.SqlServerOptionsPanel.Controls.Add(this.QueryLabel);
            this.SqlServerOptionsPanel.Controls.Add(this.TableQueryLabel);
            this.SqlServerOptionsPanel.Controls.Add(this.serversComboBox);
            this.SqlServerOptionsPanel.Controls.Add(this.buttonDatabases);
            this.SqlServerOptionsPanel.Controls.Add(this.SearchButton);
            this.SqlServerOptionsPanel.Controls.Add(this.TestLabel);
            this.SqlServerOptionsPanel.Controls.Add(this.label5);
            this.SqlServerOptionsPanel.Controls.Add(this.button1);
            this.SqlServerOptionsPanel.Controls.Add(this.label1);
            this.SqlServerOptionsPanel.Controls.Add(this.SqlServerAuthPanel);
            this.SqlServerOptionsPanel.Controls.Add(this.label2);
            this.SqlServerOptionsPanel.Controls.Add(this.bntTestSqlConnection);
            this.SqlServerOptionsPanel.Controls.Add(this.SqlServerAuthenticationComboBox);
            this.SqlServerOptionsPanel.Enabled = false;
            this.SqlServerOptionsPanel.Location = new System.Drawing.Point(25, 74);
            this.SqlServerOptionsPanel.Name = "SqlServerOptionsPanel";
            this.SqlServerOptionsPanel.Size = new System.Drawing.Size(427, 209);
            this.SqlServerOptionsPanel.TabIndex = 17;
            // 
            // QueryLabel
            // 
            this.QueryLabel.AutoSize = true;
            this.QueryLabel.Location = new System.Drawing.Point(331, 20);
            this.QueryLabel.Name = "QueryLabel";
            this.QueryLabel.Size = new System.Drawing.Size(58, 13);
            this.QueryLabel.TabIndex = 20;
            this.QueryLabel.Text = "Querying...";
            this.QueryLabel.Visible = false;
            // 
            // serversComboBox
            // 
            this.serversComboBox.FormattingEnabled = true;
            this.serversComboBox.Location = new System.Drawing.Point(116, 12);
            this.serversComboBox.Name = "serversComboBox";
            this.serversComboBox.Size = new System.Drawing.Size(173, 21);
            this.serversComboBox.TabIndex = 2;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(296, 12);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(29, 23);
            this.SearchButton.TabIndex = 3;
            this.SearchButton.Text = "...";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButtonClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Database:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Server Name:";
            // 
            // SqlServerAuthPanel
            // 
            this.SqlServerAuthPanel.Controls.Add(this.label3);
            this.SqlServerAuthPanel.Controls.Add(this.SqlServerPasswordTextBox);
            this.SqlServerAuthPanel.Controls.Add(this.SqlServerUserNameTextBox);
            this.SqlServerAuthPanel.Controls.Add(this.label4);
            this.SqlServerAuthPanel.Enabled = false;
            this.SqlServerAuthPanel.Location = new System.Drawing.Point(14, 66);
            this.SqlServerAuthPanel.Name = "SqlServerAuthPanel";
            this.SqlServerAuthPanel.Size = new System.Drawing.Size(410, 55);
            this.SqlServerAuthPanel.TabIndex = 16;
            // 
            // DatabaseCombobox
            // 
            this.databaseCombobox.FormattingEnabled = true;
            this.databaseCombobox.Location = new System.Drawing.Point(117, 125);
            this.databaseCombobox.Name = "databaseCombobox";
            this.databaseCombobox.Size = new System.Drawing.Size(173, 21);
            this.databaseCombobox.TabIndex = 23;
            // 
            // TableQueryLabel
            // 
            this.TableQueryLabel.AutoSize = true;
            this.TableQueryLabel.Location = new System.Drawing.Point(332, 128);
            this.TableQueryLabel.Name = "TableQueryLabel";
            this.TableQueryLabel.Size = new System.Drawing.Size(58, 13);
            this.TableQueryLabel.TabIndex = 21;
            this.TableQueryLabel.Text = "Querying...";
            this.TableQueryLabel.Visible = false;
            // 
            // buttonDatabases
            // 
            this.buttonDatabases.Location = new System.Drawing.Point(297, 123);
            this.buttonDatabases.Name = "buttonDatabases";
            this.buttonDatabases.Size = new System.Drawing.Size(29, 23);
            this.buttonDatabases.TabIndex = 22;
            this.buttonDatabases.Text = "...";
            this.buttonDatabases.UseVisualStyleBackColor = true;
            this.buttonDatabases.Click += new System.EventHandler(this.ButtonDatabasesClick);
            // 
            // TestLabel
            // 
            this.TestLabel.AutoSize = true;
            this.TestLabel.Location = new System.Drawing.Point(362, 157);
            this.TestLabel.Name = "TestLabel";
            this.TestLabel.Size = new System.Drawing.Size(51, 13);
            this.TestLabel.TabIndex = 21;
            this.TestLabel.Text = "Testing...";
            this.TestLabel.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(175, 152);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Create New...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "User name:";
            // 
            // SqlServerPasswordTextBox
            // 
            this.SqlServerPasswordTextBox.Location = new System.Drawing.Point(102, 30);
            this.SqlServerPasswordTextBox.Name = "SqlServerPasswordTextBox";
            this.SqlServerPasswordTextBox.PasswordChar = '*';
            this.SqlServerPasswordTextBox.Size = new System.Drawing.Size(173, 20);
            this.SqlServerPasswordTextBox.TabIndex = 6;
            // 
            // SqlServerUserNameTextBox
            // 
            this.SqlServerUserNameTextBox.Location = new System.Drawing.Point(102, 4);
            this.SqlServerUserNameTextBox.Name = "SqlServerUserNameTextBox";
            this.SqlServerUserNameTextBox.Size = new System.Drawing.Size(173, 20);
            this.SqlServerUserNameTextBox.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Password:";
            // 
            // bntTestSqlConnection
            // 
            this.bntTestSqlConnection.Enabled = false;
            this.bntTestSqlConnection.Location = new System.Drawing.Point(296, 152);
            this.bntTestSqlConnection.Name = "bntTestSqlConnection";
            this.bntTestSqlConnection.Size = new System.Drawing.Size(60, 23);
            this.bntTestSqlConnection.TabIndex = 7;
            this.bntTestSqlConnection.Text = "Test...";
            this.bntTestSqlConnection.UseVisualStyleBackColor = true;
            this.bntTestSqlConnection.Click += new System.EventHandler(this.OnBntTestSqlConnectionClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Authentication:";
            // 
            // SqlServerAuthenticationComboBox
            // 
            this.SqlServerAuthenticationComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SqlServerAuthenticationComboBox.FormattingEnabled = true;
            this.SqlServerAuthenticationComboBox.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this.SqlServerAuthenticationComboBox.Location = new System.Drawing.Point(116, 39);
            this.SqlServerAuthenticationComboBox.Name = "SqlServerAuthenticationComboBox";
            this.SqlServerAuthenticationComboBox.Size = new System.Drawing.Size(173, 21);
            this.SqlServerAuthenticationComboBox.TabIndex = 4;
            this.SqlServerAuthenticationComboBox.SelectedIndexChanged += new System.EventHandler(this.SqlServerAuthenticationComboBoxSelectedIndexChanged);
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
            this.Controls.Add(this.panel1);
            this.Name = "PersistenceOptionPanel";
            this.Size = new System.Drawing.Size(512, 328);
            this.panel1.ResumeLayout(false);
            this.groupBoxPersistenceType.ResumeLayout(false);
            this.groupBoxPersistenceType.PerformLayout();
            this.SqlServerOptionsPanel.ResumeLayout(false);
            this.SqlServerOptionsPanel.PerformLayout();
            this.SqlServerAuthPanel.ResumeLayout(false);
            this.SqlServerAuthPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private GroupBox groupBoxPersistenceType;
        private RadioButton rbtnSqlPersistence;
        private RadioButton rbtnFilePersistence;
        private Label lblRestart;
        private Button bntTestSqlConnection;
        private Panel SqlServerAuthPanel;
        private Label label3;
        private TextBox SqlServerPasswordTextBox;
        private TextBox SqlServerUserNameTextBox;
        private Label label4;
        private ComboBox SqlServerAuthenticationComboBox;
        private Label label2;
        private Label label1;
        private Panel SqlServerOptionsPanel;
        private Button button1;
        private Label label5;
        private Button SearchButton;
        private ComboBox serversComboBox;
        private Label QueryLabel;
        private Label TestLabel;
        private Label TableQueryLabel;
        private Button buttonDatabases;
        private ComboBox databaseCombobox;
    }
}
