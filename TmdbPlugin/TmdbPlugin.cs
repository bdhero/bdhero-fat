using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BDHero.BDROM;
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
        private const string TmdbApiKey = "b59b366b0f0a457d58995537d847409a";
        private Tmdb _tmdbApi;
        private TmdbMovieSearch _tmdbMovieSearch;
        private MovieResult _tmdbMovieResult;
        private string _tmdbMovieName;
        private int? _tmdbMovieYear;
        private int _tmdbID;
        private string _tmdbRootUrl;

        public IPluginHost Host { get; set; }
        public string Location { get; private set; }
        public string Guid { get; private set; }
        public string Name 
        {
            get { return "TmdbPlugin"; }
        }

        public event PluginProgressHandler ProgressUpdated;
        public event EditPluginPreferenceHandler EditPreferences;

        public void LoadPlugin(IPluginHost host, string location, string guid)
        {
            Host = host;
            Location = location;
            Guid = guid;
        }

        public void UnloadPlugin()
        {
        }

        public void GetMetadata(Job job)
        {
            ApiRequest(job);
            GetPosters(job);
        }

        private void checkConfig()
        {
            //var x = JsonConvert.DeserializeObject<Setting>(jsonText);

        }

        private void ApiRequest(Job job)
        {
            job.Movies.Clear();
            string ISO_639_1 = "en";
            int? year = null;
            TmdbApiParameters requestParameters = new TmdbApiParameters(job.Disc.SanitizedTitle, year, ISO_639_1);
                  
            try
            {
                _tmdbApi = new Tmdb(TmdbApiKey, ISO_639_1);
            }
            catch (Exception ex)
            {
                var error = new PluginException("Error: An error has occurred", ex, PluginExceptionSeverity.Error);
            }

            try
            {
                if (string.IsNullOrEmpty(_tmdbRootUrl))
                {
                    _tmdbRootUrl = _tmdbApi.GetConfiguration().images.base_url + "w185";
                }

                _tmdbMovieSearch = _tmdbApi.SearchMovie(requestParameters.Query, 1, requestParameters.ISO_639_1, false, requestParameters.Year);
                

                if (_tmdbMovieSearch != null)
                {
                    job.Movies.AddRange(_tmdbMovieSearch.results.Select(ToMovie));
                }
            }
            catch (Exception ex)
            {
                var error = new PluginException("Error: An error occurred when contacting api.themoviedb.org", ex, PluginExceptionSeverity.Error);
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
            movie.CoverArtImages.Add(new CoverArtImage
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
                    var error = new PluginException("Error: An error occurred when contacting api.themoviedb.org", ex, PluginExceptionSeverity.Error);
                }
                if (tmdbMovieImages != null)
                {
                    foreach (var poster in tmdbMovieImages.posters)
                    {
                        movie.CoverArtImages.Add(new CoverArtImage
                        {
                            Uri = _tmdbRootUrl + poster.file_path,
                            Language = I18N.Language.FromCode(poster.iso_639_1)
                        });
                    }

                    if (movie.CoverArtImages.Count != null && movie.CoverArtImages.Count > 0)
                    {
                        //To Do:
                        //wrap the Log in a new Log object
                        // Log Message:  "The Tmdb found " + movie.CoverArtImages.Count + " Movie Posters";

                    }

                }
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
    }
}
