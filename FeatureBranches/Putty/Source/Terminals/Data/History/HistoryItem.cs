using System;
using System.Xml.Serialization;
using Terminals.Data.History;
using Terminals.Network;

namespace Terminals.Data
{
    /// <summary>
    /// Represents one favorite touch when trying connect to its server.
    /// Stored is time stamp and user who accessed it. Useful for access audits.
    /// </summary>
    [Serializable]
    public class HistoryItem : IHistoryItem
    {
        public HistoryItem()
        {
            Date = Moment.Now;
        }

        [XmlIgnore]
        DateTime IHistoryItem.Date
        {
            get { return this.Date.ToLocalTime(); }
        }

        /// <summary>
        /// Gets or sets the persisted date in UTC, when the favorite connection was initialized.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the user security Id in text form. Null by default or in case of local account.
        /// Useful for application data share in domain environment.
        /// http://stackoverflow.com/questions/1140528/what-is-the-maximum-length-of-a-sid-in-sddl-format
        /// </summary>
        public string UserSid { get; set; }

        /// <summary>
        /// Gets the user name with domain prefix in form of DOMAIN\USERNAME
        /// </summary>
        [XmlIgnore]
        string IHistoryItem.UserName
        {
            get
            {
                return WindowsUserIdentifiers.ResolveUserNameFromSid(this.UserSid);
            }
        }

        /// <summary>
        /// Gets or sets associated favorite. This is only a navigation property
        /// </summary>
        [XmlIgnore]
        IFavorite IHistoryItem.Favorite { get { return this.Favorite; } }

        /// <summary>
        /// We don't serialize the favorite, the history item is serialized in groups by favorite
        /// </summary>
        [XmlIgnore]
        public Favorite Favorite { get; set; }

        /// <summary>
        /// Assigns current user security id to it, if the user account is domain.
        /// For local user account this value isn't set to preserver file persistence space,
        /// because all history items than have the same value.
        /// </summary>
        void IHistoryItem.AssignCurentUser()
        {
            this.UserSid = WindowsUserIdentifiers.GetCurrentUserSid();
        }
    }
}
