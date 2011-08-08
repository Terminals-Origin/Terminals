using System.Collections.Generic;

namespace Terminals.Integration.Import
{
  /// <summary>
  /// Import interface for all remote desktop providers
  /// </summary>
  internal interface IImport
  {
    /// <summary>
    /// Loads all favorites from the imported file
    /// </summary>
    /// <param name="Filename">Full path and file name of the file to import from</param>
    /// <returns>Null, in incorect cases; otherwise collection with found favorites.</returns>
    List<FavoriteConfigurationElement> ImportFavorites(string Filename);

    /// <summary>
    /// Gets the name of the imported file
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the file name to import, including the dot prefix.
    /// </summary>
    string KnownExtension { get; }
  }
}