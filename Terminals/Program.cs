using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Resources;
using System.Reflection;
using System.Threading;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    static partial class Program
    {
        private static string TerminalsVersion = "2.0 RC2";
        
        public static Mutex mtx;

        public static string FlickrAPIKey = "9362619635c6f6c20e7c14fe4b67c2a0";
        public static string FlickrSharedSecretKey = "ac8f3c60be0812b6";
        public static string ConfigurationFileLocation = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"Terminals.config");

        public static ResourceManager Resources = new ResourceManager("Terminals.Localization.LocalizedValues", typeof(MainForm).Assembly);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [ComVisible(true)]
        static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            mtx = new Mutex(false, "TerminalsMutex");

            Terminals.Logging.Log.Info(String.Format("{0} started", Info.TitleVersion));
            
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            ParseCommandline();

            if (ReuseExistingInstance() && SingleInstanceApplication.NotifyExistingInstance(Environment.GetCommandLineArgs()))
                return;

            SingleInstanceApplication.Initialize();

            // Check if update changes have to be made
            Updates.UpdateConfig.CheckConfigVersionUpdate();

            // Check for available application updates
            Terminals.Updates.UpdateManager.CheckForUpdates();

            // Check for Terminals master password
            if (Settings.TerminalsPassword != String.Empty)
            {
                Security.RequestPassword rp = new Terminals.Security.RequestPassword();
                DialogResult result = rp.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    Application.Exit();
                }
                else
                {
                    rp.Dispose();
                    Application.Run(new MainForm());
                }
            }
            else
            {
                try
                {
                    Application.Run(new MainForm());
                }
                catch (Exception exc)
                {
                    Terminals.Logging.Log.Fatal("Main Form Execption", exc);
                }
            }

            SingleInstanceApplication.Close();
            Terminals.Logging.Log.Info(String.Format("{0} stopped", Info.TitleVersion));
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Terminals.Logging.Log.Fatal("Application Exception", e.Exception);
        }

        private static Boolean ReuseExistingInstance()
        {
            if (Settings.SingleInstance)
                return true;

            String[] cmdLineArgs = Environment.GetCommandLineArgs();
            return (cmdLineArgs.Length > 1 && cmdLineArgs[1] == "/reuse");
        }

        private static void ParseCommandline()
        {
            String[] cmdLineArgs = Environment.GetCommandLineArgs();
            ParseCommandline(cmdLineArgs);
        }

        private static void ParseCommandline(String[] cmdLineArgs)
        {
            Terminals.CommandLine.Parser.ParseArguments(cmdLineArgs, Terminals.MainForm.CommandLineArgs);
            if (Terminals.MainForm.CommandLineArgs.config != null && Terminals.MainForm.CommandLineArgs.config != String.Empty)
            {
                Terminals.Program.ConfigurationFileLocation = Terminals.MainForm.CommandLineArgs.config;
            }
        }
    }
}
