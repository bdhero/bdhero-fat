using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.Queue
{
    public abstract class ReleaseMedium
    {
        /// <summary>
        /// TMDb movie ID (e.g., 863) or TVDB show ID.
        /// </summary>
        public int Id;

        /// <summary>
        /// Name of the movie/TV show in its primary release language (e.g., "Toy Story 2" (movie) or "Scrubs" (TV show)).
        /// </summary>
        public string Title;

        /// <summary>
        /// Gets or sets whether the BD-ROM contains this movie or TV show.
        /// </summary>
        public bool IsSelected;

        /// <summary>
        /// Poster image URLs from TMDb / TVDB.
        /// </summary>
        public readonly IList<CoverArtImage> CoverArtImages = new List<CoverArtImage>();
    }

    public enum ReleaseMediumType
    {
        Unknown = 0,
        Movie = 1,
        TVShow = 2
    }
}
