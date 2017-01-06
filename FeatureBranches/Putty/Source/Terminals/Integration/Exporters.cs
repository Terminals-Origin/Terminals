using System.Collections.Generic;
using System.Text;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Integration.Export;
using Terminals.Integration.Import;

namespace Terminals.Integration
{
    internal class Exporters : Integration<IExport>
    {
        private readonly IPersistence persistence;

        private readonly ConnectionManager connectionManager;

        public Exporters(IPersistence persistence, ConnectionManager connectionManager)
        {
            this.persistence = persistence;
            this.connectionManager = connectionManager;
        }

        protected override void LoadProviders()
        {
            if (providers == null)
            {
                providers = new Dictionary<string, IExport>();
                ITerminalsOptionsExport[] optionsExporters = this.connectionManager.GetTerminalsOptionsExporters();
                providers.Add(ImportTerminals.TERMINALS_FILEEXTENSION, new ExportTerminals(this.persistence, optionsExporters));
                providers.Add(ImportRDP.FILE_EXTENSION, new ExportRdp(this.persistence));
                var androidExport = new ExportExtraLogicAndroidRd(this.persistence);
                providers.Add(GetExtraAndroidProviderKey(), androidExport);
            }
        }

        /// <summary>
        /// Replaces XML file extension duplicity as key in providers.
        /// </summary>
        /// <returns></returns>
        private static string GetExtraAndroidProviderKey()
        {
            return ExportExtraLogicAndroidRd.EXTENSION + ExportExtraLogicAndroidRd.EXTENSION;
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

        public void Export(ExportOptions options)
        {
            IExport exporter = FindProvider(options.FileName);
            
            if (options.ProviderFilter.Contains(ExportExtraLogicAndroidRd.PROVIDER_NAME))
                exporter = this.providers[GetExtraAndroidProviderKey()];

            if (exporter != null)
                exporter.Export(options);
        }
    }
}
