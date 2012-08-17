namespace Terminals.Data
{
    /// <summary>
    /// Allows compare by store Id method
    /// </summary>
    internal interface IStoreIdEquals<in TItemType>
    {
        /// <summary>
        /// Because we dont want to replace Equals method (not possible in entity framework),
        /// and Guid isnt used in all store types, we have to use intern unique identifier
        /// to comapre the favorites by different implementations by the store.
        /// Doesnt have to compare by "Id" property.
        /// </summary>
        /// <param name="oponent">Not null object of the same implementation like this item to compare with.</param>
        /// <returns>True, if the unique identifier used in store quals; otherwise false</returns>
        bool StoreIdEquals(TItemType oponent);
    }
}