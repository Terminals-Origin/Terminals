using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terminals.Configuration;

namespace Terminals.Data
{
    /// <summary>
    /// In previous versions Groups and Tags.
    /// Now both features are solved here.
    /// </summary>
    internal class Groups : IEnumerable<Group>
    {
        private DataDispatcher dispatcher;
        private Dictionary<Guid, Group> cache;

        internal Groups(DataDispatcher dispatcher, Group[] groups)
        {
            this.dispatcher = dispatcher;
            this.cache = groups.ToDictionary(group => group.Id);
        }

        /// <summary>
        /// Gets a group by its name searching case sensitive. Returns null, if no group is found.
        /// </summary>
        public Group this[string groupName]
        {
            get 
            {
                return this.cache.Values.Where(group => group.Name.Equals(groupName))
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets a group by its unique identifier. Returns null, if the identifier is unknown.
        /// </summary>
        public Group this[Guid groupId]
        {
            get
            {
                if (this.cache.ContainsKey(groupId))
                    return this.cache[groupId];

                return null;
            }
        }

        public List<Group> GetGroupsContainingFavorite(Guid favoriteId)
        {
            return this.cache.Values.Where(group => group.Favorites
                    .Select(favorite => favorite.Id).Contains(favoriteId))
                .ToList();
        }

        public void Add(Group group)
        {
            this.cache.Add(group.Id, group);
        }

        #region IEnumerable members

        public IEnumerator<Group> GetEnumerator()
        {
            return this.cache.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Obsolete

        public string[] Tags
        {
            get
            {
                return Settings.Tags;
            }
        }

        public void AddTags(List<String> tags)
        {
            Settings.AddTags(tags);
        }

        internal void DeleteTags(List<String> tagsToDelete)
        {
            Settings.DeleteTags(tagsToDelete);
        }

        internal void RebuildTagIndex()
        {
            Settings.RebuildTagIndex();
        }

        #endregion
    }
}
