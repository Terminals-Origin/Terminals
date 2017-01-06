using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WalburySoftware
{
    public partial class TerminalEmulator : Control
    {
        private class WMCodes
        {
            public const int WM_KEYFIRST    = 0x0100;
            public const int WM_KEYDOWN     = 0x0100;
            public const int WM_KEYUP       = 0x0101;
            public const int WM_CHAR        = 0x0102;
            public const int WM_DEADCHAR    = 0x0103;
            public const int WM_SYSKEYDOWN  = 0x0104;
            public const int WM_SYSKEYUP    = 0x0105;
            public const int WM_SYSCHAR     = 0x0106;
            public const int WM_SYSDEADCHAR = 0x0107;
            public const int WM_KEYLAST     = 0x0108;
        }
    }
}
