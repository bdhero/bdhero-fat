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
    public static class HttpRequest
    {
        private const string METHOD_GET = "GET";
        private const string METHOD_PUT = "PUT";
        private const string METHOD_POST = "POST";

        private static readonly string UserAgent;

        private static readonly MemoryCache ImageCache = new MemoryCache("http_image_cache");

        static HttpRequest()
        {
            UserAgent = AssemblyUtils.GetAssemblyName(typeof(HttpRequest)) + "/" + AssemblyUtils.GetAssemblyVersion(typeof(HttpRequest));
        }

        public static string Get(string uri)
        {
            var request = BuildRequest(METHOD_GET, uri);
            using (var httpResponse = (HttpWebResponse) request.GetResponse())
            {
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        #region Images

        [CanBeNull]
        public static Image GetImage(string uri, bool cache = true)
        {
            return cache ? GetImageCached(uri) : GetImageNoCache(uri);
        }

        [CanBeNull]
        private static Image GetImageNoCache(string uri)
        {
            var request = BuildRequest(METHOD_GET, uri, cache: true);
            using (var httpResponse = (HttpWebResponse) request.GetResponse())
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
            var image = GetImage(url, cache: false);
            ImageCache.Set(url, image, new CacheItemPolicy());
            return image;
        }

        #endregion

        public static string Post(string uri, IDictionary<string, string> formData = null)
        {
            return PutOrPost(METHOD_POST, uri, formData);
        }

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
            var httpResponse = (HttpWebResponse) request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                return responseText;
            }
        }

        private static HttpWebRequest BuildRequest(string method, string uri, bool cache = false)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);

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
