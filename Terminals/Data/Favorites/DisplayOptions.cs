using System;

namespace Terminals.Data
{
    [Serializable]
    public class DisplayOptions
    {
        public Int32 Height { get; set; }
        public Int32 DesktopSizeWidth { get; set; }

        private DesktopSize desktopSize = DesktopSize.FitToWindow;
        public DesktopSize DesktopSize
        {
            get
            {
                return desktopSize;
            }
            set
            {
                desktopSize = value;
            }
        }

        private Colors colors = Colors.Bits32;
        public Colors Colors
        {
            get
            {
                return colors;
            }
            set
            {
                colors = value;
            }
        }
    }
}
