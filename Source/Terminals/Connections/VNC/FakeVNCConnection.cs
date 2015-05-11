namespace Terminals.Connections
{
    internal class FakeVNCConnection : Connection
    {
        public override bool Connected { get { return true; } }

        public void SendSpecialKeys(VncSharp.SpecialKeys Keys)
        {
        }

        public override bool Connect()
        {
            return true;
        }
    }
}