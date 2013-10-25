using Terminals.Data;
using Terminals.Integration.Import;

namespace Terminals.Integration
{
    internal static class Integrations
    {
        private static readonly Exporters exporters = new Exporters();

        internal static Importers CreateImporters(IPersistence persistence)
        {
            return new Importers(persistence);
        }

        internal static Exporters Exporters
        {
            get { return exporters; }
        }
    }
}
