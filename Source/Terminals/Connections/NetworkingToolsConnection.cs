using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Connections
{
    internal class NetworkingToolsConnection : Connection
    {
        private NetworkingToolsLayout networkingToolsLayout1;

        public override bool Connected { get { return true; } }

        public NetworkingToolsConnection()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.networkingToolsLayout1 = new NetworkingToolsLayout();
            this.SuspendLayout();
            // 
            // networkingToolsLayout1
            // 
            this.networkingToolsLayout1.Location = new System.Drawing.Point(0, 0);
            this.networkingToolsLayout1.Name = "networkingToolsLayout1";
            this.networkingToolsLayout1.Size = new System.Drawing.Size(700, 500);
            this.networkingToolsLayout1.TabIndex = 0;
            this.networkingToolsLayout1.Dock = DockStyle.Fill;
            this.ResumeLayout(false);
        }

        public override bool Connect()
        {
            networkingToolsLayout1.OnTabChanged += new NetworkingToolsLayout.TabChanged(networkingToolsLayout1_OnTabChanged);
            networkingToolsLayout1.Parent = this.TerminalTabPage;
            this.Parent = TerminalTabPage;
            return true;
        }

        private void networkingToolsLayout1_OnTabChanged(object sender, TabControlEventArgs e)
        {
            this.TerminalTabPage.Title = e.TabPage.Text;
        }

        internal void Execute(NettworkingTools action, string host, IPersistence persistence)
        {
            this.networkingToolsLayout1.Execute(action, host, persistence);
        }
    }
}