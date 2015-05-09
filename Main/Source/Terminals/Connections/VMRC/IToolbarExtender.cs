using System.Windows.Forms;

namespace Terminals.Connections
{
    internal interface IToolbarExtender
    {
        /// <summary>
        /// Allowes customize toolbar buttons
        /// </summary>
        /// <param name="standardToolbar">Not null toolbar to extend</param>
        void Visit(ToolStrip standardToolbar);
    }
}