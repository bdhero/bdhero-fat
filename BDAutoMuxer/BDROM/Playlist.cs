using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDAutoMuxer.BDROM
{
    /// <summary>
    /// Represents a .MPLS file
    /// </summary>
    class Playlist
    {
        #region DB Fields (filename, file size, length)

        /// <summary>
        /// Name of the playlist file in all uppercase (e.g., "00200.MPLS").
        /// </summary>
        public string filename;

        /// <summary>
        /// Size of the playlist file in bytes.
        /// </summary>
        public ulong filesize;

        /// <summary>
        /// Duration of the playlist in seconds (e.g., 5545 = 1:32:25).
        /// </summary>
        public int length_sec;

        #endregion

        #region DB Flags (bogus, low quality, short)

        /// <summary>
        /// Has repeated tracks or is a duplicate of another playlist.
        /// </summary>
        public bool is_bogus;

        /// <summary>
        /// Has a lower video resolution than the highest-resolution playlist.
        /// E.G., this playlist is 480p or 720i but the highest-resolution playlist is 1080p.
        /// </summary>
        public bool is_low_quality;

        /// <summary>
        /// Less than 90% of the length of the longest playlist.
        /// E.G., this playlist is 50 minutes long whereas the longest playlist is 90 minutes.
        /// </summary>
        public bool is_short;

        #endregion

        #region DB "Cut" (a.k.a. "release" or "edition")

        /// <summary>
        /// Theatrical edition.
        /// </summary>
        public bool is_theatrical_edition;

        /// <summary>
        /// Special edition.
        /// </summary>
        public bool is_special_edition;

        /// <summary>
        /// Extended edition.
        /// </summary>
        public bool is_extended_edition;

        /// <summary>
        /// Unrated edition.
        /// </summary>
        public bool is_unrated_edition;

        #endregion

        #region DB Tracks

        /// <summary>
        /// List of all tracks (TSStreams) in the order they appear in the playlist.
        /// Video, audio, and subtitles (includes hidden tracks and unsupported codecs).
        /// </summary>
        public IList<Track> tracks;

        #endregion

        #region Non-DB Properties (main, commentary, special feature)

        /// <summary>
        /// The main movie (a.k.a. feature film) without forced (burned in) video commentary.
        /// </summary>
        public bool is_main
        {
            get
            {
                var video = tracks.FirstOrDefault(track => track.is_video);
                return video != null && video.is_main;
            }
        }

        /// <summary>
        /// Director or other commentary is burned in to the video.
        /// </summary>
        public bool is_commentary
        {
            get
            {
                var video = tracks.FirstOrDefault(track => track.is_video);
                return video != null && video.is_commentary;
            }
        }

        /// <summary>
        /// Special feature.
        /// </summary>
        public bool is_special_feature
        {
            get
            {
                var video = tracks.FirstOrDefault(track => track.is_video);
                return video != null && video.is_special_feature;
            }
        }

        #endregion
    }
}
