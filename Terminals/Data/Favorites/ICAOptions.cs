using System;

namespace Terminals.Data
{
    [Serializable]
    public class ICAOptions
    {
        public string ApplicationName { get; set; }
        public string ApplicationWorkingFolder { get; set; }
        public string ApplicationPath { get; set; }
        public String ServerINI { get; set; }
        public String ClientINI { get; set; }
        public Boolean EnableEncryption { get; set; }
        public String EncryptionLevel { get; set; }
    }
}
