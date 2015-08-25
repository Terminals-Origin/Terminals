using System.Xml;
using Terminals.Connections;

namespace Terminals.Integration.Export
{
    internal class TerminalsVncExport
    {
        public static void ExportVncOptions(XmlTextWriter w, FavoriteConfigurationElement favorite)
        {
            if (favorite.Protocol == ConnectionManager.VNC)
            {
                w.WriteElementString("vncAutoScale", favorite.VncAutoScale.ToString());
                w.WriteElementString("vncViewOnly", favorite.VncViewOnly.ToString());
                w.WriteElementString("vncDisplayNumber", favorite.VncDisplayNumber.ToString());
            }
        }
    }
}