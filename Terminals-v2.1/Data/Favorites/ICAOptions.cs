using System;

namespace Terminals.Data
{
    [Serializable]
    public class ICAOptions : ProtocolOptions
    {
        public string ApplicationName { get; set; }
        public string ApplicationWorkingFolder { get; set; }
        public string ApplicationPath { get; set; }
        public string ServerINI { get; set; }
        public string ClientINI { get; set; }
        public bool EnableEncryption { get; set; }
        public string EncryptionLevel { get; set; }

        internal override ProtocolOptions Copy()
        {
            return new ICAOptions
                {
                    ApplicationName = this.ApplicationName,
                    ApplicationWorkingFolder = this.ApplicationWorkingFolder,
                    ApplicationPath = this.ApplicationPath,
                    ServerINI = this.ServerINI,
                    ClientINI = this.ClientINI,
                    EnableEncryption = this.EnableEncryption,
                    EncryptionLevel = this.EncryptionLevel
                };
        }

        internal override void FromCofigFavorite(IFavorite destination, FavoriteConfigurationElement source)
        {
            this.ApplicationName = source.ICAApplicationName;
            this.ApplicationPath = source.ICAApplicationPath;
            this.ApplicationWorkingFolder = source.ICAApplicationWorkingFolder;
            this.ClientINI = source.IcaClientINI;
            this.ServerINI = source.IcaServerINI;
            this.EnableEncryption = source.IcaEnableEncryption;
            this.EncryptionLevel = source.IcaEncryptionLevel;
        }

        internal override void ToConfigFavorite(IFavorite source, FavoriteConfigurationElement destination)
        {
            destination.ICAApplicationName = this.ApplicationName;
            destination.ICAApplicationPath = this.ApplicationPath;
            destination.ICAApplicationWorkingFolder = this.ApplicationWorkingFolder;
            destination.IcaClientINI = this.ClientINI;
            destination.IcaServerINI = this.ServerINI;
            destination.IcaEnableEncryption = this.EnableEncryption;
            destination.IcaEncryptionLevel = this.EncryptionLevel;
        }
    }
}
