using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Cache;
using System.IO;
using Newtonsoft.Json;
using BDInfo.models;

namespace BDInfo.controllers
{
    class MainMovieService
    {
        private static readonly string base_uri = "http://bd.andydvorak.net/api/v1";

        public MainMovieService()
        {

        }

        private string GetMainMovieUri(string volume_label, IList<TSPlaylistFile> mainPlaylists)
        {
            string uri = base_uri + "/mainMovie";
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
            request.UserAgent = "BDAutoRip/0.0.6";
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
    }
}
