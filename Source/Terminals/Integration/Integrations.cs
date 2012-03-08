using Terminals.Integration.Import;

namespace Terminals.Integration
{
    internal static class Integrations
    {
        private static Importers importers = new Importers();
        private static Exporters exporters = new Exporters();

        internal static Importers Importers
        {
            get { return importers; }
        }

        internal static Exporters Exporters
        {
            get { return exporters; }
        }
    }
}
