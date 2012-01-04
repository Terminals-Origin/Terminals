namespace Terminals.Data
{
    internal class Persistance
    {
        #region Thread safe singleton with lazy loading

        private Persistance()
        {
            this.persistance = new FilePersistance();
            // todo REFACTORING choose and initialize persistance type defined by settings
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
