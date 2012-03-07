using System;
using System.Xml.Serialization;

namespace Terminals.Data
{
    /// <summary>
    /// Represents association between group and it favorites.
    /// Used only for file persistance serialization
    /// </summary>
    [Serializable]
    public class FavoritesInGroup
    {
        [XmlAttribute("groupId")]
        public Guid GroupId { get; set; }
        public Guid[] Favorites { get; set; }
    }
}
