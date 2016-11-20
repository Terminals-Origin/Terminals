using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Terminals.Connections;
using Terminals.Data.History;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL Database store using Entity framework. All parts use Disconnected entities.
    /// </summary>
    internal class SqlPersistence : IPersistence, IPersistedSecurity
    {
        /// <summary>
        /// periodical download of latest changes
        /// </summary>
        private readonly Timer reLoadClock;

        private const int DEFAULT_REFRESH_INTERVAL = 1000 * 30;
        /// <summary>
        /// Gets or sets value in seconds indicating, how often should persistence
        /// ping server to check for latest state. Default value 30 seconds.
        /// </summary>
        internal int RefreshInterval 
        {
            get { return (int)this.reLoadClock.Interval / 1000; }
            set { this.reLoadClock.Interval = 1000 * value; }
        }

        /// <summary>
        /// Gets unique id of the persistence to be stored in settings (1)
        /// </summary>
        internal const int TYPE_ID = 1;

        public int TypeId { get { return TYPE_ID; } }

        public string Name { get { return "Database"; } }

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

        private readonly FavoriteIcons favoriteIcons;

        private readonly ConnectionManager connectionManager;

        public PersistenceSecurity Security { get { return this.security; } }

        internal SqlPersistence(FavoriteIcons favoriteIcons, ConnectionManager connectionManager)
        {
            this.favoriteIcons = favoriteIcons;
            this.connectionManager = connectionManager;
            this.reLoadClock = new Timer(DEFAULT_REFRESH_INTERVAL);
            this.security = new SqlPersistenceSecurity();
            this.security.AssignPersistence(this);
            this.Dispatcher = new DataDispatcher();
        }

        public void StartDelayedUpdate()
        {
            // don't reload during batch operations, otherwise recursive updates degrade performance
            this.reLoadClock.Stop();
            this.Dispatcher.StartDelayedUpdate();
        }

        public void SaveAndFinishDelayedUpdate()
        {
            // nothing to save, because changes are already committed by each operation
            this.Dispatcher.EndDelayedUpdate();
            this.reLoadClock.Start();
        }

        public void Initialize()
        {
            if(!this.TryInitializeDatabase())
                return;

            this.groups = new Groups();
            this.credentials = new StoredCredentials(this.Dispatcher);
            this.favorites = new Favorites(this.groups, this.credentials, this.security,
                this.Dispatcher, this.connectionManager, this.favoriteIcons);
            this.groups.AssignStores(this.Dispatcher, this.favorites);
            this.connectionHistory = new ConnectionHistory(this.favorites, this.Dispatcher);
            this.Factory = new Factory(this.groups, this.favorites, this.credentials, this.Dispatcher, this.connectionManager);
        }

        private bool TryInitializeDatabase()
        {
            if (DatabaseConnections.TestConnection())
            {
                bool updatedKey = this.security.UpdateDatabaseKey();
                if (updatedKey)
                    return true;
                // UpgradeDatabaseVersion();
            }

            Logging.Fatal("SQL Persistence layer failed to load. Fall back to File persistence");
            Persistence.FallBackToPrimaryPersistence(this.Security);
            return false;
        }

        public void UpdatePasswordsByNewMasterPassword(string newMasterKey)
        {
            // nothing to do in database, the application master password doesn't affect the database
            // but, the file persisted passwords may be lost, so we have to update them.
            var newSecurity = new PersistenceSecurity(this.security);
            var filePersistence = new FilePersistence(newSecurity);
            filePersistence.Initialize();
            filePersistence.UpdatePasswordsByNewMasterPassword(newMasterKey);
        }

        public void AssignSynchronizationObject(ISynchronizeInvoke synchronizer)
        {
            // this forces the clock to run the updates in gui thread, because Entity framework isn't thread safe
            this.reLoadClock.SynchronizingObject = synchronizer;
            this.reLoadClock.Elapsed += new ElapsedEventHandler(this.OnReLoadClockElapsed);
            this.reLoadClock.Start();
        }

        /// <summary>
        /// Fire simple refresh of all already cached items.
        /// </summary>
        private void OnReLoadClockElapsed(object sender, ElapsedEventArgs e)
        {
            this.reLoadClock.Stop();
            var clock = Stopwatch.StartNew();
            
            this.groups.RefreshCache();
            this.favorites.RefreshCache();
            this.credentials.RefreshCache();
            // nothing to update in History: possible changes are only for today, and that day item isn't cached

            Debug.WriteLine("Updating entities at {0} [{1} ms]", Moment.Now, clock.ElapsedMilliseconds);
            this.reLoadClock.Start();
        }

        public override string ToString()
        {
            return string.Format("SqlPersistence:Favorites={0},Groups={1},Credentials={2}",
                this.favorites.Count<DbFavorite>(), this.groups.Count(), this.Credentials.Count());
        }
    }
}