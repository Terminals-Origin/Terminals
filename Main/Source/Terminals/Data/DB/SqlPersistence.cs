using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL Database store using Entity framework
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
        public IConnectionHistory ConnectionHistory { get; private set; }
        public IFactory Factory { get; private set; }
        public DataDispatcher Dispatcher { get; private set; }

        public ICredentials Credentials { get; private set; }

        public PersistenceSecurity Security { get; private set; }

        internal SqlPersistence()
        {
            this.Security = new PersistenceSecurity();
            this.Security.AssignPersistence(this);
        }

        public void StartDelayedUpdate()
        {
            // nothing to do here, database context trancks changes for us
        }

        public void SaveAndFinishDelayedUpdate()
        {
            // nothing to do
        }

        public void Initialize()
        {
            if(!this.TryInitializeDatabase())
                return;

            this.Dispatcher = new DataDispatcher();
            this.groups = new Groups();
            this.favorites = new Favorites(this.groups, this.Dispatcher);
            this.groups.AssignStores(this.Dispatcher, this.favorites);
            this.ConnectionHistory = new ConnectionHistory(this.favorites);
            this.Credentials = new StoredCredentials();
            this.Factory = new Factory(this.groups, this.favorites, this.Dispatcher);
        }

        private bool TryInitializeDatabase()
        {
            try
            {
                using (var database = Database.CreateInstance())
                {
                    database.GetMasterPasswordHash(); // dummy test
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.Log.Fatal("SQL Perstance layer failed to load. Fall back to File persistence", exception);
                Persistence.FallBackToPrimaryPersistence(this.Security);
                return false;
            }
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
            this.reLoadClock.Start();
        }

        /// <summary>
        /// Wery simple refresh of all already cached items.
        /// </summary>
        private void OnReLoadClockElapsed(object sender, ElapsedEventArgs e)
        {
            this.reLoadClock.Stop();
            // todo check, if there is atleast something new, otherwise we dont have to download everything
            var clock = Stopwatch.StartNew();
            this.RefreshDatabaseMasterPassword();
            RefreshGroups();
            RefreshFavorites();
            RefreshSecurity();
            RefreshHistory();
            Debug.WriteLine("Updating entities at {0} [{1} ms]", DateTime.Now, clock.ElapsedMilliseconds);
            // this.reLoadClock.Start();
        }

        private void RefreshDatabaseMasterPassword()
        {
            // first download the masterpassword to ensure, that it wasnt changed,
            // otherwise we have all cached items out of date
        }

        private void RefreshHistory()
        {
            // possible changes in history are only for today
        }

        private void RefreshSecurity()
        {
            //this.Refresh(RefreshMode.ClientWins, this.Security);
        }

        private void RefreshGroups()
        {
            //this.Refresh(RefreshMode.ClientWins, this.Groups);
        }
        
        private void RefreshFavorites()
        {
            //this.database.Refresh(RefreshMode.StoreWins, this.database.Favorites);
            //this.database.Refresh(RefreshMode.ClientWins, this.database.CredentialBase);
            //this.database.Refresh(RefreshMode.ClientWins, this.database.BeforeConnectExecute);
            //this.database.Refresh(RefreshMode.ClientWins, this.database.DisplayOptions);

            // after all items are loaded, refresh already cached protocol options, which arent part of an entity
        }

        public override string ToString()
        {
            return string.Format("SqlPersistence:Favorites={0},Groups={1},Credentials={2}",
                this.favorites.Count(), this.groups.Count(), this.Credentials.Count());
        }
    }
}