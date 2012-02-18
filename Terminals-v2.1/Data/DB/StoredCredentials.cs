using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terminals.Data.DB
{
    internal class StoredCredentials : ICredentials
    {
        private DataBase dataBase;
        public event EventHandler CredentialsChanged;

        ICredentialSet ICredentials.this[Guid id]
        {
            get { throw new NotImplementedException(); }
        }

        ICredentialSet ICredentials.this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        internal StoredCredentials(DataBase dataBase)
        {
            this.dataBase = dataBase;
        }

        public void Add(ICredentialSet toAdd)
        {
            throw new NotImplementedException();
        }

        public void Remove(ICredentialSet toRemove)
        {
            throw new NotImplementedException();
        }

        public void UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        #region IEnumerable members

        public IEnumerator<ICredentialSet> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
