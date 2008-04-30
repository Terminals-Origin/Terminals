using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;

namespace Terminals.Network {
    public class ActiveDirectoryClient {
        public static List<string> ListComputers(string Domain) {
            List<string> Computers = new List<string>();
            try {
                DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}", Domain));
                DirectorySearcher mySearcher = new DirectorySearcher(entry);
                mySearcher.Filter = ("(objectClass=computer)");
                foreach(SearchResult resEnt in mySearcher.FindAll()) {
                    Computers.Add(resEnt.GetDirectoryEntry().Name);
                }
            } catch(Exception exc) {
                Terminals.Logging.Log.Error("Could not list the computers on the domain:" + Domain, exc);
            }
            return Computers;
        }
    }
}