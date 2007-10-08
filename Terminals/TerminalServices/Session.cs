using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.TerminalServices
{
    public class Session
    {
        private bool isTheActiveSession;

        public bool IsTheActiveSession
        {
            get { return isTheActiveSession; }
            set { isTheActiveSession = value; }
        }
	
        public bool LogOff()
        {
            return true;
        }
        private string serverName;

        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }
        private Client client;

        public Client Client
        {
            get { return client; }
            set { client = value; }
        }

        private int sessionID;
        public int SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
        }
        private string windowsStationName;

        public string WindowsStationName
        {
            get { return windowsStationName; }
            set { windowsStationName = value; }
        }
        private ConnectionStates state;

        public ConnectionStates State
        {
            get { return state; }
            set { state = value; }
        }
	
    }
}