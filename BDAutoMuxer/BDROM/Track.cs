using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDAutoMuxer.BDInfo;
using BDAutoMuxer.models;
using BDAutoMuxer.tools;

// ReSharper disable InconsistentNaming
namespace BDAutoMuxer.BDROM
{
    /// <summary>
    /// Represents a .M2TS file
    /// </summary>
    public class Track
    {
        #region DB Fields

        /// <summary>
        /// Language of the track.
        /// </summary>
        public Language Language = Language.FromCode("und");

        /// <summary>
        /// Track is physically present in the underlying .M2TS stream file(s), but is not listed in the .MPLS playlist file.
        /// </summary>
        public bool IsHidden;

        /// <summary>
        /// Value of MICodec.SerializableName.
        /// </summary>
        public MICodec Codec = MICodec.UnknownCodec;

        public bool IsVideo;
        public bool IsAudio;
        public bool IsSubtitle;

        #endregion

        #region DB Track type (main, commentary, special feature, descriptive, misc)

        public TrackType Type = TrackType.Misc;

        /// <summary>
        /// Main feature.
        /// </summary>
        public bool IsMainFeature { get { return Type == TrackType.MainFeature; } }

        /// <summary>
        /// Director or other commentary.
        /// </summary>
        public bool IsCommentary { get { return Type == TrackType.Commentary; } }

        /// <summary>
        /// Special feature.
        /// </summary>
        public bool IsSpecialFeature { get { return Type == TrackType.SpecialFeature; } }

        /// <summary>
        /// Descriptive audio for the blind.
        /// </summary>
        public bool IsDescriptive { get { return Type == TrackType.Descriptive; } }

        /// <summary>
        /// Miscellaneous / extra / other track (e.g., trailer, FBI warning).
        /// </summary>
        public bool IsMisc { get { return Type == TrackType.Misc; } }

        #endregion

        #region Non-DB Fields

        /// <summary>
        /// Index of the track (i.e., its position or order) in the playlist.
        /// </summary>
        public int Index;

        /// <summary>
        /// Number of audio channels (e.g., 2.0, 5.1, 7.1).
        /// </summary>
        public double ChannelCount;

        public TSVideoFormat VideoFormat;
        public TSFrameRate FrameRate;
        public TSAspectRatio AspectRatio;

        #endregion

        #region Non-DB Properties

        public TrackTypeDisplayable TypeDisplayable
        {
            get { return TrackTypeDisplayable.Get(Type); }
            set { Type = value != null ? value.Value : TrackType.Misc; }
        }

        /// <summary>
        /// Number of audio channels (e.g., 2, 6, 8).
        /// </summary>
        public string QualityDisplayable
        {
            get
            {
                if (IsVideo) return VideoFormatDisplayable;
                if (IsAudio) return ChannelCount.ToString("0.0");
                return "N/A";
            }
        }

        /// <summary>
        /// Video height (e.g., 1080, 720, 480).
        /// </summary>
        public int VideoHeight
        {
            get
            {
                return
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_1080i || VideoFormat == TSVideoFormat.VIDEOFORMAT_1080p ? 1080 :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_720p ? 720 :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_576i || VideoFormat == TSVideoFormat.VIDEOFORMAT_576p ? 576 :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_480i || VideoFormat == TSVideoFormat.VIDEOFORMAT_480p ? 480 : 0;

            }
        }

        /// <summary>
        /// Video height (e.g., 1080, 720, 480).
        /// </summary>
        public string VideoFormatDisplayable
        {
            get
            {
                return
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_1080i ? "1080i" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_1080p ? "1080p" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_720p ? "720p" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_576i ? "576i" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_576p ? "576p" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_480i ? "480i" :
                    VideoFormat == TSVideoFormat.VIDEOFORMAT_480p ? "480p" : "unknown";

            }
        }

        public string FrameRateDisplayable
        {
            get
            {
                return
                    FrameRate == TSFrameRate.FRAMERATE_23_976 ? "23.976" :
                    FrameRate == TSFrameRate.FRAMERATE_24 ? "24" :
                    FrameRate == TSFrameRate.FRAMERATE_25 ? "25" :
                    FrameRate == TSFrameRate.FRAMERATE_29_97 ? "29.97" :
                    FrameRate == TSFrameRate.FRAMERATE_50 ? "50" :
                    FrameRate == TSFrameRate.FRAMERATE_59_94 ? "59.94" : "unknown";
            }
        }

        public string AspectRatioDisplayable
        {
            get
            {
                return
                    AspectRatio == TSAspectRatio.ASPECT_16_9 ? "16:9" :
                    AspectRatio == TSAspectRatio.ASPECT_4_3 ? "4:3" : "unknown";
            }
        }

        #endregion

        public static IList<Track> Transform(IEnumerable<TSStream> streams)
        {
            return streams.Select(Transform).ToList();
        }

        public static Track Transform(TSStream stream, int index)
        {
            var audioStream = stream as TSAudioStream;
            var videoStream = stream as TSVideoStream;
            return new Track
                       {
                           Index = index,
                           Language = stream.Language,
                           IsHidden = stream.IsHidden,
                           Codec = MICodec.FromStreamType(stream.StreamType),
                           IsVideo = stream.IsVideoStream,
                           IsAudio = stream.IsAudioStream,
                           IsSubtitle = stream.IsGraphicsStream || stream.IsTextStream,
                           ChannelCount = GetChannelCount(audioStream),
                           VideoFormat = GetVideoHeight(videoStream),
                           FrameRate = GetVideoFrameRate(videoStream),
                           AspectRatio = GetVideoAspectRatio(videoStream),
                       };
        }

