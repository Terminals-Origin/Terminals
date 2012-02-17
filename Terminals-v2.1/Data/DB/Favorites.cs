using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL persisted favorites container
    /// </summary>
    internal class Favorites : IFavorites
    {
        private DataBase dataBase;
        private DataDispatcher dispatcher;

        // TODO Add eventing (Jiri Pokorny, 13.02.2012)
        // TODO Save changes to database (Jiri Pokorny, 13.02.2012)
        // TODO Not all members are implemented (Jiri Pokorny, 13.02.2012)

        internal Favorites(DataBase dataBase, DataDispatcher dispatcher)
        {
            this.dataBase = dataBase;
            this.dispatcher = dispatcher;
        }

        IFavorite IFavorites.this[Guid favoriteId]
        {
            get 
            {
                return this.dataBase.Favorites
                .FirstOrDefault(favorite => favorite.Guid == favoriteId);
            }
        }

        IFavorite IFavorites.this[string favoriteName]
        {
            get
            {
                return this.dataBase.Favorites
                  .FirstOrDefault(favorite => favorite.Name
                      .Equals(favoriteName, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public void Add(IFavorite favorite)
        {
            AddToDataBase(favorite);
            this.dispatcher.ReportFavoriteAdded(favorite);
            this.dataBase.SaveImmediatelyIfRequested();
        }

        private void AddToDataBase(IFavorite favorite)
        {
            this.dataBase.AddObject("Favorites", favorite);
        }

        public void Add(List<IFavorite> favorites)
        {
            AddAllToDatabase(favorites);
            this.dispatcher.ReportFavoritesAdded(favorites);
            this.dataBase.SaveImmediatelyIfRequested();
        }

        private void AddAllToDatabase(List<IFavorite> favorites)
        {
            foreach (var favorite in favorites)
            {
                AddToDataBase(favorite);
            }
        }

        public void Update(IFavorite favorite)
        {
            var candidate = favorite as Favorite;
            if(candidate.EntityState != EntityState.Modified)
                return;
            // todo check how to realy update an entity
            SaveAndReportFavoriteUpdated(favorite);
        }

        public void UpdateFavorite(IFavorite favorite, List<IGroup> groups)
        {
            SaveAndReportFavoriteUpdated(favorite);
        }

        private void SaveAndReportFavoriteUpdated(IFavorite favorite)
        {
            this.dispatcher.ReportFavoriteUpdated(favorite);
            this.dataBase.SaveImmediatelyIfRequested();
        }

        public void Delete(IFavorite favorite)
        {
            DeleteFavoriteFromDatabase(favorite);
            this.dispatcher.ReportFavoriteDeleted(favorite);
            this.dataBase.SaveImmediatelyIfRequested();
        }

        public void Delete(List<IFavorite> favorites)
        {
            DeleteFavoritesFromDatabase(favorites);
            this.dispatcher.ReportFavoritesDeleted(favorites);
            this.dataBase.SaveImmediatelyIfRequested();
        }

        private void DeleteFavoritesFromDatabase(List<IFavorite> favorites)
        {
            foreach (var favorite in favorites)
            {
                DeleteFavoriteFromDatabase(favorite);
            }
        }

        private void DeleteFavoriteFromDatabase(IFavorite favorite)
        {
            this.dataBase.Favorites.DeleteObject(favorite as Favorite);
        }

        public SortableList<IFavorite> ToList()
        {
            IQueryable<IFavorite> favorites = this.dataBase.Favorites.Cast<IFavorite>();
            return new SortableList<IFavorite>(favorites);
        }

        public SortableList<IFavorite> ToListOrderedByDefaultSorting()
        {
            return Data.Favorites.OrderByDefaultSorting(this.ToList());
        }

        public void ApplyCredentialsToAllFavorites(List<IFavorite> selectedFavorites, ICredentialSet credential)
        {
            Data.Favorites.ApplyCredentialsToFavorites(selectedFavorites, credential);
            SaveAndReportFavoritesUpdated(selectedFavorites);
        }

        private void SaveAndReportFavoritesUpdated(List<IFavorite> selectedFavorites)
        {
            this.dispatcher.ReportFavoritesUpdated(selectedFavorites);
            this.dataBase.SaveImmediatelyIfRequested();
        }

        public void SetPasswordToAllFavorites(List<IFavorite> selectedFavorites, string newPassword)
        {
            Data.Favorites.SetPasswordToFavorites(selectedFavorites, newPassword);
            SaveAndReportFavoritesUpdated(selectedFavorites);
        }

        public void ApplyDomainNameToAllFavorites(List<IFavorite> selectedFavorites, string newDomainName)
        {
            Data.Favorites.ApplyDomainNameToFavorites(selectedFavorites, newDomainName);
            SaveAndReportFavoritesUpdated(selectedFavorites);
        }

        public void ApplyUserNameToAllFavorites(List<IFavorite> selectedFavorites, string newUserName)
        {
            Data.Favorites.ApplyUserNameToFavorites(selectedFavorites, newUserName);
            SaveAndReportFavoritesUpdated(selectedFavorites);
        }

        #region IEnumerable members

        public IEnumerator<IFavorite> GetEnumerator()
        {
            return this.dataBase.Favorites
              .Cast<IFavorite>()
              .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
