using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Wizard
{
    public partial class MasterPassword : UserControl
    {
        public MasterPassword()
        {
            InitializeComponent();
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
