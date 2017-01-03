namespace Terminals.Data
{
    /// <summary>
    /// Defines persistence dependent master password security members
    /// </summary>
    internal interface IPersistedSecurity
    {
        /// <summary>
        /// Recalculates stored password hashes to reflect new value of the master password.
        /// </summary>
        /// <param name="newMasterKey">New master password key derived from new master password to assign</param>
        void UpdatePasswordsByNewMasterPassword(string newMasterKey);

        /// <summary>
        /// Does initial steps after access to persistence was validated
        /// </summary>
        /// <returns>True, if initialization was successful, otherwise false.</returns>
        bool Initialize();
    }
}
