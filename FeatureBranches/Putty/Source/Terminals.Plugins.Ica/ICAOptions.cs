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

        public override ProtocolOptions Copy()
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
    }
}
