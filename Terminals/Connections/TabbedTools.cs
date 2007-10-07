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

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabPage3)
            {
                
            }
        }
    }
}
