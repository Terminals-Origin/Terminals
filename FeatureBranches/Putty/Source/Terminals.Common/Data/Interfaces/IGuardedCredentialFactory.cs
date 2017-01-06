namespace Terminals.Data
{
    public interface IGuardedCredentialFactory
    {
        IGuardedCredential CreateCredential(ICredentialBase credential);

        IGuardedSecurity CreateSecurityOptoins(ISecurityOptions securityOptions);
    }
}