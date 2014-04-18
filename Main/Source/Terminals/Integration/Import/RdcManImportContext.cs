using System.Collections.Generic;

namespace Terminals.Integration.Import
{
    internal class RdcManImportContext
    {
        private readonly IEnumerable<RdcManGroup> groups;

        private readonly IEnumerable<RdcManServer> servers;

        internal List<FavoriteConfigurationElement> Imported { get; private set; }

        public RdcManImportContext(IEnumerable<RdcManGroup> groups, IEnumerable<RdcManServer> servers)
        {
            this.Imported = new List<FavoriteConfigurationElement>();
            this.groups = groups;
            this.servers = servers;
        }

        internal void ImportContent()
        {
            this.ImportFavorites();
            this.ImportGroups();
        }

        private void ImportGroups()
        {
            foreach (RdcManGroup rdcManGroup in this.groups)
            {
                var context = new RdcManImportContext(rdcManGroup.Groups, rdcManGroup.Servers);
                context.ImportContent();
                this.Imported.AddRange(context.Imported);
            }
        }

        private void ImportFavorites()
        {
            foreach (RdcManServer server in this.servers)
            {
                var serverImporter = new RdcManFavoriteImporter(server);
                FavoriteConfigurationElement favorite = serverImporter.ImportServer();
                this.Imported.Add(favorite);
            }
        }
    }
}