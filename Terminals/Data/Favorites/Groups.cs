using System;
using System.Collections.Generic;
using Terminals.Configuration;

namespace Terminals.Data
{
    /// <summary>
    /// In previous versions Groups and Tags.
    /// Now both features are solved here.
    /// </summary>
    internal class Groups
    {
        private DataDispatcher dispatcher;
        internal List<Group> Cache { get; private set; }

        internal Groups(DataDispatcher dispatcher, List<Group> groups)
        {
            this.dispatcher = dispatcher;
            this.Cache = groups;
        }

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
    }
}
