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

        internal abstract void FromCofigFavorite(FavoriteConfigurationElement favorite);

        internal abstract void ToConfigFavorite(FavoriteConfigurationElement favorite);
    }
}
