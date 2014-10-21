using Terminals;
using Terminals.Connections;

namespace Tests.Connections
{
    internal class MockConnection : Connection
    {
        public override bool Connected { get { throw new System.NotImplementedException(); } }

        public override void ChangeDesktopSize(DesktopSize size)
        {
            throw new System.NotImplementedException();
        }

        public override bool Connect()
        {
            throw new System.NotImplementedException();
        }

        public override void Disconnect()
        {
            throw new System.NotImplementedException();
        }

        internal void CallFireDisconnected()
        {
            base.FireDisconnected();
        }
        
        internal void CallLog(string text)
        {
            base.Log(text);
        }

    }
}