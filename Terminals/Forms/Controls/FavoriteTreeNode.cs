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

            // set image index depending on connection 
            // this.ImageIndex
            String service = ConnectionManager.GetPortName(favorite.Port, true);
            this.ToolTipText = String.Format("{0} service at {1} (port {2})",
                service, favorite.ServerName, favorite.Port);
        }

        /// <summary>
        /// Gets or sets the corresponding connection favorite
        /// </summary>
        internal FavoriteConfigurationElement Favorite { get; private set; }
    }
}
