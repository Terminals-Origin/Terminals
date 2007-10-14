using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Wizard
{
    public partial class CommonOptions : UserControl
    {
        public CommonOptions()
        {
            InitializeComponent();
        }
        public bool MinimizeToTray
        {
            get { return MinimizeCheckbox.Checked; }
        }
        public bool AllowOnlySingleInstance
        {
            get { return SingleCheckbox.Checked; }
        }
        public bool WarnOnDisconnect
        {
            get { return WarnCheckbox.Checked; }
        }
        public bool AutoSwitchOnCapture
        {
            get { return autoSwitchOnCapture.Checked; }
        }
    }
}
