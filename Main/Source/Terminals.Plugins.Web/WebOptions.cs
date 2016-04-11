using System;
using Terminals.Common.Converters;

namespace Terminals.Data
{
    /// <summary>
    /// Options container for Http or Https. To obtain absolute url
    /// when retrieving connection use following form:
    /// Favorite_Protocol://Favorite_ServerName:Favorite_Port/WebOptions_RelativeUrl
    /// </summary>
    [Serializable]
    public class WebOptions : ProtocolOptions, IRelativeUrlProvider
    {
        public string RelativeUrl { get; set; }

        public override ProtocolOptions Copy()
        {
            return new WebOptions
            {
                RelativeUrl = this.RelativeUrl
            };
        }
    }
}
