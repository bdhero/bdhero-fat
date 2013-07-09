﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetUtils.Annotations;
using I18N;

namespace BDHero.BDROM
{
    /// <summary>
    /// Contains useful identifying information gathered from across the BD-ROM filesystem.
    /// </summary>
    public class DiscMetadata
    {
        /// <summary>
        /// The BD-ROM volume label obtained from the BD-ROM drive, or from the name of the root BD-ROM folder
        /// if it was ripped to disk by AnyDVD HD or MakeMKV.
        /// </summary>
        /// <example><code>LOGICAL_VOLUME_ID</code></example>
        /// <example><code>THE_PHANTOM_MENACE</code></example>
        [NotNull]
        public string HardwareVolumeLabel;

        /// <summary>
        /// Volume label reported by AnyDVD HD in <c>disc.inf</c> (if the file is present).
        /// </summary>
        /// <example><code>49123204_BLACK_HAWK_DOWN</code></example>
        /// <example><code>THE_PHANTOM_MENACE</code></example>
        [CanBeNull]
        public string AnyDVDVolumeLabel;

        /// <summary>
        /// Map of ALL <c>BDMV/META/DL/bdmt_xxx.xml</c> languages (where <c>xxx</c> is the 3-letter
        /// ISO-639-2 language code in lowercase) to the titles (movie names) contained therein.
        /// May contain blank or useless titles.
        /// </summary>
        [NotNull]
        public IDictionary<Language, string> AllBdmtTitles = new Dictionary<Language, string>();

        /// <summary>
        /// Same as <see cref="AllBdmtTitles"/>, but with blank/useless titles removed.
        /// Should only contain valid, useful titles.
        /// </summary>
        [NotNull]
        public IDictionary<Language, string> ValidBdmtTitles = new Dictionary<Language, string>();

        /// <summary>
        /// Title of the movie extracted from the D-BOX XML file <c>FilmIndex.xml</c> (if it exists) in the BD-ROM root directory.
        /// </summary>
        [CanBeNull]
        public string DBOXTitle;

        /// <summary>
        /// The child V-ISAN (ISAN Version) number that identifies the particular version (release) of the movie on Blu-ray, if present on the disc.
        /// </summary>
        [CanBeNull]
        public ISAN V_ISAN;

        /// <summary>
        /// The parent ISAN number that identifies the original work (i.e., the original movie first released in theaters), if present on the disc.
        /// </summary>
        [CanBeNull]
        public ISAN ISAN;
    }
}