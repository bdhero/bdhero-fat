using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using BDHero.Plugin;
using BDHero.JobQueue;
using DotNetUtils.Extensions;
using I18N;
using WatTmdb.V3;
using System.IO;
using Newtonsoft.Json;
using log4net;
using Formatting = Newtonsoft.Json.Formatting;

namespace TmdbPlugin
{
    public class TmdbPlugin : IMetadataProviderPlugin
    {
        #region Fields

        private static readonly ILog Logger =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Tmdb _tmdbApi;
        private TmdbMovieSearch _tmdbMovieSearch;
        private string _rootImageUrl;
        private string _searchISO_639_1;
        private int? _searchYear = null;
        private string _apiKey;
        private TmdbConfiguration _configuration;

        #endregion
        
        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "TMDb"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get { return Resources.tmdb_icon; } }

        public int RunOrder { get { return 1; } }

        public EditPluginPreferenceHandler EditPreferences { get; private set; }

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public void GetMetadata(CancellationToken cancellationToken, Job job)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            Host.ReportProgress(this, 0.0, "Loading config file...");

            var pluginSettings = CheckConfigFile();
            
            LoadConfig(pluginSettings);

            if (cancellationToken.IsCancellationRequested)
                return;

            Host.ReportProgress(this, 100.0 * 1.0 / 3.0, "Querying TMDb...");

            ApiRequest(job);

            if (cancellationToken.IsCancellationRequested)
                return;

            Host.ReportProgress(this, 100.0 * 2.0 / 3.0, "Getting poster images...");

            GetPosters(job);

