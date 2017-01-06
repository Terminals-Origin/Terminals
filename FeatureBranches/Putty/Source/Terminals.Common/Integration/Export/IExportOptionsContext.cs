using System.Xml;

namespace Terminals.Integration.Export
{
    public interface IExportOptionsContext
    {
        FavoriteConfigurationElement Favorite { get; }

        XmlTextWriter Writer { get; }

        bool IncludePasswords { get; }

        string TsgwPassword { get; }

        void WriteElementString(string elementName, string elementValue);
    }
}