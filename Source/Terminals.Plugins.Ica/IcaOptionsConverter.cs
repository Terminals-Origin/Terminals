using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Ica
{
    internal class IcaOptionsConverter : IOptionsConverter
    {
        public void FromCofigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as ICAOptions;
            if (options != null)
            {
                FavoriteConfigurationElement source = context.ConfigFavorite;
                options.ApplicationName = source.ICAApplicationName;
                options.ApplicationPath = source.ICAApplicationPath;
                options.ApplicationWorkingFolder = source.ICAApplicationWorkingFolder;
                options.ClientINI = source.IcaClientINI;
                options.ServerINI = source.IcaServerINI;
                options.EnableEncryption = source.IcaEnableEncryption;
                options.EncryptionLevel = source.IcaEncryptionLevel;
            }
        }

        public void ToConfigFavorite(OptionsConversionContext context)
        {
            var options = context.Favorite.ProtocolProperties as ICAOptions;
            if (options != null)
            {
                FavoriteConfigurationElement destination = context.ConfigFavorite;
                destination.ICAApplicationName = options.ApplicationName;
                destination.ICAApplicationPath = options.ApplicationPath;
                destination.ICAApplicationWorkingFolder = options.ApplicationWorkingFolder;
                destination.IcaClientINI = options.ClientINI;
                destination.IcaServerINI = options.ServerINI;
                destination.IcaEnableEncryption = options.EnableEncryption;
                destination.IcaEncryptionLevel = options.EncryptionLevel;
            }
        }
    }
}