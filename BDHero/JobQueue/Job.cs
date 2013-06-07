using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;

namespace BDHero.JobQueue
{
    /// <summary>
    /// Represents a muxing job in the queue.  Contains all information needed to properly mux a single Blu-ray playlist to MKV,
    /// including all user selections such as tracks, languages, chapter names, and cover art.
    /// </summary>
    /// <remarks>
    /// This class can (and should) be serialized to JSON and stored on disk to allow the user to recover their muxing queue if the app crashes.
    /// </remarks>
    public class Job
    {
        public readonly Disc Disc;

        /// <summary>
        /// Playlist selected by the user to mux.
        /// </summary>
        public int SelectedPlaylistIndex;

        /// <summary>
        /// Full absolute path to the muxed output file.
        /// </summary>
        public string OutputPath;

        /// <summary>
        /// Collection of movie search results returned from TMDb.
        /// </summary>
        public readonly IList<Movie> Movies = new List<Movie>();

        /// <summary>
        /// Collection of TV show search results returned from TVDB.
        /// </summary>
        public readonly IList<TVShow> TVShows = new List<TVShow>();

        /// <summary>
        /// Auto-detected or user-selected release medium (movie or TV).
        /// </summary>
        public ReleaseMediumType ReleaseMediumType = ReleaseMediumType.Unknown;

        /// <summary>
        /// Gets the playlist selected by the user.
        /// </summary>
        public Playlist SelectedPlaylist { get { return Disc.Playlists[SelectedPlaylistIndex]; } }

        public Job(Disc disc)
        {
            Disc = disc;
        }
    }
}
