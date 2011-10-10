using System;
using System.Collections.Generic;
using System.IO;

namespace Terminals.Integration.Import
{
    internal class ImportRDP : IImport
    {
        internal const string FILE_EXTENSION = ".rdp";
        internal const string NAME = "Microsoft Remote Desktop";

        #region IImport Members

        List<FavoriteConfigurationElement> IImport.ImportFavorites(string filename)
        {
            try
            {
                List<FavoriteConfigurationElement> imported = new List<FavoriteConfigurationElement>();
                string name = Path.GetFileName(filename).Replace(Path.GetExtension(filename), "");
                if (File.Exists(filename))
                {
                    string[] lines = File.ReadAllLines(filename);
                    FavoriteConfigurationElement newFavorite = new FavoriteConfigurationElement();
                    newFavorite.Name = name;
                    ImportLines(newFavorite, lines);
                    imported.Add(newFavorite);
                }
                return imported;
            }
            catch (Exception exception)
            {
                Logging.Log.Error("RDP import failed.", exception);
                return new List<FavoriteConfigurationElement>();
            }
        }

        public string Name
        {
            get { return NAME; }
        }

        public string KnownExtension
        {
            get { return FILE_EXTENSION; }
        }

        #endregion

        private static void ImportLines(FavoriteConfigurationElement favorite, string[] lines)
        {
            foreach (string line in lines)
            {
                string propertyName = line.Substring(0, line.IndexOf(":"));
                string propertyValue = line.Substring(line.LastIndexOf(":") + 1);
                ImportProperty(propertyName, favorite, propertyValue);
            }
        }

        private static void ImportProperty(string propertyName, FavoriteConfigurationElement favorite, string propertyValue)
        {
            switch (propertyName)
            {
                case "full address":
                    favorite.ServerName = propertyValue;
                    break;
                case "server port":
                    int port = 3389;
                    int.TryParse(propertyValue, out port);
                    favorite.Port = port;
                    break;
                case "username":
                    favorite.UserName = propertyValue;
                    break;
                case "domain":
                    favorite.DomainName = propertyValue;
                    break;
                case "session bpp":
                    favorite.Colors = ConvertToColorBits(propertyValue);
                    break;
                case "screen mode id":
                    favorite.DesktopSize = ConvertToDesktopSize(propertyValue);
                    break;
                case "connect to console":
                    favorite.ConnectToConsole = ParseBoolean(propertyValue);
                    break;
                case "disable wallpaper":
                    favorite.DisableWallPaper = ParseBoolean(propertyValue);
                    break;
                case "redirectsmartcards":
                    favorite.RedirectSmartCards = ParseBoolean(propertyValue);
                    break;
                case "redirectcomports":
                    favorite.RedirectPorts = ParseBoolean(propertyValue);
                    break;
                case "redirectprinters":
                    favorite.RedirectPrinters = ParseBoolean(propertyValue);
                    break;
                case "gatewayhostname":
                    favorite.TsgwHostname = propertyValue;
                    break;
                case "gatewayusagemethod":
                    int tsgwUsageMethod = 0;
                    int.TryParse(propertyValue, out tsgwUsageMethod);
                    favorite.TsgwUsageMethod = tsgwUsageMethod;
                    break;
                case "audiomode":
                    favorite.Sounds = ConvertToSounds(propertyValue);

                    break;
                default:
                    break;
            }
        }

        private static DesktopSize ConvertToDesktopSize(string propertyValue)
        {
            if (propertyValue == "1") 
                return DesktopSize.FullScreen;

            return DesktopSize.AutoScale;
        }

        private static bool ParseBoolean(string propertyValue)
        {
            if (propertyValue == "1")
                return true;

            return false;
        }

        internal static RemoteSounds ConvertToSounds(string value)
        {
            switch (value)
            {
                case "1": return RemoteSounds.PlayOnServer;
                case "2": return RemoteSounds.DontPlay;
                default: return RemoteSounds.Redirect;
            }
        }

        private static Colors ConvertToColorBits(string value)
        {
            switch (value)
            {
                case "8":
                    return Colors.Bits8;
                case "16":
                    return Colors.Bit16;
                case "24":
                    return Colors.Bits24;
                case "32":
                    return Colors.Bits32;
                default:
                    return Colors.Bit16;
            }
        }
    }
}