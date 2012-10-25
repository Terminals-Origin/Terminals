using System.Data.Objects;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Data.DB;
using Favorite = Terminals.Data.DB.Favorite;

namespace Tests
{
    /// <summary>
    /// Shared configured store used by all SQL peristance tests
    /// </summary>
    public class SqlTestsLab
    {
        /// <summary>
        /// Gets the data store on which tests should be performed
        /// </summary>
        internal SqlPersistence PrimaryPersistence { get; private set; }

        internal SqlPersistence SecondaryPersistence { get; private set; }

        /// <summary>
        /// Gets second connector to lab database. Used to check, if data reached the store
        /// </summary>
        internal Database CheckDatabase { get; private set; }

        internal IFavorites PrimaryFavorites
        {
            get
            {
                return this.PrimaryPersistence.Favorites;
            }
        }

        internal IFavorites SecondaryFavorites
        {
            get
            {
                return this.SecondaryPersistence.Favorites;
            }
        }

        internal IFactory PrimaryFactory { get { return this.PrimaryPersistence.Factory; } }

        /// <summary>
        /// Initialzes data connectors on beginning of each test.
        /// </summary>
        protected void InitializeTestLab()
        {
            Settings.FileLocations.AssignCustomFileLocations(string.Empty, string.Empty, string.Empty);
            Settings.ConnectionString = Database.DEFAULT_CONNECTION_STRING;
            
            this.PrimaryPersistence = new SqlPersistence();
            this.PrimaryPersistence.Initialize();
            this.SecondaryPersistence = new SqlPersistence();
            this.SecondaryPersistence.Initialize();

            CheckDatabase = Database.CreateInstance();
            ClearTestLab(); // because of failed previos tests
        }

        /// <summary>
        /// Cleans up all tables in test database
        /// </summary>
        protected void ClearTestLab()
        {
            const string deleteCommand = @"DELETE FROM ";
            // first clear dependences from both Favorites and groups table because of constraints
            CheckDatabase.ExecuteStoreCommand(deleteCommand + "FavoritesInGroup");
            CheckDatabase.ExecuteStoreCommand(deleteCommand + "History");

            string favoritesTable = GetTableName(CheckDatabase.Favorites);
            CheckDatabase.ExecuteStoreCommand(deleteCommand + favoritesTable);
            string beforeConnectTable = GetTableName(CheckDatabase.BeforeConnectExecute);
            CheckDatabase.ExecuteStoreCommand(deleteCommand + beforeConnectTable);
            string securityTable = GetTableName(CheckDatabase.Security);
            CheckDatabase.ExecuteStoreCommand(deleteCommand + securityTable);
            string displayOptionsTable = GetTableName(CheckDatabase.DisplayOptions);
            CheckDatabase.ExecuteStoreCommand(deleteCommand + displayOptionsTable);
            string groupsTable = GetTableName(CheckDatabase.Groups);
            CheckDatabase.ExecuteStoreCommand(deleteCommand + groupsTable);

            string credentialBase = GetTableName(CheckDatabase.CredentialBase);
            CheckDatabase.ExecuteStoreCommand(deleteCommand + credentialBase);
            CheckDatabase.ExecuteStoreCommand(deleteCommand + "Credentials");
        }

        private string GetTableName<TEntity>(ObjectSet<TEntity> table) where TEntity : class
        {
            return table.EntitySet.Name;
        }

        /// <summary>
        /// Creates new test favorite using primary persistence. Returns this newly created instance.
        /// Doesnt add it to the persistence.
        /// </summary>
        internal Favorite CreateTestFavorite()
        {
            var favorite = this.PrimaryPersistence.Factory.CreateFavorite() as Favorite;
            // set required properties
            favorite.Name = "test";
            favorite.ServerName = "test server";
            return favorite;
        }

        /// <summary>
        /// Creates test favorite and adds it to the primary peristence.
        /// Returns newly created favorite
        /// </summary>
        internal Favorite AddFavoriteToPrimaryPersistence()
        {
            Favorite favorite = this.CreateTestFavorite();
            this.PrimaryPersistence.Favorites.Add(favorite);
            return favorite;
        }
    }
}
