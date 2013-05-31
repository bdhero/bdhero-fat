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

            ApiRequest(job.Disc.MovieTitle);
        }

        private void ApiRequest(string title)
        {
            // Language Needs to be user selectable
            string ISO_639_1 = "en";
            int? year = null;

            _tmdbApi = new Tmdb(TmdbApiKey, ISO_639_1);

            TmdbApiParameters requestParameters = new TmdbApiParameters(title, year, ISO_639_1);

            try
            {
                _tmdbMovieSearch = _tmdbApi.SearchMovie(requestParameters.Query, 1, requestParameters.ISO_639_1, false, requestParameters.Year);

                if (string.IsNullOrEmpty(_tmdbRootUrl))
                {
                    _tmdbRootUrl = _tmdbApi.GetConfiguration().images.base_url + "w185";
                }                
            }
            catch (Exception ex)
            {
               
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
