using System;
using System.Drawing;
using System.IO;
using Terminals.Configuration;
using Terminals.Connections;
using Terminals.Properties;

namespace Terminals.Data
{
    /// <summary>
    /// Loading of icons from files
    /// </summary>
    internal static class FavoriteIcons
    {
        /// <summary>
        /// Gets empty bytes arrray used as empty not assigned image data.
        /// </summary>
        internal static byte[] EmptyImageData
        {
            get
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// Gets the icon file name by icons defined in FavoritesTreeView imageListIcons
        /// </summary>
        internal static string GetTreeviewImageListKey(IFavorite favorite)
        {
            switch (favorite.Protocol)
            {
                case ConnectionManager.RDP:
                    return "treeIcon_rdp.png";
                case ConnectionManager.HTTP:
                case ConnectionManager.HTTPS:
                    return "treeIcon_http.png";
                case ConnectionManager.VNC:
                    return "treeIcon_vnc.png";
                case ConnectionManager.TELNET:
                    return "treeIcon_telnet.png";
                case ConnectionManager.SSH:
                    return "treeIcon_ssh.png";
                case ConnectionManager.VMRC:
                default:
                    return "terminalsicon.ico";
            }
        }

        /// <summary>
        /// Gets the icon indexes by icons defined in FavoritesTreeView imageListIcons
        /// </summary>
        private static Image GetProtocolImage(IFavorite favorite)
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

        internal static Image GetFavoriteIcon(IFavorite favorite)
        {
            if (String.IsNullOrEmpty(favorite.ToolBarIconFile))
            {
                return GetProtocolImage(favorite);
            }

            return LoadImage(favorite.ToolBarIconFile, Resources.smallterm);
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

        internal static Image LoadImage(string value, IFavorite favorite)
        {
            byte[] imageData = LoadImage(value);
            return LoadImage(imageData, favorite);
        }

        internal static Image LoadImage(byte[] imageData, IFavorite favorite)
        {
            try
            {
                // empty or not assign icon replace by default icon
                if (imageData.Length == 0) 
                    GetProtocolImage(favorite);

                return LoadFromBinary(imageData);
            }
            catch
            {
                return GetProtocolImage(favorite);
            }
        }

        private static Image LoadFromBinary(byte[] imageData)
        {
            using (var memoryStream = new MemoryStream(imageData))
            {
                return Image.FromStream(memoryStream, true);
            }
        }

        private static byte[] LoadImage(string imageFilePath)
        {
            try
            {
                return TryLoadImage(imageFilePath);
            }
            catch
            {
                return EmptyImageData;
            }
        }

        private static byte[] TryLoadImage(string imageFilePath)
        {
            using (var file = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                FileInfo fileInfo = new FileInfo(imageFilePath);
                int imageLength = (int)fileInfo.Length;
                byte[] imageData = new byte[imageLength];
                file.Read(imageData, 0, imageLength);
                return imageData;
            }
        }

        internal static string CopySelectedImageToThumbsDir(string newImagefilePath)
        {
            string newFileName = Path.GetFileName(newImagefilePath);
            String newFileInThumbsDir = Path.Combine(FileLocations.ThumbsDirectoryFullPath, newFileName);

            // the file wasnt selected directly from Thumbs dir, otherwise we dont need to copy it
            if (newFileInThumbsDir != newImagefilePath && !File.Exists(newFileInThumbsDir))
                File.Copy(newImagefilePath, newFileInThumbsDir);

            return newFileInThumbsDir;
        }

        internal static byte[] ImageToBinary(Image image)
        {
            if (IsDefaultProtocolImage(image))
                return EmptyImageData;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, image.RawFormat);
                return memoryStream.ToArray();
            }
        }

        private static bool IsDefaultProtocolImage(Image image)
        {
            return image == Resources.treeIcon_rdp ||
                   image == Resources.treeIcon_http ||
                   image == Resources.treeIcon_vnc ||
                   image == Resources.treeIcon_telnet ||
                   image == Resources.treeIcon_ssh ||
                   image == Resources.terminalsicon;
        }
    }
}
