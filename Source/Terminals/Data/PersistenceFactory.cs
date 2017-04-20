using System;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data.DB;

namespace Terminals.Data
{
    internal class PersistenceFactory
    {
        private readonly Settings settings;

        private readonly FavoriteIcons favoriteIcons;

        private readonly ConnectionManager connectionManager;

        public PersistenceFactory(Settings settings, ConnectionManager connectionManager, FavoriteIcons favoriteIcons)
        {
            this.settings = settings;
            this.favoriteIcons = favoriteIcons;
            this.connectionManager = connectionManager;
        }

        internal IPersistence CreatePersistence()
        {
            try
            {
                var persistence = this.CreateNewInstance();
                this.settings.PersistenceSecurity = persistence.Security;
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
            if (this.settings.PersistenceType == FilePersistence.TYPE_ID)
                return new FilePersistence(new PersistenceSecurity(), this.favoriteIcons, this.connectionManager);

            return new SqlPersistence(this.favoriteIcons, this.connectionManager);
        }

        internal IPersistence AuthenticateByMasterPassword(IPersistence persistence, IStartupUi startupUi)
        {
            IPersistence newPersistence = persistence;

            if (!persistence.Security.Authenticate(startupUi.KnowsUserPassword))
            {
                if (persistence.TypeId != FilePersistence.TYPE_ID && startupUi.UserWantsFallback())
                    newPersistence = this.FallBackToPrimaryPersistence(persistence.Security);
                else
                    startupUi.Exit();
            }

            return newPersistence;
        }

        /// <summary>
        /// Fall back to file persistence, in case of not available database
        /// </summary>
        private FilePersistence FallBackToPrimaryPersistence(PersistenceSecurity security)
        {
            Logging.Fatal("SQL Persistence layer failed to load. Fall back to File persistence");
            var newSecurity = new PersistenceSecurity(security);
            var persistence = new FilePersistence(newSecurity, this.favoriteIcons, this.connectionManager);
            this.settings.PersistenceSecurity = newSecurity;
            persistence.Initialize();
            return persistence;
        }
    }
}
