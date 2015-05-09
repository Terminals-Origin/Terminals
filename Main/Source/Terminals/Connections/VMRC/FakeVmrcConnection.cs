using System.Windows.Forms;

namespace Terminals.Connections
{
    /// <summary>
    /// Allows testing specific command without real Vmrc connction
    /// </summary>
    internal class FakeVmrcConnection : Connection, IToolbarExtender
    {
        public override bool Connected { get { return true; } }
        
        public bool ViewOnlyMode { get; set; }

        public FakeVmrcConnection()
        {
        }

        public override bool Connect()
        {
            return this.Connected;
        }

        public void AdminDisplay()
        {
        }

        public void Visit(ToolStrip standardToolbar)
        {
            var menuVisitor = new VmrcMenuVisitor();
            menuVisitor.UpdateMenu(standardToolbar);
        }
    }
}