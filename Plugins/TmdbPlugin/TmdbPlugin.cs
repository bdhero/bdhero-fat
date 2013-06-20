using System;
using System.Linq;
using System.Reflection;
using BDHero.Plugin;
using BDHero.JobQueue;
using DotNetUtils;
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
        private string _tmdbRootUrl;
        private string _iso6391;
        private int? year = null;
        private string _tmdbApiKey;

        #endregion
        
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
                throw ex;
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

            _iso6391 = settings.settings.defaultLanguage;
            _tmdbApiKey = settings.settings.apiKey;
        }

        #endregion

        #region Movie Search

        private void ApiRequest(Job job)
        {
            job.Movies.Clear();

<<<<<<< HEAD
            if (job.Disc.SanitizedTitle != null)
=======
            else
            {                
                var error = new Exception("Error: No APIKey was Found");
                var Message = "Error: No APIKey was Found for the Tmdb plugin";
                Logger.Error(Message);
            }            

            try
>>>>>>> bd3d15a6d0ab89eb70db6edf2e084bd5b0ed0628
            {
                var requestParameters = new TmdbApiParameters(job.Disc.SanitizedTitle, year, _iso6391);

                if (_tmdbApiKey != null)
                {
                    _tmdbApi = new Tmdb(_tmdbApiKey, _iso6391);
                }

                try
                {
                    SearchTmdb(requestParameters, job);
                }
                catch (Exception ex)
                {
                    TmdbErrors(ex);
                }
            }
            else
            {
                const string apiTitleMessage = "Error: The Tmdb plugin did not receive a searchable movie name";
                Logger.Error(apiTitleMessage);
                throw new Exception(apiTitleMessage);
            }
        }

        private void SearchTmdb(TmdbApiParameters requestParameters, Job job)
        {
            _tmdbMovieSearch = _tmdbApi.SearchMovie(requestParameters.Query, 1, requestParameters.Iso6391, false,
                                                    requestParameters.Year);

            if (string.IsNullOrEmpty(_tmdbRootUrl))
            {
                _tmdbRootUrl = _tmdbApi.GetConfiguration().images.base_url + "w185";
            }
            if (!string.IsNullOrEmpty(_tmdbRootUrl))
            {
                const string urlMessage = "api.themoviedb.org provided the following base url:";
                Logger.InfoFormat("{0} {1}", urlMessage, _tmdbRootUrl);
            }

            if (_tmdbMovieSearch != null)
            {
                job.Movies.AddRange(_tmdbMovieSearch.results.Select(ToMovie));
            }

            string results = null;
            if (_tmdbMovieSearch == null) return;
            foreach (var movie in _tmdbMovieSearch.results)
            {
                DateTime releaseYear;
                DateTime.TryParse(movie.release_date, out releaseYear);
                results += String.Format("{0} ({1}),", movie.original_title, releaseYear.Year);
            }
            const string bestGuessMessage = "Tmdb top search result:";
            Logger.InfoFormat("{0} {1} ", bestGuessMessage, _tmdbMovieSearch.results[0].original_title);
            const string matchesMessage = "The Tmdb plugin returned the following matches:";
            Logger.InfoFormat("{0} {1} ", matchesMessage, results);

        }

        private Movie ToMovie(MovieResult movieResult, int i)
        {
            DateTime releaseYear;
            if (!DateTime.TryParse(movieResult.release_date, out releaseYear))
                releaseYear = DateTime.MinValue;
            var movie = new Movie
<<<<<<< HEAD:TmdbPlugin/TmdbPlugin.cs
                            {
                                Id = movieResult.id,
                                ReleaseYear = releaseYear.Year,
                                Title = movieResult.title,
                                IsSelected = i == 0
                            };
            movie.CoverArtImages.Add(new CoverArtImage
                                         {
                                             Uri = _tmdbRootUrl + movieResult.poster_path,
                                             IsSelected = true
                                         });
=======
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
>>>>>>> 46e871ee1371fc80a90c1ca2203d7bb454906f46:Plugins/TmdbPlugin/TmdbPlugin.cs
            return movie;
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
<<<<<<< HEAD:TmdbPlugin/TmdbPlugin.cs
                    tmdbMovieImages = SearchPosters(movie);
=======
                    if (string.IsNullOrEmpty(_tmdbRootUrl))
                    {
                        _tmdbRootUrl = _tmdbApi.GetConfiguration().images.base_url + "original";
                    }
                    tmdbMovieImages = _tmdbApi.GetMovieImages(movie.Id, null);
                    var posterLanguages = (tmdbMovieImages.posters.Select(poster => poster.iso_639_1).ToList());
                    posterLanguages = posterLanguages.Distinct().ToList();

                    if (posterLanguages.Count == 0)
                    {
                        tmdbMovieImages = _tmdbApi.GetMovieImages(movie.Id, "en");
                    }
>>>>>>> 46e871ee1371fc80a90c1ca2203d7bb454906f46:Plugins/TmdbPlugin/TmdbPlugin.cs
                }
                catch (Exception ex)
                {
                    TmdbErrors(ex);
                }
                if (tmdbMovieImages == null) continue;
                foreach (var poster in tmdbMovieImages.posters)
                {
                    poster.file_path = _tmdbRootUrl + poster.file_path;

                    if (movie.CoverArtImages.All(x => x.Uri != poster.file_path))
                    {
<<<<<<< HEAD:TmdbPlugin/TmdbPlugin.cs
                        movie.CoverArtImages.Add(new CoverArtImage
                                                     {
                                                         Uri = poster.file_path,
                                                         Language = Language.FromCode(poster.iso_639_1)
                                                     }
                            );
=======
                        movie.CoverArtImages.Add(new CoverArt
                        {
                            Uri = _tmdbRootUrl + poster.file_path,
                            Language = I18N.Language.FromCode(poster.iso_639_1)
                        });
>>>>>>> 46e871ee1371fc80a90c1ca2203d7bb454906f46:Plugins/TmdbPlugin/TmdbPlugin.cs
                    }
                }
            }
        }

        private TmdbMovieImages SearchPosters(Movie movie)
        {
            if (string.IsNullOrEmpty(_tmdbRootUrl))
            {
                _tmdbRootUrl = _tmdbApi.GetConfiguration().images.base_url + "w185";
            }
            var tmdbMovieImages = _tmdbApi.GetMovieImages(movie.Id, null);

            if (tmdbMovieImages.posters.Count == 0)
            {
                tmdbMovieImages = _tmdbApi.GetMovieImages(movie.Id, "en");
            }
            return tmdbMovieImages;
        }

        #endregion

        #region Error Handling

        private void TmdbErrors(Exception ex)
        {
            var tmdbResponse = _tmdbApi.ResponseContent;
            try
            {
                var pluginSettings = JsonConvert.DeserializeObject<TmdbApiErrors>(tmdbResponse);
<<<<<<< HEAD:TmdbPlugin/TmdbPlugin.cs
<<<<<<< HEAD
                Logger.ErrorFormat("Error: api.themoviedb.org returned the following Status Code {0} : {1}",
                                   pluginSettings.StatusCode,
                                   pluginSettings.StatusMessage);
                throw ex;
            }
            catch
            {
                throw ex;
=======
=======
>>>>>>> 46e871ee1371fc80a90c1ca2203d7bb454906f46:Plugins/TmdbPlugin/TmdbPlugin.cs
                var message = "Error: api.themoviedb.org returned the following Status Code " +
                              pluginSettings.status_code + " : " + pluginSettings.status_message;
                throw new Exception(message, ex);
            }
            catch
            {
                throw new Exception("Unable to connect to api.themoviedb.org", ex);
<<<<<<< HEAD:TmdbPlugin/TmdbPlugin.cs
>>>>>>> bd3d15a6d0ab89eb70db6edf2e084bd5b0ed0628
=======
>>>>>>> 46e871ee1371fc80a90c1ca2203d7bb454906f46:Plugins/TmdbPlugin/TmdbPlugin.cs
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
