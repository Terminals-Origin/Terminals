using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Terminals.Connections;
using Terminals.Integration.Import;

namespace Terminals.Integration.Export
{
    /// <summary>
    /// http://www.xtralogic.com/rdc_help.shtml
    /// </summary>
    internal class ExtraLogicAndroidRd : IExport
    {
        internal const string EXTENSION = ".extrard";

        public string Name
        {
            get { return "Xtralogic Remote Desktop Client for Android"; }
        }

        public string KnownExtension
        {
            get { return EXTENSION; }
        }

        public void Export(string fileName, List<FavoriteConfigurationElement> favorites, bool includePassword)
        {
            try
            {
                XDocument doc = new XDocument(new XElement("servers"));
                ExportFavorites(doc, favorites);
                doc.Save(fileName);
            }
            catch (Exception exception)
            {
                Logging.Log.Error("Export to ExtraLogicAndroidRd failed.", exception);
            }
        }

        private static void ExportFavorites(XDocument doc, List<FavoriteConfigurationElement> favorites)
        {
            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                if (favorite.Protocol.Equals(ConnectionManager.RDP))
                {
                    doc.Root.Add(ExportFavorite(favorite));
                }
            }
        }

        private static XElement ExportFavorite(FavoriteConfigurationElement favorite)
        {
            int audioMode = ExportRdp.ConvertFromSounds(favorite.Sounds);
            int colorBits = ExportRdp.ConvertToColorBits(favorite.Colors);

            return new XElement("server",

                // analogic to RDP
                new XAttribute("full-address", favorite.ServerName),
                new XAttribute("server-port", favorite.Port),
                new XAttribute("username", favorite.UserName),
                new XAttribute("domain", favorite.DomainName),
                new XAttribute("desktopwidth", favorite.DesktopSizeWidth),
                new XAttribute("desktopheight", favorite.DesktopSizeHeight),
                new XAttribute("session-bpp", colorBits),
                new XAttribute("audiomode", audioMode),
                new XAttribute("connect-to-console", Convert.ToByte(favorite.ConnectToConsole)),
                new XAttribute("compression", Convert.ToByte(favorite.EnableCompression)),
                new XAttribute("disable-cursor-setting",
                                 Convert.ToByte(favorite.DisableCursorBlinking && favorite.DisableCursorShadow)),
                new XAttribute("disable-full-window-drag", Convert.ToByte(favorite.DisableFullWindowDrag)),
                new XAttribute("disable-menu-anims", Convert.ToByte(favorite.DisableMenuAnimations)),
                new XAttribute("disable-themes", Convert.ToByte(favorite.DisableTheming)),
                new XAttribute("disable-wallpaper", Convert.ToByte(favorite.DisableWallPaper)),
                new XAttribute("allow-font-smoothing", Convert.ToByte(favorite.EnableFontSmoothing)),
                new XAttribute("redirectdrives", "1"),
                new XAttribute("redirectclipboard", Convert.ToByte(favorite.RedirectClipboard)),
                new XAttribute("alternate-shell", ""),
                new XAttribute("shell-working-directory", ""),
                new XAttribute("gatewayusagemethod", favorite.TsgwUsageMethod),
                new XAttribute("gatewayhostname", favorite.TsgwHostname),

                // application specific
                new XAttribute("xtr-description", favorite.Notes),
                new XAttribute("xtr-security-layer", 0),
                new XAttribute("xtr-use-server-creds-for-gateway", Convert.ToByte(favorite.TsgwSeparateLogin)),
                new XAttribute("xtr-input-locale", 1033),
                new XAttribute("xtr-switch-mouse-buttons", 0)
                );
        }
    }
}
