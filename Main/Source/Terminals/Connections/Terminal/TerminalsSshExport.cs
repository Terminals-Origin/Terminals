using Terminals.Connections.Terminal;

namespace Terminals.Integration.Export
{
    internal class TerminalsSshExport : ITerminalsOptionsExport
    {
        public void ExportOptions(ExportOptionsContext context)
        {
            if (context.Favorite.Protocol == SshConnectionPlugin.SSH)
            {
                TerminalsConsoleExport.ExportConsoleOptions(context, context.Favorite);

                context.WriteElementString("ssh1", context.Favorite.SSH1.ToString());
                context.WriteElementString("authMethod", context.Favorite.AuthMethod.ToString());
                context.WriteElementString("keyTag", context.Favorite.KeyTag);
                context.WriteElementString("SSHKeyFile", context.Favorite.SSHKeyFile);
            }
        }
    }
}