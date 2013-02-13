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
        internal Database(DbConnection connection)
            : base(connection, true)
        {
            this.BeforeConnectExecute = this.Set<DbBeforeConnectExecute>();
            this.CredentialBase = this.Set<DbCredentialBase>();
            this.DisplayOptions = this.Set<DbDisplayOptions>();
            this.Favorites = this.Set<DbFavorite>();
            this.Groups = this.Set<DbGroup>();
            this.Security = this.Set<DbSecurityOptions>();
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

        /// <summary>
        /// Attaches all groups in toAttach to this context.
        /// Does not check, if the entities are already in the context.
        /// </summary>
        /// <param name="toAttach">Not null collection of items from this model</param>
        internal void AttachAll(IEnumerable<DbGroup> toAttach)
        {
            foreach (DbGroup group in toAttach)
            {
                this.Groups.Attach(group);
            }
        }

        internal void AttachAll(IEnumerable<DbFavorite> favorites)
        {
            foreach (DbFavorite favorite in favorites)
            {
                this.AttachFavorite(favorite);
            }
        }

        internal void AttachFavorite(DbFavorite favorite)
        {
            favorite.AttachDetails(this);
            this.Favorites.Attach(favorite);
        }

        internal void Detach<TEntity>(TEntity entity)
             where TEntity : class
        {
            this.Entry(entity).State = EntityState.Detached;
        }

        /// <summary>
        /// Detaches all item in entitiesToDetach from this context.
        /// Does not check, if the entities are in the context.
        /// </summary>
        /// <typeparam name="TEntity">Entity defined in this object context model</typeparam>
        /// <param name="entitiesToDetach">Not null collection of items from this model
        /// currently attached in this object context</param>
        internal void DetachAll<TEntity>(IEnumerable<TEntity> entitiesToDetach)
            where TEntity : class
        {
            foreach (TEntity entity in entitiesToDetach)
            {
                this.Detach(entity);
            }
        }

        internal void DetachAll(IEnumerable<DbFavorite> favorites)
        {
            foreach (DbFavorite favorite in favorites)
            {
                this.DetachFavorite(favorite);
            }
        }

        internal void DetachFavorite(DbFavorite favorite)
        {
            favorite.DetachDetails(this);
            this.Detach(favorite);
        }

        /// <summary>
        /// Switch toUpdate entity state to Modified. 
        /// </summary>
        internal void MarkAsModified(object toUpdate)
        {
            this.Entry(toUpdate).State = EntityState.Modified;
        }

        internal void MarkAsModified(List<DbFavorite> toUpdate)
        {
            foreach (DbFavorite favorite in toUpdate)
            {
                favorite.MarkAsModified(this);
            }
        }

        internal List<int> GetRdpFavoriteIds()
        {
            return this.Favorites.Where(candidate => candidate.Protocol == ConnectionManager.RDP)
                       .Select(rdpFavorite => rdpFavorite.Id).ToList();
        }
    }
}