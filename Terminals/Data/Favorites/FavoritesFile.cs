using System;
using System.Collections.Generic;

namespace Terminals.Data
{
    [Serializable]
    public class FavoritesFile
    {
        public List<Favorite> Favorites { get; set; }
        public List<Group> Groups { get; set; }

        public FavoritesFile()
        {
            this.Favorites = new List<Favorite>();
            this.Groups = new List<Group>();
        }
    }
}
