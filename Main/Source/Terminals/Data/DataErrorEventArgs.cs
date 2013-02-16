using System;

namespace Terminals.Data
{
    internal class DataErrorEventArgs: EventArgs
    {
        internal string Message { get; set; }

        internal bool CallStackFull { get; set; }
    }
}