            Host.ReportProgress(this, 100.0, "Finished querying TMDb");
        }

        #region Load Settings

        private PluginSettings CheckConfigFile()
        {
            if (!File.Exists(AssemblyInfo.SettingsFile))
            {
                var settings = new PluginSettings
                                   {
                                       settings = new Settings
                                                      {
                                                          apiKey = null,
                                                          defaultLanguage = "en",
                                                          url = "http://acdvorak.github.io/bdautomuxer/"
                                                      }
                                   };

                var settingJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
                if (settingJson != null)
                {
                    try
                    {
                        File.WriteAllText(AssemblyInfo.SettingsFile, settingJson);
                    }
                    catch (Exception)
                    {
                        const string apiMessage = "Error: An error occured trying to create the plugin settings file:";
                        Logger.ErrorFormat("{0} {1}", apiMessage, AssemblyInfo.SettingsFile);
                        throw;
                    }
                }
            }

            PluginSettings pluginSettings;
            try
            {
                var reader = File.ReadAllText(AssemblyInfo.SettingsFile);
                pluginSettings = JsonConvert.DeserializeObject<PluginSettings>(reader);
            }
            catch (Exception ex)
            {
                const string apiMessage = "Error: An error occured trying to load the plugin settings file:";
                Logger.ErrorFormat("{0} {1}", apiMessage, AssemblyInfo.SettingsFile);
                throw;
            }

            return pluginSettings;
        }

        private void LoadConfig(PluginSettings settings)
        {
            var newSettings = new PluginSettings {settings = new Settings()};
            if (settings.settings.apiKey == null)
            {
                settings.settings.apiKey = newSettings.settings.apiKey;
            }
            if (settings.settings.defaultLanguage == null)
            {
                settings.settings.defaultLanguage = newSettings.settings.defaultLanguage;
            }

            _searchISO_639_1 = settings.settings.defaultLanguage;
            _apiKey = settings.settings.apiKey;
        }

        #endregion

        #region TMDb Configuration

        private TmdbConfiguration GetConfiguration()
        {
            if (_configuration == null)
            {
                Logger.Debug("Getting TMDb configuration");
                _configuration = _tmdbApi.GetConfiguration();
            }
            return _configuration;
        }

        private void GetBaseImageUrl()
        {
            if (string.IsNullOrEmpty(_rootImageUrl))
            {
                _rootImageUrl = GetConfiguration().images.base_url + "w185";
            }

            if (string.IsNullOrEmpty(_rootImageUrl))
            {
                Logger.Warn("Failed to obtain base image URL");
                return;
            }

            const string urlMessage = "api.themoviedb.org provided the following base image URL";
            Logger.InfoFormat("{0}: {1}", urlMessage, _rootImageUrl);
        }

        #endregion

        #region Movie Search

        private void ApiRequest(Job job)
        {
            job.Movies.Clear();

            var searchQuery = job.SearchQuery;

            if (searchQuery != null && _apiKey != null)
            {
                _tmdbApi = new Tmdb(_apiKey, _searchISO_639_1);

                // TMDb (previously) choked on dashes - not sure if it still does or not...
                // E.G.: "The Amazing Spider-Man" --> "The Amazing Spider Man"
                searchQuery = Regex.Replace(searchQuery, @"-+", " ");

                var requestParameters = new TmdbApiParameters(searchQuery, _searchYear, _searchISO_639_1);

                try
                {
                    SearchTmdb(requestParameters, job);
                }
                catch (Exception ex)
                {
                    HandleTmdbError(ex);
                }
            }
            else
            {
                const string apiTitleMessage = "ERROR: No searchable movie name found";
                Logger.Error(apiTitleMessage);
                throw new Exception(apiTitleMessage);
            }
        }

        private void SearchTmdb(TmdbApiParameters requestParameters, Job job)
        {
            GetBaseImageUrl();

            _tmdbMovieSearch = _tmdbApi.SearchMovie(requestParameters.Query, 1, requestParameters.Iso6391, false,
                                                    requestParameters.Year);

            if (_tmdbMovieSearch == null)
            {
                Logger.Warn("TMDb movie search returned null");
                return;
            }

            job.Movies.AddRange(_tmdbMovieSearch.results.Select(ToMovie));

            LogSearchResults();
        }

        private void LogSearchResults()
        {
            if (_tmdbMovieSearch == null || !_tmdbMovieSearch.results.Any())
                return;

            var results = new List<string>();

            foreach (var movie in _tmdbMovieSearch.results)
            {
                DateTime releaseYear;
                DateTime.TryParse(movie.release_date, out releaseYear);
                results.Add(String.Format("{0} ({1})", movie.original_title, releaseYear.Year));
            }

            const string bestGuessMessage = "Top TMDb search result";
            Logger.InfoFormat("{0}: {1} ", bestGuessMessage, _tmdbMovieSearch.results[0].original_title);

            const string matchesMessage = "TMDb returned the following matches";
            Logger.InfoFormat("{0}:\n{1} ", matchesMessage, string.Join(Environment.NewLine, results));
        }

        private Movie ToMovie(MovieResult movieResult, int i)
        {
            var releaseYear = GetReleaseYear(movieResult);
            var movie = new Movie
                {
                    Id = movieResult.id,
                    ReleaseYear = releaseYear,
                    Title = movieResult.title,
                    Url = string.Format("http://www.themoviedb.org/movie/{0}", movieResult.id),
                    IsSelected = i == 0
                };
            movie.CoverArtImages.Add(new CoverArt
                {
                    Uri = _rootImageUrl + movieResult.poster_path,
                    IsSelected = true
                });
            return movie;
        }

        private static int? GetReleaseYear(MovieResult movieResult)
        {
            int? releaseYear = null;
            DateTime releaseDate;
            if (DateTime.TryParse(movieResult.release_date, out releaseDate))
                releaseYear = releaseDate.Year;
            return releaseYear;
        }

        #endregion

        #region Poster Search

        private void GetPosters(Job job)
        {
            foreach (var movie in job.Movies)
            {
                var tmdbMovieImages = new TmdbMovieImages();

                try
                {
                    if (string.IsNullOrEmpty(_rootImageUrl))
                    {
                        _rootImageUrl = GetConfiguration().images.base_url + "original";
                    }
                    tmdbMovieImages = _tmdbApi.GetMovieImages(movie.Id, null);
                    var posterLanguages = (tmdbMovieImages.posters.Select(poster => poster.iso_639_1).ToList());
                    posterLanguages = posterLanguages.Distinct().ToList();

                    if (posterLanguages.Count == 0)
                    {
                        tmdbMovieImages = _tmdbApi.GetMovieImages(movie.Id, "en");
                    }
                }
                catch (Exception ex)
                {
                    HandleTmdbError(ex);
                }

                if (tmdbMovieImages == null)
                    continue;

                foreach (var poster in tmdbMovieImages.posters)
                {
                    poster.file_path = _rootImageUrl + poster.file_path;

                    if (movie.CoverArtImages.All(x => x.Uri != poster.file_path))
                    {
                        movie.CoverArtImages.Add(new CoverArt
                        {
                            Uri = _rootImageUrl + poster.file_path,
                            Language = Language.FromCode(poster.iso_639_1)
                        });
                    }
                }
            }
        }

        #endregion

        #region Error Handling

        private void HandleTmdbError(Exception ex)
        {
            var tmdbResponse = _tmdbApi.ResponseContent;
            try
            {
                var pluginSettings = JsonConvert.DeserializeObject<TmdbApiErrors>(tmdbResponse);
                Logger.ErrorFormat("Error: api.themoviedb.org returned the following Status Code {0} : {1}",
                                   pluginSettings.StatusCode,
                                   pluginSettings.StatusMessage);
            }
            catch
            {
            }
        }

        #endregion

        #region Private Classes

        private class TmdbApiParameters
        {
            public readonly string Query;
            public readonly int? Year;
            public readonly string Iso6391;

            public TmdbApiParameters(string query, int? year, string iso6391)
            {
                Query = query;
                Year = year;
                Iso6391 = iso6391;
            }
        }

        private abstract class TmdbApiErrors
        {
            public int StatusCode { get; set; }
            public string StatusMessage { get; set; }
        }

        #endregion
    }
}
