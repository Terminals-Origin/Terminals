using System;
using System.Xml.Serialization;
using Terminals.Security;

namespace Terminals.Data
{
    [Serializable]
    public class TsGwOptions
    {
        /// <summary>
        /// TSC_PROXY_MODE_NONE_DIRECT 0 (0x0)
        /// Do not use an RD Gateway server. In the Remote Desktop Connection (RDC) client UI, the Bypass RD Gateway server for local addresses check box is cleared.
        /// 
        /// TSC_PROXY_MODE_DIRECT 1 (0x1)
        /// Always use an RD Gateway server. In the RDC client UI, the Bypass RD Gateway server for local addresses check box is cleared.
        /// 
        /// TSC_PROXY_MODE_DETECT 2 (0x2)
        /// Use an RD Gateway server if a direct connection cannot be made to the RD Session Host server. In the RDC client UI, the Bypass RD Gateway server for local addresses check box is selected.
        /// 
        /// TSC_PROXY_MODE_DEFAULT 3 (0x3)
        /// Use the default RD Gateway server settings.
        /// 
        /// TSC_PROXY_MODE_NONE_DETECT 4 (0x4)
        /// Do not use an RD Gateway server. In the RDC client UI, the Bypass RD Gateway server for local addresses check box is selected.
        /// </summary>
        public Int32 UsageMethod { get; set; }
        public Int32 CredentialSource { get; set; }
        public Boolean SeparateLogin { get; set; }

        private string hostName;
        public String HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        private string domain;
        public String Domain
        {
            get
            {
                return domain;
            }
            set
            {
                domain = value;
            }
        }
        
        private string userName;
        public String UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        private string encryptedPassword;
        public String EncryptedPassword
        {
            get
            {
                return encryptedPassword;
            }
            set
            {
                encryptedPassword = value;
            }
        }

        [XmlIgnore]
        internal String Password
        {
            get
            {
                return PasswordFunctions.DecryptPassword(EncryptedPassword);
            }
            set
            {
                EncryptedPassword = PasswordFunctions.EncryptPassword(value);
            }
        }
    }
}
