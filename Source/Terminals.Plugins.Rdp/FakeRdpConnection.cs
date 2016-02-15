namespace Terminals.Connections
{
    internal class FakeRdpConnection : Connection
    {
        public override bool Connected { get { return true; } }

        public new bool IsTerminalServer { get { return true; } }

        public override bool Connect()
        {
            return true;
        }
    }
}