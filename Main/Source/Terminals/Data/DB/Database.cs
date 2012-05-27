using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using Terminals.Configuration;
using Terminals.Security;

namespace Terminals.Data.DB
{
    internal partial class Database
    {
        private const string PROVIDER = "System.Data.SqlClient";       
        private const string METADATA = @"res://*/Data.DB.SQLPersistence.csdl|res://*/Data.DB.SQLPersistence.ssdl|res://*/Data.DB.SQLPersistence.msl";
        internal const string DEFAULT_CONNECTION_STRING = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Data\Terminals.mdf;Integrated Security=True;User Instance=False";

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

        private void OnDatabaseObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        {
            var entity = e.Entity as IEntityContext;
            if (entity != null)
                entity.Database = this;
        }

        internal void SaveImmediatelyIfRequested()
        {
            // dont ask save immediately. Here is no benefit to save in batch like in FilePersistence
            // todo Add protection against concurent updates
            this.SaveChanges();
        }

        public override int SaveChanges(SaveOptions options)
        {
            // todo lock the update, so nobody is able to update, until we are finished with save
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
