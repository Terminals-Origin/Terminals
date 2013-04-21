using System;

namespace Terminals.Updates
{
    /// <summary>
    /// Description of new release
    /// </summary>
    internal class ReleaseInfo
    {
        /// <summary>
        /// Gets flag informing, that new release is available
        /// </summary>
        internal bool NewAvailable { get; private set; }
        internal DateTime Published { get; private set; }
        internal string Version { get; private set; }

        private static readonly ReleaseInfo notAvailable = new ReleaseInfo(DateTime.MinValue, "Not available")
            {
                NewAvailable = false
            };
                
        /// <summary>
        /// Gets result of update check in a case new release is not available.
        /// </summary>
        internal static ReleaseInfo NotAvailable { get { return notAvailable; }}

        internal ReleaseInfo(DateTime published, string version)
        {
            this.NewAvailable = true;
            this.Published = published;
            this.Version = version;
        }

        public override string ToString()
        {
            return string.Format("ReleaseInfo:{0},Published={1},NewAvailable={2}",
                                 this.Version, this.Published, this.NewAvailable);
        }
    }
}