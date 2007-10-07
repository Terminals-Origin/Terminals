using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Connections
{
    public partial class NetworkingToolsLayout : UserControl
    {
        public NetworkingToolsLayout()
        {
            InitializeComponent();
        }
        public delegate void TabChanged(object sender, TabControlEventArgs e);
        public event TabChanged OnTabChanged;
        private void tabbedTools1_Load(object sender, EventArgs e)
        {
            this.tabbedTools1.OnTabChanged += new TabbedTools.TabChanged(tabbedTools1_OnTabChanged);
        }

        void tabbedTools1_OnTabChanged(object sender, TabControlEventArgs e)
        {
            if (OnTabChanged != null) OnTabChanged(sender, e);
        }
    }
}
