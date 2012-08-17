using System.Collections.Generic;
using System.Linq;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Virtual tree node used to collect favorites without assigned any group
    /// </summary>
    internal sealed class UntagedGroupNode : GroupTreeNode
    {
        internal override List<IFavorite> Favorites 
        {
            get
            {
                return GetNotGroupedFavorites();
            }
        }

        /// <summary>
        /// Gets favorites, which arent listed in any group
        /// </summary>
        internal static List<IFavorite> GetNotGroupedFavorites()
        {
            var relevantFavorites = Persistence.Instance.Favorites
                .Where(candidate => candidate.Groups.Count == 0);
            return Data.Favorites.OrderByDefaultSorting(relevantFavorites);
        }

        internal UntagedGroupNode()
            : base(Settings.UNTAGGED_NODENAME)
        {
        }

        internal override void UpdateByGroupName()
        {
            // nothing to do here, this node name is fixed
        }
    }
}
