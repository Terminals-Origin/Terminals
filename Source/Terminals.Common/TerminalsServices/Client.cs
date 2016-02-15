using System;

namespace Terminals.TerminalServices
{
    public class Client
    {
        private bool status;

        public bool Status
        {
            get { return status; }
            set { status = value; }
        }
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }


        private string stationName;

        public string StationName
        {
            get { return stationName; }
            set { stationName = value; }
        }

        private string domainName;

        public string DomianName
        {
            get { return domainName; }
            set { domainName = value; }
        }
        private string clientName;
        public string ClientName
        {
            get { return clientName; }
            set { clientName = value; }
        }
        private int addressFamily;

        public int AddressFamily
        {
            get { return addressFamily; }
            set { addressFamily = value; }
        }
        private byte[] address;

        public byte[] Address
        {
            get { return address; }
            set { address = value; }
        }
        public System.Net.IPAddress IPAddress
        {
            get
            {
                try
                {
                    return new System.Net.IPAddress(this.Address);
                }
                catch (Exception exc)
                {
                    //TODO Logging.Error("IP Address", exc);
                }
                return new System.Net.IPAddress(0);
            }
        }
        public override string ToString()
        {
            
            return string.Format("Domain:{0}, Client:{1}, Station:{2}, Address:{3}, Username:{4}, Status:{5}", DomianName, ClientName, StationName, this.IPAddress.ToString(), UserName, Status);
        }
    }
}