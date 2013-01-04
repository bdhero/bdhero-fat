using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable InconsistentNaming
namespace BDAutoMuxer.BDROM
{
    /// <summary>
    /// Represents a .MPLS file
    /// </summary>
    public class Playlist
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

        #region DB Flags (bogus, low resolution)

        /// <summary>
        /// Has repeated stream files, loops, or is a duplicate of another playlist.
        /// </summary>
        public bool is_bogus;

        /// <summary>
        /// Has a lower video resolution than the highest-resolution playlist.
        /// E.G., this playlist is 480p or 720i but the highest-resolution playlist is 1080p.
        /// </summary>
        public bool is_low_resolution;

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

        #region Protected utilities

        /// <summary>
        /// Null-safe method for testing if the first video track meets certain criteria.
        /// </summary>
        /// <param name="delegate">Does NOT need to check if the video track is null.  It will not be invoked if there are no video tracks.</param>
        /// <returns>The delegate's return value if this playlist has a video track; otherwise false.</returns>
        bool TestFirstVideoTrack(FirstVideoTrackDelegate @delegate)
        {
            var video = tracks.FirstOrDefault(track => track.is_video);
            return video != null && @delegate(video);
        }

        #endregion

        #region Non-DB video track properties (main feature, video commentary, special feature, video accessible, misc.)

        /// <summary>
        /// The main movie (a.k.a. feature film) without forced (burned in) video commentary.
        /// </summary>
        public bool is_main_feature
        {
            get
            {
                return TestFirstVideoTrack(track => track.is_main_feature);
            }
        }

        /// <summary>
        /// Director or other commentary is burned in to the primary video track.
        /// </summary>
        public bool is_video_commentary
        {
            get
            {
                return TestFirstVideoTrack(track => track.is_commentary);
            }
        }

        /// <summary>
        /// Special feature.
        /// </summary>
        public bool is_special_feature
        {
            get
            {
                return TestFirstVideoTrack(track => track.is_special_feature);
            }
        }

        /// <summary>
        /// Accessible video for the deaf.  This should never be true unless the user manually overrides it.
        /// </summary>
        public bool is_video_accessible
        {
            get
            {
                return TestFirstVideoTrack(track => track.is_accessible);
            }
        }

        /// <summary>
        /// Miscellaneous / extra / other playlist (e.g., trailer, FBI warning).
        /// </summary>
        public bool is_misc
        {
            get
            {
                return TestFirstVideoTrack(track => track.is_misc);
            }
        }

        #endregion

        #region Non-DB enum properties

        public PlaylistCut cut
        {
            get
            {
                return
                    is_special_edition  ? PlaylistCut.Special :
                    is_extended_edition ? PlaylistCut.Extended :
                    is_unrated_edition  ? PlaylistCut.Unrated :
                                          PlaylistCut.Theatrical;
            }
            set
            {
                is_theatrical_edition = value == PlaylistCut.Theatrical;
                is_special_edition    = value == PlaylistCut.Special;
                is_extended_edition   = value == PlaylistCut.Extended;
                is_unrated_edition    = value == PlaylistCut.Unrated;
            }
        }

        /// <summary>
        /// The type of the first (primary) video track.
        /// </summary>
        public TrackType type
        {
            get
            {
                var video = tracks.FirstOrDefault(track => track.is_video);
                return video != null ? video.type : TrackType.Misc;
            }
            set
            {
                var video = tracks.FirstOrDefault(track => track.is_video);
                if (video != null)
                    video.type = value;
            }
        }

        #endregion
    }

    public enum PlaylistCut
    {
        Theatrical,
        Special,
        Extended,
        Unrated
    }

    delegate bool FirstVideoTrackDelegate(Track track);
}
