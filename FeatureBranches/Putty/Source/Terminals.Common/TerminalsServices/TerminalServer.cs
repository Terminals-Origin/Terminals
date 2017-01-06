using System.Collections.Generic;
using System.Windows.Forms;
using Terminals.Common.Properties;
using Terminals.Forms;

namespace Terminals.TerminalServices
{
    public class TerminalServer
    {
        public static TerminalServer LoadServer(string ServerName)
        {
            return TerminalServicesAPI.GetSessions(ServerName);            
        }

        public static void SendMessageToSession(Session session)
        {
            if (session != null)
            {
                string prompt = Resources.Pleaseenterthemessagetosend;
                InputBoxResult result = InputBox.Show(prompt, "Send network message");
                if (result.ReturnCode == DialogResult.OK && !string.IsNullOrEmpty(result.Text))
                {
                    string meessageText = result.Text.Trim();
                    string messageHeader = Resources.MessagefromyourAdministrator;
                    TerminalServicesAPI.SendMessage(session, messageHeader, meessageText, 0, 10, false);
                }
            }
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