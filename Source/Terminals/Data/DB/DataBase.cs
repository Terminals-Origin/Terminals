using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using Terminals.Configuration;

namespace Terminals.Data.DB
{
    internal partial class DataBase
    {
        private const string PROVIDER = "System.Data.SqlClient";       
        private const string METADATA = @"res://*/Data.DB.SQLPersistance.csdl|res://*/Data.DB.SQLPersistance.ssdl|res://*/Data.DB.SQLPersistance.msl";
        internal const string DEVELOPMENT_CONNECTION_STRING = @"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Data\Terminals.mdf;Integrated Security=True;User Instance=False";
        
        private bool delaySave = false;
        // TODO Add eventing, if something is changed in database (Jiri Pokorny, 13.02.2012)
        // TODO Add new table for database extra options, which couldnt be stored localy like masterpassword (Jiri Pokorny, 26.02.2012)

        internal static DataBase CreateDatabaseInstance()
        {
            string connectionString = BuildConnectionString();
            return new DataBase(connectionString);
        }

        private static string BuildConnectionString()
        {
            var connectionBuilder = new EntityConnectionStringBuilder
            {
                Provider = PROVIDER,
                Metadata = METADATA,
                ProviderConnectionString = Settings.ConnectionString
            };

            return connectionBuilder.ToString();
        }

        internal void StartDelayedUpdate()
        {
            delaySave = true;
        }

        internal void SaveAndFinishDelayedUpdate()
        {
            delaySave = false;
            SaveImmediatelyIfRequested();
        }

        internal void SaveImmediatelyIfRequested()
        {
            if (!delaySave)
            {
                // todo Add protection against concurent updates
                this.SaveChanges();
            }
        }

        internal Favorite GetFavoriteByGuid(Guid favoriteId)
        {
            // to list, because linq to entities doesnt support Guid search
            return this.Favorites.ToList()
                .FirstOrDefault(favorite => favorite.Guid == favoriteId);
        }

        public override int SaveChanges(SaveOptions options)
        {
            this.DetachUntaggedGroups();
            var changedFavorites = this.GetChangedOrAddedFavorites();
            int returnValue = base.SaveChanges(options);
            this.SaveFavoriteProtocolProperties(changedFavorites);
            return returnValue;
        }

        /// <summary>
        /// Preserve saving Untagged group which is virtual only
        /// </summary>
        private void DetachUntaggedGroups()
        {
            IEnumerable<ObjectStateEntry> added = this.ObjectStateManager.GetObjectStateEntries(EntityState.Added);
            foreach (var change in added)
            {
                var group = change.Entity as Group;
                if (group != null && group.Name == Settings.UNTAGGED_NODENAME)
                {
                    this.Detach(group);
                }
            }
        }

        private void SaveFavoriteProtocolProperties(IEnumerable<Favorite> changedFavorites)
        {
            foreach (var favorite in changedFavorites)
            {
                favorite.SaveProperties(this);
            }
        }

        private IEnumerable<Favorite> GetChangedOrAddedFavorites()
        {
            var changes = this.GetChangedOrAddedEntitites();

            IEnumerable<Favorite> affectedFavorites = changes.Where(candidate => candidate.Entity is Favorite)
                .Select(change => change.Entity)
                .Cast<Favorite>();

            return affectedFavorites;
        }

        private List<ObjectStateEntry> GetChangedOrAddedEntitites()
        {
            IEnumerable<ObjectStateEntry> added = this.ObjectStateManager.GetObjectStateEntries(EntityState.Added);
            List<ObjectStateEntry> changed = this.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).ToList();
            changed.AddRange(added);
            return changed;
        }

        partial void OnContextCreated()
        {
            this.ObjectMaterialized += new ObjectMaterializedEventHandler(this.OnDataBaseObjectMaterialized);  
        }

        private void OnDataBaseObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        {
            var entity = e.Entity as IEntityContext;
            if(entity != null)
               entity.Database = this;   
        }
        
        internal string GetMasterPassword()
        {
            return this.GetMasterPasswordKey().FirstOrDefault();
        }

        internal void UpdateMasterPassword(string newMasterPasswordKey)
        {
            this.UpdateMasterPasswordKey(newMasterPasswordKey);
        }
    }
}
