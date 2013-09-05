using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Terminals.Data.DB
{
    internal partial class DbGroup : IGroup, IIntegerKeyEnityty
    {
        // we dont have to release the field, to refrehs the instance, because the value is loaded directly
        private DbGroup parent;

        /// <summary>
        /// Gets or sets the virtual unique identifier. This isn't used, because of internal database identifier.
        /// Only for compatibility with file persistence.
        /// </summary>
        public IGroup Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                this.parent = (DbGroup)value;
            }
        }

        private List<int?> favoriteIds;

        List<IFavorite> IGroup.Favorites
        {
            get
            {
                // see also the Favorite.Groups
                this.CacheFavoriteIds();

                // List<Favorite> recent = this.Favorites.ToList();
                // prefer to select cached items, instead of selecting from database directly
                List<DbFavorite> selected = this.favorites
                    .Where<DbFavorite>(candidate => favoriteIds.Contains(candidate.Id))
                    .ToList();
                return selected.Cast<IFavorite>().ToList();
            }
        }

        /// <summary>
        /// Gets or sets the redirection container, which is able to obtain parent
        /// </summary>
        private Groups groups;

        /// <summary>
        /// Gets or sets its associated eventing container. Used to report favorite in group membership changes.
        /// </summary>
        private DataDispatcher dispatcher;

        private Favorites favorites;

        internal DbGroup(string name)
            : this()
        {
            this.Name = name;
        }

        internal void AssignStores(Groups groups, DataDispatcher dispatcher, Favorites favorites)
        {
            this.groups = groups;
            this.dispatcher = dispatcher;
            this.favorites = favorites;
        }

        private void CacheFavoriteIds()
        {
            if (this.favoriteIds == null)
                this.favoriteIds = this.LoadFavoritesFromDatabase();
        }

        private List<int?> LoadFavoritesFromDatabase()
        {
            try
            {
                return this.TryLoadFavoritesFromDatabase();
            }
            catch (EntityException exception)
            {
                return this.dispatcher.ReportFunctionError(LoadFavoritesFromDatabase, this, exception,
                     "Unable to load group favorites from database");
            }
        }

        private List<int?> TryLoadFavoritesFromDatabase()
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                return database.GetFavoritesInGroup(this.Id).ToList();
            }
        }

        internal void ReleaseFavoriteIds()
        {
            this.favoriteIds = null;
        }

        private void AddToCachedIds(DbFavorite favorite)
        {
            if (!this.ContainsFavorite(favorite.Id))
                this.favoriteIds.Add(favorite.Id);
        }

        private void RemoveFromCachedIds(DbFavorite favorite)
        {
            if (this.ContainsFavorite(favorite.Id))
                this.favoriteIds.Remove(favorite.Id);
        }

        internal bool ContainsFavorite(int favoriteId)
        {
            this.CacheFavoriteIds();
            return this.favoriteIds.Contains(favoriteId);
        }

        public void AddFavorite(IFavorite favorite)
        {
            this.AddFavorites(new List<IFavorite> { favorite });
        }

        public void AddFavorites(List<IFavorite> favorites)
        {
            try
            {
                this.TryAddFavorites(favorites);
            }
            catch (DbUpdateException)
            {
                this.groups.RefreshCache();
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(AddFavorites, favorites, this, exception,
                                                  "Unable to add favorite to database group.");
            }
        }

        private void TryAddFavorites(List<IFavorite> favorites)
        {
            this.UpdateFavoritesMembershipInDatabase(favorites);
            this.ReportGroupChanged(this);
        }

        private void UpdateFavoritesMembershipInDatabase(List<IFavorite> favorites)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                foreach (DbFavorite favorite in favorites)
                {
                    database.InsertFavoritesInGroup(favorite.Id, this.Id);
                    this.AddToCachedIds(favorite);
                }
            }
        }

        public void RemoveFavorite(IFavorite favorite)
        {
            this.RemoveFavorites(new List<IFavorite> { favorite });
            this.ReportGroupChanged(this);
        }

        public void RemoveFavorites(List<IFavorite> favorites)
        {
            try
            {
                this.TryRemoveFavorites(favorites);
            }
            catch (DbUpdateException)
            {
                this.groups.RefreshCache();
            }
            catch (EntityException exception)
            {
                this.dispatcher.ReportActionError(RemoveFavorites, favorites, this, exception, 
                    "Unable to remove favorites from group.");
            }
        }

        private void TryRemoveFavorites(List<IFavorite> favorites)
        {
            this.RemoveFavoritesFromDatabase(favorites);
            this.ReportGroupChanged(this);
        }

        private void ReportGroupChanged(IGroup group)
        {
            this.dispatcher.ReportGroupsUpdated(new List<IGroup> { group });
        }

        private void RemoveFavoritesFromDatabase(List<IFavorite> favorites)
        {
            using (Database database = DatabaseConnections.CreateInstance())
            {
                foreach (DbFavorite favorite in favorites)
                {
                    database.DeleteFavoritesInGroup(favorite.Id, this.Id);
                    this.RemoveFromCachedIds(favorite);
                }
            }
        }

        bool IStoreIdEquals<IGroup>.StoreIdEquals(IGroup oponent)
        {
            var oponentGroup = oponent as DbGroup;
            if (oponentGroup == null)
                return false;

            return oponentGroup.Id == this.Id;
        }

        public int GetStoreIdHash()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return Group.ToString(this, this.Id.ToString());
        }

        internal void LoadFieldsFromReferences()
        {
            // loaded at once, so we expect to have the parent group instance also in cache
            this.parent = this.ParentGroup;
        }

        internal void FieldsToReferences()
        {
            this.ParentGroup = this.parent;
        }
    }
}
