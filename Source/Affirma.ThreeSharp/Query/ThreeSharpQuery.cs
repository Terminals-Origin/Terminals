/******************************************************************************* 
 *  Licensed under the Apache License, Version 2.0 (the "License"); 
 *  
 *  You may not use this file except in compliance with the License. 
 *  You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0.html 
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 *  specific language governing permissions and limitations under the License.
 * ***************************************************************************** 
 * 
 *  Joel Wetzel
 *  Affirma Consulting
 *  jwetzel@affirmaconsulting.com
 * 
 */

using System;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using Affirma.ThreeSharp;
using Affirma.ThreeSharp.Model;
using Affirma.ThreeSharp.Statistics;

namespace Affirma.ThreeSharp.Query
{
    /// <summary>
    /// Implements the IThreeSharp interface that defines the operations of an S3 service proxy.
    /// A mock object could also be built that implements it, for the purposes of offline testing.
    /// </summary>
    public class ThreeSharpQuery : IThreeSharp
    {
        private ThreeSharpConfig config = null;
        private ThreeSharpStatistics statistics = null;

        public ThreeSharpQuery(ThreeSharpConfig threeSharpConfig)
        {
            this.config = threeSharpConfig;
            this.statistics = new ThreeSharpStatistics();

            ServicePointManager.DefaultConnectionLimit = threeSharpConfig.ConnectionLimit;
        }


        // Public API ------------------------------------------------------------


        /// <summary>
        /// Adds a bucket to an S3 account
        /// </summary>
        public BucketAddResponse BucketAdd(BucketAddRequest request)
        {
            return Invoke<BucketAddResponse>(request);
        }


        /// <summary>
        /// Returns a stream of XML, describing the contents of a bucket
        /// </summary>
        public BucketListResponse BucketList(BucketListRequest request)
        {
            return Invoke<BucketListResponse>(request);
        }


        /// <summary>
        /// Streams an object up to a bucket
        /// </summary>
        public ObjectAddResponse ObjectAdd(ObjectAddRequest request)
        {
            return Invoke<ObjectAddResponse>(request);
        }


        /// <summary>
        /// Streams an object down from a bucket
        /// </summary>
        public ObjectGetResponse ObjectGet(ObjectGetRequest request)
        {
            return Invoke<ObjectGetResponse>(request);
        }

        /// <summary>
        /// Copies an object
        /// </summary>
        public ObjectCopyResponse ObjectCopy(ObjectCopyRequest request)
        {
            return Invoke<ObjectCopyResponse>(request);
        }


        /// <summary>
        /// Returns a stream of XML, describing an object's ACL
        /// </summary>
        public ACLGetResponse ACLGet(ACLGetRequest request)
        {
            return Invoke<ACLGetResponse>(request);
        }


        /// <summary>
        /// Deletes an object from a bucket
        /// </summary>
        public ObjectDeleteResponse ObjectDelete(ObjectDeleteRequest request)
        {
            return Invoke<ObjectDeleteResponse>(request);
        }


        /// <summary>
        /// Deletes a bucket
        /// </summary>
        public BucketDeleteResponse BucketDelete(BucketDeleteRequest request)
        {
            return Invoke<BucketDeleteResponse>(request);
        }


        /// <summary>
        /// Generates a URL to access an object in a bucket
        /// </summary>
        public UrlGetResponse UrlGet(UrlGetRequest request)
        {
            UrlGetResponse response = new UrlGetResponse();

            long expires = (ThreeSharpUtils.CurrentTimeMillis() + request.ExpiresIn) / 1000;

            string canonicalString = ThreeSharpUtils.MakeCanonicalString(request.Method, request.BucketName, request.Key, null, null, expires.ToString());
            string encodedCanonical = ThreeSharpUtils.Encode(config.AwsSecretAccessKey, canonicalString, true);

            StringBuilder builder = new StringBuilder();
            if (config.IsSecure)
            {
                builder.Append("https://");
            }
            else
            {
                builder.Append("http://");
            }
            builder.Append(ThreeSharpUtils.BuildUrlBase(config.Server, config.Port, request.BucketName, config.Format));
            if (!String.IsNullOrEmpty(request.BucketName) && !String.IsNullOrEmpty(request.Key))
            {
                builder.Append(request.Key);
            }

            request.QueryList.Add("Signature", encodedCanonical);
            request.QueryList.Add("Expires", "" + expires);
            request.QueryList.Add("AWSAccessKeyId", config.AwsAccessKeyID);
            builder.Append(ThreeSharpUtils.ConvertQueryListToQueryString(request.QueryList));

            String url = builder.ToString();
            byte[] urlBytes = (new ASCIIEncoding()).GetBytes(url);
            response.DataStream = new MemoryStream(urlBytes.Length);
            response.DataStream.Write(urlBytes, 0, urlBytes.Length);
            response.DataStream.Position = 0;

            response.BucketName = request.BucketName;
            response.Key = request.Key + "-URL";
            response.Method = request.Method;
            lock (this.statistics) { this.statistics.AddTransfer(response); }

            return response;
        }


