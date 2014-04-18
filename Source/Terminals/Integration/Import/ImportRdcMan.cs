using System;
using System.Collections.Generic;

namespace Terminals.Integration.Import
{
    /// <summary>
    /// Free Microsoft tool xml based file.
    /// Supports goup nesting and properties inheritance.
    /// File version is stored in root properties.
    /// Groups dont have to be unique.
    /// File doesnt support mixing groups and servers on one level.
    /// </summary>
    internal class ImportRdcMan : IImport
    {
        private const string NAME = "Microsoft Remote desktop connetion manager 2.2";

        private const string EXTENSION = ".rdg";

        public string Name { get { return NAME; } }

        public string KnownExtension { get { return EXTENSION; } }

        public List<FavoriteConfigurationElement> ImportFavorites(string fileName)
        {
            try
            {
                return TryImport(fileName);
            }
            catch (Exception exception)
            {
                Logging.Error("Unable to import RdcMan file", exception);
                return new List<FavoriteConfigurationElement>();
            }
        }

        private static List<FavoriteConfigurationElement> TryImport(string fileName)
        {
            var document = new RdcManDocument(fileName);
            if (!document.IsVersion22)
                throw new NotSupportedException("Rdc manager supports only version 2.2 import");

            var context = new RdcManImportContext(document.Groups, document.Servers);
            context.ImportContent();
            return context.Imported;
        }
    }
}
