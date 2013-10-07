using System.Windows.Forms;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Encaupsulation of Windows Forms Control.ModifierKeys static members.
    /// </summary>
    internal class KeyModifiers : IKeyModifiers
    {
        public bool WithShift
        {
            get { return Control.ModifierKeys.HasFlag(Keys.Shift); }
        }

        public bool WithControl
        {
            get { return Control.ModifierKeys.HasFlag(Keys.Control); }
        }
    }
}