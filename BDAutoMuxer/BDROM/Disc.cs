using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

// ReSharper disable InconsistentNaming
namespace BDAutoMuxer.BDROM
{
    /// <summary>
    /// Represents the top-level BD-ROM disc and movie
    /// </summary>
    public class Disc
    {
        /// <summary>
        /// E.G., "TOY_STORY_2_USA"
        /// </summary>
        public string volume_label;

        /// <summary>
        /// Name of the movie taken from bdmt_eng.xml if present (e.g., "Toy Story 2 - Blu-ray™").
        /// Most discs released after 2009 have this value.
        /// </summary>
        public string meta_title;

        /// <summary>
        /// Primary release language of the disc.
        /// A 3-letter ISO 639-2 language code in all lowercase (e.g., "eng").
        /// </summary>
        public string iso639_2;

        /// <summary>
        /// TMDb movie ID (e.g., 863).
        /// </summary>
        public int tmdb_id;

        /// <summary>
        /// Name of the movie in its primary release language (e.g., "Toy Story 2").
        /// </summary>
        public string movie_title;

        /// <summary>
        /// Year the movie was released (e.g., 1999).
        /// </summary>
        public int? movie_year;

        /// <summary>
        /// List of all playlists in the order they appear on the disc.
        /// </summary>
        public IList<Playlist> playlists;

        public static void Test()
        {
            var disc = JsonConvert.DeserializeObject<Disc>("{ \"title_name\": \"abc\" }");
            Console.WriteLine(disc);
        }
    }
}
