namespace Terminals
{
    /// <summary>
    /// Defines, how the terminal screen size will be resized.
    /// Default values used in configuration is FitToWindow.
    /// </summary>
    public enum DesktopSize
    {
        /// <summary>
        /// 640 x 480
        /// </summary>
        x640 = 0,

        /// <summary>
        /// 800 x 600
        /// </summary>
        x800,

        /// <summary>
        /// 1024 x 768
        /// </summary>
        x1024,

        /// <summary>
        /// 1152 x 864
        /// </summary>
        x1152,

        /// <summary>
        /// 1280 x 1024
        /// </summary>
        x1280,

        /// <summary>
        /// The terminal window will respect its parent window and is resizable.
        /// </summary>
        FitToWindow,

        /// <summary>
        /// The window has fixed size, which respects the maximum screen (monitor) size.
        /// </summary>
        FullScreen,

        /// <summary>
        /// Terminal window is resizable up to the maximum window size defined by user.
        /// </summary>
        AutoScale,

        /// <summary>
        /// Terminal window respects the size defined by the user and cant be changed.
        /// </summary>
        Custom
    }
}