        private static double GetChannelCount(TSAudioStream audioStream)
        {
            return audioStream != null ? audioStream.ChannelCountDouble : 0;
        }

        private static TSVideoFormat GetVideoHeight(TSVideoStream videoStream)
        {
            return videoStream != null ? videoStream.VideoFormat : 0;
        }

        private static TSFrameRate GetVideoFrameRate(TSVideoStream videoStream)
        {
            return videoStream != null ? videoStream.FrameRate : TSFrameRate.Unknown;
        }

        private static TSAspectRatio GetVideoAspectRatio(TSVideoStream videoStream)
        {
            return videoStream != null ? videoStream.AspectRatio : TSAspectRatio.Unknown;
        }

        public Json ToJsonObject()
        {
            return new Json
                       {
                           iso639_2 = Language.ISO_639_2,
                           is_hidden = IsHidden,
                           codec_id = Codec.SerializableName,
                           is_video = IsVideo,
                           is_audio = IsAudio,
                           is_subtitle = IsSubtitle,
                           type = Type
                       };
        }

        public class Json
        {
            #region DB Fields

            /// <summary>
            /// 3-letter ISO 639-2 language code in all lowercase (e.g., "eng").
            /// </summary>
            public string iso639_2;

            /// <summary>
            /// Track is physically present in the underlying .M2TS stream file(s), but is not listed in the .MPLS playlist file.
            /// </summary>
            public bool is_hidden;

            /// <summary>
            /// Value of MICodec.SerializableName.
            /// </summary>
            public string codec_id;

            public bool is_video;
            public bool is_audio;
            public bool is_subtitle;

            #endregion

            #region DB Track type (main, commentary, special feature, PiP, descriptive, misc)

            public TrackType type
            {
                get
                {
                    return
                        is_main_feature    ? TrackType.MainFeature :
                        is_commentary      ? TrackType.Commentary :
                        is_special_feature ? TrackType.SpecialFeature :
                        is_descriptive     ? TrackType.Descriptive :
                                             TrackType.Misc;
                }
                set
                {
                    is_main_feature    = value == TrackType.MainFeature;
                    is_commentary      = value == TrackType.Commentary;
                    is_special_feature = value == TrackType.SpecialFeature;
                    is_descriptive     = value == TrackType.Descriptive;
                    is_misc            = value == TrackType.Misc;
                }
            }

            /// <summary>
            /// Main feature.
            /// </summary>
            public bool is_main_feature;

            /// <summary>
            /// Director or other commentary.
            /// </summary>
            public bool is_commentary;

            /// <summary>
            /// Special feature.
            /// </summary>
            public bool is_special_feature;

            /// <summary>
            /// Descriptive audio, video, or subtitles for the blind / deaf.
            /// </summary>
            public bool is_descriptive;

            /// <summary>
            /// Miscellaneous / extra / other track (e.g., trailer, FBI warning).
            /// </summary>
            public bool is_misc;

            #endregion

            public Track ToTrack()
            {
                return new Track
                           {
                               Language = Language.FromCode(iso639_2) ?? Language.FromCode("und"),
                               IsHidden = is_hidden,
                               Codec = MICodec.FromSerializableName(codec_id),
                               IsVideo = is_video,
                               IsAudio = is_audio,
                               IsSubtitle = is_subtitle,
                               Type = type
                           };
            }
        }

        #region Public utilities

        public bool IsMainFeatureAudioTrack(int index)
        {
            return
                index == 0 ||
                index >= 1 && ChannelCount > 2;
        }

        public bool IsCommentaryAudioTrack(int index)
        {
            return index >= 1 && ChannelCount <= 2;
        }

        #endregion
    }

    public enum TrackType
    {
        MainFeature,
        Commentary,
        SpecialFeature,
        Descriptive,
        Misc
    }

    public class TrackTypeDisplayable
    {
        public TrackType Value;
        public string Displayable { get { return ToString(); } }
        public override string ToString()
        {
            return
                Value == TrackType.MainFeature ? "Main Feature" :
                Value == TrackType.Commentary ? "Commentary" :
                Value == TrackType.SpecialFeature ? "Special Feature" :
                Value == TrackType.Descriptive ? "Descriptive" :
                Value == TrackType.Misc ? "Misc." : "Unknown";
        }

        public static readonly List<TrackTypeDisplayable> List =
            new List<TrackTypeDisplayable>
                {
                    new TrackTypeDisplayable { Value = TrackType.MainFeature },
                    new TrackTypeDisplayable { Value = TrackType.Commentary },
                    new TrackTypeDisplayable { Value = TrackType.SpecialFeature },
                    new TrackTypeDisplayable { Value = TrackType.Descriptive },
                    new TrackTypeDisplayable { Value = TrackType.Misc }
                };

        public static TrackTypeDisplayable Get(TrackType trackType)
        {
            if (List.Any(displayable => displayable.Value == trackType))
                return List.First(displayable => displayable.Value == trackType);
            return List.Last();
        }
    }
}
