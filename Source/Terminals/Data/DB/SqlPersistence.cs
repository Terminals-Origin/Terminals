using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL Database store using Entity framework. All parts use Disconected entities.
    /// </summary>
    internal class SqlPersistence : IPersistence, IPersistedSecurity
    {
        /// <summary>
        /// periodicaly download latest changes
        /// </summary>
        private Timer reLoadClock;

        private Favorites favorites;
        public IFavorites Favorites
        {
            get
            {
                return this.favorites;
            }
        }

        private Groups groups;
        public IGroups Groups
        {
            get { return this.groups; }
        }

        private ConnectionHistory connectionHistory;
        public IConnectionHistory ConnectionHistory { get { return this.connectionHistory; } }
        
        public IFactory Factory { get; private set; }
        public DataDispatcher Dispatcher { get; private set; }

        private StoredCredentials credentials;
        public ICredentials Credentials { get { return this.credentials; } }

        private readonly SqlPersistenceSecurity security;
        public PersistenceSecurity Security { get { return this.security; } }

        internal SqlPersistence()
        {
            this.security = new SqlPersistenceSecurity();
            this.security.AssignPersistence(this);
        }

        public void StartDelayedUpdate()
        {
            this.Dispatcher.StartDelayedUpdate();
            Database.OpenConnection();
        }

        public void SaveAndFinishDelayedUpdate()
        {
            // nothing to save, because changes are already commited by each operation
            Database.CloseConneciton();
            this.Dispatcher.EndDelayedUpdate();
        }

        public void Initialize()
        {
            if(!this.TryInitializeDatabase())
                return;

            this.Dispatcher = new DataDispatcher();
            this.groups = new Groups();
            this.credentials = new StoredCredentials(this.security);
            this.favorites = new Favorites(this.groups, this.credentials, this.security, this.Dispatcher);
            this.groups.AssignStores(this.Dispatcher, this.favorites);
            this.connectionHistory = new ConnectionHistory(this.favorites);
            this.Factory = new Factory(this.groups, this.favorites, this.credentials, this.security, this.Dispatcher);
        }

        private bool TryInitializeDatabase()
        {
            if (Database.TestConnection())
            {
                this.security.UpdateDatabaseKey();
                return true;
            }

            Logging.Log.Fatal("SQL Perstance layer failed to load. Fall back to File persistence");
            Persistence.FallBackToPrimaryPersistence(this.Security);
            return false;
        }

        public void UpdatePasswordsByNewMasterPassword(string newMasterPassword)
        {
            // nothing to do here, the application master password doesnt affect the database
        }

        public void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            this.reLoadClock = new Timer();
            this.reLoadClock.Interval = 2000;
            // this forces the clock to run the updates in gui thread, because Entity framework isnt thread safe
            this.reLoadClock.SynchronizingObject = synchronizer;
            this.reLoadClock.Elapsed += new ElapsedEventHandler(this.OnReLoadClockElapsed);
            // this.reLoadClock.Start();
        }

        /// <summary>
        /// Wery simple refresh of all already cached items.
        /// </summary>
        private void OnReLoadClockElapsed(object sender, ElapsedEventArgs e)
        {
            this.reLoadClock.Stop();
            var clock = Stopwatch.StartNew();
            
            this.groups.RefreshCache();
            this.favorites.RefreshCache();
            this.credentials.RefreshCache();
            // nothing to update in History: possible changes are only for today, and that day item isnt cached

            Debug.WriteLine("Updating entities at {0} [{1} ms]", DateTime.Now, clock.ElapsedMilliseconds);
            //this.reLoadClock.Start();
        }

        public override string ToString()
        {
            return string.Format("SqlPersistence:Favorites={0},Groups={1},Credentials={2}",
                this.favorites.Count(), this.groups.Count(), this.Credentials.Count());
        }
    }
}