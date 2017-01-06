using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Wizard
{
    internal partial class MasterPassword : UserControl
    {
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

        public MasterPassword()
        {
            this.InitializeComponent();
        }

        internal void AssignPersistence(IPersistence persistence)
        {
            this.enterPassword1.AssignPersistence(persistence);
        }
    }
}
