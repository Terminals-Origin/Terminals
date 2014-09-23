namespace Terminals.Configuration
{
    internal partial class Settings
    {
        /// <summary>
        /// Gets the thread safe singleton instance.
        /// Use only for startup procedure, will removed in the future.
        /// </summary>
        public static Settings Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        private static class Nested
        {
            internal static readonly Settings instance = new Settings();
        }
    }
}
