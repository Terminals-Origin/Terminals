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
        
        public void Export(string fileName, List<FavoriteConfigurationElement> favorites, bool includePassword)
        {
            foreach (FavoriteConfigurationElement favorite in favorites)
            {
                if (favorites.Count > 1)
                {
                    string fileNameWithSuffix = AddFileNameSuffix(favorite.Name, fileName);
                    ExportFavorite(fileNameWithSuffix, favorite);
                }
                else
                   ExportFavorite(fileName, favorite);
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
            return String.Format("{0}_{1}{2}", filePrefix, favoriteName, ImportRDP.FILE_EXTENSION);
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
                Logging.Log.Error("RDP export failed.", exception);
            }
        }

        private static string ExportFileContent(FavoriteConfigurationElement favorite)
        {
            StringBuilder fileContent = new StringBuilder();

            AppendPropertyLine(fileContent, "full address:s:", favorite.ServerName);
            AppendPropertyLine(fileContent, "server port:i:", favorite.Port.ToString());
            AppendPropertyLine(fileContent, "username:s:", favorite.UserName);
            AppendPropertyLine(fileContent, "domain:s:", favorite.DomainName);
            AppendPropertyLine(fileContent, "session bpp:i:", ConvertToColorBits(favorite.Colors).ToString());
            AppendPropertyLine(fileContent, "screen mode id:i:", ConvertDesktopSize(favorite.DesktopSize));
            AppendPropertyLine(fileContent, "connect to console:i:", ConvertToString(favorite.ConnectToConsole));
            AppendPropertyLine(fileContent, "disable wallpaper:i:", ConvertToString(favorite.DisableWallPaper));
            AppendPropertyLine(fileContent, "redirectsmartcards:i:", ConvertToString(favorite.RedirectSmartCards));
            AppendPropertyLine(fileContent, "redirectcomports:i:", ConvertToString(favorite.RedirectPorts));
            AppendPropertyLine(fileContent, "redirectprinters:i:", ConvertToString(favorite.RedirectPrinters));
            AppendPropertyLine(fileContent, "gatewayhostname:s:", favorite.TsgwHostname);
            AppendPropertyLine(fileContent, "gatewayusagemethod:i:", favorite.TsgwUsageMethod.ToString());
            AppendPropertyLine(fileContent, "audiomode:i:", ConvertFromSounds(favorite.Sounds).ToString());
            // todo Add rest of the properties in RDP import/export

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
