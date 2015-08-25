using System;
using System.Text;
using System.Xml;
using Terminals.Connections;
using Terminals.Integration.Import;

namespace Terminals.Integration.Export
{
    /// <summary>
    /// This is the Terminals native exporter, which exports favorites into its own xml file
    /// </summary>
    internal class ExportTerminals : IExport
    {
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
                        WriteFavorite(w, options.IncludePasswords, favorite);
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

        private static void WriteFavorite(XmlTextWriter w, bool includePassword, FavoriteConfigurationElement favorite)
        {
            w.WriteStartElement("favorite");

            ExportGeneralOptions(w, favorite);
            ExportCredentials(w, includePassword, favorite);
            ExportExecuteBeforeConnect(w, favorite);

            foreach (ITerminalsOptionsExport optionsExporter in ConnectionManager.GetTerminalsOptionsExporters())
            {
                optionsExporter.ExportOptions(w, favorite);
            }

            w.WriteEndElement();
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

        private static void ExportCredentials(XmlTextWriter w, bool includePassword, FavoriteConfigurationElement favorite)
        {
            w.WriteElementString("credential", favorite.Credential);
            w.WriteElementString("domainName", favorite.DomainName);

            if (includePassword)
            {
                w.WriteElementString("userName", favorite.UserName);
                w.WriteElementString("password", favorite.Password);
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

        string IIntegration.Name
        {
            get { return ImportTerminals.PROVIDER_NAME; }
        }

        string IIntegration.KnownExtension
        {
            get { return ImportTerminals.TERMINALS_FILEEXTENSION; }
        }
    }
}
