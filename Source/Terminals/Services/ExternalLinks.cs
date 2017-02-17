using System;
using System.Diagnostics;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Forms;
using Terminals.Updates;

namespace Terminals.Services
{
    internal class ExternalLinks
    {
        internal static string TerminalsReleasesUrl
        {
            get { return Program.Resources.GetString("TerminalsURL"); }
        }

        internal static void ShowWinPCapPage()
        {
            const string MESSAGE = "It appears that WinPcap is not installed.  In order to use this feature within Terminals you must first install that product.  Do you wish to visit the download location right now?";
            if (MessageBox.Show(MESSAGE, "Download WinPcap?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                OpenPath("http://www.winpcap.org/install/default.htm");
            }
        }

        internal static void AskIfShowReleasePage(Settings settings, ReleaseInfo releaseInfo)
        {
            string message = string.Format("Version:{0}\r\nPublished:{1}\r\nDo you want to show the Terminals home page?",
                                            releaseInfo.Version, releaseInfo.Published);
            YesNoDisableResult answer = YesNoDisableForm.ShowDialog("New release is available", message);
            if (answer.Result == DialogResult.Yes)
                ShowReleasePage();

            if (answer.Disable)
                settings.NeverShowTerminalsWindow = true;
        }

        internal static void ShowReleasePage()
        {
            OpenPath("http://" + TerminalsReleasesUrl);
        }

        internal static void OpenAuthorPage()
        {
            OpenPath("http://weblogs.asp.net/rchartier/");
        }

        internal static void OpenLogsFolder()
        {
            OpenPath(FileLocations.LogDirectory);
        }

        internal static void OpenPath(string uri)
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
