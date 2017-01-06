using System.Xml;
using Terminals.Configuration;

namespace Terminals.Integration.Export
{
    public class ExportOptionsContext : IExportOptionsContext
    {
        private readonly FavoriteConfigurationSecurity favoriteSecurity;

        public XmlTextWriter Writer { get; private set; }

        public bool IncludePasswords { get; private set; }

        public FavoriteConfigurationElement Favorite { get; private set; }

        public string TsgwPassword { get { return this.favoriteSecurity.TsgwPassword; } }

        internal ExportOptionsContext(XmlTextWriter writer, FavoriteConfigurationSecurity favoriteSecurity,
            bool includePasswords, FavoriteConfigurationElement favorite)
        {
            this.favoriteSecurity = favoriteSecurity;
            this.Writer = writer;
            this.IncludePasswords = includePasswords;
            this.Favorite = favorite;
        }

        public void WriteElementString(string elementName, string value)
        {
            this.Writer.WriteElementString(elementName, value);
        }
    }
}