namespace Terminals.Data
{
    /// <summary>
    /// Allows compare by store Id method
    /// </summary>
    public interface IStoreIdEquals<in TItemType>
    {
        /// <summary>
        /// Because we don't want to replace Equals method (not possible in entity framework),
        /// and Guid isn't used in all store types, we have to use intern unique identifier
        /// to compare the favorites by different implementations by the store.
        /// Doesn't have to compare by "Id" property.
        /// </summary>
        /// <param name="oponent">Not null object of the same implementation like this item to compare with.</param>
        /// <returns>True, if the unique identifier used in store equals; otherwise false</returns>
        bool StoreIdEquals(TItemType oponent);

        int GetStoreIdHash();
    }
}