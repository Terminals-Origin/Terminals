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
        /// Newly added IGroups, currently used atleast by one connection
        /// </summary>
        internal List<IGroup> Added { get; private set; }

        /// <summary>
        /// All IGroups actualy no longer used by any favorite
        /// </summary>
        internal List<IGroup> Removed { get; private set; }

        /// <summary>
        /// Gets the value idicating if there are any added or reomoved items to report.
        /// </summary>
        internal Boolean IsEmpty
        {
            get { return this.Added.Count == 0 && this.Removed.Count == 0; }
        }

        internal GroupsChangedArgs()
        {
            this.Added = new List<IGroup>();
            this.Removed = new List<IGroup>();
        }

        internal GroupsChangedArgs(List<IGroup> addedIGroups, List<IGroup> deletedIGroups)
        {
            // merge collections to report only differences
            MergeChangeLists(addedIGroups, deletedIGroups);
            this.Added = addedIGroups;
            this.Removed = deletedIGroups;
        }

        private static void MergeChangeLists(List<IGroup> addedIGroups, List<IGroup> deletedIGroups)
        {
            int index = 0;
            while (index < deletedIGroups.Count)
            {
                IGroup deletedIGroup = deletedIGroups[index];
                if (addedIGroups.Contains(deletedIGroup))
                {   
                    addedIGroups.Remove(deletedIGroup);
                    deletedIGroups.Remove(deletedIGroup);
                }
                else
                    index++;
            }
        }

        public override String ToString()
        {
            return String.Format("TagsChangedArgs:Added={0};Removed={1}",
                this.Added.Count, this.Removed.Count);
        }
    }
}
