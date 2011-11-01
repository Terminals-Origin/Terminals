using System;
using System.Runtime.InteropServices;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Security;
using Terminals.Updates;

namespace Terminals
{
    static partial class Program
    {
        private static string TerminalsVersion = "2.0 Beta";
        
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

            Logging.Log.Info(String.Format("-------------------------------{0} started-------------------------------", Info.TitleVersion));

            //dump out commong/useful debugging data at app start
            Logging.Log.Info(String.Format("CommandLine:{0}", System.Environment.CommandLine));
            Logging.Log.Info(String.Format("CurrentDirectory:{0}", System.Environment.CurrentDirectory));
            Logging.Log.Info(String.Format("MachineName:{0}", System.Environment.MachineName));
            Logging.Log.Info(String.Format("OSVersion:{0}", System.Environment.OSVersion));
            Logging.Log.Info(String.Format("ProcessorCount:{0}", System.Environment.ProcessorCount));
            Logging.Log.Info(String.Format("UserInteractive:{0}", System.Environment.UserInteractive));
            Logging.Log.Info(String.Format("Version:{0}", System.Environment.Version));
            Logging.Log.Info(String.Format("WorkingSet:{0}", System.Environment.WorkingSet));
            

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            ParseCommandline();

            if (ReuseExistingInstance() && SingleInstanceApplication.NotifyExistingInstance(Environment.GetCommandLineArgs()))
                return;

            SingleInstanceApplication.Initialize();

            // Check if update changes have to be made
            UpdateConfig.CheckConfigVersionUpdate();

            // Check for available application updates
            UpdateManager.CheckForUpdates();

            // Check for Terminals master password
            if (Settings.IsMasterPasswordDefined)
            {
                RequestPassword rp = new RequestPassword();
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
                    Logging.Log.Fatal("Main Form Execption", exc);
                }
            }

            SingleInstanceApplication.Close();
            Logging.Log.Info(String.Format("-------------------------------{0} Stopped-------------------------------", Info.TitleVersion));
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
                ConfigurationFileLocation = MainForm.CommandLineArgs.config;
            }
        }
    }
}
