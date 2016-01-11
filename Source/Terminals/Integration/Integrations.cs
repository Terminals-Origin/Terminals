using Terminals.Data;
using Terminals.Integration.Import;

namespace Terminals.Integration
{
    internal static class Integrations
    {
        internal static Importers CreateImporters(IPersistence persistence)
        {
            return new Importers(persistence);
        }

        internal static Exporters CreateExporters(IPersistence persistence)
        {
            return new Exporters(persistence.Credentials);
        }
    }
}
