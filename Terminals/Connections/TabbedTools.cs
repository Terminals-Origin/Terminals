using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Connections
{
    public partial class TabbedTools : UserControl
    {
        public TabbedTools()
        {
            InitializeComponent();
        }
        public delegate void TabChanged(object sender, TabControlEventArgs e);
        public event TabChanged OnTabChanged;

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (OnTabChanged != null) OnTabChanged(sender, e);
        }

        private void terminalServerManager1_Load(object sender, EventArgs e)
        {

        }
    }
}
