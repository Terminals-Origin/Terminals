using System.Xml;
using Terminals.Connections;

namespace Terminals.Integration.Export
{
    internal class TerminalsVmrcExport : ITerminalsOptionsExport
    {
        public void ExportOptions(XmlTextWriter w, FavoriteConfigurationElement favorite)
        {
            if (favorite.Protocol == ConnectionManager.VMRC)
            {
                w.WriteElementString("vmrcadministratormode", favorite.VMRCAdministratorMode.ToString());
                w.WriteElementString("vmrcreducedcolorsmode", favorite.VMRCReducedColorsMode.ToString());
            }
        }
    }
}