using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using Terminals.Configuration;
using Terminals.Security;

namespace Terminals.Data.DB
{
    internal partial class Database
    {
        internal void SaveImmediatelyIfRequested()
        {
            try
            {
                // dont ask, save immediately. Here is no benefit to save in batch like in FilePersistence
                this.SaveChanges();
            }
            catch (OptimisticConcurrencyException exception)
            {
                Logging.Log.Debug("Concurency exception catched when saving SqlPersistence", exception);
                this.RefreshChangedEntities();
                this.SaveChanges();
            }
        }

        private void RefreshChangedEntities()
        {
            ObjectStateEntry modifiedEntry = this.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).FirstOrDefault();
            this.Favorites.MergeOption = MergeOption.OverwriteChanges;
            this.Refresh(RefreshMode.ClientWins, modifiedEntry);
        }

        public override int SaveChanges(SaveOptions options)
        {
            var changedFavorites = this.GetChangedOrAddedFavorites();
            return this.SaveChanges(options, changedFavorites);
        }

        private int SaveChanges(SaveOptions options, IEnumerable<Favorite> changedFavorites)
        {
            int returnValue = base.SaveChanges(options);
            this.SaveFavoriteDetails(changedFavorites);
            return returnValue;
        }

        private void SaveFavoriteDetails(IEnumerable<Favorite> changedFavorites)
        {
            foreach (Favorite favorite in changedFavorites)
            {
                favorite.SaveDetails(this);
            }
        }

        private IEnumerable<Favorite> GetChangedOrAddedFavorites()
        {
            List<ObjectStateEntry> changes = this.GetChangedOrAddedEntitites();

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

        internal Favorite GetFavoriteByGuid(Guid favoriteId)
        {
            // to list, because linq to entities doesnt support Guid search
            return this.Favorites.ToList()
                .FirstOrDefault(favorite => favorite.Guid == favoriteId);
        }

        internal byte[] GetFavoriteIcon(int favoriteId)
        {
            byte[] obtained = this.GetFavoriteIcon((int?)favoriteId).FirstOrDefault();
            if (obtained != null)
                return obtained;

            return FavoriteIcons.EmptyImageData;
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
            // todo do it in transaction to prevent inconsistent data
            this.UpdateMasterPasswordKey(newMasterPasswordKey);
        }

        /// <summary>
        /// Attaches all item in entitiesToAttach to this context.
        /// Does not check, if the enties are already in the context.
        /// </summary>
        /// <typeparam name="TEntity">Entity defined in this object context model</typeparam>
        /// <param name="entitiesToAttach">Not null collection of items from this model</param>
        internal void AttachAll<TEntity>(IEnumerable<TEntity> entitiesToAttach)
            where TEntity : IEntityWithKey
        {
            foreach (TEntity entity in entitiesToAttach)
            {
                this.Attach(entity);
            }
        }

        /// <summary>
        /// Detaches all item in entitiesToDetach from this context.
        /// Does not check, if the enties are in the context.
        /// </summary>
        /// <typeparam name="TEntity">Entity defined in this object context model</typeparam>
        /// <param name="entitiesToDetach">Not null collection of items from this model
        /// currently attached in this object context</param>
        internal void DetachAll<TEntity>(IEnumerable<TEntity> entitiesToDetach)
        {
            foreach (TEntity entity in entitiesToDetach)
            {
                this.Detach(entity);
            }
        }

        internal void DetachAll(IEnumerable<Favorite> favorites)
        {
            foreach (Favorite favorite in favorites)
            {
                this.DetachFavorite(favorite);
            }
        }

        internal void DetachFavorite(Favorite favorite)
        {
            favorite.DetachDetails(this);
            this.Detach(favorite);
        }

        /// <summary>
        /// Switch toUpdate entity state to Modified. 
        /// </summary>
        internal void MarkAsModified(object toUpdate)
        {
            this.ObjectStateManager.ChangeObjectState(toUpdate, EntityState.Modified);
        }
    }
}
