using System;
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
        public RawMetadata Raw;

        public DerivedMetadata Derived;

        public class RawMetadata
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
            /// AnyDVD HD <c>disc.inf</c> file (if present).
            /// </summary>
            [CanBeNull]
            public AnyDVDDiscInf DiscInf;

            /// <summary>
            /// Map of ALL <c>BDMV/META/DL/bdmt_xxx.xml</c> languages (where <c>xxx</c> is the 3-letter
            /// ISO-639-2 language code in lowercase) to the titles (movie names) contained therein.
            /// May contain blank or useless titles.
            /// </summary>
            [NotNull]
            public IDictionary<Language, string> AllBdmtTitles = new Dictionary<Language, string>();

            /// <summary>
            /// Gets a list of ALL <c>BDMV/META/DL/bdmt_xxx.xml</c> languages (where <c>xxx</c> is the 3-letter
            /// ISO-639-2 language code in lowercase) on the disc, regardless of whether they contain valid titles or not.
            /// This list may, therefore, contain blank or useless titles.
            /// </summary>
            [NotNull]
            public IList<Language> AllBdmtLanguages
            {
                get { return AllBdmtTitles.Keys.ToList(); }
            }

            /// <summary>
            /// Title of the movie extracted from the D-BOX XML file <c>FilmIndex.xml</c> (if it exists) in the BD-ROM root directory.
            /// </summary>
            [CanBeNull]
            public string DboxTitle;

            /// <summary>
            /// The child V-ISAN (ISAN Version) number that identifies the particular version (release) of the movie on Blu-ray, if present on the disc.
            /// </summary>
            [CanBeNull]
            public ISAN V_ISAN;
        }

        /// <summary>
        /// Contains 
        /// </summary>
        public class DerivedMetadata
        {
            /// <summary>
            /// The value of <see cref="AnyDVDDiscInf.VolumeLabel"/> if it is not <c>null</c>,
            /// otherwise the value of <see cref="RawMetadata.HardwareVolumeLabel"/>.
            /// </summary>
            [NotNull]
            public string VolumeLabel;

            /// <summary>
            /// Same as <see cref="VolumeLabel"/>, but with leading numbers and trailing region markers removed and underscores converted to spaces.
            /// </summary>
            [NotNull]
            public string VolumeLabelSanitized;

            /// <summary>
            /// Same as <see cref="RawMetadata.AllBdmtTitles"/>, but with blank/useless titles removed.
            /// Should only contain valid, useful titles.
            /// Trailing garbage like " - Blu-ray(C)" is also removed;
            /// </summary>
            [NotNull]
            public IDictionary<Language, string> ValidBdmtTitles = new Dictionary<Language, string>();

            /// <summary>
            /// Same as <see cref="RawMetadata.AllBdmtLanguages"/>, but with languages that have blank/useless titles removed.
            /// Should only contain languages that have valid, useful titles.
            /// </summary>
            [NotNull]
            public IList<Language> ValidBdmtLanguages
            {
                get { return ValidBdmtTitles.Keys.ToList(); }
            }

            /// <summary>
            /// Same as <see cref="RawMetadata.DboxTitle"/>, but cleaned to remove useless trailing junk.
            /// </summary>
            [CanBeNull]
            public string DboxTitleSanitized;

            /// <summary>
            /// The parent ISAN number that identifies the original work (i.e., the original movie first released in theaters), if present on the disc.
            /// </summary>
            [CanBeNull]
            public ISAN ISAN;

            /// <summary>
            /// Auto-detected movie name to use for initial database searches (e.g., TMDb).
            /// Derived from <see cref="ValidBdmtTitles"/>, <see cref="DboxTitleSanitized"/>, and <see cref="VolumeLabelSanitized"/>.
            /// </summary>
            public string SearchableTitle;
        }
    }

}
