using System.Collections.Generic;
using Terminals.Data;

namespace Terminals.Integration.Import.RdcMan
{
    internal class ImportContext
    {
        private readonly IEnumerable<Group> groups;

        private readonly IEnumerable<Server> servers;
        private readonly IPersistence persistence;

        internal List<FavoriteConfigurationElement> Imported { get; private set; }

        public ImportContext(IPersistence persistence, IEnumerable<Group> groups, IEnumerable<Server> servers)
        {
            this.persistence = persistence;
            this.groups = groups;
            this.servers = servers;
            this.Imported = new List<FavoriteConfigurationElement>();
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
                var context = new ImportContext(this.persistence, rdcManGroup.Groups, rdcManGroup.Servers);
                context.ImportContent();
                this.Imported.AddRange(context.Imported);
            }
        }

        private void ImportFavorites()
        {
            foreach (Server server in this.servers)
            {
                var serverImporter = new FavoriteImporter(this.persistence, server);
                FavoriteConfigurationElement favorite = serverImporter.ImportServer();
                this.Imported.Add(favorite);
            }
        }
    }
}