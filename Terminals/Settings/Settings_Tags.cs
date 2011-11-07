using System;
using System.Collections.Generic;
using SysConfig = System.Configuration;
using System.Linq;
using Terminals.Data;

namespace Terminals.Configuration
{
    internal static partial class Settings
    {
        /// <summary>
        /// Gets alphabeticaly sorted array of tags resolved from Tags store
        /// </summary>
        public static string[] Tags
        {
            get
            {
                List<string> tags = GetSection().Tags.ToList();
                tags.Sort();
                return tags.ToArray();
            }
        }

        public static void AddTags(List<String> tags)
        {
            List<String> addedTags = AddTagsToSettings(tags);
            DataDispatcher.Instance.ReportTagsAdded(addedTags);
        }

        private static List<string> AddTagsToSettings(List<String> tags)
        {
            List<String> addedTags = new List<string>();
            foreach (String tag in tags)
            {
                if (String.IsNullOrEmpty(tag))
                    continue;

                String addedTag = AddTagToSettings(tag);
                if (!String.IsNullOrEmpty(addedTag))
                    addedTags.Add(addedTag);
            }
            return addedTags;
        }

        /// <summary>
        /// Adds tag to the tags collection if it already isnt there.
        /// If the tag is in database, than it returns empty string, otherwise the commited tag.
        /// </summary>
        private static String AddTagToSettings(String tag)
        {
            if (AutoCaseTags)
                tag = ToTitleCase(tag);
            if (Tags.Contains(tag))
                return String.Empty;

            GetSection().Tags.AddByName(tag);
            SaveImmediatelyIfRequested();
            return tag;
        }

        public static void DeleteTags(List<String> tagsToDelete)
        {
            List<String> deletedTags = DeleteTagsFromSettings(tagsToDelete);
            DataDispatcher.Instance.ReportTagsDeleted(deletedTags);
        }

        private static List<String> DeleteTagsFromSettings(List<String> tagsToDelete)
        {
            List<String> deletedTags = new List<String>();
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
        private static String DeleteTagFromSettings(String tagToDelete)
        {
            if (AutoCaseTags)
                tagToDelete = ToTitleCase(tagToDelete);
            GetSection().Tags.DeleteByName(tagToDelete);
            SaveImmediatelyIfRequested();
            return tagToDelete;
        }

        private static Int32 GetNumberOfFavoritesUsingTag(String tagToRemove)
        {
            var favorites = GetFavorites().ToList();
            return favorites.Where(favorite => favorite.TagList.Contains(tagToRemove))
                .Count();
        }

        public static void RebuildTagIndex()
        {
            String[] oldTags = Tags;
            ClearTags();
            ReCreateAllTags();
            DataDispatcher.Instance.ReportTagsRecreated(Tags.ToList(), oldTags.ToList());
        }

        private static void ReCreateAllTags()
        {
            FavoriteConfigurationElementCollection favorites = GetFavorites();
            foreach (FavoriteConfigurationElement favorite in favorites)
            {   // dont report update here
                AddTagsToSettings(favorite.TagList);
            }
        }

        /// <summary>
        /// Clears all tags from database, but doesnt sed the tags changed event
        /// </summary>
        private static void ClearTags()
        {
            foreach (String tag in Tags)
            {
                DeleteTagFromSettings(tag);
            }
        }
    }
}
