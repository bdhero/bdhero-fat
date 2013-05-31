using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using BDHero.Plugin;
using BDHero.Queue;
using WatTmdb.V3;

namespace TmdbPlugin
{
    public class TmdbPlugin : IMetadataProviderPlugin
    {

        private const string TmdbApiKey = "b59b366b0f0a457d58995537d847409a";
        private Tmdb _tmdbApi;
        private TmdbMovieSearch _tmdbMovieSearch;
        private MovieResult _tmdbMovieResult;
        private BackgroundWorker tmdbGet;

        private string _tmdbMovieName;
        private int? _tmdbMovieYear;
        private int _tmdbID;
        private string _tmdbRootUrl;
        private List<string> _moviePosterList;
        private List<string> _movieLanguageList;
        private string _selectedMovieLanguage;
        private int _selectedPosterIndex;

        public IPluginHost Host { get; set; }
        public string Name 
        {
            get { return "TmdbPlugin"; }
        }

        public event PluginProgressHandler ProgressUpdated;
        public event EditPluginPreferenceHandler EditPreferences;

        public void LoadPlugin()
        {
        }

        public void UnloadPlugin()
        {
        }

        public void GetMetadata(Job job)
        {
            tmdbGet = new BackgroundWorker();
            tmdbGet.WorkerReportsProgress = false;
            tmdbGet.WorkerSupportsCancellation = false;

            ApiRequest(job);
            getPosters(job);
        }

        private void ApiRequest(Job job)
        {
            // Language Needs to be user selectable
            string ISO_639_1 = "en";
            int? year = null;

            _tmdbApi = new Tmdb(TmdbApiKey, ISO_639_1);

            TmdbApiParameters requestParameters = new TmdbApiParameters(job.Disc.MovieTitle, year, ISO_639_1);

            try
            {
                if (string.IsNullOrEmpty(_tmdbRootUrl))
                {
                    _tmdbRootUrl = _tmdbApi.GetConfiguration().images.base_url + "w185";
                }

                _tmdbMovieSearch = _tmdbApi.SearchMovie(requestParameters.Query, 1, requestParameters.ISO_639_1, false, requestParameters.Year);
                

                if (_tmdbMovieSearch != null && _tmdbMovieSearch.results.Count > 0)
                {
                    MovieResult movieResult = _tmdbMovieSearch.results[0];
                    DateTime releaseYear;
                    var date = DateTime.TryParse(movieResult.release_date, out releaseYear);

                    job.Disc.TmdbId = movieResult.id;
                    job.Disc.MovieYear = releaseYear.Year;
                    job.Disc.MovieTitle = movieResult.title;
                    job.CoverArtUrl = _tmdbRootUrl + movieResult.poster_path;
                }
            }
            catch (Exception ex)
            {
               
            }            
        }

        private void getPosters(Job job)
        {
            if (job.Disc.TmdbId != null)
            {
                try
                {
                    var tmdbMovieImages = _tmdbApi.GetMovieImages(job.Disc.TmdbId, null);
                    var posterLanguages = (tmdbMovieImages.posters.Select(poster => poster.iso_639_1).ToList());
                    posterLanguages = posterLanguages.Distinct().ToList();

                    if (posterLanguages.Count == 0)
                    {
                        tmdbMovieImages = _tmdbApi.GetMovieImages(job.Disc.TmdbId, "en");
                    }

                }
                catch { }

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
