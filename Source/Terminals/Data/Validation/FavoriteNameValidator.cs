namespace Terminals.Data.Validation
{
    /// <summary>
    /// Check consistency of favorite name against persistence and its rules.
    /// </summary>
    internal class FavoriteNameValidator : NameValidator<IFavorite>
    {
        internal const string NOT_UNIQUE = "Favorite with the same name already exists";
        private readonly IPersistence persistence;

        protected override string NotUniqueItemMessage { get { return NOT_UNIQUE; } }

        public FavoriteNameValidator(IPersistence persistence)
            : base(persistence)
        {
            this.persistence = persistence;
        }

        protected override IFavorite GetStoreItem(string name)
        {
            return this.persistence.Favorites[name];
        }

        protected override IFavorite CreateNewItem(string newName)
        {
            var newIem = this.persistence.Factory.CreateFavorite();
            newIem.Name = newName;
            return newIem;
        }
    }
}
