using System;

namespace Terminals.Data.DB
{
    internal partial class CredentialSet : ICredentialSet, IIntegerKeyEnityty
    {
        private Guid guid = Guid.NewGuid();
        Guid ICredentialSet.Id
        {
            get { return this.guid; }
        }

        public Guid Guid
        {
            get { return this.guid; }
            set { this.guid = value; }
        }
    }
}
