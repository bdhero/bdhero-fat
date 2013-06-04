using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using BDHero.Plugin;
using BDHero.JobQueue;
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
        private static string xmlResponse;

        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "ChapterGrabber"; } }

        public event PluginProgressHandler ProgressUpdated;
        public event EditPluginPreferenceHandler EditPreferences;

        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }


        public void Rename(Job job)
        {
            //var playlist = job.Disc.Playlists[job.SelectedPlaylistIndex];
            if (job.Disc.SanitizedTitle != null)
            {  
                var apiResults = GetChapters(job.Disc.SanitizedTitle);
                /*
                var apiValues = CompareChapters(apiResults, playlist.Chapters);
                if (apiValues != null && apiValues.Count > 0)
                {
                    ReplaceChapters(apiValues[0], playlist.Chapters);
                }*/

                foreach(var moviePlaylist in job.Disc.Playlists)
                {
                    var apiValues = CompareChapters(apiResults, moviePlaylist.Chapters);
                    if (apiValues != null && apiValues.Count > 0)
                    {
                        // To Do:  Allow the user to select which chapter list to use when 
                        // defaulted to [0] first chapter list that matches filter criteria
                        ReplaceChapters(apiValues[0], moviePlaylist.Chapters);

                        var Message = "Custom chapters were added to: " + moviePlaylist.FileName;
                        Logger.Info(Message);
                    }
                }    
            }
        }

        static private List<JsonChaps> GetChapters(string movieName)
        {
            var movieSearchResults = new List<JsonChaps>();
            var headers = new List<string>();
                headers.Add("ApiKey: G88IO875M9SKU6DPB82F");
                headers.Add("UserName: BDHero");
            var doc = new XmlDocument();
            
            try
            {
                xmlResponse = HttpRequest.Get(ApiGet + movieName + ApiGetEnd, headers);
            }
            catch (Exception ex)
            {
                var error = new PluginException("Error: An error occurred when contacting chapterdb.org", ex, PluginExceptionSeverity.Error);
            }

            try
            {
                doc.LoadXml(xmlResponse);
            }
            catch (Exception ex)
            {
                var error = new PluginException("Error: An error occurred when processing the response from chapterdb.org", ex, PluginExceptionSeverity.Error);
            }

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
                    catch (Exception ex)
                    {
                        var error = new PluginException("Error: An error occurred when serializing the response from chapterdb.org", ex, PluginExceptionSeverity.Error);
                    }
                }
            }
           
            if (movieSearchResults.Count > 0)
            {               
                var Message = movieSearchResults.Count + " possible matches were found for " + movieName;
                Logger.Info(Message);
            }
            else
            {
                var Message = "No matches were found for " + movieName;
                Logger.Info(Message);
            }
            return movieSearchResults;
        }

        static private List<JsonChaps> CompareChapters(List<JsonChaps> apiData, IList<BDHero.BDROM.Chapter> discData)
        {
            var apiResultsFilteredByChapter = apiData.Where(chaps => IsMatch(chaps, discData)).ToList();
            
            var apiResultsFilteredByValidName = apiResultsFilteredByChapter.Where(IsValid).ToList();

            if (apiResultsFilteredByValidName.Count > 0)
            {
                var Message = apiResultsFilteredByValidName.Count + " result(s) matched the filter criteria for custom chapters";
                Logger.Info(Message);
            }

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
                if (!DoTimecodesMatch(discChapter.StartTime.TotalSeconds, apiChapter.time.TotalSeconds))
                    return false;
            }
            return true;
        }

        private static bool DoTimecodesMatch(double timeDisc, double timeApi)
        {
            var test = Math.Abs(timeDisc - timeApi);
            return Math.Abs(timeDisc - timeApi) <= 1.0;
        }

        static private void ReplaceChapters(JsonChaps apiData, IList<BDHero.BDROM.Chapter> discData )
        {
            for (var i=0; i<discData.Count; i++)
            {
                discData[i].Title = apiData.chapterInfo.chapters.chapter[i].name;
            }
        }
    }
}
