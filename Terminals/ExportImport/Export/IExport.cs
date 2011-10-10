using System.Collections.Generic;

namespace Terminals.Integration.Export
{
    /// <summary>
    /// Contract for exporter providers, which save terminals favorites into selected file
    /// </summary>
    internal interface IExport : IIntegration
    {
        /// <summary>
        /// Exports selected favorites into the specified file.
        /// </summary>
        /// <param name="fileName">Full path and name of the destination file.</param>
        /// <param name="favorites">Not null collection of favorites to export.</param>
        /// <param name="includePassword">if set to <c>true</c>
        /// includes paswords in not encrypted form into the destination file.</param>
        void Export(string fileName, List<FavoriteConfigurationElement> favorites, bool includePassword);
    }
}
