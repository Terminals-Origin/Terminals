using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Parameters container for favorites pair during import.
    /// </summary>
    internal class ImportContext
    {
        internal IFavorite ToPerisist { get; set; }

        internal FavoriteConfigurationElement ToImport { get; private set; }

        internal bool Imported { get; set; }

        public ImportContext(FavoriteConfigurationElement toImport)
        {
            this.ToImport = toImport;
        }

        public override string ToString()
        {
            return string.Format("ImportContext:ToImport={0},Imported={1}", this.ToImport.Name, this.Imported);
        }
    }
}