using System.Collections.Generic;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Integration.Import
{
    internal class ImportTerminalsContext
    {
        private readonly IPersistence persistence;

        private FavoriteConfigurationSecurity favoriteSecurity;

        public PropertyReader Reader { get; private set; }

        public List<FavoriteConfigurationElement> Favorites { get; private set; }

        /// <summary>
        /// bacause reading more than one property into the same favorite,
        /// keep the favorite out of the read property method.
        /// </summary>
        public FavoriteConfigurationElement Current { get; private set; }

        public ImportTerminalsContext(PropertyReader reader, IPersistence persistence)
        {
            this.persistence = persistence;
            this.Reader = reader;
            this.Favorites = new List<FavoriteConfigurationElement>();
        }

        internal void SetNewCurrent()
        {
            this.Current = new FavoriteConfigurationElement();
            this.Favorites.Add(this.Current);
            this.favoriteSecurity = new FavoriteConfigurationSecurity(this.persistence, this.Current);
        }

        public void ReadTsgwPassword()
        {
            this.favoriteSecurity.TsgwPassword = this.Reader.ReadString();
        }

        public void ReadPassword()
        {
            this.favoriteSecurity.Password = this.Reader.ReadString();
        }
    }
}