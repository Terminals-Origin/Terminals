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
        
        public static string TerminalsVersion = "1.9a";
        public static DateTime BuildDate = RetrieveLinkerTimestamp();
        public static Assembly aAssembly = Assembly.GetExecutingAssembly();
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

        /// <summary>
        /// Taken from http://stackoverflow.com/questions/1600962/c-displaying-the-build-date
        /// (code by Joe Spivey)
        /// </summary>
        /// <returns></returns>
        private static DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                    s.Close();
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        } 

    }
}
