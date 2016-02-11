using System;

namespace Terminals.Data
{
    /// <summary>
    /// Used as "clone able" in favorite protocol properties
    /// </summary>
    [Serializable]
    public abstract class ProtocolOptions
    {
        public abstract ProtocolOptions Copy();

        public abstract void FromCofigFavorite(IFavorite destination, FavoriteConfigurationElement source);

        public abstract void ToConfigFavorite(IFavorite source, FavoriteConfigurationElement destination);
    }
}
