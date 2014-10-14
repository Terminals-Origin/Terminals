using TabControl;
using Terminals.Connections;
using Terminals.Data;

namespace Terminals
{
    internal class TerminalTabControlItem : TabControlItem
    {
        public IConnection Connection { get; set; }

        public IFavorite Favorite { get; set; }

        public TerminalTabControlItem(string caption)
            : base(caption, null)
        {
        }
    }
}
