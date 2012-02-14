using System;
using System.Collections.Generic;
using System.Linq;

namespace Terminals.Data.DB
{
    internal partial class Group : IGroup
    {
        private Guid guid = Guid.NewGuid();
        Guid IGroup.Id
        {
            get { return this.guid; }
            set { this.guid = value; }
        }

        /// <summary>
        /// Gets or sets the virtual unique identifer. This isnt used, because of internal database identifier.
        /// Only for compatibility with file persistance.
        /// </summary>
        public Guid Parent
        {
            get { return ((IGroup)this.ParentGroup).Id; }
            set
            {
                throw new NotImplementedException();
            }
        }

        List<IFavorite> IGroup.Favorites
        {
            get { return this.Favorites.Cast<IFavorite>().ToList(); }
        }

        public void AddFavorite(IFavorite favorite)
        {
            AddFavoriteToCache(favorite);
        }

        private void AddFavoriteToCache(IFavorite favorite)
        {
            this.Favorites.Add((Favorite)favorite);
        }

        public void AddFavorites(List<IFavorite> favorites)
        {
            foreach (IFavorite favorite in favorites)
            {
                AddFavoriteToCache(favorite);
            }
        }

        public void RemoveFavorite(IFavorite favorite)
        {
            RemoveFavoriteFromCache(favorite);
        }

        public void RemoveFavorites(List<IFavorite> favorites)
        {
            foreach (IFavorite favorite in favorites)
            {
                RemoveFavoriteFromCache(favorite);
            }
        }

        private void RemoveFavoriteFromCache(IFavorite favorite)
        {
            this.Favorites.Remove((Favorite)favorite);
        }
    }
}
