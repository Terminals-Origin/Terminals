using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.TerminalServices
{
    public class SessionProcess
    {
        private int sessionID;
        public int SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
        }

        private int processID;

        public int ProcessID
        {
            get { return processID; }
            set { processID = value; }
        }

        private string processName;

        public string ProcessName
        {
            get { return processName; }
            set { processName = value; }
        }

        private string user;

        public string User
        {
            get { return user; }
            set { user = value; }
        }

        private string userType;

        public string UserType
        {
            get { return userType; }
            set { userType = value; }
        }
    }
}
