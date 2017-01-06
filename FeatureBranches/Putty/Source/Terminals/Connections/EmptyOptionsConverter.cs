using Terminals.Common.Connections;

namespace Terminals.Connections
{
    /// <summary>
    /// Missing plugin implementation of related converter
    /// </summary>
    internal class EmptyOptionsConverter : IOptionsConverter
    {
        public void FromConfigFavorite(OptionsConversionContext context)
        {
            // nothing to do, empty implementation
        }

        public void ToConfigFavorite(OptionsConversionContext context)
        {
            // nothing to do, empty implementation
        }
    }
}