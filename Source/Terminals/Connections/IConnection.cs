using Terminals.Data;
using Terminals.TerminalServices;

namespace Terminals.Connections
{
    internal interface IConnection
    {
        IFavorite Favorite { get; }
        TerminalServer Server { get; }
        bool IsTerminalServer { get; }
        bool Connected { get; }
        void Disconnect();
        void ChangeDesktopSize(DesktopSize size);
    }
}
