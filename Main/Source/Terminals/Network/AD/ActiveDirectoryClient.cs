using System;
using System.DirectoryServices;
using System.Threading;

namespace Terminals.Network
{
    internal delegate void ListComputersDoneDelegate(bool success);
    internal delegate void ComputerFoundDelegate(ActiveDirectoryComputer computer);
    
    internal class ActiveDirectoryClient
    {
        internal event ListComputersDoneDelegate ListComputersDone;
        internal event ComputerFoundDelegate ComputerFound;
        private readonly object runLock = new object();
        
        private Boolean isRunning;
        internal Boolean IsRunning
        {
            get
            {
                lock (runLock)
                {
                    return this.isRunning;
                }
            }
            private set
            {
                lock (runLock)
                {
                    this.isRunning = value;
                }
            }
        }

        private Boolean cancelationPending;
        private Boolean CancelationPending
        {
            get
            {
                lock (runLock)
                {
                    return this.cancelationPending;
                }
            }
        }

        internal void FindComputers(string domain)
        {
            if (!this.IsRunning) // nothing is running
            {
                this.IsRunning = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(StartScan), domain); 
            }
        }

        internal void Stop()
        {
            lock (runLock)
            {
                if (this.isRunning)
                {
                    this.cancelationPending = true;
                }
            }
        }

        private void StartScan(object domain)
        {
            try
            {
                SearchComputers(domain.ToString());
                FireListComputersDone(true);
            }
            catch (Exception exc)
            {
                FireListComputersDone(false);
                Logging.Log.Error("Could not list the computers on the domain:" + domain, exc);
            }
            finally
            {
                this.IsRunning = false;
            }
        }

        private void SearchComputers(string domain)
        {
            using (DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}", domain)))
            {
                using (DirectorySearcher searcher = CreateSearcher(entry))
                {
                    foreach (SearchResult result in searcher.FindAll())
                    {
                        if (this.CancelationPending)
                            return;
                        DirectoryEntry computer = result.GetDirectoryEntry();
                        var comp = ActiveDirectoryComputer.FromDirectoryEntry(domain, computer);
                        FireComputerFound(comp);
                    }
                }
            }
        }

        private static DirectorySearcher CreateSearcher(DirectoryEntry entry)
        {
            var searcher = new DirectorySearcher(entry);
            searcher.Asynchronous = true;
            searcher.Filter = ("(objectClass=computer)");
            // extend default maximum number of returned results
            searcher.PageSize = 1000;
            searcher.SizeLimit = 5000;
            return searcher;
        }

        private void FireListComputersDone(Boolean success)
        {
            if (this.ListComputersDone != null)
                this.ListComputersDone(success);
        }

        private void FireComputerFound(ActiveDirectoryComputer computer)
        {
            if (this.ComputerFound != null)
                ComputerFound(computer);
        }
    }
}