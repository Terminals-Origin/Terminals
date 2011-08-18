using System;
using System.DirectoryServices;
using Terminals.Connections;

namespace Terminals.Network
{
    public class ActiveDirectoryComputer
    {
        private const string NAME = "name";
        private const string OS = "operatingSystem";
        private const string DN = "distinguishedName";

        internal ActiveDirectoryComputer()
        {
            this.Import = true;
            this.Protocol = ConnectionManager.RDP;
            this.ComputerName = String.Empty;
            this.OperatingSystem = String.Empty;
            this.Tags = String.Empty;
            this.Notes = String.Empty;
        }

        internal String Protocol { get; set; }
        public Boolean Import { get; set; }
        public String ComputerName { get; set; }
        internal String OperatingSystem { get; set; }
        internal String Tags { get; set; }
        internal String Notes { get; set; }

        internal static ActiveDirectoryComputer FromDirectoryEntry(String domain, DirectoryEntry computer)
        {
            ActiveDirectoryComputer comp = new ActiveDirectoryComputer();
            comp.Tags = domain;

            if (computer.Properties != null)
            {
                comp.NameFromEntry(computer);
                comp.OperationSystemFromEntry(computer);
                comp.DistinquishedNameFromEntry(computer);
            }

            return comp;
        }

        private void NameFromEntry(DirectoryEntry computer)
        {
            PropertyValueCollection nameValues = computer.Properties[NAME];
            String name = computer.Name.Replace("CN=", "");
            if (nameValues != null && nameValues.Count > 0)
            {
                name = nameValues[0].ToString();
            }
            this.ComputerName = name;
        }
        
        private void OperationSystemFromEntry(DirectoryEntry computer)
        {
            PropertyValueCollection osValues = computer.Properties[OS];
            if (osValues != null && osValues.Count > 0)
            {
                this.Tags += "," + osValues[0].ToString();
                this.OperatingSystem = osValues[0].ToString();
            }
        }

        private void DistinquishedNameFromEntry(DirectoryEntry computer)
        {
            PropertyValueCollection dnameValues = computer.Properties[DN];
            if (dnameValues != null && dnameValues.Count > 0)
            {
                string distinguishedName = dnameValues[0].ToString();
                if (distinguishedName.Contains("OU=Domain Controllers"))
                {
                    this.Tags += ",Domain Controllers";
                }
            }
        }

        internal FavoriteConfigurationElement ToFavorite(String domain)
        {
            FavoriteConfigurationElement favorite = new FavoriteConfigurationElement(this.ComputerName);
            favorite.Name = this.ComputerName;
            favorite.ServerName = this.ComputerName;
            favorite.UserName = Environment.UserName;
            favorite.DomainName = domain;
            favorite.Tags = this.Tags;
            favorite.Port = ConnectionManager.GetPort(this.Protocol);
            favorite.Protocol = this.Protocol;
            favorite.Notes = this.Notes;
            return favorite;
        }
    }
}
