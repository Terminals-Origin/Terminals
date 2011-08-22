using System;
using Terminals.Connections;

namespace Terminals.Scanner
{
    internal class NetworkScanResult
    {
        public Boolean Import { get; set; }
        public String IPAddress { get; set; }
        public String HostName { get; set; }
        internal Int32 Port { get; set; }
        internal Boolean IsVMRC { get; set; }

        public String ServiceName
        {
            get
            {
                return ConnectionManager.GetPortName(this.Port, this.IsVMRC);
            }
        }

        internal NetworkScanResult()
        {
            this.Import = true;
        }

        public override String ToString()
        {
            return String.Format("NetworkScanResult:{0},{1},{2},{3}",
                this.IPAddress, this.ServiceName, this.HostName, this.Import);
        }

        internal FavoriteConfigurationElement ToFavorite(String tags)
        {
            FavoriteConfigurationElement favorite = new FavoriteConfigurationElement();
            favorite.ServerName = this.IPAddress;
            favorite.Port = this.Port;
            favorite.Protocol = ConnectionManager.GetPortName(favorite.Port, this.IsVMRC);
            if (tags != String.Empty)
                favorite.Tags = tags;
            favorite.Name = String.Format("{0}_{1}", this.HostName, favorite.Protocol);
            favorite.DomainName = Environment.UserDomainName;
            favorite.UserName = Environment.UserName;
            return favorite;
        }
    }
}
