using System;

namespace Terminals.Configuration
{
    internal class FileChangedEventArgs : EventArgs
    {
        public string NewPath { get; set; }
    }
}