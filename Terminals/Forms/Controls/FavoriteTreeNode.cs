using System;
using System.Windows.Forms;
using Terminals.Connections;

namespace Terminals.Forms.Controls
{
    internal class FavoriteTreeNode : TreeNode
    {
        internal FavoriteTreeNode(FavoriteConfigurationElement favorite)
            : base(favorite.Name)
        {
            this.Name = favorite.Name;
            this.Favorite = favorite;
            this.Tag = favorite; // temporar solution, for backwarad compatibility only

            this.ImageIndex = IconIndexFromProtocol();
            this.SelectedImageIndex = this.ImageIndex;

            String service = ConnectionManager.GetPortName(favorite.Port, true);
            this.ToolTipText = String.Format("{0} service at {1} (port {2})",
                service, favorite.ServerName, favorite.Port);
        }

        /// <summary>
        /// Gets or sets the corresponding connection favorite
        /// </summary>
        internal FavoriteConfigurationElement Favorite { get; private set; }

        /// <summary>
        /// Gets the icon indexes by icons defined in FavoritesTreeView imageListIcons
        /// </summary>
        private Int32 IconIndexFromProtocol()
        {
            switch (this.Favorite.Protocol)
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
    }
}
