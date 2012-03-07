using System.Threading;
using Terminals.CommandLine;
using Terminals.Network;

namespace Terminals
{
    internal class SingleInstanceApplication
    {
        #region Thread safe singleton
        
        private static class Nested
        {
            internal static SingleInstanceApplication instance = new SingleInstanceApplication();
        }

        internal static SingleInstanceApplication Instance
        {
            get { return Nested.instance; }
        }

        private SingleInstanceApplication()
        {
            this.instanceLock = new Mutex(true, INSTANCELOCK_NAME, out this.firstInstance);
            // we are sure, that we own the previous one, so loct the applicatin startup immediately
            this.startupLock = new Mutex(this.firstInstance, STARTUPLOCK_NAME);
        }

        #endregion

        private const string INSTANCELOCK_NAME = "Terminals.codeplex.com.SingleInstance";
        private const string STARTUPLOCK_NAME = "Terminals.codeplex.com.CommandServerStartUp";

        /// <summary>
        /// This is a machine wide application instances counter.
        /// when the last application is shutdown the mutex will be released automatically
        /// </summary>
        private Mutex instanceLock;

        /// <summary>
        /// Prevent asking for window notifications, when the server is in startup or shutdown procedure
        /// </summary>
        private Mutex startupLock;

        private bool firstInstance;
        private CommandLineServer server;

        internal void Start(MainForm mainForm)
        {
            if (!this.firstInstance)
                return;
            
            this.server = new CommandLineServer(mainForm);
            this.server.Open();
            // startupLock obtained in constructor, the server is now available to notifications
            this.startupLock.ReleaseMutex();
        }

        /// <summary>
        /// close the server as soon as, when closing the main form,
        /// because othewise the form can be already dead and it cant process notification requests
        /// </summary>
        internal void Close()
        {
            if (!this.firstInstance)
                return;
            try
            {
                this.startupLock.WaitOne();
                this.server.Close();
            }
            finally
            {
                this.startupLock.Close();
                this.instanceLock.Close();
            }
        }

        /// <summary>
        /// If other instance is runing, then forwards the command line to it and returns true;
        /// otherwise returns false.
        /// </summary>
        internal bool NotifyExisting(CommandLineArgs args)
        {
            if (this.firstInstance)
               return false;

            return ForwardCommand(args);
        }

        private bool ForwardCommand(CommandLineArgs args)
        {
            try
            {
                // wait until the main instance startup/shutdown ends
                this.startupLock.WaitOne();
                ICommandLineService client = CommandLineServer.CreateClient();
                client.ForwardCommand(args);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                this.startupLock.ReleaseMutex();
            }
        }
    }
}
