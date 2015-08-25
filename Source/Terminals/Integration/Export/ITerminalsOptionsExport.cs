using System.Xml;

namespace Terminals.Integration.Export
{
    internal interface ITerminalsOptionsExport
    {
        void ExportOptions(XmlTextWriter w, FavoriteConfigurationElement favorite);
    }
}