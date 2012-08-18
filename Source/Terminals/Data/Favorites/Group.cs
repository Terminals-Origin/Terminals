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
        /// By default set to Guid.Empty, which means no parrent
        /// </summary>
        public Guid Parent { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of group in which this group is listed.
        /// By default set to null, which means, that it isnt listed no where 
        /// and will appear as one of root folders in first level of favorites tree.
        /// </summary>
        IGroup IGroup.Parent
        {
            get
            {
                return Persistence.Instance.Groups
                    .FirstOrDefault(candidate => ((Group)candidate).Id == this.Parent);
            }
            set
            {
                var newParent = value as Group;
                if (newParent != null)
                    this.Parent = newParent.Id;
            }
        }

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

        private readonly Dictionary<Guid, IFavorite> favorites;
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

        internal Group(string name) 
            : this(new Dictionary<Guid, IFavorite>())
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
            ReportGroupChanged(this);
        }

        void IGroup.AddFavorites(List<IFavorite> favorites)
        {
            AddFavoritesToCache(favorites);
            ReportGroupChanged(this);
        }

        void IGroup.RemoveFavorites(List<IFavorite> favorites)
        {
            RemoveFavoritesFromCache(favorites);
            ReportGroupChanged(this);
        }

        void IGroup.RemoveFavorite(IFavorite favorite)
        {
            this.RemoveFavoriteFromCache(favorite);
            ReportGroupChanged(this);
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

        internal static void ReportGroupChanged(IGroup group)
        {
            var dispatcher = Persistence.Instance.Dispatcher;
            dispatcher.ReportGroupsUpdated(new List<IGroup> { group });
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

        public override string ToString()
        {
            return ToString(this, this.Id.ToString());
        }

        internal static string ToString(IGroup group, string groupId)
        {
            string parent = "Root";
            if (group.Parent != null)
                parent = group.Name;

            return String.Format("Group:Name={0},Id={1},Parent={2},Favorites={3}",
                                 group.Name, groupId, parent, group.Favorites.Count);
        }

        bool IStoreIdEquals<IGroup>.StoreIdEquals(IGroup oponent)
        {
            var oponentGroup = oponent as Group;
            if (oponentGroup == null)
                return false;

            return oponentGroup.Id == this.Id;
        }
    }
}
