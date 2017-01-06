using System.Linq;

namespace Terminals.Data.DB
{
    /// <summary>
    /// Checks, whether favorites have identical Ids, but version has changed
    /// </summary>
    internal class ChangedVersionComparer : ByIdComparer<DbFavorite>
    {
        public override bool Equals(DbFavorite source, DbFavorite target)
        {
            var baseIs = base.Equals(source, target);
            return baseIs && IsChanged(source, target);
        }

        private static bool IsChanged(DbFavorite source, DbFavorite target)
        {
            // both inputs already checked by base Equals.
            // expecting source as older version, If source version is null, it is the first version
            if (source.Version == null || target.Version == null)
            {
                return true;
            }

            return !source.Version.SequenceEqual(target.Version);
        }
    }
}