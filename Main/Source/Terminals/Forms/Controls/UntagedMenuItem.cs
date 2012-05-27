using System;
using System.Collections.Generic;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Virtual menu item used to collect favorites without assigned any group
    /// </summary>
    internal class UntagedMenuItem : GroupMenuItem
    {
        private readonly Guid groupId;

        internal override Guid GroupId
        {
            get
            {
                return this.groupId;
            }
        }

        internal override List<IFavorite> Favorites
        {
            get
            {
                return UntagedGroupNode.GetNotGroupedFavorites();
            }
        }

        internal UntagedMenuItem()
            : base(Settings.UNTAGGED_NODENAME, true)
        {
            this.groupId = Guid.NewGuid();
        }
    }
}
