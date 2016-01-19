using System;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Forms
{
    internal class FavoriteSorting
    {
        private readonly SortProperties defaultSortProperty;

        internal FavoriteSorting()
        {
            this.defaultSortProperty = Settings.Instance.DefaultSortProperty;
        }

        internal string GetDefaultSortPropertyName()
        {
            switch (this.defaultSortProperty)
            {
                case SortProperties.ServerName:
                    return "ServerName";
                case SortProperties.Protocol:
                    return "Protocol";
                case SortProperties.ConnectionName:
                    return "Name";
                default:
                    return String.Empty;
            }
        }

        /// <summary>
        /// Returns text compare to method values selecting property to compare
        /// depending on Settings default sort property value
        /// </summary>
        /// <param name="target">not null favorite to compare with</param>
        /// <returns>result of String CompareTo method</returns>
        internal int CompareByDefaultSorting(IFavorite source, IFavorite target)
        {
            switch (this.defaultSortProperty)
            {
                case SortProperties.ServerName:
                    return source.ServerName.CompareTo(target.ServerName);
                case SortProperties.Protocol:
                    return source.Protocol.CompareTo(target.Protocol);
                case SortProperties.ConnectionName:
                    return source.Name.CompareTo(target.Name);
                default:
                    return -1;
            }
        }
    }
}