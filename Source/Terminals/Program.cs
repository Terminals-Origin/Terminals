using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using SqlScriptRunner.Versioning;
using Terminals.CommandLine;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.DB;
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
        TerminalsVersion =  version.ToString();  //debug builds, to keep track of minor/revisions, etc..
#else
        TerminalsVersion = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);        
#endif


        Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            Logging.Log.Info(String.Format("-------------------------------Title: {0} started Version:{1} Date:{2}-------------------------------", 
                Info.TitleVersion, Info.DLLVersion, Info.BuildDate));

            Logging.Log.Info("State 1 Complete");

            LogGeneralProperties();

            Logging.Log.Info("State 2 Complete");
            SetApplicationProperties();

            UpgradeDatabaseVersion();

            Logging.Log.Info("State 3 Complete");
            CommandLineArgs commandLine = ParseCommandline();

            Logging.Log.Info("State 4 Complete");

            if(UserAccountControlNotSatisfied())
                return;
            
            Logging.Log.Info("State 5 Complete");

            if (commandLine.SingleInstance && SingleInstanceApplication.Instance.NotifyExisting(commandLine))
                return;

            Logging.Log.Info("State 6 Complete");

            UpdateConfig.CheckConfigVersionUpdate();

            Logging.Log.Info("State 7 Complete");

            UpdateManager.CheckForUpdates(commandLine);

            Logging.Log.Info("State 8 Complete");

            StartMainForm(commandLine);

            Logging.Log.Info("State 9 Complete");

            Logging.Log.Info(String.Format("-------------------------------{0} Stopped-------------------------------",
                Info.TitleVersion));
        }

        private static void UpgradeDatabaseVersion()
        {
            bool filePersistence = Settings.PersistenceType == 0;
            Logging.Log.InfoFormat("File Persistence Mode:{0}", filePersistence);

            if (!filePersistence)
            {
                string connectionString = Settings.ConnectionString;
                var dbVersion = Database.DatabaseVersion(connectionString);

                var root = (new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)).Directory.FullName;
                var migrationsRoot = System.IO.Path.Combine(root, "Migrations");
                if (System.IO.Directory.Exists(migrationsRoot))
                {
                    var parser = new JustVersionParser();
                    var list = SqlScriptRunner.ScriptRunner.ResolveScriptsFromPathAndVersion(migrationsRoot, "*.sql", true,
                                                                                  migrationsRoot, dbVersion,
                                                                                  SqlScriptRunner.Versioning.Version.Max,
                                                                                  parser);
                }
            }

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
            var security = Persistence.Instance.Security;
            if (security.Authenticate(RequestPassword.KnowsUserPassword))
                RunMainForm(commandLine);
            else
                Application.Exit(); 
        }

        private static void RunMainForm(CommandLineArgs commandLine)
        {
            try
            {
                var mainForm = new MainForm();
                SingleInstanceApplication.Instance.Initialize(mainForm, commandLine);
                mainForm.HandleCommandLineActions(commandLine);
                Application.Run(mainForm);
            }
            catch (Exception exc)
            {
                Logging.Log.Fatal("Main Form Exception", exc);
            }
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
            Logging.Log.Info(String.Format("Is64BitOperatingSystem:{0}", Terminals.Native.Wow.Is64BitOperatingSystem));
            Logging.Log.Info(String.Format("Is64BitProcess:{0}", Terminals.Native.Wow.Is64BitProcess));
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
            Settings.FileLocations.AssignCustomFileLocations(commandline.configFile,
                commandline.favoritesFile, commandline.credentialsFile);
            return commandline;
        }
    }
}
