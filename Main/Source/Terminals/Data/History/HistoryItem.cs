using System;
using System.Security.Principal;
using System.Xml.Serialization;

namespace Terminals.Data
{
    /// <summary>
    /// Represents one favorite touch when trying connect to its server.
    /// Stored is time stamp and user who accessed it. Usefull for access audits.
    /// </summary>
    [Serializable]
    public class HistoryItem : IHistoryItem
    {
        public HistoryItem()
        {
            Date = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets access time stamp of time when the favorite connection was initialized.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the user security Id in text form. Null by default or in case of local account.
        /// Usefull for application data share in domain environment.
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
                try
                {
                    var userSid = new SecurityIdentifier(this.UserSid);
                    IdentityReference userLoginReference = userSid.Translate(typeof(NTAccount));
                    return userLoginReference.ToString();
                }
                catch
                {
                    return null;
                }
            } 
        }

        /// <summary>
        /// Gets or sets associated favorite. This is only a navigation property
        /// </summary>
        [XmlIgnore]
        IFavorite IHistoryItem.Favorite { get; set; }

        /// <summary>
        /// Assignes current user security id to it, if the user account is domain.
        /// For local user accaunt this value isnt set to preserver file persistance space,
        /// because all istory items than have the same value.
        /// </summary>
        void IHistoryItem.AssignCurentUser()
        {
            this.UserSid = GetCurrentUserSid();
        }

        internal static string GetCurrentUserSid()
        {
            try
            {
                string fullLogin = Environment.UserDomainName + "\\" + Environment.UserName;
                var account = new NTAccount(fullLogin);
                IdentityReference sidReference = account.Translate(typeof (SecurityIdentifier));
                return sidReference.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
}
