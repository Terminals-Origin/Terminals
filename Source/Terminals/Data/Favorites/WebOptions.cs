using System;

namespace Terminals.Data
{
    /// <summary>
    /// Options container for Http or Https. To obtain absolute url
    /// when retrieving connection use following form:
    /// Favorite_Protocol://Favorite_ServerName:Favorite_Port/WebOptions_RelativeUrl
    /// </summary>
    [Serializable]
    public class WebOptions : ProtocolOptions
    {
        /// <summary>
        /// Gets or sets the Url relative part of Url defined for web based connections.
        /// Null by default, to obtain full path use static method.
        /// </summary>
        public string RelativeUrl { get; set; }

        internal override ProtocolOptions Copy()
        {
            return new WebOptions
                {
                    RelativeUrl = this.RelativeUrl
                };
        }

        internal override void FromCofigFavorite(IFavorite destination, FavoriteConfigurationElement source)
        {
            UpdateMyFavoriteUrl(destination, source.Url);
        }

        internal override void ToConfigFavorite(IFavorite source, FavoriteConfigurationElement destination)
        {
            destination.Url = ExtractAbsoluteUrl(source);
        }

        internal static string ExtractAbsoluteUrl(IFavorite source)
        {
          try
          {
            return TryFormatAbsoluteUrl(source);
          }
          catch // UriBuilder fails on at least ICA Citrix as unknown service, use stupid URI formatting in this case
          {
            return string.Format(@"{0}://{1}:{2}/", source.Protocol, source.ServerName, source.Port);
          }
        }

      private static string TryFormatAbsoluteUrl(IFavorite source)
      {
        var webOptions = source.ProtocolProperties as WebOptions;
        string relativeUrl = string.Empty;
        if (webOptions != null)
          relativeUrl = webOptions.RelativeUrl;

        string protocol = source.Protocol.ToLower();
        var uriBuilder = new UriBuilder(protocol, source.ServerName, source.Port);
        // using relative url in uriBuilder constructor encodes ? character. So we have to make an workaround
        return uriBuilder + relativeUrl;
      }

      internal static void UpdateFavoriteUrl(IFavorite destination, string newAbsoluteUrl)
        {
            var webOptions = destination.ProtocolProperties as WebOptions;
            if (webOptions != null)
            {
                webOptions.UpdateMyFavoriteUrl(destination, newAbsoluteUrl);
            }
        }

        private void UpdateMyFavoriteUrl(IFavorite destination, string newAbsoluteUrl)
        {
            Uri url = TryParseUrl(newAbsoluteUrl);
            if (url == null)
                return;

            destination.ServerName = url.Host;
            destination.Port = url.Port; // would also manage default port for us
            string pathAndQuery = url.PathAndQuery.Equals(@"/") ? string.Empty : url.PathAndQuery;
            this.RelativeUrl = pathAndQuery;
        }

        internal static Uri TryParseUrl(string url)
        {
            try
            {
                return new Uri(url);
            }
            catch (Exception ex)
            {
                Logging.Log.Error("Web URL Parse Failed", ex);
                return null;
            }
        }
    }
}
