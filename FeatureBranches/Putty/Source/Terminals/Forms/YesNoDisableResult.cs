using System.Windows.Forms;

namespace Terminals.Forms
{
    /// <summary>
    /// Extended dialog result to provide access to resulution, when no more prompts are needed
    /// </summary>
    internal class YesNoDisableResult
    {
        internal DialogResult Result { get; private set; }
        internal bool Disable { get; private set; }

        internal YesNoDisableResult(DialogResult result, bool disable)
        {
            this.Result = result;
            this.Disable = disable;
        }
    }
}
