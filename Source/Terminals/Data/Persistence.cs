using System;
using Terminals.Configuration;
using Terminals.Data.DB;

namespace Terminals.Data
{
    internal class Persistence
    {
        #region Thread safe singleton with lazy loading

        private Persistence()
        {
            try
            {
                if (Settings.PersistenceType == 0)
                    this.persistence = new FilePersistence();
                else 
                    // todo enable SqlPeristance to be created
                    // this.persistence = new SqlPersistence();
                    this.persistence = new FilePersistence(); 
            }
            catch (Exception exception)
            {
                Logging.Log.Fatal("Perstance layer failed to load", exception);
                throw;
            }
        }

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

        private IPersistence persistence;
    }
}
