using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Terminals.Configuration;
using Terminals.Forms.Controls;
using Terminals.Security;

namespace Terminals.Data.DB
{
    internal partial class Database
    {
        private const string PROVIDER = "System.Data.SqlClient";       
        private const string METADATA = @"res://*/Data.DB.SQLPersistence.csdl|res://*/Data.DB.SQLPersistence.ssdl|res://*/Data.DB.SQLPersistence.msl";
        internal const string DEFAULT_CONNECTION_STRING = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Data\Terminals.mdf;Integrated Security=True;User Instance=False";

        /// <summary>
        /// periodicaly download latest changes
        /// </summary>
        private Timer reLoadClock;

        private bool delaySave = false;

        private object updateLock;

        internal static Database CreateDatabaseInstance()
        {
           return CreateDatabase(Settings.ConnectionString);
        }

        private static Database CreateDatabase(string connecitonString)
        {
            string connectionString = BuildConnectionString(connecitonString);
            return new Database(connectionString);
        }

        private static string BuildConnectionString(string providerConnectionString)
        {
            var connectionBuilder = new EntityConnectionStringBuilder
            {
                Provider = PROVIDER,
                Metadata = METADATA,
                ProviderConnectionString = providerConnectionString
            };

            return connectionBuilder.ToString();
        }

        partial void OnContextCreated()
        {
            this.updateLock = new object();
            this.ObjectMaterialized += new ObjectMaterializedEventHandler(this.OnDatabaseObjectMaterialized);
        }

        internal void InitializeReLoadClock(ISynchronizeInvoke synchronizer)
        {
            this.reLoadClock = new Timer();
            this.reLoadClock.Interval = 2000;
            // this.reLoadClock.SynchronizingObject = synchronizer;
            // this.reLoadClock.Elapsed += new ElapsedEventHandler(this.OnReLoadClockElapsed);
            this.reLoadClock.Start();
        }

        /// <summary>
        /// Wery simple refresh of all already cached items.
        /// </summary>
        private void OnReLoadClockElapsed(object sender, ElapsedEventArgs e)
        {
            // todo check, if there is atleast something new, otherwise we dont have to download every thing
            // todo make the reload thread safe
            // first download the masterpassword to ensure, that it wasnt change, otherwise we have
            // all cached items out of date
            // after all items are loaded, refresh already cached protocol options, which arent part of an entity
            var clock = Stopwatch.StartNew();
            this.Refresh(RefreshMode.ClientWins, this.Favorites);
            this.Refresh(RefreshMode.ClientWins, this.Groups);
            this.Refresh(RefreshMode.ClientWins, this.CredentialBase);
            this.Refresh(RefreshMode.ClientWins, this.BeforeConnectExecute);
            this.Refresh(RefreshMode.ClientWins, this.DisplayOptions);
            this.Refresh(RefreshMode.ClientWins, this.Security);

            // todo possible changes in history are only for today

            Debug.WriteLine("Updating entities at {0} [{1} ms]", DateTime.Now,  clock.ElapsedMilliseconds);
        }

        private void OnDatabaseObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        {
            var entity = e.Entity as IEntityContext;
            if (entity != null)
                entity.Database = this;
        }

        internal void StartDelayedUpdate()
        {
            delaySave = true;
        }

        internal void SaveAndFinishDelayedUpdate()
        {
            delaySave = false;
            SaveImmediatelyIfRequested();
        }

        internal void SaveImmediatelyIfRequested()
        {
            // dont ask save immediately. Here is no benefit to save in batch like in FilePersistence
            // todo Add protection against concurent updates
            this.SaveChanges();
        }

        public override int SaveChanges(SaveOptions options)
        {
            // todo lock the update so nobody is able to update, until we are finished with save
            this.DetachUntaggedGroups();
            var changedFavorites = this.GetChangedOrAddedFavorites();
            return this.SaveChanges(options, changedFavorites);
        }

