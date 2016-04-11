namespace Terminals.Common.Connections
{
    public interface IOptionsConverter
    {
        void FromCofigFavorite(OptionsConversionContext context);

        void ToConfigFavorite(OptionsConversionContext context);
    }
}