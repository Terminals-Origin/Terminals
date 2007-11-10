using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;

namespace Terminals
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [ComVisible(true)]
        static void Main()
        {
            Terminals.Logging.Log.Info("Terminals 1.6e started");
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ParseCommandline();

            if(ReuseExistingInstance() && SingleInstanceApplication.NotifyExistingInstance(Environment.GetCommandLineArgs()))
                return;

            SingleInstanceApplication.Initialize();


            if(Settings.TerminalsPassword != "")
            {
                Security.RequestPassword rp = new Terminals.Security.RequestPassword();
                DialogResult result = rp.ShowDialog();
                if(result == DialogResult.Cancel)
                {
                    Application.Exit();
                }
                else
                {
                    Application.Run(new MainForm());
                }
            }
            else
            {
                Application.Run(new MainForm());
            }
            SingleInstanceApplication.Close();

        }

        private static bool ReuseExistingInstance()
        {
            if(Settings.SingleInstance)
                return true;
            string[] cmdLineArgs = Environment.GetCommandLineArgs();
            return (cmdLineArgs.Length > 1 && cmdLineArgs[1] == "/reuse");
        }
        private static void ParseCommandline()
        {
            string[] cmdLineArgs = Environment.GetCommandLineArgs();
            ParseCommandline(cmdLineArgs);
        }

        private static void ParseCommandline(string[] cmdLineArgs)
        {
            Terminals.CommandLine.Parser.ParseArguments(cmdLineArgs, Terminals.MainForm.CommandLineArgs);
            if(Terminals.MainForm.CommandLineArgs.config != null && Terminals.MainForm.CommandLineArgs.config != "")
            {
                Terminals.MainForm.ConfigurationFileLocation = Terminals.MainForm.CommandLineArgs.config;
            }
        }
    }
}