using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using Terminals.CommandLine;
using Terminals.Configuration;
using Terminals.Security;
using Terminals.Updates;
using System.Security.Principal;

namespace Terminals
{
    internal static partial class Program
    {
        private static string TerminalsVersion = "2.0 Release Candidate 1 ";
        public static ResourceManager Resources = new ResourceManager("Terminals.Localization.LocalizedValues", 
            typeof(MainForm).Assembly);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [ComVisible(true)]
        internal static void Main()
        {
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            Logging.Log.Info(String.Format("-------------------------------Title: {0} started Version:{1} Date:{2}-------------------------------", 
                Info.TitleVersion, Info.DLLVersion, Info.BuildDate));

            LogGeneralProperties();
            SetApplicationProperties();
            CommandLineArgs commandLine = ParseCommandline();

            if(UserAccountControlNotSatisfied())
                return;

            if (commandLine.SingleInstance && SingleInstanceApplication.Instance.NotifyExisting(commandLine))
                return;

            UpdateConfig.CheckConfigVersionUpdate();
            UpdateManager.CheckForUpdates(commandLine);
            StartMainForm(commandLine);

            Logging.Log.Info(String.Format("-------------------------------{0} Stopped-------------------------------",
                Info.TitleVersion));
        }

        private static bool UserAccountControlNotSatisfied()
        {
            try 
            {
                LogNonAdministrator();
                string testFile = FileLocations.GetFullPath("WriteAccessCheck.txt");
                
                // Test to make sure that the current user has write access to the current directory.
                using (StreamWriter sw = File.AppendText(testFile)) { }
            }
            catch (Exception ex)
            {
                Logging.Log.FatalFormat("Access Denied {0}" , ex.Message);
                string message = String.Format("{0}\r\n\r\nWrite Access is denied\r\n\r\nPlease make sure you are running as administrator", ex.Message);
                MessageBox.Show(message, "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return true;
            }
            return false;
        }

        private static void LogNonAdministrator()
        {
            if (!(new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)))
            {
                Debug.WriteLine("Terminals is running in non-admin mode");
                Logging.Log.Info("Terminals is running in non-admin mode");
            }
        }

        private static void StartMainForm(CommandLineArgs commandLine)
        {
            if (Settings.IsMasterPasswordDefined)
            {
                using (RequestPassword requestPassword = new RequestPassword())
                {
                    if (requestPassword.ShowDialog() == DialogResult.Cancel)
                        Application.Exit();
                    else
                        RunMainForm(commandLine);
                }
            }
            else
                RunMainForm(commandLine);
        }

        private static void RunMainForm(CommandLineArgs commandLine)
        {
            try
            {
                var mainForm = new MainForm();
                SingleInstanceApplication.Instance.Start(mainForm);
                mainForm.HandleCommandLineActions(commandLine);
                Application.Run(mainForm);
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

        private static CommandLineArgs ParseCommandline()
        {
            var commandline = new CommandLineArgs();
            String[] cmdLineArgs = Environment.GetCommandLineArgs();
            Parser.ParseArguments(cmdLineArgs, commandline);
            if (!string.IsNullOrEmpty(commandline.config))
                Settings.ConfigurationFileLocation = commandline.config;
            return commandline;
        }
    }
}
