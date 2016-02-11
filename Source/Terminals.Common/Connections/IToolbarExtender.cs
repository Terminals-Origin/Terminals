using System.Windows.Forms;

namespace Terminals.Connections
{
    public interface IToolbarExtender
    {
        /// <summary>
        /// Allowes customize toolbar buttons
        /// </summary>
        /// <param name="toolbar">Not null toolbar to extend</param>
        void Visit(ToolStrip toolbar);
    }
}