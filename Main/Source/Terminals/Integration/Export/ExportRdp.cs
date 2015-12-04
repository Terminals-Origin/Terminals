using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Terminals.Integration.Import;

namespace Terminals.Integration.Export
{
    internal class ExportRdp : IExport
    {
        #region IExport members

        public string Name
        {
            get { return ImportRDP.NAME; }
        }

        public string KnownExtension
        {
            get { return ImportRDP.FILE_EXTENSION; }
        }
        
        public void Export(ExportOptions options)
        {
            foreach (FavoriteConfigurationElement favorite in options.Favorites)
            {
                if (options.Favorites.Count > 1)
                {
                    string fileNameWithSuffix = AddFileNameSuffix(favorite.Name, options.FileName);
                    ExportFavorite(fileNameWithSuffix, favorite);
                }
                else
                   ExportFavorite(options.FileName, favorite);
            }
        }

        #endregion

        /// <summary>
        /// Adds favorite name sufix between file name and its extension
        /// </summary>
        private static string AddFileNameSuffix(string favoriteName, string fileName)
        {
            string filePrefix = Path.GetFileNameWithoutExtension(fileName);
            favoriteName = favoriteName.Replace(" ", "_");
            string directory = Path.GetDirectoryName(fileName);
            string newFileName = String.Format("{0}_{1}{2}", filePrefix, favoriteName, ImportRDP.FILE_EXTENSION);
            return Path.Combine(directory, newFileName);
        }

        private static void ExportFavorite(string fileName, FavoriteConfigurationElement favorite)
        {
            try
            {
                string fileContent = ExportFileContent(favorite);
                File.WriteAllText(fileName, fileContent);
            }
            catch (Exception exception)
            {
                Logging.Error("RDP export failed.", exception);
            }
        }

        private static string ExportFileContent(FavoriteConfigurationElement favorite)
        {
            StringBuilder fileContent = new StringBuilder();

            AppendPropertyLine(fileContent, ImportRDP.FULLADDRES, favorite.ServerName);
            AppendPropertyLine(fileContent, ImportRDP.SERVERPORT, favorite.Port.ToString());
            AppendPropertyLine(fileContent, ImportRDP.USERNAME, favorite.ResolveUserName());
            AppendPropertyLine(fileContent, ImportRDP.DOMAIN, favorite.ResolveDomainName());
            AppendPropertyLine(fileContent, ImportRDP.COLORS, ConvertToColorBits(favorite.Colors).ToString());
            AppendPropertyLine(fileContent, ImportRDP.SCREENMODE, ConvertDesktopSize(favorite.DesktopSize));
            AppendPropertyLine(fileContent, ImportRDP.CONNECTTOCONSOLE, ConvertToString(favorite.ConnectToConsole));
            AppendPropertyLine(fileContent, ImportRDP.DISABLEWALLPAPER, ConvertToString(favorite.DisableWallPaper));
            AppendPropertyLine(fileContent, ImportRDP.REDIRECTSMARTCARDS, ConvertToString(favorite.RedirectSmartCards));
            AppendPropertyLine(fileContent, ImportRDP.REDIRECTCOMPORTS, ConvertToString(favorite.RedirectPorts));
            AppendPropertyLine(fileContent, ImportRDP.REDIRECTPRINTERS, ConvertToString(favorite.RedirectPrinters));
            AppendPropertyLine(fileContent, ImportRDP.TSGHOSTNAME, favorite.TsgwHostname);
            AppendPropertyLine(fileContent, ImportRDP.TSGUSAGEMETHOD, favorite.TsgwUsageMethod.ToString());
            AppendPropertyLine(fileContent, ImportRDP.AUDIOMODE, ConvertFromSounds(favorite.Sounds).ToString());
            AppendPropertyLine(fileContent, ImportRDP.ENABLECOMPRESSION, ConvertToString(favorite.EnableCompression));
            AppendPropertyLine(fileContent, ImportRDP.ENABLEFONTSMOOTHING, ConvertToString(favorite.EnableFontSmoothing));
            AppendPropertyLine(fileContent, ImportRDP.REDIRECTCLIPBOARD, ConvertToString(favorite.RedirectClipboard));
            AppendPropertyLine(fileContent, ImportRDP.DISABLEWINDOWSKEY, ConvertToString(favorite.DisableWindowsKey));
            AppendPropertyLine(fileContent, ImportRDP.DISPLAYCONNECTIONBAR, ConvertToString(favorite.DisplayConnectionBar));
            AppendPropertyLine(fileContent, ImportRDP.DISABLEMENUANIMATIONS, ConvertToString(favorite.DisableMenuAnimations));
            AppendPropertyLine(fileContent, ImportRDP.DISABLETHEMING, ConvertToString(favorite.DisableTheming));
            AppendPropertyLine(fileContent, ImportRDP.DISABLEFULLWINDOWDRAG, ConvertToString(favorite.DisableFullWindowDrag));
            AppendPropertyLine(fileContent, ImportRDP.ENABLEDESKTOPCOMPOSITION, ConvertToString(favorite.EnableDesktopComposition));
            bool disablecursorsettings = favorite.DisableCursorBlinking && favorite.DisableCursorShadow;
            AppendPropertyLine(fileContent, ImportRDP.DISABLECURSORSETTING, ConvertToString(disablecursorsettings));
            AppendPropertyLine(fileContent, ImportRDP.BITMAPPERISTENCE, ConvertToString(favorite.BitmapPeristence));
            AppendPropertyLine(fileContent, ImportRDP.REDIRECTDEVICES, ConvertToString(favorite.RedirectDevices));
            AppendPropertyLine(fileContent, ImportRDP.TSGWCREDSSOURCE, favorite.TsgwCredsSource.ToString());
            AppendPropertyLine(fileContent, ImportRDP.LOADBALANCEINFO, favorite.LoadBalanceInfo);

            return fileContent.ToString();
        }

        private static void AppendPropertyLine(StringBuilder builder, string property, string value)
        {
            builder.Append(property);
            builder.Append(value);
            builder.Append(Environment.NewLine);
        }

        private static string ConvertToString(bool propertyValue)
        {
            return Convert.ToByte(propertyValue).ToString();
        }

        internal static int ConvertFromSounds(RemoteSounds sounds)
        {
            switch (sounds)
            {
                case RemoteSounds.PlayOnServer: return 1;
                case RemoteSounds.DontPlay: return 2;
                default: return 0;
            }
        }

        internal static int ConvertToColorBits(Colors colorBits)
        {
            switch (colorBits)
            {
                case Colors.Bits8:
                    return 8;
                case Colors.Bit16:
                    return 16;
                case Colors.Bits24:
                    return 24;
                case Colors.Bits32:
                    return 32;
                default:
                    return 16;
            }
        }

        private static string ConvertDesktopSize(DesktopSize desktopSize)
        {
            if (desktopSize == DesktopSize.FullScreen) 
                return "1";

            return "0";
        }
    }
}
