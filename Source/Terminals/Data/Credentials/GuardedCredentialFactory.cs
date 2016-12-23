namespace Terminals.Data.Credentials
{
    internal class GuardedCredentialFactory: IGuardedCredentialFactory
    {
        private readonly IPersistence persistence;

        public GuardedCredentialFactory(IPersistence persistence)
        {
            this.persistence = persistence;
        }

        public IGuardedCredential CreateCredential(ICredentialBase credential)
        {
            return new GuardedCredential(credential, this.persistence.Security);
        }

        public IGuardedSecurity CreateSecurityOptoins(ISecurityOptions securityOptions)
        {
            return new GuardedSecurity(this.persistence, securityOptions);
        }
    }
}