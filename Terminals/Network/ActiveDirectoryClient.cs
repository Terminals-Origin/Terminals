using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;

namespace Terminals.Network {
    public class ActiveDirectoryClient {
        public static Dictionary<string, string>  ListComputers(string Domain)
        {
            Dictionary<string, string> Computers = new Dictionary<string, string>();
            try {
                DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}", Domain));
                DirectorySearcher mySearcher = new DirectorySearcher(entry);
                mySearcher.Filter = ("(objectClass=computer)");
                foreach(SearchResult resEnt in mySearcher.FindAll()) {
                    DirectoryEntry computer = resEnt.GetDirectoryEntry();
                    string name = computer.Name.Replace("CN=", "");
                    string tags = "";
                    if(computer.Properties != null && computer.Properties["name"] != null && computer.Properties["name"].Count > 0)
                    {
                        name = computer.Properties["name"][0].ToString();
                    }
                    if(computer.Properties != null && computer.Properties["operatingSystem"] != null && computer.Properties["operatingSystem"].Count > 0)
                    {
                        tags = computer.Properties["operatingSystem"][0].ToString();
                    }
                    Computers.Add(name, tags);   
                    //foreach(string name in computer.Properties.PropertyNames)
                    //{

                    //    Terminals.Logging.Log.Info("---------------------------name=" + name + "; ");
                    //    for(int x = 0; x < computer.Properties[name].Count;x++ )
                    //    {
                    //        Terminals.Logging.Log.Info(computer.Properties[name][x]);
                    //    }
                    //}

                }
            } catch(Exception exc) {
                Terminals.Logging.Log.Error("Could not list the computers on the domain:" + Domain, exc);
            }
            return Computers;
        }
    }
}