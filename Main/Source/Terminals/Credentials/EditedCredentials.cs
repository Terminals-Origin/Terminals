using Terminals.Data;

namespace Terminals.Credentials
{
    internal class EditedCredentials
    {
        public string Name { get { return this.Edited.Name; } }

        public string UserName { get; private set; }

        public string Domain { get; private set; }

        public ICredentialSet Edited { get; private set; }

        public EditedCredentials(ICredentialSet credentialSet, string userName, string domain)
        {
            this.UserName = userName;
            this.Domain = domain;
            this.Edited = credentialSet;
        }
    }
}