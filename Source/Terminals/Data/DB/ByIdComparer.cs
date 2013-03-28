using System.Collections.Generic;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Comparer implementation, to check two Favorite items by their Ids
    /// </summary>
    internal class ByIdComparer : EqualityComparer<DbFavorite>
    {
        public override bool Equals(DbFavorite source, DbFavorite target)
        {
            if (source == null || target == null)
                return false;

            return source.Id == target.Id;
        }

        public override int GetHashCode(DbFavorite favorite)
        {
            if (favorite == null)
                return 0;

            return favorite.Id;
        }
    }
}