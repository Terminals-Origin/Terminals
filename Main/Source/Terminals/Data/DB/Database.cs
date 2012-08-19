using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
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
        }

        internal void SaveImmediatelyIfRequested()
        {
            try
            {
                // dont ask save immediately. Here is no benefit to save in batch like in FilePersistence
                this.SaveChanges();
            }
            catch (OptimisticConcurrencyException exception)
            {
                Logging.Log.Debug("Concurency exception catched when saving SqlPersistence", exception);
                this.RefreshChangedEntities();
                this.SaveChanges();
            }
        }

        private void RefreshChangedEntities()
        {
            var favoriteToUpdate = this.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).FirstOrDefault();
            this.Favorites.MergeOption = MergeOption.OverwriteChanges;
            this.Refresh(RefreshMode.ClientWins, favoriteToUpdate);
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
            lock (this.updateLock)
            {
                returnValue = base.SaveChanges(options);
                this.SaveFavoriteDetails(changedFavorites);
            }
            return returnValue;
        }

        private void SaveFavoriteDetails(IEnumerable<Favorite> changedFavorites)
        {
            foreach (Favorite favorite in changedFavorites)
            {
                favorite.SaveDetails(this);
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

            return String.Empty;
        }

        internal void UpdateMasterPassword(string newMasterPasswordKey)
        {
            lock (this.updateLock)
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
        
        /// <summary>
        /// Attaches all item in entitiesToAttach to this context.
        /// Does not check, if the enties are already in the context.
        /// </summary>
        /// <typeparam name="TEntity">Entity defined in this object context model</typeparam>
        /// <param name="entitiesToAttach">Not null collection of items from this model</param>
        internal void AttachAll<TEntity>(IEnumerable<TEntity> entitiesToAttach)
            where TEntity : IEntityWithKey
        {
            foreach (TEntity entity in entitiesToAttach)
            {
                this.Attach(entity);
            }
        }

        /// <summary>
        /// Detaches all item in entitiesToDetach from this context.
        /// Does not check, if the enties are in the context.
        /// </summary>
        /// <typeparam name="TEntity">Entity defined in this object context model</typeparam>
        /// <param name="entitiesToDetach">Not null collection of items from this model
        /// currently attached in this object context</param>
        internal void DetachAll<TEntity>(IEnumerable<TEntity> entitiesToDetach)
        {
            foreach (TEntity entity in entitiesToDetach)
            {
                this.Detach(entity);
            }
        }

        internal void DetachAll(IEnumerable<Favorite> favorites)
        {
            foreach (Favorite favorite in favorites)
            {
                this.DetachFavorite(favorite);
            }
        }

        internal void DetachFavorite(Favorite favorite)
        {
            favorite.DetachDetails(this);
            this.Detach(favorite);
        }

        /// <summary>
        /// Switch toUpdate entity state to Modified. 
        /// </summary>
        internal void MarkAsModified(object toUpdate)
        {
            this.ObjectStateManager.ChangeObjectState(toUpdate, EntityState.Modified);
        }
    }
}
