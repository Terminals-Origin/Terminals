using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using Terminals.Connections;

namespace Terminals.Data.DB
{
    internal partial class Database : DbContext
    {
        /// <summary>
        /// Gets this instance connector for cached entities
        /// </summary>
        public CacheConnector Cache { get; private set; }

        internal Database(DbConnection connection)
            : base(connection, true)
        {
            // disable change tracking, we use the context disconnected
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;

            this.BeforeConnectExecute = this.Set<DbBeforeConnectExecute>();
            this.CredentialBase = this.Set<DbCredentialBase>();
            this.DisplayOptions = this.Set<DbDisplayOptions>();
            this.Favorites = this.Set<DbFavorite>();
            this.Groups = this.Set<DbGroup>();
            this.Security = this.Set<DbSecurityOptions>();
            
            this.Cache = new CacheConnector(this);
        }

        internal void SaveImmediatelyIfRequested()
        {
            // don't ask, save immediately. Here is no benefit to save in batch like in FilePersistence
            this.SaveChanges();
        }

        public override int SaveChanges()
        {
            IEnumerable<DbFavorite> changedFavorites = this.GetChangedOrAddedFavorites();
            // add to database first, otherwise the favorite properties cant be committed.
            int returnValue = base.SaveChanges();
            this.SaveFavoriteDetails(changedFavorites);
            return returnValue;
        }

        private void SaveFavoriteDetails(IEnumerable<DbFavorite> changedFavorites)
        {
            foreach (DbFavorite favorite in changedFavorites)
            {
                favorite.SaveDetails(this);
            }
        }

        private IEnumerable<DbFavorite> GetChangedOrAddedFavorites()
        {
            return this.ChangeTracker.Entries<DbFavorite>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(change => change.Entity)
                .ToList();
        }

        internal byte[] GetFavoriteIcon(int favoriteId)
        {
            byte[] obtained = this.GetFavoriteIcon((int?)favoriteId).FirstOrDefault();
            if (obtained != null)
                return obtained;

            return FavoriteIcons.EmptyImageData;
        }

        internal string GetProtocolPropertiesByFavorite(int favoriteId)
        {
            return GetFavoriteProtocolProperties(favoriteId).FirstOrDefault();
        }

        internal string GetMasterPasswordHash()
        {
            string obtained = this.GetMasterPasswordKey().FirstOrDefault();
            if (obtained != null)
                return obtained;

            return String.Empty;
        }

        internal void UpdateMasterPassword(string newMasterPasswordKey)
        {
            this.UpdateMasterPasswordKey(newMasterPasswordKey);
        }

        internal List<int> GetRdpFavoriteIds()
        {
            return this.Favorites.Where(candidate => candidate.Protocol == ConnectionManager.RDP)
                       .Select(rdpFavorite => rdpFavorite.Id).ToList();
        }
    }
}