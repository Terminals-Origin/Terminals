using System;
using System.Collections.Generic;
using System.Linq;
using Terminals.Data.DB;

namespace Terminals.Data
{
    /// <summary>
    /// Helper class for working with lists of items
    /// </summary>
    internal static class ListsHelper
    {
        /// <summary>
        /// Finds all items from source list, which are not in destination list.
        /// Returns not null collection of differences.
        /// </summary>
        internal static List<TType> GetMissingSourcesInTarget<TType>(List<TType> sourceItems, List<TType> targetItems)
            where TType : class, IStoreIdEquals<TType>
        {
            return sourceItems.Except(targetItems, new ByIdComparer<TType>()).ToList();
        }

        /// <summary>
        /// Finds all items from source list, which are not in destination list.
        /// Returns not null collection of differences. Compares by custom comparer.
        /// </summary>
        internal static List<TType> GetMissingSourcesInTarget<TType>(List<TType> sourceItems, List<TType> targetItems,
            IEqualityComparer<TType> comparer)
        {
            return sourceItems.Except(targetItems, comparer).ToList();
        }
    }
}
