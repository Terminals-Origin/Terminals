namespace Terminals.Common.Converters
{
    public interface IRelativeUrlProvider
    {
        /// <summary>
        /// Gets or sets the Url relative part of Url defined for web based connections.
        /// Null by default, to obtain full path <see cref="UrlConverter"/> .
        /// </summary>
        string RelativeUrl { get; set; }
    }
}