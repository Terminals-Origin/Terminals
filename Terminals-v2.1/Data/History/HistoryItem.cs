using System;
using System.Security.Principal;
using System.Xml.Serialization;
using Terminals.Data;

namespace Terminals.History
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

        [XmlIgnore]
        string IHistoryItem.DateGroup
        {
            get { return GetDateGroup(this.Date.Date); }
        }

        private static string GetDateGroup(DateTime itemDate)
        {
            if (itemDate >= DateTime.Now.Date)
                return HistoryByFavorite.TODAY;

            if (itemDate >= DateTime.Now.AddDays(-1).Date)
                return HistoryByFavorite.YESTERDAY;

            if (itemDate >= DateTime.Now.AddDays(-7).Date)
                return HistoryByFavorite.WEEK;

            if (itemDate >= DateTime.Now.AddDays(-14).Date)
                return HistoryByFavorite.TWOWEEKS;

            if (itemDate >= DateTime.Now.AddMonths(-1).Date)
                return HistoryByFavorite.MONTH;

            if (itemDate >= DateTime.Now.AddMonths(-6).Date)
                return HistoryByFavorite.OVERONEMONTH;

            if (itemDate >= DateTime.Now.AddYears(-1).Date)
                return HistoryByFavorite.YEAR;

            return HistoryByFavorite.YEAR;
        }

        /// <summary>
        /// Assignes current user security id to it, if the user account is domain.
        /// For local user accaunt this value isnt set to preserver file persistance space,
        /// because all istory items than have the same value.
        /// </summary>
        void IHistoryItem.AssignCurentUser()
        {
            try
            {
                string fullLogin = Environment.UserDomainName + "\\" + Environment.UserName;
                var account = new NTAccount(fullLogin);
                IdentityReference sidReference = account.Translate(typeof(SecurityIdentifier));
                this.UserSid = sidReference.ToString();
            }
            catch
            {
                this.UserSid = null;
            }
        }
    }
}