        private int SaveChanges(SaveOptions options, IEnumerable<Favorite> changedFavorites)
        {
            int returnValue;
            lock (updateLock)
            {
                returnValue = base.SaveChanges(options);
                this.SaveFavoriteProtocolProperties(changedFavorites);
            }
            return returnValue;
        }

        /// <summary>
        /// Preserve saving Untagged group which is virtual only
        /// </summary>
        private void DetachUntaggedGroups()
        {
            IEnumerable<ObjectStateEntry> added = this.ObjectStateManager.GetObjectStateEntries(EntityState.Added);
            foreach (var change in added)
            {
                var group = change.Entity as Group;
                if (group != null && group.Name == Settings.UNTAGGED_NODENAME)
                {
                    this.Detach(group);
                }
            }
        }

        private void SaveFavoriteProtocolProperties(IEnumerable<Favorite> changedFavorites)
        {
            foreach (var favorite in changedFavorites)
            {
                favorite.SaveProperties(this);
                favorite.UpdateImageInDatabase(this);
            }
        }

        private IEnumerable<Favorite> GetChangedOrAddedFavorites()
        {
            var changes = this.GetChangedOrAddedEntitites();

            IEnumerable<Favorite> affectedFavorites = changes.Where(candidate => candidate.Entity is Favorite)
                .Select(change => change.Entity)
                .Cast<Favorite>();

            return affectedFavorites;
        }

        private List<ObjectStateEntry> GetChangedOrAddedEntitites()
        {
            IEnumerable<ObjectStateEntry> added = this.ObjectStateManager.GetObjectStateEntries(EntityState.Added);
            List<ObjectStateEntry> changed = this.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).ToList();
            changed.AddRange(added);
            return changed;
        }

        internal Favorite GetFavoriteByGuid(Guid favoriteId)
        {
            // to list, because linq to entities doesnt support Guid search
            return this.Favorites.ToList()
                .FirstOrDefault(favorite => favorite.Guid == favoriteId);
        }

        internal byte[] GetFavoriteIcon(int favoriteId)
        {
            byte[] obtained = this.GetFavoriteIcon((int?)favoriteId).FirstOrDefault();
            if (obtained != null)
                return obtained;

            return FavoriteIcons.EmptyImageData;
        }

        internal string GetMasterPasswordHash()
        {
            string obtained = this.GetMasterPasswordKey().FirstOrDefault();
            if (obtained != null)
                return obtained;

            return string.Empty;
        }

        internal void UpdateMasterPassword(string newMasterPasswordKey)
        {
            lock (updateLock)
            {
                // todo do it in transaction to prevent inconsistent data
                this.UpdateMasterPasswordKey(newMasterPasswordKey);
            }
        }

        /// <summary>
        /// Tryes to execute simple command on database to ensure, that the conneciton works.
        /// </summary>
        /// <param name="connectionStringToTest">Not null MS SQL connection string
        ///  to use to create new database instance</param>
        /// <param name="databasePassword">Not encrypted database pasword</param>
        /// <returns>True, if connection test was sucessfull; otherwise false
        /// and string containg the error message</returns>
        internal static Tuple<bool, string> TestConnection(string connectionStringToTest, string databasePassword)
        {
            try
            {
                var passwordIsValid = TestDatabasePassword(connectionStringToTest, databasePassword);
                return new Tuple<bool, string>(passwordIsValid, "Database password doesnt match.");
            }
            catch(Exception exception)
            {
                string message = exception.Message;
                if (exception.InnerException != null)
                    message = exception.InnerException.Message;
                return new Tuple<bool, string>(false, message);
            }
        }

        private static bool TestDatabasePassword(string connectionStringToTest, string databasePassword)
        {
            Database database = CreateDatabase(connectionStringToTest);
            string databasePasswordHash = PasswordFunctions.EncryptPassword(databasePassword);
            bool passwordIsValid = databasePasswordHash == database.GetMasterPasswordHash();
            return passwordIsValid;
        }
    }
}
