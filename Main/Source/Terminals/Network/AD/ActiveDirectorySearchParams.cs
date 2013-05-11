using System;

namespace Terminals.Network
{
    /// <summary>
    /// Crate for search parameters of active directory search
    /// </summary>
    internal class ActiveDirectorySearchParams
    {
        internal const  string DEFAULT_FILTER = "(&(objectclass=computer)(name=*))";

        internal const int DEFAULT_MAX_RESULTS = 1000;

        private const int MAXIMUM_MAXRESULTS = 5000;

        /// <summary>
        /// Gets LDAP search filter. Default is 
        /// </summary>
        internal string Filter { get; private set; }

        internal string Domain { get; private set; }

        internal string Searchbase { get; private set; }
        
        /// <summary>
        /// Gets maximum number of results parsed from constructor parameter.
        /// Default is 1000, maximum is 5000.
        /// </summary>
        internal int MaximumResults { get; private set; }

        internal ActiveDirectorySearchParams(string domain, string filter, string maximumResults, string searchbase)
        {
            this.Domain = domain;
            this.ParseFilter(filter);
            this.ParseMaximumResults(maximumResults);
            this.ParseSearchBase(searchbase);
        }

        private void ParseFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                this.Filter = DEFAULT_FILTER;
            else
                this.Filter = filter;
        }

        private void ParseMaximumResults(string maximumResults)
        {
            int parsed = DEFAULT_MAX_RESULTS;
            if (Int32.TryParse(maximumResults, out parsed))
                this.MaximumResults = parsed;
            else
                this.MaximumResults = DEFAULT_MAX_RESULTS;

            if (this.MaximumResults > MAXIMUM_MAXRESULTS)
                this.MaximumResults = MAXIMUM_MAXRESULTS;
        }

        private void ParseSearchBase(string searchbase)
        {
            if (string.IsNullOrEmpty(searchbase))
                this.Searchbase = this.Domain;
            else
                this.Searchbase = searchbase;
        }

        public override string ToString()
        {
            return string.Format("ActiveDirectorySearchParams:Domain={0},Filter='{1}',MaximumResults={2},Searchbase={3}",
                                 this.Domain, this.Filter, this.MaximumResults, this.Searchbase);
        }
    }
}