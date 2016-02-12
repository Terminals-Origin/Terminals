using Terminals.Connections.VMRC;

namespace Terminals.Integration.Export
{
    internal class TerminalsVmrcExport : ITerminalsOptionsExport
    {
        public void ExportOptions(IExportOptionsContext context)
        {
            if (context.Favorite.Protocol == VmrcConnectionPlugin.VMRC)
            {
                context.WriteElementString("vmrcadministratormode", context.Favorite.VMRCAdministratorMode.ToString());
                context.WriteElementString("vmrcreducedcolorsmode", context.Favorite.VMRCReducedColorsMode.ToString());
            }
        }
    }
}