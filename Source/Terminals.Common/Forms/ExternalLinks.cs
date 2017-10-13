using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Terminals.Common.Forms
{
    public class ExternalLinks
    {
        public static void OpenPath(string uri, string arguments = "")
        {
            try
            {
                Task.Factory.StartNew(() => Process.Start(uri, arguments));
            }
            catch (Exception)
            {
                string message = string.Format("Unable to open path:\r\n'{0}'", uri);
                MessageBox.Show(message, "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
