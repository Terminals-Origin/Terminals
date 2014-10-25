using System.Windows.Forms;
using Terminals.CaptureManager;

namespace Terminals.Connections
{
    internal class CaptureManagerConnection : Connection
    {
        public override bool Connected { get { return true; } }

        private CaptureManagerLayout layout;

        public CaptureManagerConnection()
        {
            InitializeComponent();
        }

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

        public void RefreshView()
        {
            layout.RefreshView();
        }

        public override bool Connect()
        {
            // no idea, why this is necessary. Otherwise the layout control doesnt apper
            layout.Parent = this.Parent;
            return true;
        }
    }
}