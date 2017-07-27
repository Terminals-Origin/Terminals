using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Terminals.Common.Forms
{
    public class ExternalLinks
    {
        public static void OpenPath(string uri)
        {
            try
            {
                Process.Start(uri);
            }
            catch (Exception)
            {
                string message = string.Format("Unable to open path:\r\n'{0}'", uri);
                MessageBox.Show(message, "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
