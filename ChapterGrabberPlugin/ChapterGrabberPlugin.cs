using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;
using System.Text.RegularExpressions;



namespace ChapterGrabberPlugin
{
    public class ChapterGrabberPlugin
    {
        static void Main(string[] args)
        {
            var lisT = GetChapters("Dark Knight");
            var apiValues = CompareChapters(lisT);
            if (apiValues != null && apiValues.Count > 0)
            {
                ReplaceChapters(apiValues);
            }
        }

        private const string _apiGet = "http://chapterdb.org/chapters/search?title=";
        private const string _apiGetEnd = "&chapterCount=0 HTTP/1.1";

        static private List<JsonChaps> GetChapters(string movieName)
        {
            var movieSearchResults = new List<JsonChaps>();
            var headers = new List<string>();
            headers.Add("ApiKey: G88IO875M9SKU6DPB82F");
            headers.Add("UserName: ChapterGrabber");
            var xmlResponse = HttpRequest.Get(_apiGet + movieName + _apiGetEnd, headers);
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

        static private List<JsonChaps> CompareChapters(List<JsonChaps> apiData)
        {
            #region test data

            List<TimeSpan> _chapterDisc = new List<TimeSpan>();
            _chapterDisc.Add(TimeSpan.Parse("00:00:00.000"));
            _chapterDisc.Add(TimeSpan.Parse("00:06:23.966"));
            _chapterDisc.Add(TimeSpan.Parse("00:10:36.260"));
            _chapterDisc.Add(TimeSpan.Parse("00:13:52.581"));
            _chapterDisc.Add(TimeSpan.Parse("00:17:54.198"));
            _chapterDisc.Add(TimeSpan.Parse("00:21:39.339"));
            _chapterDisc.Add(TimeSpan.Parse("00:26:10.026"));
            _chapterDisc.Add(TimeSpan.Parse("00:29:36.274"));
            _chapterDisc.Add(TimeSpan.Parse("00:31:42.483"));
            _chapterDisc.Add(TimeSpan.Parse("00:37:49.058"));
            _chapterDisc.Add(TimeSpan.Parse("00:43:34.069"));
            _chapterDisc.Add(TimeSpan.Parse("00:46:39.546"));
            _chapterDisc.Add(TimeSpan.Parse("00:48:57.768"));
            _chapterDisc.Add(TimeSpan.Parse("00:53:19.446"));
            _chapterDisc.Add(TimeSpan.Parse("00:57:15.640"));
            _chapterDisc.Add(TimeSpan.Parse("00:59:26.354"));
            _chapterDisc.Add(TimeSpan.Parse("01:03:59.627"));
            _chapterDisc.Add(TimeSpan.Parse("01:08:23.557"));
            _chapterDisc.Add(TimeSpan.Parse("01:10:48.327"));
            _chapterDisc.Add(TimeSpan.Parse("01:14:24.918"));
            _chapterDisc.Add(TimeSpan.Parse("01:18:05.639"));
            _chapterDisc.Add(TimeSpan.Parse("01:22:22.437"));
            _chapterDisc.Add(TimeSpan.Parse("01:24:48.583"));
            _chapterDisc.Add(TimeSpan.Parse("01:30:48.442"));
            _chapterDisc.Add(TimeSpan.Parse("01:34:04.972"));
            _chapterDisc.Add(TimeSpan.Parse("01:39:57.783"));
            _chapterDisc.Add(TimeSpan.Parse("01:42:05.494"));
            _chapterDisc.Add(TimeSpan.Parse("01:44:59.918"));
            _chapterDisc.Add(TimeSpan.Parse("01:47:00.872"));
            _chapterDisc.Add(TimeSpan.Parse("01:51:09.913"));
            _chapterDisc.Add(TimeSpan.Parse("01:54:14.639"));
            _chapterDisc.Add(TimeSpan.Parse("01:57:04.017"));
            _chapterDisc.Add(TimeSpan.Parse("02:00:14.248"));
            _chapterDisc.Add(TimeSpan.Parse("02:03:37.576"));
            _chapterDisc.Add(TimeSpan.Parse("02:09:07.614"));
            _chapterDisc.Add(TimeSpan.Parse("02:12:27.314"));
            _chapterDisc.Add(TimeSpan.Parse("02:15:22.781"));
            _chapterDisc.Add(TimeSpan.Parse("02:19:58.681"));
            _chapterDisc.Add(TimeSpan.Parse("02:24:26.658"));

            #endregion
            //To Do: Replace _chapterDisc with Disc.Chapters from Disc Object in BD Hero Core
            var apiResultsFilteredByChapter = apiData.Where(chaps => IsMatch(chaps, _chapterDisc)).ToList();

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

        private static bool IsMatch(JsonChaps jsonChaps, List<TimeSpan> chapterDisc)
        {
            var chapterCountMatches = jsonChaps.chapterInfo.chapters.chapter.Count == chapterDisc.Count ||
                                      jsonChaps.chapterInfo.chapters.chapter.Count == chapterDisc.Count + 1;
            if (!chapterCountMatches)
                return false;

            for (var i = 0; i < chapterDisc.Count; i++)
            {
                var discChapter = chapterDisc[i];
                var apiChapter = jsonChaps.chapterInfo.chapters.chapter[i];
                if ((int)discChapter.TotalSeconds != (int)apiChapter.time.TotalSeconds)
                    return false;
            }
            return true;
        }

        static private void ReplaceChapters(List<JsonChaps> apiData)
        {
            /*  Sudo for replacing the Default Chapter Names in the 
            for (var i=0; i<Disc.Chapters.Count; i++)
            {
                Disc.Chapters[i].Title       
            }
             */

        }




    }
}
