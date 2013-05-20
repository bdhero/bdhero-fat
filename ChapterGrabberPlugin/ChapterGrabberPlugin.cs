﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BDHero.Plugin;
using BDHero.Queue;
using DotNetUtils;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
using System.Text.RegularExpressions;



namespace ChapterGrabberPlugin
{
    public class ChapterGrabberPlugin : INameProviderPlugin
    {
        private const string ApiGet = "http://chapterdb.org/chapters/search?title=";
        private const string ApiGetEnd = "&chapterCount=0 HTTP/1.1";

        public IPluginHost Host { get; set; }
        public string Name { get; set; }
        public void LoadPlugin()
        {

        }

        public void UnloadPlugin()
        {

        }

        public event EditPluginPreferenceHandler EditPreferences;
        public void Rename(Job job)
        {
            var playlist = job.Disc.Playlists[job.PlaylistIndex];

            var apiResults = GetChapters(job.Disc.MovieTitle);

            var apiValues = CompareChapters(apiResults, playlist.Chapters);
            if (apiValues != null && apiValues.Count > 0)
            {
                ReplaceChapters(apiValues, playlist.Chapters);
            }
        }

        static private List<JsonChaps> GetChapters(string movieName)
        {
            var movieSearchResults = new List<JsonChaps>();
            var headers = new List<string>();
            headers.Add("ApiKey: G88IO875M9SKU6DPB82F");
            headers.Add("UserName: ChapterGrabber");
            var xmlResponse = HttpRequest.Get(ApiGet + movieName + ApiGetEnd, headers);
            var doc = new XmlDocument();

            doc.LoadXml(xmlResponse);

            if (doc.DocumentElement != null)
            {
                foreach (var node in doc.DocumentElement)
                {
                    string jsonText;
                    try
                    {
                        jsonText = Regex.Replace(JsonConvert.SerializeXmlNode((XmlElement)node, Formatting.Indented),
                                                "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
                        jsonText = jsonText.Replace("?xml", "xml");

                        movieSearchResults.Add(JsonConvert.DeserializeObject<JsonChaps>(jsonText));
                    }
                    catch
                    {
                        Console.WriteLine("Error: One or more of the Results were rejected for containing bad data");
                    }
                }
            }
            if (movieSearchResults.Count > 0)
            {
                Console.WriteLine(movieSearchResults.Count + " matches were found for " + movieName);
            }
            else
            {
                Console.WriteLine("No matches were found for " + movieName);
            }
            return movieSearchResults;
        }

        static private List<JsonChaps> CompareChapters(List<JsonChaps> apiData, IList<BDHero.BDROM.Chapter> discData)
        {
            var apiResultsFilteredByChapter = apiData.Where(chaps => IsMatch(chaps, discData)).ToList();
            
            var apiResultsFilteredByValidName = apiResultsFilteredByChapter.Where(IsValid).ToList();
            return apiResultsFilteredByValidName;
        }

        private static bool IsValid(JsonChaps jsonChaps)
        {
            List<JsonChapter> jsonChapters = jsonChaps.chapterInfo.chapters.chapter;
            var isInvalid = jsonChapters.All(IsInvalidChapter);
            return !isInvalid;
        }

        private static bool IsInvalidChapter(JsonChapter jsonChapter)
        {
            var trimmed = jsonChapter.name.Trim();
            TimeSpan parsed;
            var timespan = TimeSpan.TryParse(trimmed, out parsed);
            if (timespan)
                return true;
            if (Regex.IsMatch(trimmed, @"^(Chapter\s*)?[0-9]+$", RegexOptions.IgnoreCase))
                return true;
            return false;
        }

        private static bool IsMatch(JsonChaps jsonChaps, IList<BDHero.BDROM.Chapter> chapterDisc)
        {
            var chapterCountMatches = jsonChaps.chapterInfo.chapters.chapter.Count == chapterDisc.Count ||
                                      jsonChaps.chapterInfo.chapters.chapter.Count == chapterDisc.Count + 1;
            if (!chapterCountMatches)
                return false;

            for (var i = 0; i < chapterDisc.Count; i++)
            {
                var discChapter = chapterDisc[i];
                var apiChapter = jsonChaps.chapterInfo.chapters.chapter[i];
                if ((int)discChapter.StartTime.TotalSeconds != (int)apiChapter.time.TotalSeconds)
                    return false;
            }
            return true;
        }

        static private void ReplaceChapters(List<JsonChaps> apiData, IList<BDHero.BDROM.Chapter> discData )
        {
            // Sudo for replacing the Default Chapter Names in the 
            for (var i=0; i<discData.Count; i++)
            {
                discData[i].Title = apiData[0].chapterInfo.chapters.chapter[i].name;
            }
        }
    }
}
