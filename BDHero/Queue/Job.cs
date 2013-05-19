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
        public Disc Disc;

        /// <summary>
        /// Playlist selected by the user to mux.
        /// </summary>
        public int PlaylistIndex;

        /// <summary>
        /// Full absolute path to the muxed output file.
        /// </summary>
        public string OutputPath;

        /// <summary>
        /// Poster image URL from TMDb.
        /// </summary>
        public string CoverArtUrl;
    }
}
