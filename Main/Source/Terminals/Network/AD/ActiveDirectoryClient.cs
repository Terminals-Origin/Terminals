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

        internal void FindComputers(ActiveDirectorySearchParams searchParams)
        {
            if (!this.IsRunning) // nothing is running
            {
                this.cancelationPending = false;
                this.IsRunning = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(StartScan), searchParams); 
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


        private void StartScan(object state)
        {
            try
            {
                var searchParams = state as ActiveDirectorySearchParams;
                SearchComputers(searchParams);
                FireListComputersDone(true);
            }
            catch (Exception exc)
            {
                FireListComputersDone(false);
                Logging.Error("Could not list the computers on the domain: " + state, exc);
            }
            finally
            {
                this.IsRunning = false;
            }
        }

        private void SearchComputers(ActiveDirectorySearchParams searchParams)
        {
            using (DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}", searchParams.Domain)))
            {
                using (DirectorySearcher searcher = CreateSearcher(entry, searchParams))
                {
                    using (SearchResultCollection found = searcher.FindAll())
                    {
                        this.ImportResults(searchParams, found);
                    }
                }
            }
        }

        private void ImportResults(ActiveDirectorySearchParams searchParams, SearchResultCollection found)
        {
            foreach (SearchResult result in found)
            {
                if (this.CancelationPending)
                    return;
                DirectoryEntry computer = result.GetDirectoryEntry();
                var comp = ActiveDirectoryComputer.FromDirectoryEntry(searchParams.Domain, computer);
                this.FireComputerFound(comp);
            }
        }

        private static DirectorySearcher CreateSearcher(DirectoryEntry entry, ActiveDirectorySearchParams searchParams)
        {
            var searcher = new DirectorySearcher(entry);
            searcher.Asynchronous = true;
            searcher.Filter = searchParams.Filter;
            searcher.SearchRoot = new DirectoryEntry("LDAP://"+searchParams.Searchbase);
            searcher.PageSize = searchParams.PageSize;
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