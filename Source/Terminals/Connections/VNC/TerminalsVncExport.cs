using Terminals.Connections.VNC;

namespace Terminals.Integration.Export
{
    internal class TerminalsVncExport : ITerminalsOptionsExport
    {
        public void ExportOptions(ExportOptionsContext context)
        {
            if (context.Favorite.Protocol == VncConnectionPlugin.VNC)
            {
                context.WriteElementString("vncAutoScale", context.Favorite.VncAutoScale.ToString());
                context.WriteElementString("vncViewOnly", context.Favorite.VncViewOnly.ToString());
                context.WriteElementString("vncDisplayNumber", context.Favorite.VncDisplayNumber.ToString());
            }
        }
    }
}