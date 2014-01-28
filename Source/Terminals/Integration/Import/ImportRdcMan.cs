using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Terminals.Integration.Import
{
    /// <summary>
    /// Free Microsoft tool xml based file.
    /// Supports goup nesting and properties inheritance.
    /// File version is stored in root properties.
    /// Groups dont have to be unique.
    /// </summary>
    internal class ImportRdcMan : IImport
    {
        private const string NAME = "Microsoft Remote desktop connetion manager 2.2";

        private const string EXTENSION = ".rdg";

        public string Name { get { return NAME; } }

        public string KnownExtension { get { return EXTENSION; } }

        public List<FavoriteConfigurationElement> ImportFavorites(string filename)
        {
            try
            {
                return TryImport(filename);
            }
            catch (Exception exception)
            {
                Logging.Error("Unable to import RdcMan file", exception);
                return new List<FavoriteConfigurationElement>();
            }
        }

        private static List<FavoriteConfigurationElement> TryImport(string filename)
        {
            var importedItems = new List<FavoriteConfigurationElement>();
            XDocument document = XDocument.Load(filename);
            var root = new RdcManDocument(document.Root);

            var rootProperties = root.Properties;
            var groups = root.Groups;

            return importedItems;
        }
    }
}
