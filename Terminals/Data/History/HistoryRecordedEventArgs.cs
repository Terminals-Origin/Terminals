using System;

namespace Terminals.History
{
    /// <summary>
    /// Event arguments informing about favorite added to the history
    /// </summary>
    internal class HistoryRecordedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the favorite name added to the history
        /// </summary>
        internal string ConnectionName { get; set; }
    }
}
