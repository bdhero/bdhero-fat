using System.Collections.Generic;
using System.Linq;
using I18N;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace BDHero.BDROM
{
    /// <summary>
    /// Represents the top-level BD-ROM disc and movie
    /// </summary>
    public class Disc
    {
        #region DB Fields

        /// <summary>
        /// E.G., "TOY_STORY_2_USA"
        /// </summary>
        [JsonProperty(PropertyName = "volume_label")]
        public string VolumeLabel;

        /// <summary>
        /// Name of the movie taken from bdmt_eng.xml if present (e.g., "Toy Story 2 - Blu-ray™").
        /// Most discs released after 2009 have this value.
        /// </summary>
        [JsonProperty(PropertyName = "meta_title")]
        public string MetaTitle;

        /// <summary>
        /// Primary release language of the disc.
        /// </summary>
        [JsonProperty(PropertyName = "primary_language")]
        public Language PrimaryLanguage = Language.Undetermined;

        /// <summary>
        /// List of all playlists in the order they appear on the disc.
        /// </summary>
        [JsonProperty(PropertyName = "playlists")]
        public List<Playlist> Playlists = new List<Playlist>();

        #endregion

        #region DB (TMDb / TVDB) Metadata

        /// <summary>
        /// TMDb movie ID (e.g., 863).
        /// </summary>
        [JsonProperty(PropertyName = "tmdb_id")]
        public int TmdbId;

        /// <summary>
        /// Name of the movie in its primary release language (e.g., "Toy Story 2").
        /// </summary>
        [JsonProperty(PropertyName = "movie_title")]
        public string MovieTitle;

        /// <summary>
        /// Year the movie was released (e.g., 1999).
        /// </summary>
        [JsonProperty(PropertyName = "movie_year")]
        public int? MovieYear;

        #endregion

        #region Constructor

        public Disc()
        {
            Languages = new List<Language>();
        }

        #endregion

        #region Non-DB Properties

        /// <summary>
        /// Returns a list of all languages found on the disc, with the primary disc language first if it can be automatically detected.
        /// </summary>
        [JsonIgnore]
        public IList<Language> Languages { get; private set; }

        public IList<Playlist> ValidMainFeaturePlaylists
        {
            get { return Playlists.Where(playlist => playlist.IsMainFeature && !playlist.IsBogus).ToList(); }
        }

        #endregion
    }
}
