using Terminals.Data;

namespace Terminals.Forms
{
    /// <summary>
    /// Favorite view model 1:1 wrapper around IFavorite. To be bound in Organize favorites form, 
    /// because we need flatern complex properties like UserName obtained from Security.
    /// WinForms grid is not able to bound "Security.UserName" as DataProperty.
    /// </summary>
    internal class FavoriteViewModel
    {
        private ICredentials storedCredentials;

        internal IFavorite Favorite { get; private set; }
        
        public string Name
        {
            get { return this.Favorite.Name; }
            set { this.Favorite.Name = value; }
        }

        public string ServerName
        {
            get { return this.Favorite.ServerName; }
        }

        public string Protocol
        {
            get { return this.Favorite.Protocol; }
        }

        public string UserName
        {
            get { return this.Favorite.Security.UserName; }
        }

        public string Credential
        {
            get
            {
                var credentialSet = this.storedCredentials[this.Favorite.Security.Credential];
                if (credentialSet != null)
                    return credentialSet.Name;
                
                return string.Empty;
            }
        }

        public string GroupNames
        {
            get { return this.Favorite.GroupNames; }
        }

        public string Notes
        {
            get { return this.Favorite.Notes; }
        }

        internal FavoriteViewModel(IFavorite favorite, ICredentials storedCredentials)
        {
            this.Favorite = favorite;
            this.storedCredentials = storedCredentials;
        }
    }
}
