using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using Newtonsoft.Json;

namespace BDHero.Queue
{
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

        public readonly IList<Movie> Movies = new List<Movie>();

        public readonly IList<TVShow> TVEpisodes = new List<TVShow>();

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
