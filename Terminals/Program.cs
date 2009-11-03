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
    static class Program
    {
        public static string TerminalsVersion = "1.8b";
//        public static string SupportedProtocols = "RDP, VNC, VMRC, RAS, Telnet, SSH, ICA Citrix, Amazon S3";
        //  reminder to update the buildate for each release
//        public static DateTime BuildDate = new DateTime(2009, 10, 5);  //used for checking project releases.  yeah yeah, this could be smarter about things...
        public static DateTime BuildDate = DateTime.Now;
//        public static string SupportedProtocols = Assembly.GetExecutingAssembly().

        public static Assembly aAssembly = Assembly.GetExecutingAssembly();
        //public static string TerminalsVersion = aAssembly.GetName().Version.ToString();
        public static AssemblyDescriptionAttribute desc = (AssemblyDescriptionAttribute)AssemblyDescriptionAttribute.GetCustomAttribute(aAssembly, typeof(AssemblyDescriptionAttribute));
        public static AssemblyTitleAttribute title = (AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(aAssembly, typeof(AssemblyTitleAttribute));

        public static string AboutText = string.Format("{0} v{1} ({2}) - {3}", title.Title, TerminalsVersion, desc.Description, BuildDate.ToShortDateString());
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
                    Application.Exit();
                else
                    Application.Run(new MainForm());
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
                Terminals.Program.ConfigurationFileLocation = Terminals.MainForm.CommandLineArgs.config;
            }
        }
    }
}