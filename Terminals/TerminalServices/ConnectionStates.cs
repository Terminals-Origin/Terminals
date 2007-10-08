using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.TerminalServices
{
    public enum ConnectionStates
    {
        WTSActive,
        WTSConnected,
        WTSConnectQuery,
        WTSShadow,
        WTSDisconnected,
        WTSIdle,
        WTSListen,
        WTSReset,
        WTSDown,
        WTSInit
    }
}