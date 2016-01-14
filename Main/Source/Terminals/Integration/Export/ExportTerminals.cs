using System;
using System.Text;
using System.Xml;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Integration.Import;

namespace Terminals.Integration.Export
{
    /// <summary>
    /// This is the Terminals native exporter, which exports favorites into its own xml file
    /// </summary>
    internal class ExportTerminals : IExport
    {
        private readonly IPersistence persistence;

        string IIntegration.Name
        {
            get { return ImportTerminals.PROVIDER_NAME; }
        }

        string IIntegration.KnownExtension
        {
            get { return ImportTerminals.TERMINALS_FILEEXTENSION; }
        }

        public ExportTerminals(IPersistence persistence)
        {
            this.persistence = persistence;
        }

        public void Export(ExportOptions options)
        {
            try
            {
                using (var w = new XmlTextWriter(options.FileName, Encoding.UTF8))
                {
                    w.Formatting = Formatting.Indented;
                    w.WriteStartDocument();
                    w.WriteStartElement("favorites");
                    foreach (FavoriteConfigurationElement favorite in options.Favorites)
                    {
                        var favoriteSecurity = new FavoriteConfigurationSecurity(this.persistence, favorite);
                        var context = new ExportOptionsContext(w, favoriteSecurity, options.IncludePasswords, favorite);
                        WriteFavorite(context);
                    }
                    w.WriteEndElement();
                    w.WriteEndDocument();
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                Logging.Error("Export XML Failed", ex);
            }
        }

        private void WriteFavorite(ExportOptionsContext context)
        {
            context.Writer.WriteStartElement("favorite");
            
            ExportGeneralOptions(context.Writer, context.Favorite);
            ExportCredentials(context);
            ExportExecuteBeforeConnect(context.Writer, context.Favorite);

            foreach (ITerminalsOptionsExport optionsExporter in ConnectionManager.Instance.GetTerminalsOptionsExporters())
            {
                optionsExporter.ExportOptions(context);
            }

            context.Writer.WriteEndElement();
        }

        private static void ExportGeneralOptions(XmlTextWriter w, FavoriteConfigurationElement favorite)
        {
            w.WriteElementString("protocol", favorite.Protocol);
            w.WriteElementString("port", favorite.Port.ToString());
            w.WriteElementString("serverName", favorite.ServerName);
            w.WriteElementString("url", favorite.Url);
            w.WriteElementString("name", favorite.Name);
            w.WriteElementString("notes", favorite.Notes);
            w.WriteElementString("tags", favorite.Tags);
            w.WriteElementString("newWindow", favorite.NewWindow.ToString());
            w.WriteElementString("toolBarIcon", favorite.ToolBarIcon);
            w.WriteElementString("bitmapPeristence", favorite.Protocol);
        }

        private void ExportCredentials(ExportOptionsContext context)
        {
            FavoriteConfigurationElement favorite = context.Favorite;

            var favoriteSecurity = new FavoriteConfigurationSecurity(this.persistence, favorite);
            context.WriteElementString("credential", favorite.Credential);
            context.WriteElementString("domainName", favoriteSecurity.ResolveDomainName());

            if (context.IncludePasswords)
            {
                context.WriteElementString("userName", favoriteSecurity.ResolveUserName());
                context.WriteElementString("password", favoriteSecurity.Password);
            }
        }

        private static void ExportExecuteBeforeConnect(XmlTextWriter w, FavoriteConfigurationElement favorite)
        {
            if (favorite.ExecuteBeforeConnect)
            {
                w.WriteElementString("executeBeforeConnect", favorite.ExecuteBeforeConnect.ToString());
                w.WriteElementString("executeBeforeConnectCommand", favorite.ExecuteBeforeConnectCommand);
                w.WriteElementString("executeBeforeConnectArgs", favorite.ExecuteBeforeConnectArgs);
                w.WriteElementString("executeBeforeConnectInitialDirectory", favorite.ExecuteBeforeConnectInitialDirectory);
                w.WriteElementString("executeBeforeConnectWaitForExit", favorite.ExecuteBeforeConnectWaitForExit.ToString());
            }
        }
    }
}
