using Terminals.Configuration;
using Terminals.Data.DB;

namespace Tests
{
    /// <summary>
    /// Shared configured store used by all SQL peristance tests
    /// </summary>
    internal class SqlTestsLab
    {
        /// <summary>
        /// Gets the data store on which tests should be performed
        /// </summary>
        internal SqlPersistence Persistence { get; private set; }

        /// <summary>
        /// Gets second connector to lab database. Used to check, if data reached the store
        /// </summary>
        internal Database CheckDatabase { get; private set; }

        /// <summary>
        /// Initialzes data connectors on beginning of each test.
        /// </summary>
        internal void InitializeTestLab()
        {
            Settings.FileLocations.AssignCustomFileLocations(string.Empty, string.Empty, string.Empty);
            Settings.ConnectionString = Database.DEVELOPMENT_CONNECTION_STRING;
            Persistence = new SqlPersistence();
            this.Persistence.Initialize();
            CheckDatabase = Database.CreateDatabaseInstance();
            ClearTestLab(); // because of failed previos tests
        }

        /// <summary>
        /// Cleans up all tables in test database
        /// </summary>
        internal void ClearTestLab()
        {
            const string deleteCommand = @"DELETE FROM ";
            // first clear dependences from both Favorites and groups table because of constraints
            CheckDatabase.ExecuteStoreCommand(deleteCommand + "FavoritesInGroup");
            CheckDatabase.ExecuteStoreCommand(deleteCommand + "History");
            
            string favoritesTable = CheckDatabase.Favorites.EntitySet.Name;
            CheckDatabase.ExecuteStoreCommand(deleteCommand + favoritesTable);
            string beforeConnectTable = CheckDatabase.BeforeConnectExecute.EntitySet.Name;
            CheckDatabase.ExecuteStoreCommand(deleteCommand + beforeConnectTable);
            string securityTable = CheckDatabase.Security.EntitySet.Name;
            CheckDatabase.ExecuteStoreCommand(deleteCommand + securityTable);
            string displayOptionsTable = CheckDatabase.DisplayOptions.EntitySet.Name;
            CheckDatabase.ExecuteStoreCommand(deleteCommand + displayOptionsTable);
            string groupsTable = CheckDatabase.Groups.EntitySet.Name;
            CheckDatabase.ExecuteStoreCommand(deleteCommand + groupsTable);

            string credentialBase = CheckDatabase.CredentialBase.EntitySet.Name;
            CheckDatabase.ExecuteStoreCommand(deleteCommand + credentialBase);
        }

        internal Favorite CreateTestFavorite()
        {
            var favorite = this.Persistence.Factory.CreateFavorite() as Favorite;
            // set required properties
            favorite.Name = "test";
            favorite.ServerName = "test server";
            return favorite;
        }
    }
}
