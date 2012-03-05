using System;
using Terminals.Configuration;
using Terminals.Data.DB;

namespace Terminals.Data
{
    internal class Persistance
    {
        #region Thread safe singleton with lazy loading

        private Persistance()
        {
            try
            {
                if (Settings.PersistenceType == 0)
                    this.persistance = new FilePersistance();
                else 
                    // todo enable SqlPeristance to be created
                    this.persistance = new SqlPersistance();
                    //this.persistance = new FilePersistance(); 
            }
            catch (Exception exception)
            {
                Logging.Log.Fatal("Perstance layer failed to load", exception);
                throw;
            }
        }

        /// <summary>
        /// Gets the thread safe singleton instance of the persistance layer
        /// </summary>
        public static IPersistance Instance
        {
            get
            {
                return Nested.instance.persistance;
            }
        }

        private static class Nested
        {
            internal static readonly Persistance instance = new Persistance();
        }

        #endregion

        private IPersistance persistance;
    }
}
