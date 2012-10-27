using System;
using Terminals.Configuration;
using Terminals.Data.DB;

namespace Terminals.Data
{
    internal class Persistence
    {
        private IPersistence persistence;
     
        private Persistence()
        {
            try
            {
                if (Settings.PersistenceType == 0)
                    this.persistence = new FilePersistence();
                else
                    this.persistence = new FilePersistence();
                    // this.persistence = new SqlPersistence();
            }
            catch (Exception exception)
            {
                Logging.Log.Fatal("Perstance layer failed to load", exception);
                throw;
            }
        }

        /// <summary>
        /// Fall back to file persistence, in case of not available database
        /// </summary>
        internal static void FallBackToPrimaryPersistence(PersistenceSecurity security)
        {
            var newSecurity = new PersistenceSecurity(security);
            var persistence = new FilePersistence(newSecurity);
            Nested.instance.persistence = persistence;
            persistence.Initialize();
        }

        #region Thread safe singleton with lazy loading
        
        /// <summary>
        /// Gets the thread safe singleton instance of the persistence layer
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
