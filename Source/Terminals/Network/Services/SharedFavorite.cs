using System;
using System.Collections.Generic;
using System.Text;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Network {
    [Serializable()]
    public class SharedFavorite {
        public static Terminals.FavoriteConfigurationElement ConvertFromFavorite(SharedFavorite Favorite) {
            Terminals.FavoriteConfigurationElement fav = new Terminals.FavoriteConfigurationElement();
            fav.Colors = Favorite.Colors;
            fav.ConnectToConsole = Favorite.ConnectToConsole;
            fav.DesktopShare = Favorite.DesktopShare;
            fav.DesktopSize = Favorite.DesktopSize;
            fav.DomainName = Favorite.DomainName;
            fav.Name = Favorite.Name;
            fav.Port = Favorite.Port;
            fav.Protocol = Favorite.Protocol;
            fav.RedirectClipboard = Favorite.RedirectClipboard;
            fav.RedirectDevices = Favorite.RedirectDevices;
            fav.RedirectedDrives = Favorite.RedirectedDrives;
            fav.RedirectPorts = Favorite.RedirectPorts;
            fav.RedirectPrinters = Favorite.RedirectPrinters;
            fav.RedirectSmartCards = Favorite.RedirectSmartCards;
            fav.ServerName = Favorite.ServerName;
            fav.DisableWallPaper = Favorite.DisableWallPaper;
            fav.Sounds = Favorite.Sounds;
            fav.Tags = Favorite.Tags;
            fav.ConsoleBackColor = Favorite.ConsoleBackColor;
            fav.ConsoleCols = Favorite.ConsoleCols;
            fav.ConsoleCursorColor = Favorite.ConsoleCursorColor;
            fav.ConsoleFont = Favorite.ConsoleFont;
            fav.ConsoleRows = Favorite.ConsoleRows;
            fav.ConsoleTextColor = Favorite.ConsoleTextColor;
            fav.VMRCAdministratorMode = Favorite.VMRCAdministratorMode;
            fav.VMRCReducedColorsMode = Favorite.VMRCReducedColorsMode;

            return fav;
        }
        internal static SharedFavorite ConvertFromFavorite(IPersistence persistence, FavoriteConfigurationElement Favorite) {
            var favoriteSecurity = new FavoriteConfigurationSecurity(persistence.Credentials, Favorite);
            SharedFavorite fav = new SharedFavorite();
            fav.Colors = Favorite.Colors;
            fav.ConnectToConsole = Favorite.ConnectToConsole;
            fav.DesktopShare = Favorite.DesktopShare;
            fav.DesktopSize = Favorite.DesktopSize;
            fav.DomainName = favoriteSecurity.ResolveDomainName();
            fav.Name = Favorite.Name;
            fav.Port = Favorite.Port;
            fav.Protocol = Favorite.Protocol;
            fav.RedirectClipboard = Favorite.RedirectClipboard;
            fav.RedirectDevices = Favorite.RedirectDevices;
            fav.RedirectedDrives = Favorite.RedirectedDrives;
            fav.RedirectPorts = Favorite.RedirectPorts;
            fav.RedirectPrinters = Favorite.RedirectPrinters;
            fav.RedirectSmartCards = Favorite.RedirectSmartCards;
            fav.ServerName = Favorite.ServerName;
            fav.DisableWallPaper = Favorite.DisableWallPaper;
            fav.Sounds = Favorite.Sounds;
            fav.Tags = Favorite.Tags;
            fav.ConsoleBackColor = Favorite.ConsoleBackColor;
            fav.ConsoleCols = Favorite.ConsoleCols;
            fav.ConsoleCursorColor = Favorite.ConsoleCursorColor;
            fav.ConsoleFont = Favorite.ConsoleFont;
            fav.ConsoleRows = Favorite.ConsoleRows;
            fav.ConsoleTextColor = Favorite.ConsoleTextColor;
            fav.VMRCAdministratorMode = Favorite.VMRCAdministratorMode;
            fav.VMRCReducedColorsMode = Favorite.VMRCReducedColorsMode;
            return fav;
        }
        public bool VMRCReducedColorsMode;
        public bool Telnet;
        public int ConsoleRows;
        public int ConsoleCols;
        public bool VMRCAdministratorMode;
        public string Protocol;
        public string ConsoleFont;
        public string ConsoleBackColor;
        public string ConsoleTextColor;
        public string ConsoleCursorColor;
        public string Name;
        public string ServerName;
        public string DomainName;
        public bool ConnectToConsole;
        public DesktopSize DesktopSize;
        public Colors Colors;
        public RemoteSounds Sounds;
        public bool RedirectDrives;
        public List<string> RedirectedDrives;
        public bool RedirectPorts;
        public bool RedirectPrinters;
        public bool RedirectSmartCards;
        public bool RedirectClipboard;
        public bool RedirectDevices;
        public int Port;
        public string DesktopShare;
        public bool DisableWallPaper;
        public string Tags;
    }
}