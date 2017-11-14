using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Credentials;
using Terminals.Data.Credentials;
using Terminals.Common.Data.Interfaces;

namespace Terminals.Data
{
    internal class SecurityService : ISecurityService
    {
        private IPersistence _persistence;

        private Settings _settings;

        public IEnumerable<ICredentialSet> Credentials => _persistence.Credentials;

        public SecurityService(IPersistence persistence, Settings settings)
        {
            _persistence = persistence;
            _settings = settings;
        }

        public IGuardedCredential FromCredentialSet(ICredentialSet credentialSet)
        {
            return new GuardedCredential(credentialSet, _persistence.Security);
        }

        public IGuardedCredential FromSecurityOption(ISecurityOptions securityOptions)
        {
            return new GuardedSecurity(_persistence, securityOptions);
        }

        public Form GetCredentialForm()
        {
            return new CredentialManager(_persistence);
        }
    }
}
