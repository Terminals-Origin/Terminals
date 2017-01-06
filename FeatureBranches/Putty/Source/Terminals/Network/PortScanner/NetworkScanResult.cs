using System;
using Terminals.Connections;

namespace Terminals.Scanner
{
    internal class NetworkScanResult
    {
        public String IPAddress { get; set; }
        public String HostName { get; set; }
        internal Int32 Port { get; set; }
        public String ServiceName { get; set; }

        public override String ToString()
        {
            return String.Format("NetworkScanResult:{0},{1},{2}",
                this.IPAddress, this.ServiceName, this.HostName);
        }

        internal FavoriteConfigurationElement ToFavorite(String tags)
        {
            FavoriteConfigurationElement favorite = new FavoriteConfigurationElement();
            favorite.ServerName = this.IPAddress;
            favorite.Port = this.Port;
            favorite.Protocol = this.ServiceName;
            if (tags != String.Empty)
                favorite.Tags = tags;
            favorite.Name = String.Format("{0}_{1}", this.HostName, favorite.Protocol);
            favorite.DomainName = Environment.UserDomainName;
            favorite.UserName = Environment.UserName;
            return favorite;
        }
    }
}
