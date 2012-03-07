using System;
using System.Xml.Serialization;

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

        [XmlIgnore]
        public Int32 PerformanceFlags
        {
            get
            {
                Int32 favorite = 0;

                if (DisableCursorShadow) favorite += (Int32)PerfomanceOptions.TS_PERF_DISABLE_CURSOR_SHADOW;
                if (DisableCursorBlinking) favorite += (Int32)PerfomanceOptions.TS_PERF_DISABLE_CURSORSETTINGS;
                if (DisableFullWindowDrag) favorite += (Int32)PerfomanceOptions.TS_PERF_DISABLE_FULLWINDOWDRAG;
                if (DisableMenuAnimations) favorite += (Int32)PerfomanceOptions.TS_PERF_DISABLE_MENUANIMATIONS;
                if (DisableTheming) favorite += (Int32)PerfomanceOptions.TS_PERF_DISABLE_THEMING;
                if (DisableWallPaper) favorite += (Int32)PerfomanceOptions.TS_PERF_DISABLE_WALLPAPER;
                if (EnableDesktopComposition) favorite += (Int32)PerfomanceOptions.TS_PERF_ENABLE_DESKTOP_COMPOSITION;
                if (EnableFontSmoothing) favorite += (Int32)PerfomanceOptions.TS_PERF_ENABLE_FONT_SMOOTHING;

                return favorite;
            }
        }

        internal RdpUserInterfaceOptions Copy()
        {
            return new RdpUserInterfaceOptions
                {
                    DisableWindowsKey = this.DisableWindowsKey,
                    DoubleClickDetect = this.DoubleClickDetect,
                    DisplayConnectionBar = this.DisplayConnectionBar,
                    DisableControlAltDelete = this.DisableControlAltDelete,
                    AcceleratorPassthrough = this.AcceleratorPassthrough,
                    EnableCompression = this.EnableCompression,
                    BitmapPeristence = this.BitmapPeristence,
                    AllowBackgroundInput = this.AllowBackgroundInput,
                    DisableTheming = this.DisableTheming,
                    DisableMenuAnimations = this.DisableMenuAnimations,
                    DisableFullWindowDrag = this.DisableFullWindowDrag,
                    DisableCursorBlinking = this.DisableCursorBlinking,
                    EnableDesktopComposition = this.EnableDesktopComposition,
                    EnableFontSmoothing = this.EnableFontSmoothing,
                    DisableCursorShadow = this.DisableCursorShadow,
                    DisableWallPaper = this.DisableWallPaper
                };
        }

        internal void FromConfigFavorite(FavoriteConfigurationElement favorite)
        {
            this.AcceleratorPassthrough = favorite.AcceleratorPassthrough;
            this.AllowBackgroundInput = favorite.AllowBackgroundInput;
            this.BitmapPeristence = favorite.BitmapPeristence;
            this.DisableControlAltDelete = favorite.DisableControlAltDelete;
            this.DisableCursorBlinking = favorite.DisableCursorBlinking;
            this.DisableCursorShadow = favorite.DisableCursorShadow;
            this.DisableFullWindowDrag = favorite.DisableFullWindowDrag;
            this.DisableMenuAnimations = favorite.DisableMenuAnimations;
            this.DisableTheming = favorite.DisableTheming;
            this.DisableWallPaper = favorite.DisableWallPaper;
            this.DisableWindowsKey = favorite.DisableWindowsKey;
            this.DisplayConnectionBar = favorite.DisplayConnectionBar;
            this.DoubleClickDetect = favorite.DoubleClickDetect;
            this.EnableCompression = favorite.EnableCompression;
            this.EnableDesktopComposition = favorite.EnableDesktopComposition;
            this.EnableFontSmoothing = favorite.EnableFontSmoothing;
        }

        internal void ToConfigFavorite(FavoriteConfigurationElement favorite)
        {
            favorite.AcceleratorPassthrough = this.AcceleratorPassthrough;
            favorite.AllowBackgroundInput = this.AllowBackgroundInput;
            favorite.BitmapPeristence = this.BitmapPeristence;
            favorite.DisableControlAltDelete = this.DisableControlAltDelete;
            favorite.DisableCursorBlinking = this.DisableCursorBlinking;
            favorite.DisableCursorShadow = this.DisableCursorShadow;
            favorite.DisableFullWindowDrag = this.DisableFullWindowDrag;
            favorite.DisableMenuAnimations = this.DisableMenuAnimations;
            favorite.DisableTheming = this.DisableTheming;
            favorite.DisableWallPaper = this.DisableWallPaper;
            favorite.DisableWindowsKey = this.DisableWindowsKey;
            favorite.DisplayConnectionBar = this.DisplayConnectionBar;
            favorite.DoubleClickDetect = this.DoubleClickDetect;
            favorite.EnableCompression = this.EnableCompression;
            favorite.EnableDesktopComposition = this.EnableDesktopComposition;
            favorite.EnableFontSmoothing = this.EnableFontSmoothing;
        }
    }
}
