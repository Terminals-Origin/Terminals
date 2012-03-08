using System.Collections.Generic;

namespace Terminals.Integration.Export
{
    /// <summary>
    /// Export parameters container
    /// </summary>
    internal class ExportOptions
    {
        internal string ProviderFilter { get; set; }

        /// <summary>
        /// Full path and name of the destination file including extension.
        /// </summary>
        internal string FileName { get; set; }

        /// <summary>
        /// Not null collection of favorites to export.
        /// </summary>
        internal List<FavoriteConfigurationElement> Favorites { get; set; }

        /// <summary>
        /// if set to <c>true</c> includes paswords in not encrypted form into the destination file.
        /// </summary>
        internal bool IncludePasswords { get; set; }
    }
}
