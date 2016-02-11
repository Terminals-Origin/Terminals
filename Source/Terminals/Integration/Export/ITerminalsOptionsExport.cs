using System.Xml;

namespace Terminals.Integration.Export
{
    internal interface ITerminalsOptionsExport
    {
        void ExportOptions(IExportOptionsContext context);
    }

    internal interface IExportOptionsContext
    {
        FavoriteConfigurationElement Favorite { get; }

        XmlTextWriter Writer { get; }

        bool IncludePasswords { get; }

        string TsgwPassword { get; }

        void WriteElementString(string elementName, string elementValue);
    }
}