        // Private API ------------------------------------------------------------


        /// <summary>
        /// Interprets a Request object to generate an HttpWebRequest, which is sent to the S3 service.
        /// The HttpWebResponse is then interpreted into a Response object, which is returned.
        /// </summary>
        private T Invoke<T>(Request request) where T : Response, new()
        {
            T response = default(T);

            /* Submit the request and read response body */
            try
            {
                HttpWebRequest httpWebRequest = GenerateAndSendHttpWebRequest(request);
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                
                response = new T();
                response.DataStream = httpWebResponse.GetResponseStream();
                response.BucketName = request.BucketName;
                response.Key = request.Key;
                response.Method = request.Method;
                response.StatusCode = httpWebResponse.StatusCode;
                foreach (String key in httpWebResponse.Headers.Keys)
                {
                    response.Headers.Add(key, httpWebResponse.Headers[key]);
                }

                ThrowIfErrors(httpWebResponse);

                if (response.Method == "GET")
                {
                    lock (this.statistics) { this.statistics.AddTransfer(response); }
                }

            }
            catch (WebException we)
            {
                using (HttpWebResponse httpErrorResponse = (HttpWebResponse)we.Response as HttpWebResponse)
                {
                    if (httpErrorResponse == null)
                    {
                        throw new ThreeSharpException(we);
                    }
                    ThrowIfErrors(httpErrorResponse);
                    throw;
                }
            }
            catch (Exception e)
            {
                if (e is ThreeSharpException)
                {
                    throw;
                }
                else
                {
                    throw new ThreeSharpException(e.Message, e);
                }
            }

            return response;
        }

