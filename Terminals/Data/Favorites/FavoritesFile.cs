using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Terminals.Data
{
    [Serializable]
    [XmlRoot(Namespace = "http://Terminals.codeplex.com")]
    public class FavoritesFile
    {
        [XmlAttribute]
        public string FileVerion { get; set; }
        public List<Favorite> Favorites { get; set; }
        public List<Group> Groups { get; set; }

        public FavoritesFile()
        {
            this.FileVerion = "2.0";
            this.Favorites = new List<Favorite>();
            this.Groups = new List<Group>();
        }
    }
}
