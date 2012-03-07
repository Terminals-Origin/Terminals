using System;

namespace Terminals.Data
{
    public interface IDisplayOptions
    {
        Int32 Height { get; set; }
        Int32 Width { get; set; }
        DesktopSize DesktopSize { get; set; }
        Colors Colors { get; set; }
    }
}