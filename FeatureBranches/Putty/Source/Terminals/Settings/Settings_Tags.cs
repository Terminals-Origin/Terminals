using System;
using System.Collections.Generic;
using SysConfig = System.Configuration;
using System.Linq;
using Terminals.Converters;

namespace Terminals.Configuration
{
    internal partial class Settings
    {
        /// <summary>
        /// Gets alphabeticaly sorted array of tags resolved from Tags store.
        /// Since version 2. only for updates. Use new persistence instead.
        /// </summary>
        internal string[] Tags
        {
            get
            {
                List<string> tags = GetSection().Tags.ToList();
                tags.Sort();
                return tags.ToArray();
            }
        }

        private void AddTagsToSettings(List<string> tags)
        {
            foreach (String tag in tags)
            {
                if (String.IsNullOrEmpty(tag))
                    continue;

                AddTagToSettings(tag);
            }
        }

        /// <summary>
        /// Adds tag to the tags collection if it already isnt there.
        /// If the tag is in database, than it returns empty string, otherwise the commited tag.
        /// </summary>
        private void AddTagToSettings(String tag)
        {
            if (AutoCaseTags)
                tag = TextConverter.ToTitleCase(tag);
            if (Tags.Contains(tag))
                return;

            GetSection().Tags.AddByName(tag);
            SaveImmediatelyIfRequested();
        }

        private void DeleteTags()
        {
            List<string> tagsToDelete = Tags.ToList();
            DeleteTagsFromSettings(tagsToDelete);
        }

        private List<string> DeleteTagsFromSettings(List<string> tagsToDelete)
        {
            List<string> deletedTags = new List<string>();
            foreach (String tagTodelete in tagsToDelete)
            {
                if (GetNumberOfFavoritesUsingTag(tagTodelete) > 0)
                    continue;

                deletedTags.Add(DeleteTagFromSettings(tagTodelete));
            }
            return deletedTags;
        }

        /// <summary>
        /// Removes the tag from settings, but doesnt send the Tag removed event
        /// </summary>
        private String DeleteTagFromSettings(String tagToDelete)
        {
            if (AutoCaseTags)
                tagToDelete = TextConverter.ToTitleCase(tagToDelete);
            GetSection().Tags.DeleteByName(tagToDelete);
            SaveImmediatelyIfRequested();
            return tagToDelete;
        }

        private Int32 GetNumberOfFavoritesUsingTag(String tagToRemove)
        {
            var favorites = GetFavorites().ToList();
            return favorites.Count(favorite => this.tagsConverter.ResolveTagsList(favorite).Contains(tagToRemove));
        }
    }
}
