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
        public static string TerminalsVersion = "1.6l";
        //  reminder to update the buildate for each release
        public static DateTime BuildDate = new DateTime(2008, 5, 11);  //used for checking project releases.  yeah yeah, this could be smarter about things...
        public static Mutex mtx;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [ComVisible(true)]
        static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            mtx = new Mutex(false, "TerminalsMutex");

            Terminals.Logging.Log.Info("Terminals " + Program.TerminalsVersion + " started");
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            ParseCommandline();

            if(ReuseExistingInstance() && SingleInstanceApplication.NotifyExistingInstance(Environment.GetCommandLineArgs()))
                return;

            SingleInstanceApplication.Initialize();

            Terminals.Updates.UpdateManager.CheckForUpdates();

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
                try {
                    Application.Run(new MainForm());
                } catch(Exception exc) {
                    Terminals.Logging.Log.Error("Main Form Execption",exc);
                }
            }
            SingleInstanceApplication.Close();
            Terminals.Logging.Log.Info("Terminals " + Program.TerminalsVersion + " stopped");
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
            Terminals.Logging.Log.Fatal("Application Exception", e.Exception);
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