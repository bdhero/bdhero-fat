using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDAutoMuxer.BDInfo;
using MediaInfoWrapper;

// ReSharper disable InconsistentNaming
namespace BDAutoMuxer.BDROM
{
    /// <summary>
    /// Represents a .MPLS file
    /// </summary>
    public class Playlist
    {
        #region Private constants

        /// <summary>
        /// Threshold for considering a playlist to be "feature length" when comparing its length to the length of the longest playlist.
        /// </summary>
        private const double FeatureLengthPercentage = 0.9;

        /// <summary>
        /// Minimum length (in seconds) for a playlist to be considered a "Main Feature."
        /// </summary>
        private const int MinLengthSec = 120;

        #endregion

        #region Private static properties

        private static TimeSpan MinLength 
        {
            get { return TimeSpan.FromSeconds(MinLengthSec); }
        }

        #endregion

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
        /// Duration of the playlist.
        /// </summary>
        public TimeSpan Length { get; private set; }

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

        #region DB Tracks and Chapters

        /// <summary>
        /// List of all tracks (TSStreams) in the order they appear in the playlist.
        /// Video, audio, and subtitles (includes hidden tracks and unsupported codecs).
        /// </summary>
        public IList<Track> Tracks = new List<Track>();

        public IList<Chapter> Chapters = new List<Chapter>();

        #endregion

        #region Non-DB Flags (max quality, duplicate, loops, hidden first tracks, bogus)

        /// <summary>
        /// Has a lower video resolution than the highest-resolution playlist or
        /// a lower number of audio channels than the playlist with the highest number of channels.
        /// E.G., this playlist is 480p or 720i but the highest-resolution playlist is 1080p,
        /// or this playlist has at most 2.0 audio channels but another playlist has 5.1.
        /// </summary>
        public bool IsMaxQuality;

        /// <summary>
        /// Is a duplicate of another playlist.
        /// </summary>
        public bool IsDuplicate;

        /// <summary>
        /// Contains duplicate stream clips.
        /// </summary>
        public bool HasDuplicateStreamClips;

        /// <summary>
        /// Contains loops.
        /// </summary>
        public bool HasLoops;

        /// <summary>
        /// First video track and/or first audio track is hidden.
        /// </summary>
        public bool HasHiddenFirstTracks
        {
            get
            {
                return
                    (VideoTracks.Any() && VideoTracks.First().IsHidden) ||
                    (AudioTracks.Any() && AudioTracks.First().IsHidden);
            }
        }

        /// <summary>
        /// Has repeated stream clips (.M2TS files), contains loops, is a duplicate of another playlist, or has hidden primary audio/video tracks.
        /// </summary>
        public bool IsBogus
        {
            get { return IsDuplicate || HasDuplicateStreamClips || HasLoops || HasHiddenFirstTracks; }
        }

        #endregion

        #region Non-DB Public properties (full path, stream clips, video language)

        /// <summary>
        /// Full absolute path to the .MPLS file.
        /// </summary>
        public string FullPath;

        /// <summary>
        /// List of .M2TS files referenced by this playlist.
        /// </summary>
        public List<StreamClip> StreamClips = new List<StreamClip>();

        public Language VideoLanguage
        {
            get { return VideoTracks.Any() ? VideoTracks.First().Language : null; }
            set { if (VideoTracks.Any()) { VideoTracks.First().Language = value; } }
        }

        #endregion

        #region UI Properties (human length, warnings, chapter count)

        public string LengthHuman
        {
            get
            {
                return string.Format("{0}:{1}:{2}", Length.Hours.ToString("00"), Length.Minutes.ToString("00"), Length.Seconds.ToString("00"));
            }
        }

        public string Warnings
        {
            get
            {
                var warnings = new List<string>();
                if (IsBogus)
                    warnings.Add("Bogus");
                if (!IsMaxQuality)
                    warnings.Add("Low Quality");
                return string.Join(", ", warnings);
            }
        }

        public int ChapterCount
        {
            get { return Chapters.Count; }
        }

        #endregion

        #region Non-DB Video track properties (main feature, video commentary, special feature, misc.)

        /// <summary>
        /// The type of the first (primary) video track.
        /// </summary>
        public TrackType Type
        {
            get
            {
                var video = VideoTracks.FirstOrDefault();
                return video != null ? video.Type : TrackType.Misc;
            }
            set
            {
                var video = VideoTracks.FirstOrDefault();
                if (video != null)
                    video.Type = value;
            }
        }

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

        #region Non-DB Audio / Video properties

        public IList<Track> VideoTracks
        {
            get { return Tracks.Where(track => track.IsVideo).ToList(); }
        }

        public IList<Track> AudioTracks
        {
            get { return Tracks.Where(track => track.IsAudio).ToList(); }
        }

        public IList<Track> SubtitleTracks
        {
            get { return Tracks.Where(track => track.IsSubtitle).ToList(); }
        }

        /// <summary>
        /// The maximum height (in pixels) of this playlist's video tracks (e.g., 1080 or 720).
        /// </summary>
        public int MaxVideoResolution
        {
            get
            {
                return VideoTracks.OrderByDescending(v => v.VideoHeight).Select(track => track.VideoHeight).FirstOrDefault();
            }
        }

