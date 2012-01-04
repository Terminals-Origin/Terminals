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

        private Dictionary<Guid, IFavorite> favorites = new Dictionary<Guid, IFavorite>();
        [XmlIgnore]
        List<IFavorite> IGroup.Favorites
        {
            get
            {
                return favorites.Values.ToList();
            }
        }

        public Group()
        {
            this.Parent = Guid.Empty;
            this.Id = Guid.NewGuid();
        }

        public override string ToString()
        {
            string parent = "Root";
            if (this.Parent != Guid.Empty)
                parent = this.Parent.ToString();

            return String.Format("Group:Name={0},Id={1},Parent={2},Favorites={3}",
                this.name, this.Id, parent, this.favorites.Count);
        }

        void IGroup.AddFavorite(IFavorite favorite)
        {
            if (favorite == null)
                return;

            if (!this.favorites.ContainsKey(favorite.Id))
                this.favorites.Add(favorite.Id, favorite);
            else
                this.favorites[favorite.Id] = favorite;
        }

        void IGroup.RemoveFavorite(IFavorite favorite)
        {
            if (this.favorites.ContainsKey(favorite.Id))
                this.favorites.Remove(favorite.Id);
        }

        internal FavoritesInGroup GetGroupReferences()
        {
            Guid[] favoriteIds = ((IGroup)this).Favorites.Select(favorite => favorite.Id).ToArray();
            return new FavoritesInGroup
            {
                GroupId = this.Id,
                Favorites = favoriteIds
            };
        }

        internal void UpdateFavorites(List<IFavorite> newFavorites)
        {
            List<IFavorite> current = ((IGroup)this).Favorites;
            this.RemoveRedundantFavorites(current, newFavorites);
            this.AddMissingFavorites(current, newFavorites);
        }

        private void AddMissingFavorites(List<IFavorite> current, List<IFavorite> newFavorites)
        {
            var favoritesToAdd = ListsHelper.GetMissingSourcesInTarget(newFavorites, current);
            foreach (Favorite favorite in favoritesToAdd)
            {
                ((IGroup)this).AddFavorite(favorite);
            }
        }

        private void RemoveRedundantFavorites(List<IFavorite> current, List<IFavorite> newFavorites)
        {
            var favoritesToRemove = ListsHelper.GetMissingSourcesInTarget(current, newFavorites);
            foreach (Favorite favorite in favoritesToRemove)
            {
                ((IGroup)this).AddFavorite(favorite);
            }
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
    }
}
