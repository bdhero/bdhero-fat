using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Runtime.Caching;
using DotNetUtils.Annotations;

namespace DotNetUtils
{
    /// <summary>
    /// Helper utility class for making HTTP requests and parsing/caching the responses.
    /// </summary>
    public static class HttpRequest
    {
        private static readonly log4net.ILog Logger =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string METHOD_GET = "GET";
        private const string METHOD_PUT = "PUT";
        private const string METHOD_POST = "POST";

        /// <summary>
        /// Gets or sets the User-Agent HTTP request header sent to the web server when making requests.
        /// Defaults to <c>"ENTRY_ASSEMBLY_NAME/ENTRY_ASSEMBLY_VERSION"</c>.
        /// </summary>
        public static string UserAgent;

        private static readonly MemoryCache ImageCache = new MemoryCache("http_image_cache");

        static HttpRequest()
        {
            UserAgent = AssemblyUtils.GetAssemblyName() + "/" + AssemblyUtils.GetAssemblyVersion();
        }

        /// <summary>
        /// Performs a synchronous HTTP GET request and returns the full response as a string.
        /// </summary>
        /// <param name="uri">URI of the web resource to GET</param>
        /// <param name="headers">Optional list of fully-formatted headers of the form <c>Header-Name: Header-Value</c></param>
        /// <returns></returns>
        public static string Get(string uri, List<string> headers = null)
        {
            var request = BuildRequest(METHOD_GET, uri, false, headers);
            using (var httpResponse = request.GetResponse())
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        #region Images

        /// <summary>
        /// Performs a synchronous HTTP GET request and returns the full response as an Image.
        /// </summary>
        /// <param name="uri">URI of the web resource to GET</param>
        /// <param name="cache">Determines whether a successful response should be cached to speed up future requests for the same <paramref name="uri"/></param>
        /// <returns></returns>
        [CanBeNull]
        public static Image GetImage(string uri, bool cache = true)
        {
            return cache ? GetImageCached(uri) : GetImageNoCache(uri);
        }

        [CanBeNull]
        private static Image GetImageNoCache(string uri)
        {
            var request = BuildRequest(METHOD_GET, uri, cache: true);
            using (var httpResponse = request.GetResponse())
            {
                using (var stream = httpResponse.GetResponseStream())
                {
                    return stream != null ? Image.FromStream(stream) : null;
                }
            }
        }

        [CanBeNull]
        private static Image GetImageCached([NotNull] string url)
        {
            return ImageCache.Contains(url) ? ImageCache[url] as Image : FetchAndCacheImage(url);
        }

        [CanBeNull]
        private static Image FetchAndCacheImage([NotNull] string url)
        {
            var image = GetImageNoCache(url);
            ImageCache.Set(url, image, new CacheItemPolicy());
            return image;
        }

        #endregion

        /// <summary>
        /// Performs a synchronous HTTP POST request and returns the full response as a string.
        /// </summary>
        /// <param name="uri">URI of the web resource to POST to</param>
        /// <param name="formData">Optional form data to send to the server in the request body</param>
        /// <returns></returns>
        public static string Post(string uri, IDictionary<string, string> formData = null)
        {
            return PutOrPost(METHOD_POST, uri, formData);
        }

        /// <summary>
        /// Performs a synchronous HTTP PUT request and returns the full response as a string.
        /// </summary>
        /// <param name="uri">URI of the web resource to POST to</param>
        /// <param name="formData">Optional form data to send to the server in the request body</param>
        /// <returns></returns>
        public static string Put(string uri, IDictionary<string, string> formData = null)
        {
            return PutOrPost(METHOD_PUT, uri, formData);
        }

        private static string PutOrPost(string method, string uri, IDictionary<string, string> formData)
        {
            var request = BuildRequest(method, uri);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var body = new List<string>();
                if (formData != null)
                {
                    body.AddRange(formData.Keys.Select(key => EncodeForPostBody(key, formData[key])));
                }
                streamWriter.Write(string.Join("&", body) + "\n");
                streamWriter.Flush();
                streamWriter.Close();
            }

            // This actually sends the request
            var httpResponse = request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                return responseText;
            }
        }

        private static HttpWebRequest BuildRequest(string method, string uri, bool cache = false, List<string> headers = null)
        {
            var strHeaders = (headers != null && headers.Count > 0 ? string.Format(" with headers: {0}", string.Join("; ", headers)) : "");
            Logger.DebugFormat("{0} {1} -- cache = {2}{3}", method, uri, cache, strHeaders);

            var request = (HttpWebRequest)WebRequest.Create(uri);

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header);
                }
            }

            request.Method = method;
            request.UserAgent = UserAgent;
            request.KeepAlive = true;
            request.CachePolicy = new RequestCachePolicy(cache ? RequestCacheLevel.CacheIfAvailable : RequestCacheLevel.BypassCache);
            request.Expect = null;

            if (METHOD_POST == method || METHOD_PUT == method)
                request.ContentType = "application/x-www-form-urlencoded";

            return request;
        }

        private static string EncodeForPostBody(string key, string value)
        {
            return Uri.EscapeUriString(key) + "=" + Uri.EscapeUriString(value);
        }
    }
}
