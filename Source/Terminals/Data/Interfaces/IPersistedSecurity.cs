namespace Terminals.Data
{
    /// <summary>
    /// Defines persistence dependent masterpassword security members
    /// </summary>
    internal interface IPersistedSecurity
    {
        /// <summary>
        /// Recalculates stored password hashes to reflect new value of the master password.
        /// </summary>
        /// <param name="newMasterPassword">not encrypted master password</param>
        void UpdatePasswordsByNewMasterPassword(string newMasterPassword);

        /// <summary>
        /// Does initial steps after access to persistence was validated
        /// </summary>
        void Initialize();
    }
}
