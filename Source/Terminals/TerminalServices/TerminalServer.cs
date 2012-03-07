using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Terminals.TerminalServices
{
    public class TerminalServer
    {
        public static TerminalServer LoadServer(string ServerName)
        {
            return TerminalServicesAPI.GetSessions(ServerName);            
        }

        private List<string> errors = new List<string>();

        public List<string> Errors
        {
            get { return errors; }
            set { errors = value; }
        }


        private List<Session> sessions;

        public List<Session> Sessions
        {
            get { return sessions; }
            set { sessions = value; }
        }


        private bool isATerminalServer;

        public bool IsATerminalServer
        {
            get { return isATerminalServer; }
            set { isATerminalServer = value; }
        }


        private string serverName;

        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        private System.IntPtr serverPointer;

        public System.IntPtr ServerPointer
        {
            get { return serverPointer; }
            set { serverPointer = value; }
        }
		
	


    }
}