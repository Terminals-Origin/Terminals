using System;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data.DB;

namespace Terminals.Data
{
    internal class Persistence
    {
        private static Action fallbackPrompt; 
        private IPersistence persistence;

        private static readonly FavoriteIcons favoriteIcons = FavoriteIcons.Instance;

        private static readonly ConnectionManager connectionManager = ConnectionManager.Instance;

        private Persistence()
        {
            try
            {
                this.InitializePersistence();
                Settings.Instance.PersistenceSecurity = this.persistence.Security;
            }
            catch (Exception exception)
            {
                Logging.Fatal("Persistence layer failed to load", exception);
                throw;
            }
        }

        private void InitializePersistence()
        {
            if (Settings.Instance.PersistenceType == FilePersistence.TYPE_ID)
                this.persistence = new FilePersistence(new PersistenceSecurity(), favoriteIcons, connectionManager);
            else
                this.persistence = new SqlPersistence(favoriteIcons, connectionManager);
        }

        /// <summary>
        /// Fall back to file persistence, in case of not available database
        /// </summary>
        internal static void FallBackToPrimaryPersistence(PersistenceSecurity security)
        {
            PromptForFallback();
            var newSecurity = new PersistenceSecurity(security);
            var persistence = new FilePersistence(newSecurity, favoriteIcons, connectionManager);
            Settings.Instance.PersistenceSecurity = newSecurity;
            Nested.instance.persistence = persistence;
            persistence.Initialize();
        }

        private static void PromptForFallback()
        {
            if (fallbackPrompt != null)
                fallbackPrompt();
        }

        internal static void AssignFallbackPrompt(Action newPrompt)
        {
            fallbackPrompt = newPrompt;
        }

        #region Thread safe singleton with lazy loading
        
        /// <summary>
        /// Gets the thread safe singleton instance of the persistence layer.
        /// Use only for startup procedure, will removed in the future.
        /// </summary>
        public static IPersistence Instance
        {
            get
            {
                return Nested.instance.persistence;
            }
        }

        private static class Nested
        {
            internal static readonly Persistence instance = new Persistence();
        }

        #endregion

    }
}
