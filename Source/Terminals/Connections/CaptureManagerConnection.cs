using System;
using System.Windows.Forms;
using Terminals.CaptureManager;

namespace Terminals.Connections
{
    internal class CaptureManagerConnection : Connection
    {
        public CaptureManagerConnection()
        {
            InitializeComponent();
        }
        public void RefreshView()
        {
            layout.RefreshView();
        }
        private CaptureManagerLayout layout;

        #region IConnection Members
        private bool connected = false;
        public override void ChangeDesktopSize(DesktopSize Size)
        {
        }

        public override bool Connected { get { return connected; } }


        public override bool Connect()
        {
            layout.Parent = base.TerminalTabPage;
            this.Parent = TerminalTabPage;
            TerminalTabPage.Connection = this;
            return true;
        }

        public override void Disconnect()
        {
            try
            {
                connected = false;
            }
            catch (Exception e)
            {
                Logging.Error("Error on Disconnect", e);
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this.layout = new CaptureManagerLayout();
            this.SuspendLayout();
            // 
            // networkingToolsLayout1
            // 
            this.layout.Location = new System.Drawing.Point(0, 0);
            this.layout.Name = "layout1";
            this.layout.Size = new System.Drawing.Size(700, 500);
            this.layout.TabIndex = 0;
            this.layout.Dock = DockStyle.Fill;
            this.ResumeLayout(false);
        }
    }
}