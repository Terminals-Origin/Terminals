using System;
using System.Collections.Generic;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    /// <summary>
    /// Virtual tree node used to collect favorites without assigned any group
    /// </summary>
    [Obsolete("Favorites loaded doesnt group untagged favorites into separate tree node")]
    internal sealed class UntagedGroupNode : GroupTreeNode
    {
        internal UntagedGroupNode()
            : base(Settings.UNTAGGED_NODENAME)
        {
        }

        internal override void UpdateByGroup(IGroup @group)
        {
            // nothing to do here, this node name is fixed
        }

        internal override bool HasGroupIn(IEnumerable<IGroup> requiredGroups)
        {
            return false; // no associated group
        }
    }
}
