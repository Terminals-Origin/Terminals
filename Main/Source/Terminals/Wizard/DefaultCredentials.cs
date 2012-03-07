using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Terminals.Configuration;

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

            if(Environment.UserDomainName != Environment.MachineName) {
                if(this.domainTextbox.Text == "") this.domainTextbox.Text = Environment.UserDomainName;
            }
            if(this.usernameTextbox.Text == "") this.usernameTextbox.Text = Environment.UserName;
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
