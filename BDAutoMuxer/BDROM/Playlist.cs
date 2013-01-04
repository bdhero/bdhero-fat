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
        public string Filename { get; private set; }

        /// <summary>
        /// Size of the playlist file in bytes.
        /// </summary>
        public ulong Filesize { get; private set; }

        /// <summary>
        /// Duration of the playlist in seconds (e.g., 5545 = 1:32:25).
        /// </summary>
        public int LengthSec { get; private set; }

        #endregion

        #region DB Flags (bogus, low resolution)

        /// <summary>
        /// Has repeated stream files, loops, or is a duplicate of another playlist.
        /// </summary>
        public bool IsBogus;

        /// <summary>
        /// Has a lower video resolution than the highest-resolution playlist.
        /// E.G., this playlist is 480p or 720i but the highest-resolution playlist is 1080p.
        /// </summary>
        public bool IsLowResolution;

        #endregion

        #region DB "Cut" (a.k.a. "release" or "edition")

        /// <summary>
        /// Cut (a.k.a. "release" or "edition") of the film.
        /// </summary>
        public PlaylistCut Cut;

        /// <summary>
        /// Theatrical edition.
        /// </summary>
        public bool IsTheatricalEdition { get { return Cut == PlaylistCut.Theatrical; } }

        /// <summary>
        /// Special edition.
        /// </summary>
        public bool IsSpecialEdition { get { return Cut == PlaylistCut.Special; } }

        /// <summary>
        /// Extended edition.
        /// </summary>
        public bool IsExtendedEdition { get { return Cut == PlaylistCut.Extended; } }

        /// <summary>
        /// Unrated edition.
        /// </summary>
        public bool IsUnratedEdition { get { return Cut == PlaylistCut.Unrated; } }

        #endregion

        #region DB Tracks

        /// <summary>
        /// List of all tracks (TSStreams) in the order they appear in the playlist.
        /// Video, audio, and subtitles (includes hidden tracks and unsupported codecs).
        /// </summary>
        public IList<Track> Tracks = new List<Track>();

        #endregion

        #region DB Chapters

        public IList<Chapter> Chapters = new List<Chapter>();

        #endregion

        public class Json
        {
            #region DB Fields (filename, file size, length)

            public string filename;
            public ulong filesize;
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

            public PlaylistCut cut
            {
                get
                {
                    return
                        is_special_edition ? PlaylistCut.Special :
                        is_extended_edition ? PlaylistCut.Extended :
                        is_unrated_edition ? PlaylistCut.Unrated :
                                              PlaylistCut.Theatrical;
                }
                set
                {
                    is_theatrical_edition = value == PlaylistCut.Theatrical;
                    is_special_edition = value == PlaylistCut.Special;
                    is_extended_edition = value == PlaylistCut.Extended;
                    is_unrated_edition = value == PlaylistCut.Unrated;
                }
            }

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
            public IList<Track.Json> tracks = new List<Track.Json>();

            #endregion

            #region DB Chapters

            public IList<Chapter.Json> chapters = new List<Chapter.Json>();

            #endregion

            public Playlist FromJson()
            {
                return new Playlist
                           {
                               Filename = filename,
                               Filesize = filesize,
                               LengthSec = length_sec,
                               IsBogus = is_bogus,
                               IsLowResolution = is_low_resolution,
                               Cut = cut,
                               Tracks = tracks.Select(track => track.FromJson()).ToList(),
                               Chapters = chapters.Select(chapter => chapter.FromJson()).ToList()
                           };
            }
        }

        #region Protected utilities

        /// <summary>
        /// Null-safe method for testing if the first video track meets certain criteria.
        /// </summary>
        /// <param name="delegate">Does NOT need to check if the video track is null.  It will not be invoked if there are no video tracks.</param>
        /// <returns>The delegate's return value if this playlist has a video track; otherwise false.</returns>
        bool TestFirstVideoTrack(FirstVideoTrackDelegate @delegate)
        {
            var video = Tracks.FirstOrDefault(track => track.IsVideo);
            return video != null && @delegate(video);
        }

        #endregion

        #region Non-DB video track properties (main feature, video commentary, special feature, video accessible, misc.)

        /// <summary>
        /// The main movie (a.k.a. feature film) without forced (burned in) video commentary.
        /// </summary>
        public bool IsMainFeature
        {
            get
            {
                return TestFirstVideoTrack(track => track.IsMainFeature);
            }
        }

        /// <summary>
        /// Director or other commentary is burned in to the primary video track.
        /// </summary>
        public bool IsVideoCommentary
        {
            get
            {
                return TestFirstVideoTrack(track => track.IsCommentary);
            }
        }

        /// <summary>
        /// Special feature.
        /// </summary>
        public bool IsSpecialFeature
        {
            get
            {
                return TestFirstVideoTrack(track => track.IsSpecialFeature);
            }
        }

        /// <summary>
        /// Accessible video for the deaf.  This should never be true unless the user manually overrides it.
        /// </summary>
        public bool IsVideoAccessible
        {
            get
            {
                return TestFirstVideoTrack(track => track.IsAccessible);
            }
        }

        /// <summary>
        /// Miscellaneous / extra / other playlist (e.g., trailer, FBI warning).
        /// </summary>
        public bool IsMisc
        {
            get
            {
                return TestFirstVideoTrack(track => track.IsMisc);
            }
        }

        #endregion

        /// <summary>
        /// The type of the first (primary) video track.
        /// </summary>
        public TrackType Type
        {
            get
            {
                var video = Tracks.FirstOrDefault(track => track.IsVideo);
                return video != null ? video.Type : TrackType.Misc;
            }
            set
            {
                var video = Tracks.FirstOrDefault(track => track.IsVideo);
                if (video != null)
                    video.Type = value;
            }
        }
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
