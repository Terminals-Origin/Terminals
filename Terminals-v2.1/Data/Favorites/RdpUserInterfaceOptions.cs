using System;

namespace Terminals.Data
{
    [Serializable]
    public class RdpUserInterfaceOptions
    {
        public Boolean DisableWindowsKey { get; set; }
        public Boolean DoubleClickDetect { get; set; }
        public Boolean DisplayConnectionBar { get; set; }
        public Boolean DisableControlAltDelete { get; set; }
        public Boolean AcceleratorPassthrough { get; set; }
        public Boolean EnableCompression { get; set; }
        public Boolean BitmapPeristence { get; set; }
        public Boolean AllowBackgroundInput { get; set; }
        public Boolean DisableTheming { get; set; }
        public Boolean DisableMenuAnimations { get; set; }
        public Boolean DisableFullWindowDrag { get; set; }
        public Boolean DisableCursorBlinking { get; set; }
        public Boolean EnableDesktopComposition { get; set; }
        public Boolean EnableFontSmoothing { get; set; }
        public Boolean DisableCursorShadow { get; set; }
        public Boolean DisableWallPaper { get; set; }
    }
}
