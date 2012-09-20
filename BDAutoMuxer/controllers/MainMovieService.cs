using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Cache;
using System.IO;
using Newtonsoft.Json;
using BDAutoMuxer.models;
using System.Reflection;

namespace BDAutoMuxer.controllers
{
    class MainMovieService
    {
        private readonly string base_uri = "http://bd.andydvorak.net/api/v1/movies";
        private readonly string user_agent;

        public MainMovieService()
        {
            user_agent = BDAutoMuxerSettings.AssemblyName + "/" + BDAutoMuxerSettings.AssemblyVersion;
        }

        private string GetMainMovieUri(string volume_label, IList<TSPlaylistFile> mainPlaylists)
        {
            string uri = base_uri + "/main";
            uri += "?volume_label=" + Uri.EscapeUriString(volume_label);
            foreach(TSPlaylistFile playlist in mainPlaylists)
            {
                uri += "&playlist=" + playlist.Name + ";" + playlist.FileSize + ";" + (int) playlist.TotalLength;
            }
            return uri;
        }

        public JsonSearchResult FindMainMovie(string volume_label, IList<TSPlaylistFile> mainPlaylists)
        {
            string uri = GetMainMovieUri(volume_label, mainPlaylists);

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
                var responseText = streamReader.ReadToEnd();
                JsonSearchResult searchResult = JsonConvert.DeserializeObject<JsonSearchResult>(responseText);
                return searchResult;
            }
        }

        public JsonSearchResult PostDisc(JsonDisc jsonDisc)
        {
            string jsonString = JsonConvert.SerializeObject(jsonDisc);
            string uri = base_uri + "/main?api_key=" + Uri.EscapeUriString(BDAutoMuxerSettings.ApiKey);

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
                streamWriter.Write("json=" + Uri.EscapeUriString(jsonString) + "\n");
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                // This actually sends the request
                var responseText = streamReader.ReadToEnd();

                // TODO: Deserializing throws an exception.  Find out why.
                // TODO: Rename JsonSearchResult to something more generic.
                // I know this isn't actually "searching" the DB, but the response format is nearly identical.
                JsonSearchResult postResult = JsonConvert.DeserializeObject<JsonSearchResult>(responseText);
                return postResult;
            }
        }
    }
}
