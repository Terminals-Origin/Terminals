using System;
using System.Collections;
using System.Collections.Generic;

namespace Terminals.Data.DB
{
    /// <summary>
    /// SQL persisted favorites container
    /// </summary>
    internal class Favorites : IFavorites
    {
        private DataBase dataBase;

        // TODO Add eventing (Jiri Pokorny, 13.02.2012)
        // TODO Save changes to database (Jiri Pokorny, 13.02.2012)
        // TODO Not all members are implemented (Jiri Pokorny, 13.02.2012)

        internal Favorites(DataBase dataBase)
        {
            this.dataBase = dataBase;
        }

        public IEnumerator<IFavorite> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IFavorite IFavorites.this[Guid favoriteId]
        {
            get { throw new NotImplementedException(); }
        }

        IFavorite IFavorites.this[string favoriteName]
        {
            get { throw new NotImplementedException(); }
        }

        public void Add(IFavorite favorite)
        {
            AddToDataBase(favorite);
        }

        private void AddToDataBase(IFavorite favorite)
        {
            this.dataBase.AddObject("Favorites", favorite);
        }

        public void Add(List<IFavorite> favorites)
        {
            foreach (var favorite in favorites)
            {
                AddToDataBase(favorite);
            }
        }

        public void Update(IFavorite favorite)
        {
            throw new NotImplementedException();
        }

        public void UpdateFavorite(IFavorite favorite, List<IGroup> groups)
        {
            throw new NotImplementedException();
        }

        public void Delete(IFavorite favorite)
        {
            throw new NotImplementedException();
        }

        public void Delete(List<IFavorite> favorites)
        {
            throw new NotImplementedException();
        }

        public SortableList<IFavorite> ToList()
        {
            throw new NotImplementedException();
        }

        public SortableList<IFavorite> ToListOrderedByDefaultSorting()
        {
            throw new NotImplementedException();
        }

        public void ApplyCredentialsToAllFavorites(List<IFavorite> selectedFavorites, ICredentialSet credential)
        {
            throw new NotImplementedException();
        }

        public void SetPasswordToAllFavorites(List<IFavorite> selectedFavorites, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void ApplyDomainNameToAllFavorites(List<IFavorite> selectedFavorites, string newDomainName)
        {
            throw new NotImplementedException();
        }

        public void ApplyUserNameToAllFavorites(List<IFavorite> selectedFavorites, string newUserName)
        {
            throw new NotImplementedException();
        }
    }
}
