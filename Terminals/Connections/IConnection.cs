using Terminals.TerminalServices;

namespace Terminals.Connections
{
    public interface IConnection
    {
        TerminalTabControlItem TerminalTabPage { get; set; }
        FavoriteConfigurationElement Favorite { get; set; }
        MainForm ParentForm { get; set; }
        bool Connect();
        void Disconnect();
        bool Connected { get; }
        void ChangeDesktopSize(DesktopSize Size);
        TerminalServer Server { get; set; }
        bool IsTerminalServer { get; set; }
    }
}
