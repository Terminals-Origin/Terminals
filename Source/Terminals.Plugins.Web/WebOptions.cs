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
        public string UsernameID { get; set; }
        public string PasswordID { get; set; }
        public string OptionalID { get; set; }
        public string OptionalValue { get; set; }
        public string SubmitID { get; set; }
        public bool EnableHTMLAuth { get; set; }
        public bool EnableFormsAuth { get; set; }

        public override ProtocolOptions Copy()
        {
            return new WebOptions
            {
                RelativeUrl = this.RelativeUrl,
                UsernameID = this.UsernameID,
                PasswordID = this.PasswordID,
                OptionalID = this.OptionalID,
                OptionalValue = this.OptionalValue,
                SubmitID = this.SubmitID,
                EnableHTMLAuth = this.EnableHTMLAuth,
                EnableFormsAuth = this.EnableFormsAuth,
            };
        }
    }
}
