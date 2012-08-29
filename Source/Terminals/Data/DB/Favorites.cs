using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL persisted favorites container
    /// </summary>
    internal class Favorites : IFavorites
    {
        private readonly Groups groups;
        private readonly DataDispatcher dispatcher;

        internal Favorites(Groups groups, DataDispatcher dispatcher)
        {
            this.groups = groups;
            this.dispatcher = dispatcher;
        }

        IFavorite IFavorites.this[Guid favoriteId]
        {
            get
            {
                using (var database = Database.CreateInstance())
                {
                    return database.GetFavoriteByGuid(favoriteId);
                }
            }
        }

        IFavorite IFavorites.this[string favoriteName]
        {
            get
            {
                using (var database = Database.CreateInstance())
                {
                    return database.Favorites.FirstOrDefault(
                            favorite => favorite.Name.Equals(favoriteName, StringComparison.CurrentCultureIgnoreCase));
                }
            }
        }

        public void Add(IFavorite favorite)
        {
            var favoritesToAdd = new List<IFavorite> { favorite };
            Add(favoritesToAdd);
        }

        public void Add(List<IFavorite> favorites)
        {
            using (var database = Database.CreateInstance())
            {
                AddAllToDatabase(database, favorites);
                database.SaveImmediatelyIfRequested();
                database.DetachAll(favorites.Cast<Favorite>());
                this.dispatcher.ReportFavoritesAdded(favorites);
            }
        }

        private void AddAllToDatabase(Database database, List<IFavorite> favorites)
        {
            IEnumerable<Favorite> toAdd = favorites.Cast<Favorite>();
            AddAllToDatabase(database, toAdd);
        }

        private void AddAllToDatabase(Database database, IEnumerable<Favorite> favorites)
        {
            foreach (Favorite favorite in favorites)
            {
                database.Favorites.AddObject(favorite);
            }
        }

        public void Update(IFavorite favorite)
        {
            using (var database = Database.CreateInstance())
            {
                var toUpdate = favorite as Favorite;
                if (toUpdate != null)
                {
                    database.Attach(toUpdate);
                    this.TrySaveAndReportFavoriteUpdate(toUpdate, database);
                }
            }
        }

        public void UpdateFavorite(IFavorite favorite, List<IGroup> newGroups)
        {
            using (var database = Database.CreateInstance())
            {
                var toUpdate = favorite as Favorite;
                if (toUpdate == null)
                    return;

                database.Attach(toUpdate);
                List<IGroup> addedGroups = this.groups.AddToDatabase(database, newGroups);

                List<IGroup> redundantGroups = ListsHelper.GetMissingSourcesInTarget(favorite.Groups, newGroups);
                List<IGroup> missingGroups = ListsHelper.GetMissingSourcesInTarget(newGroups, favorite.Groups);
                
                Data.Favorites.AddIntoMissingGroups(favorite, missingGroups);
                Data.Groups.RemoveFavoritesFromGroups(new List<IFavorite> { favorite }, redundantGroups);
                
                List<IGroup> removedGroups = this.groups.DeleteEmptyGroupsFromDatabase(database);
                database.SaveImmediatelyIfRequested();
                this.dispatcher.ReportGroupsRecreated(addedGroups, removedGroups);
                this.TrySaveAndReportFavoriteUpdate(toUpdate, database);
            }
        }

        private void TrySaveAndReportFavoriteUpdate(Favorite toUpdate, Database database)
        {
            try
            {
                this.SaveAndReportFavoriteUpdated(database, toUpdate);
            }
            catch (OptimisticConcurrencyException)
            {
                this.TryToRefreshUpdatedFavorite(toUpdate, database);
            }
        }

        private void TryToRefreshUpdatedFavorite(Favorite toUpdate, Database database)
        {
            try
            {
                database.Refresh(RefreshMode.ClientWins, toUpdate);
                this.SaveAndReportFavoriteUpdated(database, toUpdate);
            }
            catch (InvalidOperationException)
            {
                this.dispatcher.ReportFavoriteDeleted(toUpdate);
            }
        }

        private void SaveAndReportFavoriteUpdated(Database database, Favorite favorite)
        {
            favorite.MarkAsModified(database);
            database.SaveImmediatelyIfRequested();
            database.DetachFavorite(favorite);
            this.dispatcher.ReportFavoriteUpdated(favorite);
        }

        public void Delete(IFavorite favorite)
        {
            var favoritesToDelete = new List<IFavorite> { favorite };
            Delete(favoritesToDelete);
        }

        public void Delete(List<IFavorite> favorites)
        {
            using (var database = Database.CreateInstance())
            {
                DeleteFavoritesFromDatabase(database, favorites);
                List<IGroup> deletedGroups = this.groups.DeleteEmptyGroupsFromDatabase(database);
                database.SaveImmediatelyIfRequested();
                this.dispatcher.ReportGroupsDeleted(deletedGroups);
                this.dispatcher.ReportFavoritesDeleted(favorites);
            }
        }

        private void DeleteFavoritesFromDatabase(Database database, List<IFavorite> favorites)
        {
            IEnumerable<Favorite> favoritesToAdd = favorites.Cast<Favorite>().ToList();
            database.AttachAll(favoritesToAdd);
            DeleteFavoritseFromDatabase(database, favoritesToAdd);
        }

        private void DeleteFavoritseFromDatabase(Database database, IEnumerable<Favorite> favoritesToAdd)
        {
            foreach (Favorite favorite in favoritesToAdd)
            {
                database.Favorites.DeleteObject(favorite);
            }
        }

        public SortableList<IFavorite> ToList()
        {
            IEnumerable<IFavorite> favorites = GetFavorites();
            return new SortableList<IFavorite>(favorites);
        }

        private IEnumerable<IFavorite> GetFavorites()
        {
            using (var database = Database.CreateInstance())
            {
                // to list because Linq to entities allowes only cast to primitive types
                List<Favorite> favorites = database.Favorites.ToList();
                database.DetachAll(favorites);
                return favorites;
            }
        }

        public SortableList<IFavorite> ToListOrderedByDefaultSorting()
        {
            return Data.Favorites.OrderByDefaultSorting(this);
        }

        public void ApplyCredentialsToAllFavorites(List<IFavorite> selectedFavorites, ICredentialSet credential)
        {
            using (var database = Database.CreateInstance())
            {
                Data.Favorites.ApplyCredentialsToFavorites(selectedFavorites, credential);
                SaveAndReportFavoritesUpdated(database, selectedFavorites);
            }
        }

        private void SaveAndReportFavoritesUpdated(Database database, List<IFavorite> selectedFavorites)
        {
            database.SaveImmediatelyIfRequested();
            this.dispatcher.ReportFavoritesUpdated(selectedFavorites);
        }

        public void SetPasswordToAllFavorites(List<IFavorite> selectedFavorites, string newPassword)
        {
            using (var database = Database.CreateInstance())
            {
                Data.Favorites.SetPasswordToFavorites(selectedFavorites, newPassword);
                SaveAndReportFavoritesUpdated(database, selectedFavorites);
            }
        }

        public void ApplyDomainNameToAllFavorites(List<IFavorite> selectedFavorites, string newDomainName)
        {
            using (var database = Database.CreateInstance())
            {
                Data.Favorites.ApplyDomainNameToFavorites(selectedFavorites, newDomainName);
                SaveAndReportFavoritesUpdated(database, selectedFavorites);
            }
        }

        public void ApplyUserNameToAllFavorites(List<IFavorite> selectedFavorites, string newUserName)
        {
            using (var database = Database.CreateInstance())
            {
                Data.Favorites.ApplyUserNameToFavorites(selectedFavorites, newUserName);
                SaveAndReportFavoritesUpdated(database, selectedFavorites);
            }
        }

        #region IEnumerable members

        public IEnumerator<IFavorite> GetEnumerator()
        {
            return this.GetFavorites()
              .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
