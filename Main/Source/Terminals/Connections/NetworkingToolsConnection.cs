using System.Windows.Forms;

namespace Terminals.Connections
{
    internal class NetworkingToolsConnection : Connection
    {
        public NetworkingToolsConnection()
        {
            InitializeComponent();
        }

        private NetworkingToolsLayout networkingToolsLayout1;

        #region IConnection Members
        private bool connected;
        public override void ChangeDesktopSize(DesktopSize size)
        {
        }

        public override bool Connected { get { return connected; } }


        public override bool Connect()
        {
            networkingToolsLayout1.OnTabChanged += new NetworkingToolsLayout.TabChanged(networkingToolsLayout1_OnTabChanged);
            networkingToolsLayout1.Parent = this.TerminalTabPage;
            this.Parent = TerminalTabPage;
            this.connected = true;
            return true;
        }

        private void networkingToolsLayout1_OnTabChanged(object sender, TabControlEventArgs e)
        {
            this.TerminalTabPage.Title = e.TabPage.Text;
        }

        public override void Disconnect()
        {
            // nothing to do here
            this.connected = false;
        }

        #endregion

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

        internal void Execute(NettworkingTools action, string host)
        {
            this.networkingToolsLayout1.Execute(action, host);
        }
    }
}