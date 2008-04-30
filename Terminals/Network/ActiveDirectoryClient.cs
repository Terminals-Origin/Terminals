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
                using(DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}", Domain)))
                {
                    using(DirectorySearcher mySearcher = new DirectorySearcher(entry))
                    {
                        mySearcher.Filter = ("(objectClass=computer)");
                        foreach(SearchResult resEnt in mySearcher.FindAll())
                        {
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
                            if(computer.Properties != null && computer.Properties["distinguishedName"] != null && computer.Properties["distinguishedName"].Count > 0)
                            {
                                string distinguishedName = computer.Properties["distinguishedName"][0].ToString();
                                if(distinguishedName.Contains("OU=Domain Controllers"))
                                {
                                    tags += ",Domain Controllers";
                                }
                            }
                            //Terminals.Logging.Log.Info("---------------");
                            //Terminals.Logging.Log.Info(name);
                            //Terminals.Logging.Log.Info("---------------");
                            //foreach(string n in computer.Properties.PropertyNames)
                            //{
                            //    Terminals.Logging.Log.Info("-->" + n);
                            //    for(int x = 0; x < computer.Properties[n].Count; x++)
                            //    {
                            //        Terminals.Logging.Log.Info(computer.Properties[n][x]);
                            //    }
                            //}

                            
                            Computers.Add(name, tags);
                        }
                    }
                }
            } catch(Exception exc) {
                Terminals.Logging.Log.Error("Could not list the computers on the domain:" + Domain, exc);
            }
            return Computers;
        }
    }
}