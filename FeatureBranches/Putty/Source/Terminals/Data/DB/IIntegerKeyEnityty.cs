namespace Terminals.Data.DB
{
    /// <summary>
    /// Contract for entities, which support integer unique identifier
    /// </summary>
    internal interface IIntegerKeyEnityty
    {
        /// <summary>
        /// Gets unique identifier of an entity to distinguish entities in cache
        /// </summary>
        int Id { get; }
    }
}