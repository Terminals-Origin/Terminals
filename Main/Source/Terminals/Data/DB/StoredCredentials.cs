using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data.DB
{
    internal class StoredCredentials : ICredentials
    {
        private readonly DataBase dataBase;
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

        internal StoredCredentials(DataBase dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Add(ICredentialSet toAdd)
        {
            var credentialToAdd = toAdd as CredentialSet;
            this.dataBase.CredentialBase.AddObject(credentialToAdd);
        }

        public void Remove(ICredentialSet toRemove)
        {
            var credentailToRemove = toRemove as CredentialSet;
            this.dataBase.CredentialBase.DeleteObject(credentailToRemove);
        }

        public void UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
            foreach (CredentialBase credentialSet in this.dataBase.CredentialBase)
            {
                credentialSet.UpdatePasswordByNewKeyMaterial(newKeyMaterial);
            }
        }

        public void Save()
        {
            this.dataBase.SaveImmediatelyIfRequested();
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
            return this.dataBase.CredentialBase
                .OfType<CredentialSet>()
                .ToList();
        }
    }
}
