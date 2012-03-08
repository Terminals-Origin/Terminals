using System;
using System.Windows.Forms;

namespace Terminals.Connections
{
    internal partial class NetworkingToolsLayout : UserControl
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
        public void Execute(string Action, string Host)
        {
            this.tabbedTools1.Execute(Action, Host);
        }

        void tabbedTools1_OnTabChanged(object sender, TabControlEventArgs e)
        {
            if(OnTabChanged != null) OnTabChanged(sender, e);
        }
    }
}
