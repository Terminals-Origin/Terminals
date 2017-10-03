using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Terminals.Common.Forms;
using Terminals.Native;

namespace Terminals.Plugins.Putty
{
    internal class Executables
    {
        private const string RESOURCES = "Resources"; 
        internal const string PUTTY_BINARY = "putty.exe";
        private const string PAGEANT = "pageant";
        internal const string PAGEANT_BINARY = PAGEANT + ".exe";


        internal static void LaunchPutty()
        {
            var puttyBinaryPath = GetPuttyBinaryPath();
            ExternalLinks.OpenPath(puttyBinaryPath);
        }

        internal static void LaunchPageant()
        {
            Process[] pageantProcesses = EnsurePageantProcessRunning();

            if (pageantProcesses.Any())
                ShowEditCertificatesWindow(pageantProcesses[0]);
            else
                MessageBox.Show(Properties.Resources.PageantUnableToStart, PAGEANT_BINARY, MessageBoxButtons.OK);
        }

        private static Process[] EnsurePageantProcessRunning()
        {
            var pageantProcesses = Process.GetProcessesByName(PAGEANT);
            if (pageantProcesses.Any())
                return pageantProcesses;

            var puttyBinaryPath = GetBinaryPath(PAGEANT_BINARY);
            ExternalLinks.OpenPath(puttyBinaryPath);
            return Process.GetProcessesByName(PAGEANT);
        }

        private static void ShowEditCertificatesWindow(Process pageantProcess)
        {
            pageantProcess.WaitForInputIdle(5000);

            IntPtr pageantMainWindowHandle = pageantProcess.MainWindowHandle;

            if (pageantMainWindowHandle == IntPtr.Zero)
            {
                MessageBox.Show(Properties.Resources.PageantMessage, PAGEANT_BINARY, MessageBoxButtons.OK);
                return;
            }

            Methods.ShowWindowAsync(pageantMainWindowHandle, (int)ShowWindowCommands.ShowDefault);
            Methods.ShowWindowAsync(pageantMainWindowHandle, (int)ShowWindowCommands.Show);
            Methods.SetForegroundWindow(pageantMainWindowHandle);
        }

        internal static string GetPuttyBinaryPath()
        {
            return GetBinaryPath(PUTTY_BINARY);
        }

        private static string GetBinaryPath(string assembly)
        {
            string baseLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(baseLocation, RESOURCES, assembly);
        }
    }
}