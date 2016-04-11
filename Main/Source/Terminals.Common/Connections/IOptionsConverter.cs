namespace Terminals.Common.Connections
{
    public interface IOptionsConverter
    {
        void FromConfigFavorite(OptionsConversionContext context);

        void ToConfigFavorite(OptionsConversionContext context);
    }
}