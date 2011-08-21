using System.Collections.Generic;

namespace Terminals.Integration.Import
{
    internal class ImportTerminals : IImport
    {
        internal const string TERMINALS_FILEEXTENSION = ".xml";
        
        #region IImport members

        List<FavoriteConfigurationElement> IImport.ImportFavorites(string Filename)
        {
            return ExportImport.ExportImport.ImportXML(Filename, true);
        }

        string IImport.Name
        {
            get { return "Terminals favorites"; }
        }

        string IImport.KnownExtension
        {
            get { return TERMINALS_FILEEXTENSION; }
        }
        
        #endregion
    }
}
