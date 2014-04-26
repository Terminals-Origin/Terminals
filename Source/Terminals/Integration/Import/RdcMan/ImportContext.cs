using System.Collections.Generic;

namespace Terminals.Integration.Import.RdcMan
{
    internal class ImportContext
    {
        private readonly IEnumerable<Group> groups;

        private readonly IEnumerable<Server> servers;

        internal List<FavoriteConfigurationElement> Imported { get; private set; }

        public ImportContext(IEnumerable<Group> groups, IEnumerable<Server> servers)
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
            foreach (Group rdcManGroup in this.groups)
            {
                var context = new ImportContext(rdcManGroup.Groups, rdcManGroup.Servers);
                context.ImportContent();
                this.Imported.AddRange(context.Imported);
            }
        }

        private void ImportFavorites()
        {
            foreach (Server server in this.servers)
            {
                var serverImporter = new FavoriteImporter(server);
                FavoriteConfigurationElement favorite = serverImporter.ImportServer();
                this.Imported.Add(favorite);
            }
        }
    }
}