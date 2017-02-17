﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Data;
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

        internal static void CallExecuteBeforeConnected(IBeforeConnectExecuteOptions executeOptions)
        {
            if (executeOptions.Execute && !string.IsNullOrEmpty(executeOptions.Command))
            {
                var processStartInfo = new ProcessStartInfo(executeOptions.Command, executeOptions.CommandArguments);
                processStartInfo.WorkingDirectory = executeOptions.InitialDirectory;
                Process process = Process.Start(processStartInfo);
                if (executeOptions.WaitForExit)
                {
                    process.WaitForExit();
                }
            }
        }

        internal static void Launch(SpecialCommandConfigurationElement command)
        {
            try
            {
                TryLaunch(command);
            }
            catch (Exception ex)
            {
                string message = String.Format("Could not Launch the shortcut application: '{0}'", command.Name);
                MessageBox.Show(message);
                Logging.Error(message, ex);
            }
        }

        private static void TryLaunch(SpecialCommandConfigurationElement command)
        {
            string exe = command.Executable;
            if (exe.Contains("%"))
                exe = exe.Replace("%systemroot%", Environment.GetEnvironmentVariable("systemroot"));

            var startInfo = new ProcessStartInfo(exe, command.Arguments);
            startInfo.WorkingDirectory = command.WorkingFolder;
            Process.Start(startInfo);
        }
    }
}
