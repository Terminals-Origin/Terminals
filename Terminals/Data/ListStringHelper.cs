using System;
using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data
{
    /// <summary>
    /// Helper class for working with lists of strings
    /// </summary>
    internal static class ListStringHelper
    {
        /// <summary>
        /// Finds all items from source list, which are not in destination list.
        /// Returns not null collection of differences. Compares ignoring case.
        /// </summary>
        internal static List<String> GetMissingSourcesInTarget(List<String> sourceItems, List<String> targetItems)
        {
            return sourceItems.Except(targetItems, StringComparer.CurrentCultureIgnoreCase).ToList();
        }
    }
}
