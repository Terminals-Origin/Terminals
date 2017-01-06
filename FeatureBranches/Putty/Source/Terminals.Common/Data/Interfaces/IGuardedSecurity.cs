namespace Terminals.Data
{
    public interface IGuardedSecurity : IGuardedCredential
    {
        /// <summary>
        /// Gets this credentials replaced first by Stored credential and then by default
        /// stored credentials for each value, if the value is empty
        /// </summary>
        IGuardedSecurity GetResolvedCredentials();

        void UpdateFromCredential(ICredentialSet credentials);
    }
}