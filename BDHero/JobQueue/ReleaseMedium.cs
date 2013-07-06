using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.JobQueue
{
    /// <summary>
    /// Abstract base class representing a release medium (e.g., movie, TV, radio, newspaper).
    /// </summary>
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
        /// URL of a website with more detailed information about this ReleaseMedium.
        /// </summary>
        public string Url;

        /// <summary>
        /// Gets or sets whether the BD-ROM contains this movie or TV show.
        /// </summary>
        public bool IsSelected;

        /// <summary>
        /// Poster image URLs from TMDb / TVDB.
        /// </summary>
        public readonly IList<CoverArt> CoverArtImages = new List<CoverArt>();
    }

    public class Movie : ReleaseMedium
    {
        /// <summary>
        /// Year the movie was released.
        /// </summary>
        public int ReleaseYear;
    }

    public class TVShow : ReleaseMedium
    {
        /// <summary>
        /// Collection of all episodes of this TV show ever released throughout all of its seasons.
        /// </summary>
        public readonly IList<Episode> Episodes = new List<Episode>();

        /// <summary>
        /// Gets or sets the index of the episode in <see cref="Episodes"/> selected by the user.
        /// </summary>
        public int SelectedEpisodeIndex;

        /// <summary>
        /// Gets the episode selected by the user.
        /// </summary>
        public Episode SelectedEpisode
        {
            get { return Episodes[SelectedEpisodeIndex]; }
        }

        public class Episode
        {
            public int Id;
            public string Title;
            public int SeasonNumber;
            public int EpisodeNumber;
            public DateTime ReleaseDate;
        }
    }

    /// <summary>
    /// Enumeration of available <see cref="ReleaseMedium"/>s.
    /// </summary>
    public enum ReleaseMediumType
    {
        Unknown = 0,
        Movie = 1,
        TVShow = 2
    }
}
