using System;

namespace Terminals.Data.History
{
    /// <summary>
    /// Custom provide of current time. Uniform resolution of DateTime in UTC, for all persistence types
    /// </summary>
    public interface IDateService
    {
        /// <summary>
        /// Gets current time in UTC
        /// </summary>
        DateTime UtcNow { get; }
    }
}