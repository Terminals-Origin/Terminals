using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data.DB
{
    internal class StoredCredentials : ICredentials
    {
        public event EventHandler CredentialsChanged;

        ICredentialSet ICredentials.this[Guid id]
        {
            get 
            {
                var credentials = GetCredentials();
                return credentials.FirstOrDefault(candidate => candidate.Guid == id);
            }
        }

        ICredentialSet ICredentials.this[string name]
        {
            get
            {
                var credentials = GetCredentials();
                return credentials.FirstOrDefault(candidate => candidate.Name
                   .Equals(name, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public void Add(ICredentialSet toAdd)
        {
            using (var database = Database.CreateInstance())
            {
                var credentialToAdd = toAdd as CredentialSet;
                database.CredentialBase.AddObject(credentialToAdd);
                database.SaveImmediatelyIfRequested();
                database.Detach(toAdd);
            }
        }

        public void Remove(ICredentialSet toRemove)
        {
            using (var database = Database.CreateInstance())
            {
                var credentailToRemove = toRemove as CredentialSet;
                database.Attach(credentailToRemove);
                database.CredentialBase.DeleteObject(credentailToRemove);
                database.SaveImmediatelyIfRequested();
            }
        }

        public void UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
            using (var database = Database.CreateInstance())
            {
                foreach (CredentialBase credentialSet in database.CredentialBase)
                {
                    credentialSet.UpdatePasswordByNewKeyMaterial(newKeyMaterial);
                }
            }
        }

        public void Save()
        {
            // nothing to do
        }

        #region IEnumerable members

        public IEnumerator<ICredentialSet> GetEnumerator()
        {
            return GetCredentials()
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        private IEnumerable<CredentialSet> GetCredentials()
        {
            using (var database = Database.CreateInstance())
            {
                return database.CredentialBase.OfType<CredentialSet>().ToList();
            }
        }
    }
}