        /// <summary>
        /// The maximum number of audio channels found in this playlist's audio tracks (e.g., 8, 6, or 2).
        /// </summary>
        public double MaxAudioChannels
        {
            get
            {
                return AudioTracks.OrderByDescending(v => v.ChannelCount).Select(track => track.ChannelCount).FirstOrDefault();
            }
        }

        #endregion

        #region Transformers

        /// <summary>
        /// Returns a List of TSPlaylistFile objects from the given Dictionary.
        /// </summary>
        public static List<TSPlaylistFile> Transform(IEnumerable<KeyValuePair<string, TSPlaylistFile>> tsPlaylistFiles)
        {
            return tsPlaylistFiles.Select(pair => pair.Value).ToList();
        }

        /// <summary>
        /// Transforms a List of TSPlaylistFile objects into a List of Playlist objects.
        /// </summary>
        /// <param name="playlistFiles"></param>
        /// <returns></returns>
        public static List<Playlist> Transform(IEnumerable<TSPlaylistFile> playlistFiles)
        {
            return playlistFiles.OrderBy(file => file.Name).Select(Transform).ToList();
        }

        public static Playlist Transform(TSPlaylistFile playlistFile)
        {
            return new Playlist
                       {
                           Filename = playlistFile.Name,
                           FullPath = playlistFile.FullName,
                           Filesize = playlistFile.FileSize,
                           Length = TimeSpan.FromMilliseconds(playlistFile.TotalLength * 1000),
                           StreamClips = StreamClip.Transform(playlistFile.StreamClips),
                           Tracks = Track.Transform(playlistFile.SortedStreams),
                           Chapters = Chapter.Transform(playlistFile.Chapters),
                           HasDuplicateStreamClips = playlistFile.HasDuplicateClips,
                           HasLoops = playlistFile.HasLoops
                       };
        }

        #endregion

        #region Feature detection

        public bool IsMainFeaturePlaylist(TimeSpan maxPlaylistLength)
        {
            return
                IsMaxQuality &&
                IsFeatureLength(maxPlaylistLength) &&
                VideoTracks.Count >= 1 &&
                AudioTracks.Count >= 2 /* or >= 1? */ &&
                SubtitleTracks.Count >= 1 &&
                Chapters.Count >= 2 &&
                Length > MinLength;
        }

        public bool IsSpecialFeaturePlaylist(TimeSpan maxPlaylistLength)
        {
            return (IsMaxQuality && !IsFeatureLength(maxPlaylistLength) && AudioTracks.Count == 1 && Length > MinLength) ||
                   (!IsMaxQuality && IsFeatureLength(maxPlaylistLength) && AudioTracks.Count == 1 && Length > MinLength);
        }

        public bool IsFeatureLength(TimeSpan maxPlaylistLength)
        {
            return Length >= TimeSpan.FromMilliseconds(FeatureLengthPercentage * maxPlaylistLength.TotalMilliseconds);
        }

        #endregion

        #region Protected utilities

        /// <summary>
        /// Null-safe method for testing if the first video track meets certain criteria.
        /// </summary>
        /// <param name="delegate">Does NOT need to check if the video track is null.  It will not be invoked if there are no video tracks.</param>
        /// <returns>The delegate's return value if this playlist has a video track; otherwise false.</returns>
        bool TestFirstVideoTrack(FirstVideoTrackDelegate @delegate)
        {
            var video = VideoTracks.FirstOrDefault();
            return video != null && @delegate(video);
        }

        #endregion

        #region Default method overrides (GetHashCode)

        public override int GetHashCode()
        {
            var hashes = string.Format("{0}, {1} bytes, {2} seconds, {3} tracks, {4} chapters", Filename, Filesize, Length, Tracks.Count, Chapters.Count);
            return hashes.GetHashCode();
        }

        #endregion

        #region JSON Conversion

        public Json ToJsonObject()
        {
            return Json.ToJsonObject(this);
        }

        public class Json
        {
            #region DB Fields (filename, file size, length)

            public string filename;
            public ulong filesize;
            public double length_sec;

            #endregion

            #region DB "Cut" (a.k.a. "release" or "edition")

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

            public bool is_theatrical_edition;
            public bool is_special_edition;
            public bool is_extended_edition;
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

            public Playlist ToPlaylist()
            {
                return new Playlist
                           {
                               Filename = filename,
                               Filesize = filesize,
                               Length = TimeSpan.FromSeconds(length_sec),
                               Cut = cut,
                               Tracks = tracks.Select(track => track.ToTrack()).ToList(),
                               Chapters = chapters.Select(chapter => chapter.ToChapter()).ToList()
                           };
            }

            public static Json ToJsonObject(Playlist playlist)
            {
                return new Json
                           {
                               filename = playlist.Filename,
                               filesize = playlist.Filesize,
                               length_sec = playlist.Length.TotalSeconds,
                               cut = playlist.Cut,
                               tracks = playlist.Tracks.Select(track => track.ToJsonObject()).ToList(),
                               chapters = playlist.Chapters.Select(chapter => chapter.ToJsonObject()).ToList()
                           };
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
