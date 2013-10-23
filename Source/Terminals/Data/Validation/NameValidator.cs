namespace Terminals.Data.Validation
{
    /// <summary>
    /// Template for validation of name property for new or existing store items.
    /// </summary>
    /// <typeparam name="TItem">Type of an item to validate</typeparam>
    internal abstract class NameValidator<TItem>
        where TItem : class, IStoreIdEquals<TItem>, INamedItem 
    {
        protected abstract string NotUniqueItemMessage { get; }

        /// <summary>
        /// Validates already present item before new name is assigned.
        /// Validates both newName rules and uniquenes in persistence.
        /// Use when trying to rename item. 
        /// </summary>
        internal string ValidateCurrent(TItem current, string newName)
        {
            if (this.NotUniqueInPersistence(current, newName))
                return this.NotUniqueItemMessage;

            return this.ValidateNameValue(newName);
        }

        /// <summary>
        /// Check only, if the newName is unique in current persistence.
        /// </summary>
        internal bool NotUniqueInPersistence(TItem current, string newName)
        {
            TItem concurrent = this.GetStoreItem(newName);
            return concurrent != null && !current.StoreIdEquals(concurrent);
        }

        /// <summary>
        /// Returns error mesages obtained from validator
        /// or empty string, if item name is valid in current persistence.
        /// Use to check validity of newly created items.
        /// </summary>
        internal string ValidateNew(string newName)
        {
            if (this.GetStoreItem(newName) != null)
                return this.NotUniqueItemMessage;

            return this.ValidateNameValue(newName);
        }

        protected abstract TItem GetStoreItem(string name);

        /// <summary>
        /// Checks, if the new is acceptable agains persitence rules only.
        /// Doesnt check the uniqueness in persistence.
        /// </summary>
        internal string ValidateNameValue(string newName)
        {
            TItem item = this.CreateNewItem(newName);
            var results = Validations.ValidateNameProperty(item);
            return results[Validations.NAME_PROPERTY];
        }

        protected abstract TItem CreateNewItem(string newName);
    }
}