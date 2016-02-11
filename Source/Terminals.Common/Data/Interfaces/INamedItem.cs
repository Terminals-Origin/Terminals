namespace Terminals.Data
{
    /// <summary>
    /// Item, which contains 'Name' property.
    /// </summary>
    public interface INamedItem
    {
        /// <summary>
        /// Gets not null name of an item. This is usually validated against persistence to case sensitive unique.
        /// </summary>
        string Name { get; }
    }
}