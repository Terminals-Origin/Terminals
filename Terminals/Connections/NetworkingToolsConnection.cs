using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Terminals.Connections {
    public class NetworkingToolsConnection : Connection//System.Windows.Forms.Control//
    {

        public NetworkingToolsConnection()
        {
            InitializeComponent();
        }

        private NetworkingToolsLayout networkingToolsLayout1;

        #region IConnection Members
        private bool connected = false;
        public override void ChangeDesktopSize(Terminals.DesktopSize Size)
        {
        }

        public override bool Connected { get { return connected; } }


        public override bool Connect()
        {
            networkingToolsLayout1.OnTabChanged += new NetworkingToolsLayout.TabChanged(networkingToolsLayout1_OnTabChanged);
            networkingToolsLayout1.Parent = base.TerminalTabPage;
            this.Parent = TerminalTabPage;
            return true;
        }

        void networkingToolsLayout1_OnTabChanged(object sender, TabControlEventArgs e)
        {
            this.TerminalTabPage.Title = e.TabPage.Text;
        }



        public override void Disconnect()
        {
            try
            {
                connected = false;
            }
            catch (Exception e) { }
        }

        #endregion

        private void InitializeComponent()
        {
            this.networkingToolsLayout1 = new Terminals.Connections.NetworkingToolsLayout();
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
        public void Execute(string Action, string Host)
        {
            this.networkingToolsLayout1.Execute(Action, Host);

        }
        
    }
}