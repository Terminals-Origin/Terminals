using System;
using System.Windows.Forms;

namespace Terminals.Network.WMI
{
    internal partial class WMIServerCredentials : UserControl
    {
        public WMIServerCredentials()
        {
            InitializeComponent();


        }
        public string SelectedServer
        {
            get
            {
                return this.comboBox1.Text;
            }
            set
            {
                this.comboBox1.Text = value;
            }
        }
        public string Username
        {
            get
            {
                return this.UsernameTextbox.Text;
            }
            set
            {
                this.UsernameTextbox.Text = value;
            }
        }
        public string Password
        {
            get
            {
                return this.PasswordTextbox.Text;
            }
            set
            {
                this.PasswordTextbox.Text = value;
            }
        }

        private void WMIServerCredentials_Load(object sender, EventArgs e)
        {
            if (System.Environment.UserDomainName != null && System.Environment.UserDomainName != "")
            {
                this.UsernameTextbox.Text = string.Format(@"{0}\{1}", System.Environment.UserDomainName, System.Environment.UserName);
            }
            else
            {
                this.UsernameTextbox.Text = System.Environment.UserName;
            }

            //try {
            //    foreach(FavoriteConfigurationElement elm in Settings.GetFavorites()) {
            //        this.comboBox1.Items.Add(elm.ServerName);
            //    }
            //} catch(Exception exc) {
            //    Terminals.Logging.Log.Error("WMI Server Credentials Favorite Query Failed", exc);
            //}
        }
    }
}