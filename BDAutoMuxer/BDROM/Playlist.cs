﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDAutoMuxer.BDInfo;

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
        public double LengthSec { get; private set; }

        #endregion

        #region DB Flags (bogus, low resolution)

        /// <summary>
        /// Has repeated stream files, loops, or is a duplicate of another playlist.
        /// </summary>
        public bool IsBogus;

        /// <summary>
        /// Has a lower video resolution than the highest-resolution playlist or
        /// a lower number of audio channels than the playlist with the highest number of channels.
        /// E.G., this playlist is 480p or 720i but the highest-resolution playlist is 1080p,
        /// or this playlist has at most 2.0 audio channels but another playlist has 5.1.
        /// </summary>
        public bool IsLowQuality;

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

        #region Non-DB Video track properties (main feature, video commentary, special feature, misc.)

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

        public int MaxVideoResolution
        {
            get
            {
                return Tracks.Where(track => track.IsVideo).OrderByDescending(v => v.VideoHeight).Select(track => track.VideoHeight).FirstOrDefault();
            }
        }

        public int MaxAudioChannels
        {
            get
            {
                return Tracks.Where(track => track.IsAudio).OrderByDescending(v => v.ChannelCount).Select(track => track.ChannelCount).FirstOrDefault();
            }
        }

        #endregion

        /// <summary>
        /// Full absolute path to the .MPLS file.
        /// </summary>
        public string FullPath;

        public static IList<Playlist> Transform(IEnumerable<KeyValuePair<string, TSPlaylistFile>> playlistFiles)
        {
            return Transform(playlistFiles.Select(pair => pair.Value).OrderBy(file => file.Name));
        }

        public static IList<Playlist> Transform(IEnumerable<TSPlaylistFile> playlistFiles)
        {
            return playlistFiles.Select(Transform).ToList();
        }

        public static Playlist Transform(TSPlaylistFile playlistFile)
        {
            return new Playlist
                       {
                           Filename = playlistFile.Name,
                           FullPath = playlistFile.FullName,
                           Filesize = playlistFile.FileSize,
                           LengthSec = (int) playlistFile.TotalLength,
                           Tracks = Track.Transform(playlistFile.SortedStreams),
                           Chapters = Chapter.Transform(playlistFile.Chapters),
                           IsBogus = playlistFile.HasDuplicateClips
                       };
        }

        public override int GetHashCode()
        {
            var hashes = string.Format("{0}, {1} bytes, {2} seconds, {3} tracks, {4} chapters", Filename, Filesize, LengthSec, Tracks.Count, Chapters.Count);
            return hashes.GetHashCode();
        }

        public Json ToJsonObject()
        {
            return new Json
                       {
                           filename = Filename,
                           filesize = Filesize,
                           length_sec = LengthSec,
                           is_bogus = IsBogus,
                           is_low_quality = IsLowQuality,
                           cut = Cut,
                           tracks = Tracks.Select(track => track.ToJsonObject()).ToList(),
                           chapters = Chapters.Select(chapter => chapter.ToJsonObject()).ToList()
                       };
        }

        public class Json
        {
            #region DB Fields (filename, file size, length)

            public string filename;
            public ulong filesize;
            public double length_sec;

            #endregion

            #region DB Flags (bogus, low quality)

            public bool is_bogus;
            public bool is_low_quality;

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
                               LengthSec = length_sec,
                               IsBogus = is_bogus,
                               IsLowQuality = is_low_quality,
                               Cut = cut,
                               Tracks = tracks.Select(track => track.ToTrack()).ToList(),
                               Chapters = chapters.Select(chapter => chapter.ToChapter()).ToList()
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