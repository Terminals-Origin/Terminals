using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data.DB
{
    internal partial class DbGroup : IGroup, IIntegerKeyEnityty
    {
        private IGroup parent;
        /// <summary>
        /// Gets or sets the virtual unique identifier. This isn't used, because of internal database identifier.
        /// Only for compatibility with file persistence.
        /// </summary>
        public IGroup Parent
        {
            get
            {
                this.LoadParent();
                return this.parent;
            }
            set
            {
                this.SaveParentToDatabase(value);
                this.parent = value;
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
                List<DbFavorite> selected = this.favorites.Cached
                    .Where(candidate => favoriteIds.Contains(candidate.Id))
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

        internal DbGroup(string name) :this()
        {
            this.Name = name;
        }

        internal void AssignStores(Groups groups, DataDispatcher dispatcher, Favorites favorites)
        {
            this.groups = groups;
            this.dispatcher = dispatcher;
            this.favorites = favorites;
        }

        private void SaveParentToDatabase(IGroup value)
        {
            using (var database = Database.CreateInstance())
            {
                database.Groups.Attach(this);
                this.ParentGroup = value as DbGroup;
                database.SaveImmediatelyIfRequested();
                database.Detach(this);
            }
        }

        private void LoadParent()
        {
            if (this.parent != null)
                return;

            this.LoadFromDatabase();
        }

        private void LoadFromDatabase()
        {
            using (var database = Database.CreateInstance())
            {
                database.Groups.Attach(this);
                database.Entry(this).Reference(g => g.ParentGroup).Load();
                // pick up the item from cache instead of temporal connection to use only cached items
                // groups should be already loaded, because this Group was also resolved from cache
                int parentGroupId = this.ParentGroup != null ? this.ParentGroup.Id : -1;
                this.parent = this.groups[parentGroupId];
                database.Detach(this);
            }
        }

        private void CacheFavoriteIds()
        {
            if (this.favoriteIds == null)
                this.favoriteIds = this.LoadFavoritesFromDatabase();
        }

        private List<int?> LoadFavoritesFromDatabase()
        {
            using (var database = Database.CreateInstance())
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

        private void UpdateFavoritesMembershipInDatabase(List<IFavorite> favorites)
        {
            using (var database = Database.CreateInstance())
            {
                foreach (DbFavorite favorite in favorites)
                {
                    database.InsertFavoritesInGroup(favorite.Id, this.Id);
                    this.AddToCachedIds(favorite);
                }
            }
        }

        public void AddFavorites(List<IFavorite> favorites)
        {
            this.UpdateFavoritesMembershipInDatabase(favorites);
            this.ReportGroupChanged(this);
        }

        public void RemoveFavorite(IFavorite favorite)
        {
            this.RemoveFavorites(new List<IFavorite> { favorite });
            this.ReportGroupChanged(this);
        }

        public void RemoveFavorites(List<IFavorite> favorites)
        {
            RemoveFavoritesFromDatabase(favorites);
            this.ReportGroupChanged(this);
        }

        private void ReportGroupChanged(IGroup group)
        {
            this.dispatcher.ReportGroupsUpdated(new List<IGroup> { group });
        }

        private void RemoveFavoritesFromDatabase(List<IFavorite> favorites)
        {
            using (var database = Database.CreateInstance())
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

        public override string ToString()
        {
            return Data.Group.ToString(this, this.Id.ToString());
        }
    }
}
