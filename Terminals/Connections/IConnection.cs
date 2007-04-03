using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Connections {
    public interface IConnection {
        TerminalTabControlItem TerminalTabPage { get; set; }
        FavoriteConfigurationElement Favorite { get;set;}
        bool Connect();
        void Disconnect();
        bool Connected { get;}
        void ChangeDesktopSize(Terminals.DesktopSize Size);
    }
}
