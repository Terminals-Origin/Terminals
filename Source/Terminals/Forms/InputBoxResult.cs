using System;
using System.Windows.Forms;

namespace Terminals.Forms
{
    /// <summary>
    /// Class used to store the result of an InputBox.Show message.
    /// </summary>
    internal class InputBoxResult 
    {
        public DialogResult ReturnCode { get; set; }

        public String Text { get; set; }
    }
}