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
            user_agent = BDAutoMuxerSettings.AssemblyName + "/" + BDAutoMuxerSettings.AssemblyVersionDisplay;
        }

        private string GetMainMovieUri(string volume_label, IList<TSPlaylistFile> mainPlaylists)
        {
            string uri = base_uri + "/main";
            uri += "?volume_label=" + Uri.EscapeUriString(volume_label);
            foreach(TSPlaylistFile playlist in mainPlaylists)
            {
                uri += "&playlist=" + playlist.Name + ";" + playlist.FileSize + ";" + (int)playlist.TotalLength;
            }
            return uri;
        }

        public JsonSearchResult FindMainMovie(string volume_label, IList<TSPlaylistFile> mainPlaylists)
        {
            string uri = GetMainMovieUri(volume_label, mainPlaylists);
            string responseText = HttpRequest.Get(uri);
            JsonSearchResult searchResult = JsonConvert.DeserializeObject<JsonSearchResult>(responseText);
            return searchResult;
        }

        public JsonSearchResult PostDisc(JsonDisc jsonDisc)
        {
            string uri = base_uri + "/main?api_key=" + Uri.EscapeUriString(BDAutoMuxerSettings.ApiKey);
            string jsonString = JsonConvert.SerializeObject(jsonDisc);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("json", jsonString);

            string responseText = HttpRequest.Post(uri, data);

            // TODO: Rename JsonSearchResult to something more generic.
            JsonSearchResult postResult = JsonConvert.DeserializeObject<JsonSearchResult>(responseText);
            return postResult;
        }
    }
}
