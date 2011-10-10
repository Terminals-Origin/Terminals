using System;
using System.Collections.Generic;
using System.Text;
using Terminals.Integration.Export;
using Terminals.Integration.Import;

namespace Terminals.Integration
{
    internal class Exporters : Integration<IExport>
    {
        protected override void LoadProviders()
        {
            if (providers == null)
            {
                providers = new Dictionary<string, IExport>();
                providers.Add(ImportTerminals.TERMINALS_FILEEXTENSION, new ExportTerminals());
            }
        }

        internal string GetProvidersDialogFilter()
        {
            LoadProviders();

            StringBuilder filters = new StringBuilder();
            foreach (KeyValuePair<string, IExport> exporter in providers)
            {
                AddProviderFilter(filters, exporter.Value);
            }

            return filters.ToString();
        }

        public void Export(string fileName, List<FavoriteConfigurationElement> favorites, bool includePassword)
        {
            IExport exporter = FindProvider(fileName);

            if (exporter != null)
                exporter.Export(fileName, favorites, includePassword);
        }
    }
}
