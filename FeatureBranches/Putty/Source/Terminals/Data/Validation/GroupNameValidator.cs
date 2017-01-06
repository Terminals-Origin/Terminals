namespace Terminals.Data.Validation
{
    /// <summary>
    /// Check consistency of group name against persistence and its rules.
    /// </summary>
    internal class GroupNameValidator : NameValidator<IGroup>
    {
        internal const string NOT_UNIQUE = "Group with the same name already exists";

        protected override string NotUniqueItemMessage { get { return NOT_UNIQUE; } }

        private readonly IPersistence persistence;

        internal GroupNameValidator(IPersistence persistence) 
            : base(persistence)
        {
            this.persistence = persistence;
        }

        protected override IGroup GetStoreItem(string name)
        {
            return this.persistence.Groups[name];
        }

        protected override IGroup CreateNewItem(string newName)
        {
            return this.persistence.Factory.CreateGroup(newName);
        }
    }
}