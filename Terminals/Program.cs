using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Security;
using Terminals.Updates;

namespace Terminals
{
    internal static partial class Program
    {
        private static string TerminalsVersion = "2.0 Beta";
        
        public static Mutex mtx;
        public static ResourceManager Resources = new ResourceManager("Terminals.Localization.LocalizedValues", typeof(MainForm).Assembly);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [ComVisible(true)]
        internal static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            mtx = new Mutex(false, "TerminalsMutex");

            Logging.Log.Info(String.Format("-------------------------------{0} started-------------------------------", 
                Info.TitleVersion));

            LogGeneralProperties();
            SetApplicationProperties();
            ParseCommandline();

            if (ReuseExistingInstance() && SingleInstanceApplication.NotifyExistingInstance(Environment.GetCommandLineArgs()))
                return;

            SingleInstanceApplication.Initialize();

            // Check if update changes have to be made
            UpdateConfig.CheckConfigVersionUpdate();

            // Check for available application updates
            UpdateManager.CheckForUpdates();

            StartMainForm();

            SingleInstanceApplication.Close();
            Logging.Log.Info(String.Format("-------------------------------{0} Stopped-------------------------------",
                Info.TitleVersion));
        }

        private static void StartMainForm()
        {
            if (Settings.IsMasterPasswordDefined)
            {
                using (RequestPassword requestPassword = new RequestPassword())
                {
                    if (requestPassword.ShowDialog() == DialogResult.Cancel)
                        Application.Exit();
                    else
                        RunMainForm();
                }
            }
            else
            {
                RunMainForm();
            }
        }

        private static void RunMainForm()
        {
            try
            {
                Application.Run(new MainForm());
            }
            catch (Exception exc)
            {
                Logging.Log.Fatal("Main Form Execption", exc);
            }
        }

        /// <summary>
        /// dump out commong/useful debugging data at app start
        /// </summary>
        private static void LogGeneralProperties() 
        {
            Logging.Log.Info(String.Format("CommandLine:{0}", Environment.CommandLine));
            Logging.Log.Info(String.Format("CurrentDirectory:{0}", Environment.CurrentDirectory));
            Logging.Log.Info(String.Format("MachineName:{0}", Environment.MachineName));
            Logging.Log.Info(String.Format("OSVersion:{0}", Environment.OSVersion));
            Logging.Log.Info(String.Format("ProcessorCount:{0}", Environment.ProcessorCount));
            Logging.Log.Info(String.Format("UserInteractive:{0}", Environment.UserInteractive));
            Logging.Log.Info(String.Format("Version:{0}", Environment.Version));
            Logging.Log.Info(String.Format("WorkingSet:{0}", Environment.WorkingSet));
        }

        private static void SetApplicationProperties()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Logging.Log.Fatal("Application Exception", e.Exception);
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
            CommandLine.Parser.ParseArguments(cmdLineArgs, MainForm.CommandLineArgs);
            if (MainForm.CommandLineArgs.config != null && MainForm.CommandLineArgs.config != String.Empty)
            {
                Settings.ConfigurationFileLocation = MainForm.CommandLineArgs.config;
            }
        }
    }
}
