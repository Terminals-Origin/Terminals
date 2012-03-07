namespace Terminals.Integration
{
    internal interface IIntegration
    {
        /// <summary>
        /// Gets the name of the import/export provider
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the file name to import/export, including the dot prefix.
        /// </summary>
        string KnownExtension { get; }
    }
}
