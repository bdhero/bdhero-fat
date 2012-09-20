using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Cache;
using System.IO;

namespace BDAutoMuxer.controllers
{
    public class HttpRequest
    {
        private static readonly string user_agent;

        static HttpRequest()
        {
            user_agent = BDAutoMuxerSettings.AssemblyName + "/" + BDAutoMuxerSettings.AssemblyVersionDisplay;
        }

        public static string Get(string uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.UserAgent = user_agent;
            request.KeepAlive = true;

            var cachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            request.CachePolicy = cachePolicy;
            request.Expect = null;

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

        public static string Post(string uri, IDictionary<string, string> data = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = user_agent;
            request.KeepAlive = true;

            var cachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            request.CachePolicy = cachePolicy;
            request.Expect = null;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string strData = "";
                if (data != null)
                {
                    foreach (string key in data.Keys)
                    {
                        strData += (strData.Length > 0 ? "&" : "") + Uri.EscapeUriString(key) + "=" + Uri.EscapeUriString(data[key]);
                    }
                }
                streamWriter.Write(strData + "\n");
                streamWriter.Flush();
                streamWriter.Close();
            }

            // This actually sends the request
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                return responseText;
            }
        }
    }
}
