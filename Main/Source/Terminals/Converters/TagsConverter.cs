using System;
using System.Collections.Generic;
using Terminals.Configuration;

namespace Terminals.Converters
{
    internal class TagsConverter
    {
        internal List<string> ResolveTagsList(FavoriteConfigurationElement favorite)
        {
            List<string> tagList = new List<string>();
            string tags = this.ResolveTags(favorite);
            string[] splittedTags = tags.Split(',');

            if (!(splittedTags.Length == 1 && string.IsNullOrEmpty(splittedTags[0])))
            {
                foreach (string tag in splittedTags)
                {
                    tagList.Add(tag);
                }
            }

            return tagList;
        }

        internal string ResolveTags(FavoriteConfigurationElement favorite)
        {
            if (Settings.Instance.AutoCaseTags)
                return TextConverter.ToTitleCase(favorite.Tags);

            return favorite.Tags;
        }
    }
}
