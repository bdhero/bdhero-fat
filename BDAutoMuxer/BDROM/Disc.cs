using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDAutoMuxer.BDInfo;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace BDAutoMuxer.BDROM
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
        public string VolumeLabel;

        /// <summary>
        /// Name of the movie taken from bdmt_eng.xml if present (e.g., "Toy Story 2 - Blu-ray™").
        /// Most discs released after 2009 have this value.
        /// </summary>
        public string MetaTitle;

        /// <summary>
        /// Primary release language of the disc.
        /// </summary>
        public Language Language;

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
        public IList<Playlist> Playlists;

        #endregion

        public Json ToJson()
        {
            return new Json
                       {
                           volume_label = VolumeLabel,
                           meta_title = MetaTitle,
                           iso639_2 = Language.ISO_639_2,
                           tmdb_id = TmdbId,
                           movie_title = MovieTitle,
                           movie_year = MovieYear,
                           playlists = Playlists.Select(playlist => playlist.ToJson()).ToList()
                       };
        }

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

            public Disc FromJson()
            {
                return new Disc
                           {
                               VolumeLabel = volume_label,
                               MetaTitle = meta_title,
                               Language = Language.GetLanguage(iso639_2),
                               TmdbId = tmdb_id,
                               MovieTitle = movie_title,
                               MovieYear = movie_year,
                               Playlists = playlists.Select(playlist => playlist.FromJson()).ToList()
                           };
            }
        }
    }
}
