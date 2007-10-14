using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Terminals.Connections {
    public class CaptureManagerConnection : Connection//System.Windows.Forms.Control//
    {

        public CaptureManagerConnection()
        {
            InitializeComponent();
        }
        public void RefreshView()
        {
            layout.RefreshView();
        }
        private Terminals.CaptureManager.CaptureManagerLayout layout;

        #region IConnection Members
        private bool connected = false;
        public override void ChangeDesktopSize(Terminals.DesktopSize Size)
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
            catch (Exception e) { }
        }

        #endregion

        private void InitializeComponent()
        {
            this.layout = new Terminals.CaptureManager.CaptureManagerLayout();
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