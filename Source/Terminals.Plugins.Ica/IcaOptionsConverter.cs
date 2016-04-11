using Terminals.Common.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Ica
{
    internal class IcaOptionsConverter : OptionsConverterTemplate<ICAOptions>, IOptionsConverter
    {
        protected override void FromConfigFavorite(FavoriteConfigurationElement source, ICAOptions options)
        {
            options.ApplicationName = source.ICAApplicationName;
            options.ApplicationPath = source.ICAApplicationPath;
            options.ApplicationWorkingFolder = source.ICAApplicationWorkingFolder;
            options.ClientINI = source.IcaClientINI;
            options.ServerINI = source.IcaServerINI;
            options.EnableEncryption = source.IcaEnableEncryption;
            options.EncryptionLevel = source.IcaEncryptionLevel;
        }

        protected override void ToConfigFavorite(FavoriteConfigurationElement destination, ICAOptions options)
        {
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