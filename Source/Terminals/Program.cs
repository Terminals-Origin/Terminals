using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using Terminals.CommandLine;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Security;
using Terminals.Updates;
using System.Security.Principal;

namespace Terminals
{
    internal static partial class Program
    {
        private static string TerminalsVersion;
        public static ResourceManager Resources = new ResourceManager("Terminals.Localization.LocalizedValues",
            typeof(MainForm).Assembly);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [ComVisible(true)]
        internal static void Main()
        {
            SetUnhandledExceptions();
            SetApplicationVersion();

            Logging.Log.Info(String.Format("-------------------------------Title: {0} started Version:{1} Date:{2}-------------------------------",
                  Info.TitleVersion, Info.DLLVersion, Info.BuildDate));
            Logging.Log.Info("Start state 1 Complete: Unhandled exceptions");

            LogGeneralProperties();
            Logging.Log.Info("Start state 2 Complete: Log General properties");

            SetApplicationProperties();
            Logging.Log.Info("Start state 3 Complete: Set application properties");

            CommandLineArgs commandLine = ParseCommandline();
            Logging.Log.Info("Start state 4 Complete: Parse command line");

            if (UserAccountControlNotSatisfied())
                return;
            Logging.Log.Info("Start state 5 Complete: User account control");
            
            if (commandLine.SingleInstance && SingleInstanceApplication.Instance.NotifyExisting(commandLine))
                return;
            Logging.Log.Info("Start state 6 Complete: Set Single instance mode");

            UpdateConfig.CheckConfigVersionUpdate();
            Logging.Log.Info("Start state 7 Complete: Configuration upgrade");

            UpdateManager.CheckForUpdates(commandLine);
            Logging.Log.Info("Start state 8 Complete: Check application updates");

            StartMainForm(commandLine);

            Logging.Log.Info(String.Format("-------------------------------{0} Stopped-------------------------------",
                Info.TitleVersion));
        }

        private static void SetApplicationVersion()
        {
            //MAJOR.MINOR.PATCH.BUILD
            //MAJOR == Breaking Changes in API or features
            //MINOR == Non breaking changes, but significant feature changes
            //PATH (Or Revision) == Bug fixes only, etc...
            //BUILD == Build increments
            //
            //Incremental builds, daily, etc will include full M.M.P.B
            //Release builds only include M.M.P
            //
            //.NET Likes:  MAJOR.MINOR.BUILD.REVISION

            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

#if DEBUG
            TerminalsVersion = version.ToString(); //debug builds, to keep track of minor/revisions, etc..
#else
        TerminalsVersion = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);        
#endif
        }

        private static void SetUnhandledExceptions()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomainUnhandledException);
            Application.ThreadException += new ThreadExceptionEventHandler(ApplicationThreadException);
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowApplicationExit(e.ExceptionObject);
        }

        private static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ShowApplicationExit(e.Exception);
        }

        private static void ShowApplicationExit(object messageToLog)
        {
            Logging.Log.Fatal(messageToLog);
            Logging.Log.Fatal("Application has to be terminated.");
            UnhandledTerminationForm.ShowRipDialog();
        }

        private static bool UserAccountControlNotSatisfied()
        {
            try
            {
                LogNonAdministrator();
                string testFile = FileLocations.WriteAccessLock;

                // Test to make sure that the current user has write access to the current directory.
                using (StreamWriter sw = File.AppendText(testFile)) { }
            }
            catch (Exception ex)
            {
                Logging.Log.FatalFormat("Access Denied {0}", ex.Message);
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
            IPersistence persistence = Persistence.Instance;
            PersistenceErrorForm.RegisterDataEventHandler(persistence.Dispatcher);
            if (persistence.Security.Authenticate(RequestPassword.KnowsUserPassword))
                RunMainForm(commandLine);
            else
                Application.Exit();
        }

        private static void RunMainForm(CommandLineArgs commandLine)
        {
            var mainForm = new MainForm();
            SingleInstanceApplication.Instance.Initialize(mainForm, commandLine);
            mainForm.HandleCommandLineActions(commandLine);
            Application.Run(mainForm);
        }

        /// <summary>
        /// dump out common/useful debugging data at app start
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
            Logging.Log.Info(String.Format("Is64BitOperatingSystem:{0}", Native.Wow.Is64BitOperatingSystem));
            Logging.Log.Info(String.Format("Is64BitProcess:{0}", Native.Wow.Is64BitProcess));
        }

        private static void SetApplicationProperties()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        private static CommandLineArgs ParseCommandline()
        {
            var commandline = new CommandLineArgs();
            String[] cmdLineArgs = Environment.GetCommandLineArgs();
            Parser.ParseArguments(cmdLineArgs, commandline);
            Settings.FileLocations.AssignCustomFileLocations(commandline.configFile,
                commandline.favoritesFile, commandline.credentialsFile);
            return commandline;
        }
    }
}
