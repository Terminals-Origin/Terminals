using System;
using Terminals.Data;
using Terminals.TerminalServices;

namespace Terminals.Connections
{
    internal interface IConnection : IDisposable
    {
        IFavorite Favorite { get; }
        TerminalServer Server { get; }
        bool IsTerminalServer { get; }
        bool Connected { get; }
        void ChangeDesktopSize(DesktopSize size);
    }
}
