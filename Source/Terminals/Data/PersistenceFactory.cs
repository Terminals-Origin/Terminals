using System;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data.DB;

namespace Terminals.Data
{
    internal class PersistenceFactory
    {
        private static readonly Settings settings = Settings.Instance;

        private static readonly FavoriteIcons favoriteIcons = FavoriteIcons.Instance;

        private static readonly ConnectionManager connectionManager = ConnectionManager.Instance;

        internal IPersistence CreatePersistence()
        {
            try
            {
                var persistence = this.CreateNewInstance();
                settings.PersistenceSecurity = persistence.Security;
                return persistence;
            }
            catch (Exception exception)
            {
                Logging.Fatal("Persistence layer failed to load", exception);
                throw;
            }
        }

        private IPersistence CreateNewInstance()
        {
            if (settings.PersistenceType == FilePersistence.TYPE_ID)
                return new FilePersistence(new PersistenceSecurity(), favoriteIcons, connectionManager);

            return new SqlPersistence(favoriteIcons, connectionManager);
        }

        /// <summary>
        /// Fall back to file persistence, in case of not available database
        /// </summary>
        internal FilePersistence FallBackToPrimaryPersistence(PersistenceSecurity security)
        {
            Logging.Fatal("SQL Persistence layer failed to load. Fall back to File persistence");
            var newSecurity = new PersistenceSecurity(security);
            var persistence = new FilePersistence(newSecurity, favoriteIcons, connectionManager);
            settings.PersistenceSecurity = newSecurity;
            persistence.Initialize();
            return persistence;
        }
    }
}
