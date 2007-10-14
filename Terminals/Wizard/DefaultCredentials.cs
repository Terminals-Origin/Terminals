using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Wizard
{
    public partial class DefaultCredentials : UserControl
    {
        public DefaultCredentials()
        {
            InitializeComponent();

            this.domainTextbox.Text = Settings.DefaultDomain;
            this.passwordTextbox.Text = Settings.DefaultPassword;
            this.usernameTextbox.Text = Settings.DefaultUsername;

            if(this.domainTextbox.Text == "") this.domainTextbox.Text = System.Environment.UserDomainName;
            if(this.usernameTextbox.Text == "") this.usernameTextbox.Text = System.Environment.UserName;
        }

        public string DefaultDomain
        {
            get { return this.domainTextbox.Text; }
        }
        public string DefaultPassword
        {
            get { return this.passwordTextbox.Text; }
        }
        public string DefaultUsername
        {
            get { return this.usernameTextbox.Text; }
        }
    }
}
