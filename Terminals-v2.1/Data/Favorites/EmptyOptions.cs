using System;
namespace Terminals.Data
{
    /// <summary>
    /// Options without any properties.
    /// Used to support "never null" concept in protocol options.
    /// </summary>
    [Serializable]
    public class EmptyOptions : ProtocolOptions
    {
        internal override ProtocolOptions Copy()
        {
            return new EmptyOptions();
        }

        internal override void FromCofigFavorite(IFavorite destination, FavoriteConfigurationElement source)
        {
            // nothing to do
        }

        internal override void ToConfigFavorite(IFavorite source, FavoriteConfigurationElement destination)
        {
            // nothing to do
        }
    }
}
