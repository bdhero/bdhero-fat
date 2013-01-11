using System.Collections.Generic;
using System.Linq;
using BDAutoMuxerCore.BDInfo;
using MediaInfoWrapper;

// ReSharper disable InconsistentNaming
namespace BDAutoMuxerCore.BDROM
{
    /// <summary>
    /// Represents a .M2TS file
    /// </summary>
    public class Track
    {
        #region DB User-configurable fields (language)

        /// <summary>
        /// Language of the track.
        /// </summary>
        public Language Language = Language.FromCode("und");

        #endregion

        #region DB Matching fields (index, PID, hidden, codec)

        /// <summary>
        /// Index of the track (i.e., its position or order) in the playlist.
        /// </summary>
        public int Index;

        // MPEG-2 Transport Stream (M2TS) packet ID (PID) that uniquely identifies the track in the playlist.
        public int PID;

        /// <summary>
        /// Track is physically present in the underlying .M2TS stream file(s), but is not listed in the .MPLS playlist file.
        /// </summary>
        public bool IsHidden;

        /// <summary>
        /// Value of MICodec.SerializableName.
        /// </summary>
        public MICodec Codec = MICodec.UnknownCodec;

        #endregion

        #region DB "FYI" fields (not used for comparison, searching, or filtering)

        public bool IsVideo;
        public bool IsAudio;
        public bool IsSubtitle;

        /// <summary>
        /// Number of audio channels (e.g., 2.0, 5.1, 7.1).
        /// </summary>
        public double ChannelCount;

        public TSVideoFormat VideoFormat;
        public TSFrameRate FrameRate;
        public TSAspectRatio AspectRatio;

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

        #region UI display properties

        /// <summary>
        /// Number of audio channels (e.g., 2, 6, 8).
        /// </summary>
        public string QualityDisplayable
        {
            get
            {
                if (IsVideo) return VideoFormatDisplayable;
                if (IsAudio) return ChannelCount.ToString("0.0");
                return "";
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

        #region Transformers

        public static IList<Track> Transform(IEnumerable<TSStream> streams)
        {
            return streams.Select(Transform).ToList();
        }

        public static Track Transform(TSStream stream, int index)
        {
            var videoStream = stream as TSVideoStream;
            var audioStream = stream as TSAudioStream;
            return new Track
                       {
                           Index = index,
                           PID = stream.PID,
                           Language = stream.Language,
                           IsHidden = stream.IsHidden,
                           Codec = MediaInfoHelper.CodecFromStream(stream),
                           IsVideo = stream.IsVideoStream,
                           IsAudio = stream.IsAudioStream,
                           IsSubtitle = stream.IsGraphicsStream || stream.IsTextStream,
                           ChannelCount = audioStream != null ? audioStream.ChannelCountDouble : 0,
                           VideoFormat = videoStream != null ? videoStream.VideoFormat : 0,
                           FrameRate = videoStream != null ? videoStream.FrameRate : TSFrameRate.Unknown,
                           AspectRatio = videoStream != null ? videoStream.AspectRatio : TSAspectRatio.Unknown,
                       };
        }

        #endregion

        #region Feature detection

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

        #region JSON Conversion

        public Json ToJsonObject()
        {
            return Json.ToJsonObject(this);
        }

        public class Json
        {
            #region DB User-configurable fields (language)

            public string iso639_2;

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

            #region DB Matching fields (index, pid, hidden, codec ID)

            public int index;
            public int pid;
            public bool is_hidden;
            public string codec_id;

            #endregion

            #region DB "FYI" fields (a/v/s, channel count, video format, frame rate, aspect ratio)

            public bool is_video;
            public bool is_audio;
            public bool is_subtitle;

            public double channel_count;
            public string video_format;
            public string frame_rate;
            public string aspect_ratio;

            #endregion

            #region Converters

            public Track ToTrack()
            {
                return new Track
                           {
                               Index = index,
                               PID = pid,
                               Language = Language.FromCode(iso639_2) ?? Language.Undetermined,
                               IsHidden = is_hidden,
                               Codec = MICodec.FromSerializableName(codec_id),
                               IsVideo = is_video,
                               IsAudio = is_audio,
                               IsSubtitle = is_subtitle,
                               Type = type
                           };
            }

            public static Json ToJsonObject(Track track)
            {
                return new Json
                           {
                               index = track.Index,
                               pid = track.PID,
                               iso639_2 = (track.Language ?? Language.Undetermined).ISO_639_2,
                               is_hidden = track.IsHidden,
                               codec_id = track.Codec.SerializableName,
                               is_video = track.IsVideo,
                               is_audio = track.IsAudio,
                               is_subtitle = track.IsSubtitle,
                               type = track.Type
                           };
            }

            #endregion

            #region Matching (for DB search)

            public bool Matches(Json other)
            {
                return
                    other != null &&
                    index == other.index &&
                    pid == other.pid &&
                    string.Equals(iso639_2, other.iso639_2) &&
                    is_hidden.Equals(other.is_hidden) &&
                    string.Equals(codec_id, other.codec_id);
            }

            #endregion
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
}
