using System;
using System.Xml.Serialization;

namespace Terminals.Data
{
    /// <summary>
    /// Represents one favorite touch when trying connect to its server.
    /// Stored is time stamp and user who accessed it. Usefull for access audits.
    /// </summary>
    internal interface IHistoryItem
    {
        /// <summary>
        /// Gets or sets access time stamp of time when the favorite connection was initialized.
        /// </summary>
        DateTime Date { get; set; }

        /// <summary>
        /// Gets the user name with domain prefix in form of DOMAIN\USERNAME
        /// </summary>
        [XmlIgnore]
        string UserName { get; }

        /// <summary>
        /// Gets or sets associated favorite. This is only a navigation property
        /// </summary>
        [XmlIgnore]
        IFavorite Favorite { get; set; }

        /// <summary>
        /// Assignes current user security id to it, if the user account is domain.
        /// For local user accaunt this value isnt set to preserver file persistance space,
        /// because all istory items than have the same value.
        /// </summary>
        void AssignCurentUser();
    }
}