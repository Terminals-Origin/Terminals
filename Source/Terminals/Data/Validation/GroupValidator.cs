namespace Terminals.Data.Validation
{
    /// <summary>
    /// Check consistency of group against persistence and its rules.
    /// </summary>
    internal class GroupValidator
    {
        internal const string NOT_UNIQUE = "Group with the same name already exists";
        private readonly IPersistence persistence;

        internal GroupValidator(IPersistence persistence)
        {
            this.persistence = persistence;
        }

        /// <summary>
        /// Validates already present group before new name is assigned.
        /// Use when trying to rename group.
        /// </summary>
        internal string ValidateCurrent(IGroup current, string newGroupName)
        {
            IGroup concurrent = this.persistence.Groups[newGroupName];
            if (concurrent != null && !current.StoreIdEquals(concurrent))
                return NOT_UNIQUE;

            return this.ValidateNameValue(newGroupName);
        }

        /// <summary>
        /// Returns error mesages obtained from validator
        /// or empty string, if group name is valid in current persistence.
        /// Use to check validity of newly created groups.
        /// </summary>
        internal string ValidateNew(string newGroupName)
        {
            if (this.persistence.Groups[newGroupName] != null)
                return NOT_UNIQUE;

            return this.ValidateNameValue(newGroupName);
        }

        private string ValidateNameValue(string groupName)
        {
            IGroup group = this.persistence.Factory.CreateGroup(groupName);
            var results = Validations.ValidateGroupName(group);
            return results[Validations.GROUP_NAME];
        }
    }
}