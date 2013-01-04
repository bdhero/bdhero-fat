using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDAutoMuxer.BDInfo;
using BDAutoMuxer.models;

// ReSharper disable InconsistentNaming
namespace BDAutoMuxer.BDROM
{
    /// <summary>
    /// Represents a .M2TS file
    /// </summary>
    public class Track
    {
        /// <summary>
        /// Language of the track.
        /// </summary>
        public Language Language;

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

        #region DB track type (main, commentary, special feature, accessible, misc)

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
        /// Accessible audio, video, or subtitles for the blind / deaf.
        /// </summary>
        public bool IsAccessible { get { return Type == TrackType.Accessible; } }

        /// <summary>
        /// Miscellaneous / extra / other track (e.g., trailer, FBI warning).
        /// </summary>
        public bool IsMisc { get { return Type == TrackType.Misc; } }

        #endregion

        public class Json
        {
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

            #region DB "Enum" type flags (main, commentary, special feature, accessible, misc)

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
            /// Accessible audio, video, or subtitles for the blind / deaf.
            /// </summary>
            public bool is_accessible;

            /// <summary>
            /// Miscellaneous / extra / other track (e.g., trailer, FBI warning).
            /// </summary>
            public bool is_misc;

            #endregion

            #region Non-DB Properties

            public TrackType type
            {
                get
                {
                    return
                        is_main_feature ? TrackType.MainFeature :
                        is_commentary ? TrackType.Commentary :
                        is_special_feature ? TrackType.SpecialFeature :
                        is_accessible ? TrackType.Accessible :
                                             TrackType.Misc;
                }
                set
                {
                    is_main_feature = value == TrackType.MainFeature;
                    is_commentary = value == TrackType.Commentary;
                    is_special_feature = value == TrackType.SpecialFeature;
                    is_accessible = value == TrackType.Accessible;
                    is_misc = value == TrackType.Misc;
                }
            }

            #endregion

            public Track FromJson()
            {
                return new Track
                           {
                               Language = Language.GetLanguage(iso639_2),
                               IsHidden = is_hidden,
                               Codec = MICodec.FromSerializableName(codec_id),
                               IsVideo = is_video,
                               IsAudio = is_audio,
                               IsSubtitle = is_subtitle,
                               Type = type
                           };
            }
        }
    }

    public enum TrackType
    {
        MainFeature,
        Commentary,
        SpecialFeature,
        Accessible,
        Misc
    }
}
