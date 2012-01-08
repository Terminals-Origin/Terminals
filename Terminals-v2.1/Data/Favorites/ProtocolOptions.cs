using System;

namespace Terminals.Data
{
    /// <summary>
    /// Used as "clonable" in favorite protocol properties
    /// </summary>
    [Serializable]
    public abstract class ProtocolOptions
    {
        internal abstract ProtocolOptions Copy();

        internal abstract void FromCofigFavorite(IFavorite destination, FavoriteConfigurationElement source);

        internal abstract void ToConfigFavorite(IFavorite source, FavoriteConfigurationElement destination);
    }
}
