using System;

namespace Terminals.Data.History
{
    /// <summary>
    /// Uniform access to the current DateTime in Universal format.
    /// Allows to define custom time moments by unit tests.
    /// </summary>
    internal static class Moment
    {
        /// <summary>
        /// Gets current time in UTC provided by Service
        /// </summary>
        internal static DateTime Now
        {
            get { return service.UtcNow; }
        }

        // don't make it read only, is used by tests.
        private static IDateService service = new NowService();

        private class NowService : IDateService
        {
            public DateTime UtcNow
            {
                get { return DateTime.UtcNow; }
            }
        }
    }
}