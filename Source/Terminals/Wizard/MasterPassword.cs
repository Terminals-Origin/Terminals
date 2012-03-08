using System.Windows.Forms;

namespace Terminals.Wizard
{
    internal partial class MasterPassword : UserControl
    {
        public MasterPassword()
        {
            InitializeComponent();
        }
        public bool StorePassword
        {
            get
            {
                return this.enterPassword1.StorePassword;
            }
        }

        public string Password
        {
            get
            {
                return this.enterPassword1.Password;
            }
        }
    }
}
