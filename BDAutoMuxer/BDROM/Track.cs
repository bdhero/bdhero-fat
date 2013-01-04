using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDAutoMuxer.BDROM
{
    /// <summary>
    /// Represents a .M2TS file
    /// </summary>
    class Track
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

        #region "Enum" flags

        /// <summary>
        /// Main feature.
        /// </summary>
        public bool is_main;

        /// <summary>
        /// Director or other commentary.
        /// </summary>
        public bool is_commentary;

        /// <summary>
        /// Special feature.
        /// </summary>
        public bool is_special_feature;

        /// <summary>
        /// Accessible secondary audio track for the blind and visually impaired.
        /// </summary>
        public bool is_accessible;

        #endregion
    }
}
