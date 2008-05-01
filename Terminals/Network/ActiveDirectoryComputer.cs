using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Network
{
    public class ActiveDirectoryComputer
    {

        private string protocol = "RDP";

        public string Protocol
        {
            get { return protocol; }
            set { protocol = value; }
        }


        private bool import=true;

        public bool Import
        {
            get { return import; }
            set { import = value; }
        }

        private string computerName;

        public string ComputerName
        {
            get { return computerName; }
            set { computerName = value; }
        }
        private string operatingSystem;

        public string OperatingSystem
        {
            get { return operatingSystem; }
            set { operatingSystem = value; }
        }
        private string tags;

        public string Tags
        {
            get { return tags; }
            set { tags = value; }
        }
        private string notes = "";

        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }
    }
}
