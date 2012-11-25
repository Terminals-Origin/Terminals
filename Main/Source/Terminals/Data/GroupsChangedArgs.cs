using System;
using System.Collections.Generic;

namespace Terminals.Data
{
    /// <summary>
    /// Tags changed event arguments container, informing about changes in Tags collection
    /// </summary>
    internal class GroupsChangedArgs : EventArgs
    {
        /// <summary>
        /// Newly added IGroups, currently used at least by one connection
        /// </summary>
        internal List<IGroup> Added { get; private set; }

        internal List<IGroup> Updated { get; private set; }

        /// <summary>
        /// All IGroups actually no longer used by any favorite
        /// </summary>
        internal List<IGroup> Removed { get; private set; }

        /// <summary>
        /// Gets the value indicating if there are any added or removed items to report.
        /// </summary>
        internal Boolean IsEmpty
        {
            get
            {
                return this.Added.Count == 0 &&
                       this.Removed.Count == 0 &&
                       this.Updated.Count == 0; 
            }
        }

        internal GroupsChangedArgs()
        {
            this.Added = new List<IGroup>();
            this.Updated = new List<IGroup>();
            this.Removed = new List<IGroup>();
        }

        internal GroupsChangedArgs(List<IGroup> addedGroups, List<IGroup> deletedGroups)
            : this()
        {
            // merge collections to report only differences
            MergeChangeLists(addedGroups, deletedGroups);
            this.Added.AddRange(addedGroups);
            this.Removed.AddRange(deletedGroups);
        }

        private static void MergeChangeLists(List<IGroup> addedGroups, List<IGroup> deletedGroups)
        {
            int index = 0;
            while (index < deletedGroups.Count)
            {
                IGroup deletedIGroup = deletedGroups[index];
                if (addedGroups.Contains(deletedIGroup))
                {   
                    addedGroups.Remove(deletedIGroup);
                    deletedGroups.Remove(deletedIGroup);
                }
                else
                    index++;
            }
        }

        internal void AddFrom(GroupsChangedArgs source)
        {
            var toAdd = ListsHelper.GetMissingSourcesInTarget(source.Added, this.Added);
            this.Added.AddRange(toAdd);
            var toUpdate = ListsHelper.GetMissingSourcesInTarget(source.Updated, this.Updated);
            this.Updated.AddRange(toUpdate);
            var toRemove = ListsHelper.GetMissingSourcesInTarget(source.Removed, this.Removed);
            this.Removed.AddRange(toRemove);
        }

        public override String ToString()
        {
            return String.Format("GroupsChangedArgs:Added={0};Updated {1};Removed={2}",
                this.Added.Count, this.Updated.Count, this.Removed.Count);
        }
    }
}
