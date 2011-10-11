using System;
using System.Collections.Generic;
using System.IO;

namespace Terminals.Integration.Import
{
    /// <summary>
    /// Note, that passwords are no longer stored in rdp file since version protocol version 6,
    /// so they couldnt be imported/exported
    /// </summary>
    internal class ImportRDP : IImport
    {
        internal const string FILE_EXTENSION = ".rdp";
        internal const string NAME = "Microsoft Remote Desktop";

        #region Property names

        internal const string AUDIOMODE = "audiomode:i:";
        internal const string BITMAPPERISTENCE = "bitmapcachepersistenable:i:";
        internal const string CONNECTTOCONSOLE = "connect to console:i:";
        internal const string COLORS = "session bpp:i:";
        internal const string DISABLECURSORSETTING = "disable cursor setting:i:";
        internal const string DISABLEFULLWINDOWDRAG = "disable full window drag:i:";
        internal const string DISABLEWALLPAPER = "disable wallpaper:i:";
        internal const string DISABLEMENUANIMATIONS = "disable menu anims:i:";
        internal const string DISABLETHEMING = "disable themes:i:";
        /// <summary>
        /// 0 - On the local computer, 1 - On the remote computer, 2 - In fullscreen mode only
        /// </summary>
        internal const string DISABLEWINDOWSKEY = "keyboardhook:i:";
        internal const string DISPLAYCONNECTIONBAR = "displayconnectionbar:i:";
        internal const string DOMAIN = "domain:s:";
        internal const string ENABLECOMPRESSION = "compression:i:";
        internal const string ENABLEDESKTOPCOMPOSITION = "allow desktop composition:i:";
        internal const string ENABLEFONTSMOOTHING = "allow font smoothing:i:";
        internal const string FULLADDRES = "full address:s:";
        internal const string REDIRECTCLIPBOARD = "redirectclipboard:i:";
        internal const string REDIRECTDEVICES = "redirectposdevices:i:";
        internal const string REDIRECTCOMPORTS = "redirectcomports:i:";
        internal const string REDIRECTPRINTERS = "redirectprinters:i:";
        internal const string REDIRECTSMARTCARDS = "redirectsmartcards:i:";
        internal const string SCREENMODE = "screen mode id:i:";
        internal const string SERVERPORT = "server port:i:";
        internal const string TSGWCREDSSOURCE = "gatewaycredentialssource:i:";
        internal const string TSGHOSTNAME = "gatewayhostname:s:";
        internal const string TSGUSAGEMETHOD = "gatewayusagemethod:i:";
        internal const string USERNAME = "username:s:";

        #endregion

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
                int valueStartIndex = line.LastIndexOf(":") + 1;
                string propertyName = line.Substring(0, valueStartIndex);
                string propertyValue = line.Substring(valueStartIndex);
                ImportProperty(favorite, propertyName, propertyValue);
            }
        }

        private static void ImportProperty(FavoriteConfigurationElement favorite, string propertyName, string propertyValue)
        {
            switch (propertyName)
            {
                case FULLADDRES:
                    favorite.ServerName = propertyValue;
                    break;
                case SERVERPORT:
                    int port = 3389;
                    int.TryParse(propertyValue, out port);
                    favorite.Port = port;
                    break;
                case USERNAME:
                    favorite.UserName = propertyValue;
                    break;
                case DOMAIN:
                    favorite.DomainName = propertyValue;
                    break;
                case COLORS:
                    favorite.Colors = ConvertToColorBits(propertyValue);
                    break;
                case SCREENMODE:
                    favorite.DesktopSize = ConvertToDesktopSize(propertyValue);
                    break;
                case CONNECTTOCONSOLE:
                    favorite.ConnectToConsole = ParseBoolean(propertyValue);
                    break;
                case DISABLEWALLPAPER:
                    favorite.DisableWallPaper = ParseBoolean(propertyValue);
                    break;
                case REDIRECTSMARTCARDS:
                    favorite.RedirectSmartCards = ParseBoolean(propertyValue);
                    break;
                case REDIRECTCOMPORTS:
                    favorite.RedirectPorts = ParseBoolean(propertyValue);
                    break;
                case REDIRECTPRINTERS:
                    favorite.RedirectPrinters = ParseBoolean(propertyValue);
                    break;
                case TSGHOSTNAME:
                    favorite.TsgwHostname = propertyValue;
                    break;
                case TSGUSAGEMETHOD:
                    int tsgwUsageMethod = 0;
                    int.TryParse(propertyValue, out tsgwUsageMethod);
                    favorite.TsgwUsageMethod = tsgwUsageMethod;
                    break;
                case AUDIOMODE:
                    favorite.Sounds = ConvertToSounds(propertyValue);
                    break;
                case ENABLECOMPRESSION:
                    favorite.EnableCompression = ParseBoolean(propertyValue);
                    break;
                case ENABLEFONTSMOOTHING:
                    favorite.EnableFontSmoothing = ParseBoolean(propertyValue);
                    break;
                case REDIRECTCLIPBOARD:
                    favorite.RedirectClipboard = ParseBoolean(propertyValue);
                    break;
                case DISABLEWINDOWSKEY:
                    favorite.DisableWindowsKey = ParseBoolean(propertyValue);
                    break;
                case DISPLAYCONNECTIONBAR:
                    favorite.DisplayConnectionBar = ParseBoolean(propertyValue);
                    break;
                case DISABLEMENUANIMATIONS:
                    favorite.DisableMenuAnimations = ParseBoolean(propertyValue);
                    break;
                case DISABLETHEMING:
                    favorite.DisableTheming = ParseBoolean(propertyValue);
                    break;
                case DISABLEFULLWINDOWDRAG:
                    favorite.DisableFullWindowDrag = ParseBoolean(propertyValue);
                    break;
                case ENABLEDESKTOPCOMPOSITION:
                    favorite.EnableDesktopComposition = ParseBoolean(propertyValue);
                    break;
                case DISABLECURSORSETTING:
                    favorite.DisableCursorBlinking = ParseBoolean(propertyValue);
                    favorite.DisableCursorShadow = favorite.DisableCursorBlinking;
                    break;
                case BITMAPPERISTENCE:
                    favorite.BitmapPeristence = ParseBoolean(propertyValue);
                    break;
                case REDIRECTDEVICES:
                    favorite.RedirectDevices = ParseBoolean(propertyValue);
                    break;
                case TSGWCREDSSOURCE:
                    int tsgwCredsSource = 0;
                    int.TryParse(propertyValue, out tsgwCredsSource);
                    favorite.TsgwCredsSource = tsgwCredsSource;
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