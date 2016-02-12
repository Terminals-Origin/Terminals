using System.Windows.Forms;

namespace Terminals.Connections
{
    /// <summary>
    /// Allows testing specific command without real Vmrc connction
    /// </summary>
    internal class FakeVmrcConnection : Connection
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
    }
}