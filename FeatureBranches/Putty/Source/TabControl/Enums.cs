using System;
using System.Collections.Generic;
using System.Text;

namespace TabControl
{
    /// <summary>
    /// Theme Type
    /// </summary>
    public enum ThemeTypes
    {
        WindowsXP,
        Office2000,
        Office2003
    }

    /// <summary>
    /// Indicates a change into TabControl collection
    /// </summary>
    public enum TabControlItemChangeTypes
    {
        Added,
        Removed,
        Changed,
        SelectionChanged
    }
}
