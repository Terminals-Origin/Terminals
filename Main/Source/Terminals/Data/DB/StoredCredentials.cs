﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL database implementation of managing credentials
    /// </summary>
    internal class StoredCredentials : ICredentials
    {
        private readonly SqlPersistenceSecurity persistenceSecurity;

        public event EventHandler CredentialsChanged;

        private readonly EntitiesCache<CredentialSet> cache = new EntitiesCache<CredentialSet>();

        private bool isLoaded;

        public StoredCredentials(SqlPersistenceSecurity persistenceSecurity)
        {
            this.persistenceSecurity = persistenceSecurity;
        }

        ICredentialSet ICredentials.this[Guid id]
        {
            get
            {
                return this[id];
            }
        }

        internal CredentialSet this[Guid id]
        {
            get
            {
                this.EnsureCache();
                return this.cache.FirstOrDefault(candidate => candidate.Guid == id);
            }
        }

        internal CredentialSet this[int storeId]
        {
            get
            {
                this.EnsureCache();
                return this.cache.FirstOrDefault(candidate => candidate.Id == storeId);
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

        public void Update(ICredentialSet toUpdate)
        {
            using (var database = Database.CreateInstance())
            {
                var credentialToUpdate = toUpdate as CredentialSet;
                if (credentialToUpdate == null)
                    return;
                database.Attach(credentialToUpdate);
                database.MarkAsModified(credentialToUpdate);
                database.SaveImmediatelyIfRequested();
                database.Detach(credentialToUpdate);
                this.cache.Update(credentialToUpdate);
            }
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
            this.RefreshCache();
        }

        public void Save()
        {
            // todo check, if the Updated credentials are committed to the database properly
            // nothing to do
        }

        private void EnsureCache()
        {
            if (this.isLoaded)
                return;

            this.ReloadCache();
            this.isLoaded = true;
        }

        internal void RefreshCache()
        {
            this.cache.Clear();
            this.ReloadCache();
            if (CredentialsChanged != null)
                CredentialsChanged(this, new EventArgs());
        }

        private void ReloadCache()
        {
            List<CredentialSet> loaded = LoadFromDatabase();
            this.AssignSecurity(loaded);
            this.cache.Add(loaded);
        }

        private void AssignSecurity(List<CredentialSet> loaded)
        {
            foreach (CredentialSet credentialSet in loaded)
            {
                credentialSet.AssignSecurity(this.persistenceSecurity);
            }
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