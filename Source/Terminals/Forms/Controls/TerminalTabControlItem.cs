using AxMSTSCLib;
using TabControl;
using Terminals.Data;

namespace Terminals
{
    internal class TerminalTabControlItem : TabControlItem
    {
        private Connections.IConnection _connection;
        private AxMsRdpClient6 _terminalControl;
        private IFavorite _favorite;

        public TerminalTabControlItem(string caption) : base(caption, null)
        {
        }

        public Connections.IConnection Connection
        {
            get
            {
                return this._connection;
            }
            set
            {
                this._connection = value;
            }
        }

        public AxMsRdpClient6 TerminalControl
        {
            get
            {
                return this._terminalControl;
            }
            set
            {
                this._terminalControl = value;
            }
        }

        public IFavorite Favorite
        {
            get
            {
                return this._favorite;
            }
            set
            {
                this._favorite = value;
            }
        }
    }
}
