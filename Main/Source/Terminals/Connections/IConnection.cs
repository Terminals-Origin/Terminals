using Terminals.Data;
using Terminals.TerminalServices;

namespace Terminals.Connections
{
    internal interface IConnection
    {
        TerminalTabControlItem TerminalTabPage { get; set; }
        IFavorite Favorite { get; set; }
        MainForm ParentForm { get; set; }
        bool Connect();
        void Disconnect();
        bool Connected { get; }
        void ChangeDesktopSize(DesktopSize Size);
        TerminalServer Server { get; set; }
        bool IsTerminalServer { get; set; }

        string LastError { get; set; }
    }
}
