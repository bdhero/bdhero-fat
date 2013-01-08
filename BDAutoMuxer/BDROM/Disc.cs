using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDAutoMuxer.BDInfo;

// ReSharper disable InconsistentNaming
namespace BDAutoMuxer.BDROM
{
    /// <summary>
    /// Represents the top-level BD-ROM disc and movie
    /// </summary>
    public partial class Disc
    {
        #region DB Fields

        /// <summary>
        /// E.G., "TOY_STORY_2_USA"
        /// </summary>
        public string VolumeLabel;

        /// <summary>
        /// Name of the movie taken from bdmt_eng.xml if present (e.g., "Toy Story 2 - Blu-ray™").
        /// Most discs released after 2009 have this value.
        /// </summary>
        public string MetaTitle;

        /// <summary>
        /// Primary release language of the disc.
        /// </summary>
        public Language PrimaryLanguage = Language.FromCode("und");

        /// <summary>
        /// TMDb movie ID (e.g., 863).
        /// </summary>
        public int TmdbId;

        /// <summary>
        /// Name of the movie in its primary release language (e.g., "Toy Story 2").
        /// </summary>
        public string MovieTitle;

        /// <summary>
        /// Year the movie was released (e.g., 1999).
        /// </summary>
        public int? MovieYear;

        /// <summary>
        /// List of all playlists in the order they appear on the disc.
        /// </summary>
        public List<Playlist> Playlists = new List<Playlist>();

        #endregion

        #region Constructor

        private Disc()
        {
            Languages = new List<Language>();
        }

        #endregion

        #region Non-DB Properties

        /// <summary>
        /// Returns a list of all languages found on the disc, with the primary disc language first if it can be automatically detected.
        /// </summary>
        public IList<Language> Languages { get; private set; }

        #endregion

        #region Default method overrides

        public override int GetHashCode()
        {
            var playlistHashes = string.Join(", ", Playlists.Select(playlist => playlist.GetHashCode()));
            var hashes = string.Format("{0}: [ {1} ]", VolumeLabel, playlistHashes);
            return hashes.GetHashCode();
        }

        #endregion

        #region JSON Conversion

        #region Serialization / Deserialization

        public Json ToJsonObject()
        {
            return new Json
                       {
                           volume_label = VolumeLabel,
                           meta_title = MetaTitle,
                           iso639_2 = PrimaryLanguage.ISO_639_2,
                           tmdb_id = TmdbId,
                           movie_title = MovieTitle,
                           movie_year = MovieYear,
                           playlists = Playlists.Select(playlist => playlist.ToJsonObject()).ToList()
                       };
        }

        public static IList<Disc> DeserializeJson(IEnumerable<Disc.Json> discs)
        {
            return discs.Select(jsonObject => jsonObject.ToDisc()).ToList();
        }

        #endregion

        #region JSON Class

        public class Json
        {
            #region DB Fields

            public string volume_label;
            public string meta_title;
            public string iso639_2;
            public int tmdb_id;
            public string movie_title;
            public int? movie_year;
            public IList<Playlist.Json> playlists;

            #endregion

            /// <summary>
            /// Transforms this Json object into a Disc object.
            /// </summary>
            /// <returns>Disc object populated with the same data as this Json object.</returns>
            public Disc ToDisc()
            {
                return new Disc
                           {
                               VolumeLabel = volume_label,
                               MetaTitle = meta_title,
                               PrimaryLanguage = Language.FromCode(iso639_2) ?? Language.FromCode("und"),
                               TmdbId = tmdb_id,
                               MovieTitle = movie_title,
                               MovieYear = movie_year,
                               Playlists = playlists.Select(playlist => playlist.ToPlaylist()).ToList()
                           };
            }
        }

        #endregion

        #endregion
    }
}
