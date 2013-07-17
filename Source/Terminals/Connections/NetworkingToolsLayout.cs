using System;
using System.Windows.Forms;

namespace Terminals.Connections
{
    internal partial class NetworkingToolsLayout : UserControl
    {
        public delegate void TabChanged(object sender, TabControlEventArgs e);

        public event TabChanged OnTabChanged;

        public NetworkingToolsLayout()
        {
            this.InitializeComponent();
        }

        private void TabbedTools1_Load(object sender, EventArgs e)
        {
            this.tabbedTools1.OnTabChanged += new TabbedTools.TabChanged(tabbedTools1_OnTabChanged);
        }

        public void Execute(NettworkingTools action, string host)
        {
            this.tabbedTools1.Execute(action, host);
        }

        private void tabbedTools1_OnTabChanged(object sender, TabControlEventArgs e)
        {
            if(OnTabChanged != null)
                OnTabChanged(sender, e);
        }
    }
}
