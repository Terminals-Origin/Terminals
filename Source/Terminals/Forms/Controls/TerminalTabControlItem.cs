using TabControl;
using Terminals.Connections;
using Terminals.Data;

namespace Terminals
{
    internal class TerminalTabControlItem : TabControlItem
    {
        /// <summary>
        /// Gets or sets the associated favorite.
        /// Not all tabs are using it, so it is optional.
        /// </summary>
        public IConnection Connection { get; set; }

        /// <summary>
        /// Gets or sets the associated favorite.
        /// Not all tabs are using it, so it is optional.
        /// </summary>
        public IFavorite Favorite { get; set; }

        public TerminalTabControlItem(string caption)
            : base(caption, null)
        {
        }
    }
}
