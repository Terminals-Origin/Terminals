using System;

namespace Terminals.Data.DB
{
    internal partial class DisplayOptions : IDisplayOptions
    {
        int IDisplayOptions.Height
        {
            get
            {
                return this.Height == null ? 0 : (int)this.Height;
            }
            set
            {
                this.Height = value == 0 ? (int?) null : value;
            }
        }

        int IDisplayOptions.Width
        {
            get
            {
                return this.Width == null ? 0 : (int)this.Width;
            }
            set
            {
                this.Width = value == 0 ? (int?)null : value;
            }
        }

        public DesktopSize DesktopSize
        {
            get
            {
                return this.Size == null ? DesktopSize.FitToWindow : (DesktopSize)this.Size;
            }
            set
            {
                this.Size = value == DesktopSize.FitToWindow ? null : (byte?)value;
            }
        }

        Colors IDisplayOptions.Colors
        {
            get
            {
                return this.Colors == null ? Terminals.Colors.Bits32 : (Colors)this.Colors;
            }
            set
            {
                this.Colors = value == Terminals.Colors.Bits32 ? null : (byte?)value;
            }
        }

        internal DisplayOptions Copy()
        {
            return new DisplayOptions
                {
                    Height = this.Height,
                    Width = this.Width,
                    DesktopSize = this.DesktopSize,
                    Colors = this.Colors
                };
        }
    }
}
