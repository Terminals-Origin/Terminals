using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WalburySoftware
{
    public partial class TerminalEmulator : Control
    {
        private void DispatchMessage(System.Object sender, string strText)
        {
            if (this.XOFF == true || OnDataRequested == null)
            {
                // store the characters in the outputbuffer
                OutBuff += strText;
            }
            else
            {
                if (OutBuff != String.Empty)
                {
                    strText = OutBuff + strText;
                    OutBuff = String.Empty;
                }

                if (strText == '\r'.ToString())
                {
                    history.Add(keyboardBuffer);
                    keyboardBuffer = String.Empty;
                }
                else if (strText == "Paste")
                {
                    strText = string.Empty;
                    DispatchMessage(this, Clipboard.GetText());
                }
                else
                {
                    if (this.Keyboard.UpArrow)
                    {
                        // wipe the current input
                        // replace it with the history index -1
                    }
                    else
                    {
                        keyboardBuffer += strText;
                    }
                }

                OnDataRequested(Encoding.Default.GetBytes(strText));
            }
        }
    }
}
