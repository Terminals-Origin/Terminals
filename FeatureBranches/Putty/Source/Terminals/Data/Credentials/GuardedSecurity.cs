using System;
using Terminals.Configuration;

namespace Terminals.Data.Credentials
{
    internal class GuardedSecurity : GuardedCredential, IGuardedSecurity
    {
        private readonly ICredentials credentials;

        private readonly ISecurityOptions securityOptions;

        private readonly IPersistence persistence;

        internal GuardedSecurity(IPersistence persistence, ISecurityOptions securityOptions)
            :base(securityOptions, persistence.Security)
        {
            this.persistence = persistence;
            this.credentials = persistence.Credentials;
            this.securityOptions = securityOptions;
        }

        public IGuardedSecurity GetResolvedCredentials()
        {
            var resolved = new SecurityOptions();
            IGuardedSecurity result = new GuardedSecurity(this.persistence, resolved);
            this.ResolveCredentials(resolved, this.securityOptions.Credential);
            return result;
        }

        public void ResolveCredentials(ISecurityOptions result, Guid credentialId)
        {
            ICredentialSet source = this.credentials[credentialId];
            this.UpdateFromCredential(source, result);
            this.UpdateFromDefaultValues(result);
        }

        private void UpdateFromDefaultValues(ICredentialBase target)
        {
            Settings settings = Settings.Instance;
            var guarded = new GuardedCredential(target, this.PersistenceSecurity);

            if (string.IsNullOrEmpty(guarded.Domain))
                guarded.Domain = settings.DefaultDomain;
            
            if (string.IsNullOrEmpty(guarded.UserName))
                guarded.UserName = settings.DefaultUsername;

            if (string.IsNullOrEmpty(guarded.Password))
                guarded.Password = settings.DefaultPassword;
        }

        public void UpdateFromCredential(ICredentialSet credentials)
        {
            this.UpdateFromCredential(credentials, this.securityOptions);
        }

        private void UpdateFromCredential(ICredentialSet source, ISecurityOptions target)
        {
            if (source != null)
            {
                target.Credential = source.Id;
                var guardedSource = new GuardedCredential(source, this.PersistenceSecurity);
                var guardedTarget = new GuardedCredential(target, this.PersistenceSecurity);
                guardedTarget.Domain = guardedSource.Domain;
                guardedTarget.UserName = guardedSource.UserName;
                target.EncryptedPassword = source.EncryptedPassword;
            }
        }
    }
}