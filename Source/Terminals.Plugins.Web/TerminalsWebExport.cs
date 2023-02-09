using Terminals.Common.Connections;

namespace Terminals.Integration.Export
{
    internal class TerminalsWebExport : ITerminalsOptionsExport
    {
        public void ExportOptions(IExportOptionsContext context)
        {
            if (context.Favorite.Protocol == KnownConnectionConstants.HTTP || context.Favorite.Protocol == KnownConnectionConstants.HTTPS)
            {
                context.WriteElementString("UsernameID", context.Favorite.UsernameID);
                context.WriteElementString("PasswordID", context.Favorite.PasswordID);
                context.WriteElementString("OptionalID", context.Favorite.OptionalID);
                context.WriteElementString("OptionalValue", context.Favorite.OptionalValue);
                context.WriteElementString("SubmitID", context.Favorite.SubmitID);
                context.WriteElementString("EnableHTMLAuth", context.Favorite.SubmitID);
                context.WriteElementString("EnableFormsAuth", context.Favorite.SubmitID);
            }
        }
    }
}
