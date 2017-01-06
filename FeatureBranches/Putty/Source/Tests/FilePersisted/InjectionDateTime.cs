using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data.History;

namespace Tests.FilePersisted
{
    /// <summary>
    /// For testing purpose allows define fix DateTime to be used instead of DateTime.Now.
    /// </summary>
    internal class InjectionDateTime : IDateService
    {
        public DateTime UtcNow { get; set; }

        /// <summary>
        /// Injects current UTC time as fixed constant as replacement of DateTime.UtcNow for testing purposes
        /// </summary>
        internal static void SetTestDateTime()
        {
            var moment = new PrivateType(typeof(Moment));
            var current = DateTime.UtcNow;
            // reduce precision to seconds, because Ticks may be extracted by SQL server
            DateTime precise = new DateTime(current.Year, current.Month, current.Day, current.Hour, current.Minute, current.Second);
            var customMoment = new InjectionDateTime { UtcNow = precise };
            moment.SetStaticField("service", customMoment);
        }
    }
}
