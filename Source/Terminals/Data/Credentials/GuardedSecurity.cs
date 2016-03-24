using System;
using Terminals.Configuration;

namespace Terminals.Data.Credentials
{
    internal class GuardedSecurity : GuardedCredential, IGuardedSecurity
    {
        private readonly ISecurityOptions securityOptions;

        internal GuardedSecurity(PersistenceSecurity persistenceSecurity, ISecurityOptions securityOptions)
            :base(securityOptions, persistenceSecurity)
        {
            this.securityOptions = securityOptions;
        }

        public IGuardedSecurity GetResolvedCredentials()
        {
            var resolved = new SecurityOptions();
            IGuardedSecurity result = new GuardedSecurity(this.PersistenceSecurity, resolved);
            this.ResolveCredentials(resolved, this.securityOptions.Credential);
            return result;
        }

        public void ResolveCredentials(ISecurityOptions result, Guid credentialId)
        {
            ICredentialSet source = Persistence.Instance.Credentials[credentialId];
            UpdateFromCredential(source, result, this.PersistenceSecurity);
            UpdateFromDefaultValues(result);
        }

        private static void UpdateFromDefaultValues(ICredentialBase target)
        {
            Settings settings = Settings.Instance;

            if (string.IsNullOrEmpty(target.Domain))
                target.Domain = settings.DefaultDomain;

            var guarded = new GuardedCredential(target, Persistence.Instance.Security);
            if (string.IsNullOrEmpty(guarded.UserName))
                guarded.UserName = settings.DefaultUsername;

            if (string.IsNullOrEmpty(target.Password))
                target.Password = settings.DefaultPassword;
        }
    }
}