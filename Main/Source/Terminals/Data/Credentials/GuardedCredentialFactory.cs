namespace Terminals.Data.Credentials
{
    internal class GuardedCredentialFactory: IGuardedCredentialFactory
    {
        private readonly PersistenceSecurity persistenceSecurity;

        public GuardedCredentialFactory(PersistenceSecurity persistenceSecurity)
        {
            this.persistenceSecurity = persistenceSecurity;
        }

        public IGuardedCredential CreateCredential(ICredentialBase credential)
        {
            return new GuardedCredential(credential, this.persistenceSecurity);
        }

        public IGuardedSecurity CreateSecurityOptoins(ISecurityOptions securityOptions)
        {
            return new GuardedSecurity(this.persistenceSecurity, securityOptions);
        }
    }
}