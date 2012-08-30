using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL database implementation of managing credentails
    /// </summary>
    internal class StoredCredentials : ICredentials
    {
        public event EventHandler CredentialsChanged;

        private readonly EntitiesCache<CredentialSet> cache = new EntitiesCache<CredentialSet>();

        ICredentialSet ICredentials.this[Guid id]
        {
            get
            {
                this.EnsureCache();
                return this.cache.FirstOrDefault(candidate => candidate.Guid == id);
            }
        }

        ICredentialSet ICredentials.this[string name]
        {
            get
            {
                this.EnsureCache();
                return this.cache.FirstOrDefault(candidate => candidate.Name
                   .Equals(name, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public void Add(ICredentialSet toAdd)
        {
            var credentialToAdd = toAdd as CredentialSet;
            AddToDatabase(toAdd, credentialToAdd);
            this.cache.Add(credentialToAdd);
        }

        private static void AddToDatabase(ICredentialSet toAdd, CredentialSet credentialToAdd)
        {
            using (var database = Database.CreateInstance())
            {
                database.CredentialBase.AddObject(credentialToAdd);
                database.SaveImmediatelyIfRequested();
                database.Detach(toAdd);
            }
        }

        public void Remove(ICredentialSet toRemove)
        {
            var credentailToRemove = toRemove as CredentialSet;
            DeleteFromDatabase(credentailToRemove);
            this.cache.Delete(credentailToRemove);
        }

        private static void DeleteFromDatabase(CredentialSet credentailToRemove)
        {
            using (var database = Database.CreateInstance())
            {
                database.Attach(credentailToRemove);
                database.CredentialBase.DeleteObject(credentailToRemove);
                database.SaveImmediatelyIfRequested();
            }
        }

        public void UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
            UpdatePasswordsInDatabase(newKeyMaterial);
            this.cache.Clear();
            this.ReloadCache();
        }

        private static void UpdatePasswordsInDatabase(string newKeyMaterial)
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

        private void EnsureCache()
        {
            if (this.cache.IsEmpty)
            {
                this.ReloadCache();
            }
        }

        private void ReloadCache()
        {
            List<CredentialSet> loaded = LoadFromDatabase();
            this.cache.Add(loaded);
        }

        private static List<CredentialSet> LoadFromDatabase()
        {
            using (var database = Database.CreateInstance())
            {
                return database.CredentialBase.OfType<CredentialSet>().ToList();
            }
        }

        #region IEnumerable members

        public IEnumerator<ICredentialSet> GetEnumerator()
        {
            this.EnsureCache();
            return this.cache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public override string ToString()
        {
            return string.Format("StoredCredentials:Cached={0}", this.cache.Count());
        }
    }
}
