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

        #region DB Track type (main, commentary, special feature, PiP, descriptive, misc)

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
        /// Number of audio channels (e.g., 2, 6, 8).
        /// </summary>
        public int ChannelCount;

        /// <summary>
        /// Video height (e.g., 1080, 720, 480).
        /// </summary>
        public int VideoHeight;

        #endregion

        public static IList<Track> Transform(IEnumerable<TSStream> streams)
        {
            return streams.Select(Transform).ToList();
        }

        public static Track Transform(TSStream stream)
        {
            return new Track
                       {
                           Language = stream.Language,
                           IsHidden = stream.IsHidden,
                           Codec = MICodec.FromStreamType(stream.StreamType),
                           IsVideo = stream.IsVideoStream,
                           IsAudio = stream.IsAudioStream,
                           IsSubtitle = stream.IsGraphicsStream || stream.IsTextStream,
                           ChannelCount = GetChannelCount(stream as TSAudioStream),
                           VideoHeight = GetVideoHeight(stream as TSVideoStream)
                       };
        }

        private static int GetChannelCount(TSAudioStream audioStream)
        {
            return audioStream != null ? audioStream.ChannelCount : 0;
        }

        private static int GetVideoHeight(TSVideoStream videoStream)
        {
            return videoStream != null ? videoStream.Height : 0;
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
}
