using System;

namespace Terminals.Data
{
    [Serializable]
    public class RdpSecurityOptions
    {
        public Boolean EnableSecuritySettings { get; set; }
        public Boolean EnableEncryption { get; set; }
        
        public Boolean EnableTLSAuthentication { get; set; }
        public Boolean EnableNLAAuthentication { get; set; }

        private string workingFolder;
        public String WorkingFolder
        {
            get
            {
                return this.workingFolder;
            }
            set
            {
                this.workingFolder = value;
            }
        }

        private string startProgram;
        public String StartProgram
        {
            get
            {
                return this.startProgram;
            }
            set
            {
                this.startProgram = value;
            }
        }
    }
}
