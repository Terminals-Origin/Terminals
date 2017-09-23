namespace Terminals.Integration.Export
{
    internal class TerminalsHTTPExport : ITerminalsOptionsExport
    {
        public void ExportOptions(IExportOptionsContext context)
        {
            if (context.Favorite.Protocol.Contains("HTTP"))
                TerminalsWebExport.ExportOptions(ref context);
        }
    }

    internal class TerminalsHTTPSExport : ITerminalsOptionsExport
    {
        public void ExportOptions(IExportOptionsContext context)
        {
            if (context.Favorite.Protocol.Contains("HTTP"))
                TerminalsWebExport.ExportOptions(ref context);
        }
    }

    internal class TerminalsWebExport
    {
        public static void ExportOptions(ref IExportOptionsContext context)
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
