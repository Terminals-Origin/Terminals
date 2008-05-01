using System;
using System.Collections.Generic;
using System.Text;
using System.DirectoryServices;

namespace Terminals.Network
{
    public class ActiveDirectoryClient
    {

        public delegate void ListComputersDoneDelegate(List<ActiveDirectoryComputer> Computers, bool Success);
        public event ListComputersDoneDelegate OnListComputersDoneDelegate;

        private void ListComputersThread(object state)
        {
            List<ActiveDirectoryComputer> Computers = new List<ActiveDirectoryComputer>();
            bool Success = true;
            string Domain = (string)state;
            try
            {
                using(DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}", Domain)))
                {
                    using(DirectorySearcher mySearcher = new DirectorySearcher(entry))
                    {
                        mySearcher.Filter = ("(objectClass=computer)");
                        foreach(SearchResult resEnt in mySearcher.FindAll())
                        {
                            ActiveDirectoryComputer comp = new ActiveDirectoryComputer();

                            DirectoryEntry computer = resEnt.GetDirectoryEntry();
                            string name = computer.Name.Replace("CN=", "");

                            comp.Tags = Domain;

                            if(computer.Properties != null && computer.Properties["name"] != null && computer.Properties["name"].Count > 0)
                            {
                                name = computer.Properties["name"][0].ToString();
                            }
                            comp.ComputerName = name;

                            if(computer.Properties != null && computer.Properties["operatingSystem"] != null && computer.Properties["operatingSystem"].Count > 0)
                            {
                                comp.Tags += "," + computer.Properties["operatingSystem"][0].ToString();
                                comp.OperatingSystem = computer.Properties["operatingSystem"][0].ToString();

                            }
                            if(computer.Properties != null && computer.Properties["distinguishedName"] != null && computer.Properties["distinguishedName"].Count > 0)
                            {
                                string distinguishedName = computer.Properties["distinguishedName"][0].ToString();
                                if(distinguishedName.Contains("OU=Domain Controllers"))
                                {
                                    comp.Tags += ",Domain Controllers";
                                }
                            }

                            Computers.Add(comp);

                        }
                    }
                }
            }
            catch(Exception exc)
            {
                Success = false;
                Terminals.Logging.Log.Error("Could not list the computers on the domain:" + Domain, exc);
            }
            if(OnListComputersDoneDelegate != null) OnListComputersDoneDelegate(Computers, Success);
        }

        public void ListComputers(string Domain)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ListComputersThread), (object)Domain);
        }
    }
}