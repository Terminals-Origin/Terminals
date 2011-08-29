using System;
using System.Collections.Generic;

namespace Terminals.Data
{
    /// <summary>
    /// Tags changed event arguments container, informing about changes in Tags collection
    /// </summary>
    internal class TagsChangedArgs : EventArgs
    {
        /// <summary>
        /// All tags actualy no longer used by any favorite
        /// </summary>
        internal List<String> Added { get; private set; }

        /// <summary>
        /// Newly added tags, currently used atleast by one connection
        /// </summary>
        internal List<String> Removed { get; private set; }

        /// <summary>
        /// Gets the value idicating if there are any added or reomoved items to report.
        /// </summary>
        internal Boolean IsEmpty
        {
            get { return this.Added.Count == 0 && this.Removed.Count == 0; }
        }

        internal TagsChangedArgs()
        {
            this.Added = new List<String>();
            this.Removed = new List<String>();
        }

        internal TagsChangedArgs(List<String> addedTags, List<String> deletedTags)
        {
            // merge collections to report only difference
            MergeChangeLists(addedTags, deletedTags);
            this.Added = addedTags;
            this.Removed = deletedTags;
        }

        private static void MergeChangeLists(List<String> addedTags, List<String> deletedTags)
        {
            int index = 0;
            while (index < deletedTags.Count)
            {
                String deletedTag = deletedTags[index];
                if (addedTags.Contains(deletedTag))
                {   
                    addedTags.Remove(deletedTag);
                    deletedTags.Remove(deletedTag);
                }
                else
                    index++;
            }
        }

        public override String ToString()
        {
            String added = String.Join(";", this.Added.ToArray());
            String removed = String.Join(";", this.Removed.ToArray());
            return String.Format("TagsChangedArgs:Added={0}{{{1}}};Removed={2}{{{3}}}",
                this.Added.Count, added, this.Removed.Count, removed);
        }
    }
}
