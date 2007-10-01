using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Terminals
{
    public enum DesktopSize { x640 = 0, x800, x1024, x1280, FitToWindow, FullScreen, AutoScale };
    public enum Colors { Bits8 = 0, Bit16, Bits24, Bits32 };
    public enum RemoteSounds { Redirect = 0, PlayOnServer = 1, DontPlay = 2 };

}
