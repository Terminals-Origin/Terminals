namespace Terminals.Data
{
    /// <summary>
    /// Defines persistence dependent masterpassword security members
    /// </summary>
    internal interface IPersistedSecurity
    {
        /// <summary>
        /// Gets or sets the stored master password hash.
        /// </summary>
        string MasterPasswordHash { get; set; }

        /// <summary>
        /// Recalculates stored password hashes to reflect new value of the master password.
        /// </summary>
        /// <param name="newMasterPassword">not encrypted master password</param>
        void UpdatePasswordsByNewMasterPassword(string newMasterPassword);
    }
}
