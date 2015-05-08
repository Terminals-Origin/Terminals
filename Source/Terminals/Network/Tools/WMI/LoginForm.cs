using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WMITestClient
{
    /// <summary>
    /// Summary description for LoginForm.
    /// </summary>
    internal class LoginForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TextBox MachineNameTextBox;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.Button ButtonLogin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

        private bool isCancelled = true;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.MachineNameTextBox = new System.Windows.Forms.TextBox();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.ButtonLogin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(112, 120);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 0;
            this.ButtonCancel.Text = "&Cancel";
            this.ButtonCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // MachineNameTextBox
            // 
            this.MachineNameTextBox.Location = new System.Drawing.Point(110, 14);
            this.MachineNameTextBox.Name = "MachineNameTextBox";
            this.MachineNameTextBox.Size = new System.Drawing.Size(162, 20);
            this.MachineNameTextBox.TabIndex = 1;
            this.MachineNameTextBox.Text = "\\\\localhost";
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Location = new System.Drawing.Point(110, 50);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(162, 20);
            this.UsernameTextBox.TabIndex = 2;
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(110, 88);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.PasswordChar = '*';
            this.PasswordTextBox.Size = new System.Drawing.Size(162, 20);
            this.PasswordTextBox.TabIndex = 3;
            // 
            // ButtonLogin
            // 
            this.ButtonLogin.Location = new System.Drawing.Point(200, 120);
            this.ButtonLogin.Name = "ButtonLogin";
            this.ButtonLogin.Size = new System.Drawing.Size(75, 23);
            this.ButtonLogin.TabIndex = 4;
            this.ButtonLogin.Text = "&Login";
            this.ButtonLogin.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Machine Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Password:";
            // 
            // LoginForm
            // 
            this.AcceptButton = this.ButtonLogin;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(280, 149);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonLogin);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.MachineNameTextBox);
            this.Controls.Add(this.ButtonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.Text = "Login...";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Properties

        public bool Cancelled
        {
            get
            {
                return this.isCancelled;
            }

            set
            {
                this.isCancelled = value;
            }
        }

        public string MachineName
        {
            get
            {
                return this.MachineNameTextBox.Text;
            }

            set
            {
                this.MachineNameTextBox.Text = value;
            }
        }

        public string UserName
        {
            get
            {
                return this.UsernameTextBox.Text;
            }

            set
            {
                this.UsernameTextBox.Text = value;
            }
        }

        public string Password
        {
            get
            {
                return this.PasswordTextBox.Text;
            }

            set
            {
                this.PasswordTextBox.Text = value;
            }
        }

        #endregion

        private void CancelButton_Click(object sender, System.EventArgs e)
        {
            this.Visible = false;
        }

        private void LoginButton_Click(object sender, System.EventArgs e)
        {

            bool success = false;

            if (this.MachineNameTextBox.Text == string.Empty)
            {
                this.MachineNameTextBox.Text = @"\\localhost\root\cimv2";
            }
            else
            {
                if (!this.MachineNameTextBox.Text.StartsWith(@"\\")) 
                    this.MachineNameTextBox.Text = @"\\" + this.MachineNameTextBox.Text;
            }

            if (!this.MachineNameTextBox.Text.StartsWith(@"\\localhost"))
            {
                if (this.UsernameTextBox.Text != string.Empty && this.PasswordTextBox.Text != string.Empty && this.MachineNameTextBox.Text != string.Empty)
                {
                    try
                    {
                        System.Management.ConnectionOptions oConn = new System.Management.ConnectionOptions();
                        oConn.Username = this.UsernameTextBox.Text;
                        oConn.Password = this.PasswordTextBox.Text;
                        oConn.Impersonation = System.Management.ImpersonationLevel.Impersonate;
                        //oConn.Authority = this.MachineNameTextBox.Text;
                        oConn.Authentication = System.Management.AuthenticationLevel.Connect;
                        System.Management.ManagementScope oMs = new System.Management.ManagementScope(this.MachineNameTextBox.Text, oConn);
                        oMs.Path = new System.Management.ManagementPath( this.MachineNameTextBox.Text );
                        oMs.Connect();
                        success = oMs.IsConnected;
                    }
                    catch (Exception exc)
                    {
                        Terminals.Logging.Info("Login Failed", exc);
                        MessageBox.Show(exc.Message);
                    }

                    if (success)
                    {
                        this.Cancelled = false;
                        this.Visible = false;
                    }
                }
            }
            else
            {
                this.Cancelled = false;
                this.Visible = false;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

            if (this.MachineNameTextBox.Text == string.Empty)
            {
                this.MachineNameTextBox.Text = @"\\localhost\root\cimv2";
            }
            else
            {
                if (!this.MachineNameTextBox.Text.StartsWith(@"\\"))
                    this.MachineNameTextBox.Text = @"\\" + this.MachineNameTextBox.Text;
            }
        }
    }
}