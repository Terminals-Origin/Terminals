namespace Terminals
{
  /// <summary>
  /// Defines, how the terminal screen size will be resized.
  /// Default values used in configuration is FitToWindow.
  /// </summary>
  public enum DesktopSize
  {
    x640 = 0,
    x800,
    x1024,
    x1152,
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
