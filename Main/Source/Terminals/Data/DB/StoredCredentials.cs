using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL database implementation of managing credentials
    /// </summary>
    internal class StoredCredentials : ICredentials
    {
        private readonly SqlPersistenceSecurity persistenceSecurity;

        private readonly DataDispatcher dispatcher;

        public event EventHandler CredentialsChanged;

        private readonly EntitiesCache<DbCredentialSet> cache = new EntitiesCache<DbCredentialSet>();

        private bool isLoaded;

        public StoredCredentials(SqlPersistenceSecurity persistenceSecurity, DataDispatcher dispatcher)
        {
            this.persistenceSecurity = persistenceSecurity;
            this.dispatcher = dispatcher;
        }

        ICredentialSet ICredentials.this[Guid id]
        {
            get
            {
                return this[id];
            }
        }

        internal DbCredentialSet this[Guid id]
        {
            get
            {
                this.EnsureCache();
                return this.cache.FirstOrDefault(candidate => candidate.Guid == id);
            }
        }

        internal DbCredentialSet this[int storeId]
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
            try // no concurrency here, because there are no dependences on other tables
            {
                this.TryAdd(toAdd);
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(Add, toAdd, this, exception, "Unable to add credential to database");
            }
        }

        private void TryAdd(ICredentialSet toAdd)
        {
            var credentialToAdd = toAdd as DbCredentialSet;
            AddToDatabase(credentialToAdd);
            this.cache.Add(credentialToAdd);
        }

        private static void AddToDatabase(DbCredentialSet credentialToAdd)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                database.CredentialBase.Add(credentialToAdd);
                database.SaveImmediatelyIfRequested();
                database.Cache.Detach(credentialToAdd);
            }
        }

        public void Remove(ICredentialSet toRemove)
        {
            try
            {
                this.TryRemove(toRemove);
            }
            catch (DbUpdateConcurrencyException)
            {
                this.cache.Delete((DbCredentialSet)toRemove);
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(Remove, toRemove, this, exception,
                                                  "Unable to remove credential from database.");
            }
        }

        private void TryRemove(ICredentialSet toRemove)
        {
            var credentailToRemove = toRemove as DbCredentialSet;
            DeleteFromDatabase(credentailToRemove);
            this.cache.Delete(credentailToRemove);
        }

        private static void DeleteFromDatabase(DbCredentialSet credentailToRemove)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                database.CredentialBase.Attach(credentailToRemove);
                database.CredentialBase.Remove(credentailToRemove);
                database.SaveImmediatelyIfRequested();
            }
        }

        public void Update(ICredentialSet toUpdate)
        {
            try
            {
                this.TryUpdate(toUpdate);
            }
            catch (DbUpdateConcurrencyException) // item already removed
            {
                this.cache.Delete((DbCredentialSet)toUpdate);
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(Update, toUpdate, this, exception, "Unable to update credential set.");
            }
        }

        private void TryUpdate(ICredentialSet toUpdate)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                var credentialToUpdate = toUpdate as DbCredentialSet;
                database.CredentialBase.Attach(credentialToUpdate);
                database.Cache.MarkAsModified(credentialToUpdate);
                database.SaveImmediatelyIfRequested();
                database.Cache.Detach(credentialToUpdate);
                this.cache.Update(credentialToUpdate);
            }
        }

        public void UpdatePasswordsByNewKeyMaterial(string newKeyMaterial)
        {
            this.RefreshCache();
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
            List<DbCredentialSet> loaded = LoadFromDatabase();
            this.AssignSecurity(loaded);
            this.cache.Add(loaded);
        }

        private void AssignSecurity(List<DbCredentialSet> loaded)
        {
            foreach (DbCredentialSet credentialSet in loaded)
            {
                credentialSet.AssignSecurity(this.persistenceSecurity);
            }
        }

        private List<DbCredentialSet> LoadFromDatabase()
        {
            try
            {
                return TryLoadFromDatabase();
            }
            catch (EntityException exception)
            {
                return this.dispatcher.ReportFunctionError(LoadFromDatabase, this, exception,
                    "Unable to load credentials from database.");
            }
        }

        private static List<DbCredentialSet> TryLoadFromDatabase()
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                return database.CredentialBase.OfType<DbCredentialSet>().ToList();
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
