using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Docking
{
    [System.Serializable()]
    public class DockSavePositions
    {
        public DockSavePosition Top;
        public DockSavePosition Bottom;
        public DockSavePosition Left;
        public DockSavePosition Right;
    }
}
