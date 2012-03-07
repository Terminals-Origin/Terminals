using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Terminals
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            string args = "";
            string filename = "";
            try
            {
                // If we start TerminalsLauncher, then if trm files are not already registered, then we should register
                // them with TerminalsLauncher. This means that ProtocolHandler.Register call in the main routine should
                // not do anything as the the extension should already be registered.
                ProtocolHandler.Register();

                string[] commandLineArgs = Environment.GetCommandLineArgs();
                args = String.Join(" ", commandLineArgs, 1, commandLineArgs.Length-1);
                filename = Path.Combine(Path.GetDirectoryName(commandLineArgs[0]), "Terminals.exe");
                Process.Start(filename, args);
            }
            catch (Exception ex)
            {
                Logging.Log.FatalFormat("Error Starting Terminals:{0}: Filename:{1} Args:{2}" ,
                    ex.Message, filename , args);
                string message = String.Format("{0}\r\n\r\nCmdLine\r\n{1} {2}", ex.Message, filename, args);
                MessageBox.Show(message, "Error starting Terminals");
            }
        }
    }
}
