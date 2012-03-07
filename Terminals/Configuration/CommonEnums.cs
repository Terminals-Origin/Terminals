using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Terminals
{
    public enum DesktopSize { x640 = 0, x800, x1024, x1152, x1280, FitToWindow, FullScreen, AutoScale, Custom };
    public enum Colors { Bits8 = 0, Bit16, Bits24, Bits32 };
    public enum RemoteSounds { Redirect = 0, PlayOnServer = 1, DontPlay = 2 };
    public enum PerfomanceOptions : int { TS_PERF_DISABLE_CURSOR_SHADOW = 0x00000020, TS_PERF_DISABLE_CURSORSETTINGS = 0x00000040, TS_PERF_DISABLE_FULLWINDOWDRAG = 0x00000002, TS_PERF_DISABLE_MENUANIMATIONS = 0x00000004, TS_PERF_DISABLE_NOTHING = 0x00000000, TS_PERF_DISABLE_THEMING = 0x00000008, TS_PERF_DISABLE_WALLPAPER = 0x00000001, TS_PERF_ENABLE_FONT_SMOOTHING = 0x00000080, TS_PERF_ENABLE_DESKTOP_COMPOSITION = 0x00000100 }

}
