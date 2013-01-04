using System;
using System.Collections.Generic;
using System.Linq;
using BDAutoMuxer.BDInfo;
using BDAutoMuxer.models;
using Newtonsoft.Json;

namespace BDAutoMuxer.controllers
{
    static class MainMovieService
    {
        private const string BaseUri = "http://bd.andydvorak.net/api/v1/movies";

        private static string GetMainMovieUri(string volumeLabel, IEnumerable<TSPlaylistFile> mainPlaylists)
        {
            const string uri = BaseUri + "/main";

            var qs = new List<string> {"volume_label=" + Uri.EscapeUriString(volumeLabel)};
            qs.AddRange(mainPlaylists.Select(ToParam));

            return string.Format("{0}?{1}", uri, string.Join("&", qs));
        }

        private static string ToParam(TSPlaylistFile playlist)
        {
            return "playlist=" + playlist.Name + ";" + playlist.FileSize + ";" + (int) playlist.TotalLength;
        }

        public static JsonSearchResult FindMainMovie(string volumeLabel, IList<TSPlaylistFile> mainPlaylists)
        {
            var uri = GetMainMovieUri(volumeLabel, mainPlaylists);
            var responseText = HttpRequest.Get(uri);
            return JsonConvert.DeserializeObject<JsonSearchResult>(responseText);
        }

        public static JsonSearchResult PostDisc(JsonDisc jsonDisc)
        {
            var uri = BaseUri + "/main?api_key=" + Uri.EscapeUriString(BDAutoMuxerSettings.ApiKey);
            var jsonString = JsonConvert.SerializeObject(jsonDisc);
            var data = new Dictionary<string, string> {{"json", jsonString}};

            var responseText = HttpRequest.Post(uri, data);

            // TODO: Rename JsonSearchResult to something more generic.
            return JsonConvert.DeserializeObject<JsonSearchResult>(responseText);
        }
    }
}
