namespace Terminals.Network
{
    /// <summary>
    /// Crate for search parameters of active directory search
    /// </summary>
    internal class ActiveDirectorySearchParams
    {
        internal const string DEFAULT_FILTER = "(&(objectclass=computer)(name=*))";

        private const int DEFAULT_MAX_RESULTS = 1000;

        /// <summary>
        /// Gets LDAP search filter. Default is 
        /// </summary>
        internal string Filter { get; private set; }

        internal string Domain { get; private set; }

        internal string Searchbase { get; private set; }

        /// <summary>
        /// Gets maximum number of results parsed from constructor parameter.
        /// Default is 1000, maximum is 5000.
        /// http://stackoverflow.com/questions/90652/can-i-get-more-than-1000-records-from-a-directorysearcher-in-asp-net
        /// </summary>
        internal int PageSize { get; private set; }

        internal ActiveDirectorySearchParams(string domain, string filter, string searchbase)
        {
            this.PageSize = DEFAULT_MAX_RESULTS;
            this.Domain = domain;
            this.ParseFilter(filter);
            this.ParseSearchBase(searchbase);
        }

        private void ParseFilter(string filter)
        {
            if (string.IsNullOrEmpty(filter))
                this.Filter = DEFAULT_FILTER;
            else
                this.Filter = filter;
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
                                 this.Domain, this.Filter, this.PageSize, this.Searchbase);
        }
    }
}