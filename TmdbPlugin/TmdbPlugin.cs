using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BDHero.Plugin;
using BDHero.JobQueue;
using DotNetUtils;
using WatTmdb.V3;
using System.IO;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace TmdbPlugin
{
    public class TmdbPlugin : IMetadataProviderPlugin
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Tmdb _tmdbApi;
        private TmdbMovieSearch _tmdbMovieSearch;
        private MovieResult _tmdbMovieResult;
        private string _tmdbMovieName;
        private int? _tmdbMovieYear;
        private int _tmdbID;
        private string _tmdbRootUrl;
        string ISO_639_1;
        int? year = null;
        private string TmdbApiKey;

        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "TMDb"; } }

        public event EditPluginPreferenceHandler EditPreferences;

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public void GetMetadata(Job job)
        {
            Host.ReportProgress(this, 0.0, "Loading config file...");

            var pluginSettings = CheckConfigFile();

            LoadConfig(pluginSettings);

            Host.ReportProgress(this, 100.0 * 1.0 / 3.0, "Querying TMDb...");

            ApiRequest(job);

            Host.ReportProgress(this, 100.0 * 2.0 / 3.0, "Getting poster images...");

            GetPosters(job);

            Host.ReportProgress(this, 100.0, "Finished querying TMDb");
        }

        private PluginSettings CheckConfigFile()
        {
            if (!File.Exists(AssemblyInfo.SettingsFile))
            {     
                PluginSettings settings = new PluginSettings
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
                    File.WriteAllText(AssemblyInfo.SettingsFile, settingJson);
                }
            }

            var reader = File.ReadAllText(AssemblyInfo.SettingsFile);
            PluginSettings pluginSettings = JsonConvert.DeserializeObject<PluginSettings>(reader);
            return pluginSettings;
        }

        private void LoadConfig(PluginSettings settings)
        {
            PluginSettings newSettings = new PluginSettings { settings = new Settings { } };
            if (settings.settings.apiKey == null)
            {
                settings.settings.apiKey = newSettings.settings.apiKey;
            }
            if (settings.settings.defaultLanguage == null)
            {
                settings.settings.defaultLanguage = newSettings.settings.defaultLanguage;
            }
            
            ISO_639_1 = settings.settings.defaultLanguage;
            TmdbApiKey = settings.settings.apiKey;
        }

        private void ApiRequest(Job job)
        {
            job.Movies.Clear();
            
            TmdbApiParameters requestParameters = new TmdbApiParameters(job.Disc.SanitizedTitle, year, ISO_639_1);
                
            if(TmdbApiKey != null)
            {
                _tmdbApi = new Tmdb(TmdbApiKey, ISO_639_1);
            }

            else
            {                
                var error = new Exception("Error: No APIKey was Found");
                var Message = "Error: No APIKey was Found for the Tmdb plugin";
                Logger.Error(Message);
            }            

            try
            {
                _tmdbMovieSearch = _tmdbApi.SearchMovie(requestParameters.Query, 1, requestParameters.ISO_639_1, false, requestParameters.Year);
                
                if (string.IsNullOrEmpty(_tmdbRootUrl))
                {
                    _tmdbRootUrl = _tmdbApi.GetConfiguration().images.base_url + "w185";
                }

                if (_tmdbMovieSearch != null)
                {
                    job.Movies.AddRange(_tmdbMovieSearch.results.Select(ToMovie));
                }

                string results = null;
                foreach (var movie in _tmdbMovieSearch.results)
                {
                    DateTime releaseYear;
                    var releaseDate = DateTime.TryParse(movie.release_date, out releaseYear);
                    results += movie.original_title + " (" + releaseYear.Year + ')' + ", ";
                }

                var Message = "The Tmdb plugin returned the following matches " + results;
                Logger.Info(Message);
            }
            catch (Exception ex)
            {
                tmdbErrors(ex);
            }            
        }

        private Movie ToMovie(MovieResult movieResult, int i)
        {
            DateTime releaseYear;
            if (!DateTime.TryParse(movieResult.release_date, out releaseYear))
                releaseYear = DateTime.MinValue;
            var movie = new Movie
                {
                    Id = movieResult.id,
                    ReleaseYear = releaseYear.Year,
                    Title = movieResult.title,
                    IsSelected = i == 0
                };
            movie.CoverArtImages.Add(new CoverArt
                {
                    Uri = _tmdbRootUrl + movieResult.poster_path,
                    IsSelected = true
                });
            return movie;
        }

        private void GetPosters(Job job)
        {
            foreach (var movie in job.Movies)
            {
                var tmdbMovieImages = new TmdbMovieImages();
                try
                {
                    if (string.IsNullOrEmpty(_tmdbRootUrl))
                    {
                        _tmdbRootUrl = _tmdbApi.GetConfiguration().images.base_url + "w185";
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
                    tmdbErrors(ex);
                }
                if (tmdbMovieImages != null)
                {
                    foreach (var poster in tmdbMovieImages.posters)
                    {
                        movie.CoverArtImages.Add(new CoverArt
                        {
                            Uri = _tmdbRootUrl + poster.file_path,
                            Language = I18N.Language.FromCode(poster.iso_639_1)
                        });
                    }
                }
            }            
        }

        private void tmdbErrors(Exception ex)
        {
            var tmdbResponse = _tmdbApi.ResponseContent;
            try
            {
                var pluginSettings = JsonConvert.DeserializeObject<TmdbApiErrors>(tmdbResponse);
                var message = "Error: api.themoviedb.org returned the following Status Code " +
                              pluginSettings.status_code + " : " + pluginSettings.status_message;
                throw new Exception(message, ex);
            }
            catch
            {
                throw new Exception("Unable to connect to api.themoviedb.org", ex);
            }
        }

        private class TmdbApiParameters
        {
            public readonly string Query;
            public readonly int? Year;
            public readonly string ISO_639_1;
            public TmdbApiParameters(string query, int? year, string ISO_639_1)
            {
                Query = query;
                Year = year;
                this.ISO_639_1 = ISO_639_1;
            }
        }

        private class TmdbApiErrors
        {
            public int status_code { get; set; }
            public string status_message { get; set; }
        }

    }
}
