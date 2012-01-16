using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Terminals.Configuration;

namespace Terminals.Data
{
    [Serializable]
    public class Group : IGroup
    {
        [XmlAttribute("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of parent group. 
        /// This allwes deep tree structure of favorite groups.
        /// Empty Guid by default.
        /// </summary>
        public Guid Parent { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (Settings.AutoCaseTags)
                    name = Settings.ToTitleCase(value);
                name = value;
            }
        }

        private Dictionary<Guid, IFavorite> favorites;
        [XmlIgnore]
        List<IFavorite> IGroup.Favorites
        {
            get
            {
                return favorites.Values.ToList();
            }
        }

        public Group()
            : this(new Dictionary<Guid, IFavorite>())
        { }

        internal Group(string name, List<IFavorite> favorites) 
            : this(favorites.ToDictionary(favorite => favorite.Id))
        {
            this.Name = name;
        }

        private Group(Dictionary<Guid, IFavorite> favorites)
        {
            this.Parent = Guid.Empty;
            this.Id = Guid.NewGuid();
            this.favorites = favorites;
        }

        void IGroup.AddFavorite(IFavorite favorite)
        {
            AddFavoriteToCache(favorite);
            ReportThisGroupChanged();
        }

        void IGroup.AddFavorites(List<IFavorite> favoritesToAdd)
        {
            AddFavoritesToCache(favoritesToAdd);
            ReportThisGroupChanged();
        }

        void IGroup.RemoveFavorites(List<IFavorite> favoritesToRemove)
        {
            RemoveFavoritesFromCache(favoritesToRemove);
            this.ReportThisGroupChanged();
        }

        void IGroup.RemoveFavorite(IFavorite favorite)
        {
            this.RemoveFavoriteFromCache(favorite);
            this.ReportThisGroupChanged();
        }

        private void AddFavoriteToCache(IFavorite favorite)
        {
            if (favorite == null)
                return;

            if (!this.favorites.ContainsKey(favorite.Id))
                this.favorites.Add(favorite.Id, favorite);
            else
                this.favorites[favorite.Id] = favorite;
        }

        private void AddFavoritesToCache(List<IFavorite> favoritesToAdd)
        {
            foreach (Favorite favorite in favoritesToAdd)
            {
                this.AddFavoriteToCache(favorite);
            }
        }

        private void RemoveFavoriteFromCache(IFavorite favorite)
        {
            if (this.favorites.ContainsKey(favorite.Id))
                this.favorites.Remove(favorite.Id);
        }

        private void RemoveFavoritesFromCache(List<IFavorite> favoritesToRemove)
        {
            foreach (Favorite favorite in favoritesToRemove)
            {
                this.RemoveFavoriteFromCache(favorite);
            }
        }

        private void ReportThisGroupChanged()
        {
            var dispatcher = Persistance.Instance.Dispatcher;
            dispatcher.ReportGroupsUpdated(new List<IGroup> { this });
        }

        internal FavoritesInGroup GetGroupReferences()
        {
            Guid[] favoriteIds = this.favorites.Keys.ToArray();
            return new FavoritesInGroup
            {
                GroupId = this.Id,
                Favorites = favoriteIds
            };
        }

        internal bool UpdateFavorites(List<IFavorite> newFavorites)
        {
            List<IFavorite> current = this.favorites.Values.ToList();
            bool someRemoved = this.RemoveRedundantFavorites(current, newFavorites);
            bool someAdded = this.AddMissingFavorites(current, newFavorites);
            return someAdded || someRemoved;
        }

        private bool AddMissingFavorites(List<IFavorite> current, List<IFavorite> newFavorites)
        {
            var favoritesToAdd = ListsHelper.GetMissingSourcesInTarget(newFavorites, current);
            AddFavoritesToCache(favoritesToAdd);
            return favoritesToAdd.Count > 0;
        }

        private bool RemoveRedundantFavorites(List<IFavorite> current, List<IFavorite> newFavorites)
        {
            var favoritesToRemove = ListsHelper.GetMissingSourcesInTarget(current, newFavorites);
            RemoveFavoritesFromCache(favoritesToRemove);
            return favoritesToRemove.Count > 0;
        }

        public override bool Equals(object group)
        {
            Group oponent = group as Group;
            if (oponent == null)
                return false;

            return this.Id.Equals(oponent.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            string parent = "Root";
            if (this.Parent != Guid.Empty)
                parent = this.Parent.ToString();

            return String.Format("Group:Name={0},Id={1},Parent={2},Favorites={3}",
                this.name, this.Id, parent, this.favorites.Count);
        }
    }
}
