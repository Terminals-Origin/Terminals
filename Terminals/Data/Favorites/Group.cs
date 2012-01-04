using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Terminals.Configuration;

namespace Terminals.Data
{
    [Serializable]
    public class Group
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

        private Dictionary<Guid, Favorite> favorites = new Dictionary<Guid, Favorite>();
        [XmlIgnore]
        public List<Favorite> Favorites
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
                this.name, this.Id, parent, this.Favorites.Count);
        }

        public void AddFavorite(Favorite favorite)
        {
            if (favorite == null)
                return;

            if (!this.favorites.ContainsKey(favorite.Id))
                this.favorites.Add(favorite.Id, favorite);
            else
                this.favorites[favorite.Id] = favorite;
        }

        //internal FavoritesInGroup GetGroupReferences()
        //{
        //    Guid[] favoriteIds = this.Favorites.Select(favorite => favorite.Id).ToArray();
        //    return new FavoritesInGroup
        //    {
        //        GroupId = this.Id,
        //        Favorites = favoriteIds
        //    };
        //}
    }
}
