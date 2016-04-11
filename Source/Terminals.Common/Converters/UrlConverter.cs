using System;
using Terminals.Data;

namespace Terminals.Common.Converters
{
    /// <summary>
    /// Because general properties are no able to inject controls to simplify the UI.
    /// </summary>
    public class UrlConverter
    {
        public static Uri TryParseUrl(string url)
        {
            try
            {
                return new Uri(url);
            }
            catch (Exception ex)
            {
                Logging.Error("Web URL Parse Failed", ex);
                return null;
            }
        }

        public static void UpdateFavoriteUrl(IFavorite destination, string newAbsoluteUrl)
        {
            var webOptions = destination.ProtocolProperties as IRelativeUrlProvider;
            if (webOptions != null)
            {
                UpdateMyFavoriteUrl(destination, webOptions, newAbsoluteUrl);
            }
        }

        private static void UpdateMyFavoriteUrl(IFavorite destination, IRelativeUrlProvider relativeUrl, string newAbsoluteUrl)
        {
            Uri url = UrlConverter.TryParseUrl(newAbsoluteUrl);
            if (url == null)
                return;

            destination.ServerName = url.Host;
            destination.Port = url.Port; // would also manage default port for us
            string pathAndQuery = url.PathAndQuery.Equals(@"/") ? String.Empty : url.PathAndQuery;
            relativeUrl.RelativeUrl = pathAndQuery;
        }

        public static string ExtractAbsoluteUrl(IFavorite source)
        {
            try
            {
                return TryFormatAbsoluteUrl(source);
            }
            catch // UriBuilder fails on at least ICA Citrix as unknown service, use stupid URI formatting in this case
            {
                return String.Format(@"{0}://{1}:{2}/", source.Protocol, source.ServerName, source.Port);
            }
        }

        private static string TryFormatAbsoluteUrl(IFavorite source)
        {
            var webOptions = source.ProtocolProperties as IRelativeUrlProvider;
            string relativeUrl = String.Empty;
            if (webOptions != null)
                relativeUrl = webOptions.RelativeUrl;

            string protocol = source.Protocol.ToLower();
            var uriBuilder = new UriBuilder(protocol, source.ServerName, source.Port);
            // using relative url in uriBuilder constructor encodes ? character. So we have to make an workaround
            return uriBuilder + relativeUrl;
        }
    }
}
