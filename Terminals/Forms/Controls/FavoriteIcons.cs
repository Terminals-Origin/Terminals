using System;
using System.Drawing;
using System.IO;
using Terminals.Connections;
using Terminals.Properties;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Loading of icons from files
    /// </summary>
    internal static class FavoriteIcons
    {
        /// <summary>
        /// Gets the icon indexes by icons defined in FavoritesTreeView imageListIcons
        /// </summary>
        internal static Int32 GetTreeviewImageListIndex(FavoriteConfigurationElement favorite)
        {
            switch (favorite.Protocol)
            {
                case ConnectionManager.RDP:
                    return 2;
                case ConnectionManager.HTTP:
                case ConnectionManager.HTTPS:
                    return 3;
                case ConnectionManager.VNC:
                    return 4;
                case ConnectionManager.TELNET:
                    return 5;
                case ConnectionManager.SSH:
                    return 6;
                case ConnectionManager.VMRC:
                default:
                    return 7;
            }
        }

        /// <summary>
        /// Gets the icon indexes by icons defined in FavoritesTreeView imageListIcons
        /// </summary>
        private static Image GetProtocolImage(FavoriteConfigurationElement favorite)
        {
            switch (favorite.Protocol)
            {
                case ConnectionManager.RDP:
                    return Resources.treeIcon_rdp;
                case ConnectionManager.HTTP:
                case ConnectionManager.HTTPS:
                    return Resources.treeIcon_http;
                case ConnectionManager.VNC:
                    return Resources.treeIcon_vnc;
                case ConnectionManager.TELNET:
                    return Resources.treeIcon_telnet;
                case ConnectionManager.SSH:
                    return Resources.treeIcon_ssh;
                case ConnectionManager.VMRC:
                default:
                    return Resources.terminalsicon;
            }
        }

        internal static Image GetFavoriteIcon(FavoriteConfigurationElement favorite)
        {
            if (String.IsNullOrEmpty(favorite.ToolBarIcon))
            {
                return GetProtocolImage(favorite);
            }

            return LoadImage(favorite.ToolBarIcon, Resources.smallterm);
        }

        internal static Image LoadImage(String imageFilePath, Image defaultIcon)
        {
            try
            {
                if (!String.IsNullOrEmpty(imageFilePath) && File.Exists(imageFilePath))
                {
                    return Image.FromFile(imageFilePath);
                }
            }
            catch (Exception ex)
            {
                Logging.Log.Error("Error Loading menu item image", ex);
            }

            return defaultIcon;
        }
    }
}
