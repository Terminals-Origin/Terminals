using System;
using System.Runtime.InteropServices;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using Terminals.CommandLine;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Security;
using Terminals.Updates;

namespace Terminals
{
    internal static partial class Program
    {
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
            Info.SetApplicationVersion();

            Logging.Info(String.Format("-------------------------------Title: {0} started Version:{1} Date:{2}-------------------------------",
                  Info.TitleVersion, Info.DLLVersion, Info.BuildDate));
            Logging.Info("Start state 1 Complete: Unhandled exceptions");

            LogGeneralProperties();
            Logging.Info("Start state 2 Complete: Log General properties");

            SetApplicationProperties();
            Logging.Info("Start state 3 Complete: Set application properties");

            CommandLineArgs commandLine = ParseCommandline();
            Logging.Info("Start state 4 Complete: Parse command line");

            if (!EnsureDataAreWriteAble())
                return;
            Logging.Info("Start state 5 Complete: User account control");
            
            if (commandLine.SingleInstance && SingleInstanceApplication.Instance.NotifyExisting(commandLine))
                return;
            
            Logging.Info("Start state 6 Complete: Set Single instance mode");

            var connectionManager = ConnectionManager.Instance;
            var persistenceFactory = new PersistenceFactory(Settings.Instance, connectionManager, FavoriteIcons.Instance);
            // do it before config update, because it may import favorites from previous version
            IPersistence persistence = persistenceFactory.CreatePersistence();
            Logging.Info("Start state 7 Complete: Initilizing Persistence");

            TryUpdateConfig(persistence, connectionManager);
            Logging.Info("Start state 8 Complete: Configuration upgrade");

            ShowFirstRunWizard(persistence, connectionManager);
            persistence = AuthenticateByMasterPassword(persistenceFactory, persistence);
            RunMainForm(persistence, connectionManager, commandLine);

            Logging.Info(String.Format("-------------------------------{0} Stopped-------------------------------",
                Info.TitleVersion));
        }

        private static void TryUpdateConfig(IPersistence persistence, ConnectionManager connectionManager)
        {
            var updateConfig = new UpdateConfig(persistence, connectionManager);
            updateConfig.CheckConfigVersionUpdate();
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
            Logging.Fatal(messageToLog);
            Logging.Fatal("Application has to be terminated.");
            UnhandledTerminationForm.ShowRipDialog();
            Environment.Exit(-1);
        }

        private static bool EnsureDataAreWriteAble()
        {
            bool hasDataAccess = FileLocations.UserHasAccessToDataDirectory();
            if (!hasDataAccess)
            {
                string message = String.Format("Write Access is denied to:\r\n{0}\r\n" +
                                               "Please make sure you have write permissions to the data directory",
                                               FileLocations.WriteAccessLock);
                MessageBox.Show(message, "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return hasDataAccess;
        }


        private static void ShowFirstRunWizard(IPersistence persistence, ConnectionManager connectionManager)
        {
            if (Settings.Instance.ShowWizard)
            {
                //settings file doesn't exist
                using (var wzrd = new FirstRunWizard(persistence, connectionManager))
                    wzrd.ShowDialog();
            }
        }

        private static IPersistence AuthenticateByMasterPassword(PersistenceFactory persistenceFactory, IPersistence persistence)
        {
            IPersistence newPersistence = persistence;
            PersistenceErrorForm.RegisterDataEventHandler(persistence.Dispatcher);

            if (!persistence.Security.Authenticate(RequestPassword.KnowsUserPassword))
            {
                if (PersistenceFallback())
                    newPersistence = persistenceFactory.FallBackToPrimaryPersistence(persistence.Security);
                else
                    Environment.Exit(-1);
            }

            return newPersistence;
        }

        private static void RunMainForm(IPersistence persistence, ConnectionManager connectionManager, CommandLineArgs commandLine)
        {
            var mainForm = new MainForm(persistence, connectionManager);
            SingleInstanceApplication.Instance.Initialize(mainForm, commandLine);
            mainForm.HandleCommandLineActions(commandLine);
            Application.Run(mainForm);
        }

        /// <summary>
        /// dump out common/useful debugging data at app start
        /// </summary>
        private static void LogGeneralProperties()
        {
            Logging.Info(String.Format("CommandLine:{0}", Environment.CommandLine));
            Logging.Info(String.Format("CurrentDirectory:{0}", Environment.CurrentDirectory));
            Logging.Info(String.Format("MachineName:{0}", Environment.MachineName));
            Logging.Info(String.Format("OSVersion:{0}", Environment.OSVersion));
            Logging.Info(String.Format("ProcessorCount:{0}", Environment.ProcessorCount));
            Logging.Info(String.Format("UserInteractive:{0}", Environment.UserInteractive));
            Logging.Info(String.Format("Version:{0}", Environment.Version));
            Logging.Info(String.Format("WorkingSet:{0}", Environment.WorkingSet));
            Logging.Info(String.Format("Is64BitOperatingSystem:{0}", Native.Wow.Is64BitOperatingSystem));
            Logging.Info(String.Format("Is64BitProcess:{0}", Native.Wow.Is64BitProcess));
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
            Settings.Instance.FileLocations.AssignCustomFileLocations(commandline.configFile,
                commandline.favoritesFile, commandline.credentialsFile);
            return commandline;
        }

        private static bool PersistenceFallback()
        {
            const string MESSAGE = "Do you wan't to start with local files store?\r\n" +
                                   "Yes - start with files store (You will be able to fix the configuration.)\r\n" +
                                   "No - Exit application";
            DialogResult fallback = MessageBox.Show(MESSAGE, "Terminals database connection failed",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            return fallback == DialogResult.Yes;
        }
    }
}