        private HttpWebRequest GenerateAndSendHttpWebRequest(Request request)
        {
            HttpWebRequest httpWebRequest = ConfigureWebRequest(request);

            if (httpWebRequest.Method == "PUT")
            {
                using (Stream httpWebRequestStream = httpWebRequest.GetRequestStream())
                {
                    if (request.DataStream != null)
                    {
                        // Do some statistical tracking
                        lock (this.statistics) { this.statistics.AddTransfer(request); }       // All operations on this.statistics must be thread-safe

                        byte[] buffer = new byte[1024];
                        int bytesRead = 0;
                        while (true)
                        {
                            bytesRead = request.DataStream.Read(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                                break;
                            httpWebRequestStream.Write(buffer, 0, bytesRead);
                            request.BytesTransferred += bytesRead;
                        }

                        request.DataStream.Close();
                    }
                }
            }

            return httpWebRequest;
        }


        /// <summary>
        /// Checks for presense of the Errors in the response
        /// If errors found, constructs and throws ThreeSharpException
        /// with information from the Errors
        /// </summary>
        private void ThrowIfErrors(HttpWebResponse httpWebResponse)
        {
            if (httpWebResponse.StatusCode != HttpStatusCode.OK && httpWebResponse.StatusCode != HttpStatusCode.TemporaryRedirect)
            {
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string responseString = streamReader.ReadToEnd();

                if (responseString.IndexOf("<Error>") != -1)
                {
                    Match errorMatcher = Regex.Match(responseString,
                        "(<Error><Code>(.*)</Code><Message>(.*)</Message>).*(</Error>)?");
                    if (errorMatcher.Success)
                    {
                        String xml = errorMatcher.Groups[0].Value;
                        String code = errorMatcher.Groups[2].Value;
                        String message = errorMatcher.Groups[3].Value;
                        ThreeSharpException ex = new ThreeSharpException(message, httpWebResponse.StatusCode, code, null, xml);
                        throw ex;
                    }
                }
            }
        }

        
        /// <summary>
        /// Configures an HttpWebRequest with settings from the
        /// ThreeSharpConfig instance and the Request object instance
        /// </summary>
        private HttpWebRequest ConfigureWebRequest(Request request)
        {
            StringBuilder url = new StringBuilder();
            if (!String.IsNullOrEmpty(request.RedirectUrl))
            {
                url.Append(request.RedirectUrl);
            }
            else
            {
                url.Append(config.IsSecure ? "https://" : "http://");
                url.Append(ThreeSharpUtils.BuildUrlBase(config.Server, config.Port, request.BucketName, config.Format));

                if (!String.IsNullOrEmpty(request.Key))
                {
                    url.Append(request.Key);
                }

                // build the query string parameter
                url.Append(ThreeSharpUtils.ConvertQueryListToQueryString(request.QueryList));
            }

            HttpWebRequest httpWebRequest = WebRequest.Create(url.ToString()) as HttpWebRequest;
            httpWebRequest.AllowWriteStreamBuffering = false;
            httpWebRequest.AllowAutoRedirect = false;
            httpWebRequest.UserAgent = config.UserAgent;
            httpWebRequest.Method = request.Method;
            httpWebRequest.Timeout = request.Timeout;
            httpWebRequest.ContentType = request.ContentType;
            httpWebRequest.ContentLength = request.BytesTotal;
            if (this.config.Proxy != null)
                httpWebRequest.Proxy = this.config.Proxy;

            AddHeaders(httpWebRequest, request.Headers);
            AddMetadataHeaders(httpWebRequest, request.MetaData);
            AddAuthorizationHeader(httpWebRequest, request.BucketName, request.Key, request.QueryList);

            return httpWebRequest;
        }


        /// <summary>
        /// Add the given headers to the WebRequest
        /// </summary>
        /// <param name="req">Web request to add the headers to.</param>
        /// <param name="headers">A map of string to string representing the HTTP headers to pass (can be null)</param>
        private void AddHeaders(WebRequest req, SortedList headers)
        {
            AddHeaders(req, headers, "");
        }


        /// <summary>
        /// Add the given metadata fields to the WebRequest.
        /// </summary>
        /// <param name="webRequest">Web request to add the headers to.</param>
        /// <param name="metadata">A map of string to string representing the S3 metadata for this resource.</param>
        private void AddMetadataHeaders(WebRequest webRequest, SortedList metadata)
        {
            AddHeaders(webRequest, metadata, ThreeSharpUtils.METADATA_PREFIX);
        }


        /// <summary>
        /// Add the given headers to the WebRequest with a prefix before the keys.
        /// </summary>
        /// <param name="webRequest">WebRequest to add the headers to.</param>
        /// <param name="headers">Headers to add.</param>
        /// <param name="prefix">String to prepend to each before before adding it to the WebRequest</param>
        private void AddHeaders(WebRequest webRequest, SortedList headers, string prefix)
        {
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    if (prefix.Length == 0 && key.Equals("Content-Type"))
                    {
                        webRequest.ContentType = headers[key] as string;
                    }
                    else
                    {
                        webRequest.Headers.Add(prefix + key, headers[key] as string);
                    }
                }
            }
        }


        /// <summary>
        /// Add the appropriate Authorization header to the WebRequest
        /// </summary>
        private void AddAuthorizationHeader(WebRequest webRequest, string bucket, string key, SortedList query)
        {
            if (webRequest.Headers[ThreeSharpUtils.ALTERNATIVE_DATE_HEADER] == null)
            {
                webRequest.Headers.Add(ThreeSharpUtils.ALTERNATIVE_DATE_HEADER, ThreeSharpUtils.GetHttpDate());
            }

            string canonicalString = ThreeSharpUtils.MakeCanonicalString(bucket, key, query, webRequest);
            string encodedCanonical = ThreeSharpUtils.Encode(config.AwsSecretAccessKey, canonicalString, false);
            webRequest.Headers.Add(HttpRequestHeader.Authorization, "AWS " + config.AwsAccessKeyID + ":" + encodedCanonical);
        }


        /// <summary>
        /// Returns an array of Transfer objects, which contain statistics about a data transfer operation
        /// </summary>
        public Transfer[] GetTransfers()
        {
            Transfer[] transfers = null;
            lock (this.statistics)
            {
                transfers = this.statistics.GetTransfers();
            }
            return transfers;
        }

        /// <summary>
        /// Returns statistics about a single data transfer operation
        /// </summary>
        public Transfer GetTransfer(String id)
        {
            Transfer transfer = null;
            lock (this.statistics)
            {
                transfer = this.statistics.GetTransfer(id);
            }
            return transfer;
        }

        public long GetTotalBytesUploaded()
        {
            long count = 0;
            lock (this.statistics)
            {
                count = this.statistics.TotalBytesUploaded;
            }
            return count;
        }

        public long GetTotalBytesDownloaded()
        {
            long count = 0;
            lock (this.statistics)
            {
                count = this.statistics.TotalBytesDownloaded;
            }
            return count;
        }

    }
}
