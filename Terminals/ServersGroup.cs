using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals
{
    internal class ServersGroupOld
    {
        internal ServersGroupOld()
        {
            favoriteNames = new List<string>();
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<string> favoriteNames;

        public List<string> FavoriteNames
        {
            get { return favoriteNames; }
            set { favoriteNames = value; }
        }
    }
}
