using System.Collections.Generic;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Comparer implementation, to check two Favorite items by their Ids
    /// </summary>
    internal class ByIdComparer<TItem> : EqualityComparer<TItem>
        where TItem : class, IStoreIdEquals<TItem>
    {
        public override bool Equals(TItem source, TItem target)
        {
            if (source == null || target == null)
                return false;

            return source.StoreIdEquals(target);
        }

        public override int GetHashCode(TItem favorite)
        {
            if (favorite == null)
                return 0;

            return favorite.GetStoreIdHash();
        }
    }
}