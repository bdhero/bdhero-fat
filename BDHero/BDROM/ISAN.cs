using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotNetUtils.Annotations;

namespace BDHero.BDROM
{
    /// <summary>
    /// ISAN (or ISAN number): International Standard Audiovisual Number.
    /// A globally unique number allocated for the exclusive identification of an AV Work,
    /// in accordance with the ISO 15706-1 standard.
    /// </summary>
    /// <remarks>
    /// <para>
    /// ISANs are globally unique and managed by a central authority.
    /// This gives us a way to uniquely identify Blu-ray Discs, as long as the studio includes it in the
    /// <c>AACS/mcmf.xml</c> file.  The majority of movies contain a valid ISAN, but about 1/3 do not.
    /// </para>
    /// <para>All ISANs have the following format:</para>
    /// <code>
    ///      +----Root----+ +EP+   +Version+
    ///      |            | |  |   |       |
    /// ISAN 0000-0000-14A9-0000-K-0000-0002-A
    ///                          |           |
    ///                          Check   Check
    ///                          Digit   Digit
    /// </code>
    /// <list type="list">
    ///     <item>First 12 digits: Root segment, which identifies the audiovisual work</item>
    ///     <item>Next 4 digits: Episode segment, which identifies the parts of a serialized work</item>
    ///     <item><i>(optional check character)</i></item>
    ///     <item>Last 8 digits: Version segment, which identifies the versions of a work</item>
    ///     <item><i>(optional check character)</i></item>
    /// </list>
    /// <para>
    /// ISANs may optionally contain 2 embedded check characters which guard against errors
    /// resulting from improper transcription of an ISAN.  The check characters are automatically
    /// assigned by a computer algorithm.  They are not mandatory, however, and may safely be omitted.
    /// </para>
    /// <para>Thus:</para>
    /// <code>0000-0000-14A9-0000-K-0000-0002-A</code>
    /// <para>is equivalent to:</para>
    /// <code>0000-0000-14A9-0000-0000-0002</code>
    /// <para>
    /// <see cref="Number"/> and <see cref="NumberFormatted"/> do <b>NOT</b> contain check digits.
    /// The ISAN found in the <c>AACS/mcmf.xml</c> file on Blu-ray Discs does not contain the optional check digits,
    /// so BDHero doesn't bother calculating them.
    /// </para>
    /// <para>
    /// ISANs are hierarchical, with a top-level ISAN identifying the original work, and one or more child V-ISANs
    /// identifying the various versions (releases or editions) of that work on Blu-ray.  According to
    /// the official ISAN user guide (http://www.isan.org/docs/isan_user_guide.pdf):
    /// </para>
    /// <blockquote>
    /// Version means a particular version, or aggregation of elements that affects the content of an AV Work.
    /// For example, any change that affects the content of an AV Work (e.g., artistic content, language, editing,
    /// technical format, distribution) and which requires separate identification for the use or exploitation
    /// of that specific content can be treated as a new Version for the purposes of assigning a V-ISAN.
    /// </blockquote>
    /// <para>
    /// For example:
    /// </para>
    /// <code>
    /// ISAN 0000-0000-14A9-0000-K-0000-0000-E: Blade runner (1982 - 117 min)
    ///     |- V-ISAN 0000-0000-14A9-0000-K-0000-0001-C: Blade Runner (2007 - 118 min): Europe BD Final Cut
    ///     |- V-ISAN 0000-0000-14A9-0000-K-0000-0002-A: Blade Runner (2007 - 118 min): NA BD Branching
    ///     |- V-ISAN 0000-0000-14A9-0000-K-0000-0003-8: Blade Runner (2007 - 140 min): NA BD Work Print
    ///     |- V-ISAN 0000-0000-14A9-0000-K-0000-0004-6: Blade Runner (2007 - 118 min): NA/Japan BD Final Cut
    /// </code>
    /// </remarks>
    public class ISAN
    {
        /// <summary>
        /// Matches a string that ENDS with a valid ISAN (leading characters that don't match are ignored).
        /// </summary>
        private static readonly Regex IsanRegex =
            new Regex(
                "([a-f0-9]{4})-?([a-f0-9]{4})-?([a-f0-9]{4})-?([a-f0-9]{4})-?(?:[a-z0-9]-?)?([a-f0-9]{4})-?([a-f0-9]{4})(?:-?[a-z0-9])?$",
                RegexOptions.IgnoreCase);

        private const string ZeroRoot = "000000000000";

        /// <summary>
        /// Unformatted ISAN without dashes or check digits.
        /// Consists of 24 hexadecimal digits.
        /// </summary>
        /// <example>
        /// <code>
        ///  +---Root---++EP++Version+
        ///  |          ||  ||      |
        ///  0000000014A9000000000002 = V-ISAN for BLADE_RUNNER_BRANCH
        ///  0000000014A9000000000000 = ISAN for BLADE_RUNNER_BRANCH
        /// </code>
        /// </example>
        public string Number;

        /// <summary>
        /// Formatted ISAN with dashes but without check digits.
        /// Consists of 6 sets of 4 hexadecimal digits separated by dashes ('-').
        /// </summary>
        /// <example>
        /// <code>
        ///  +----Root----+ +EP+ +Version+
        ///  |            | |  | |       |
        ///  0000-0000-14A9-0000-0000-0002 = V-ISAN for BLADE_RUNNER_BRANCH
        ///  0000-0000-14A9-0000-0000-0000 = ISAN for BLADE_RUNNER_BRANCH
        /// </code>
        /// </example>
        public string NumberFormatted
        {
            get
            {
                if (IsanRegex.IsMatch(Number))
                {
                    var match = IsanRegex.Match(Number);
                    var groups = match.Groups;
                    return string.Format("{0}-{1}-{2}-{3}-{4}-{5}",
                                         groups[1].Value,
                                         groups[2].Value,
                                         groups[3].Value,
                                         groups[4].Value,
                                         groups[5].Value,
                                         groups[6].Value
                        );
                }
                return null;
            }
        }

        /// <summary>
        /// Name of the audiovisual work (i.e., movie title).
        /// </summary>
        public string Title;

        /// <summary>
        /// Year the audiovisual work was released.  Note that this may differ between ISANs and V-ISANs (e.g., Blade Runner).
        /// </summary>
        public int? Year;

        /// <summary>
        /// Gets the first 3 groups of 4 hex characters without dashes.
        /// </summary>
        public string Root
        {
            get
            {
                var groups = Parse(Number);
                return string.Join("", groups.Take(3));
            }
        }

        /// <summary>
        /// Gets the first 3 groups of 4 hex characters with dashes between each group.
        /// </summary>
        public string RootFormatted
        {
            get
            {
                var groups = Parse(Number);
                return string.Join("-", groups.Take(3));
            }
        }

        /// <summary>
        /// Gets the 4th group of 4 hex characters.
        /// </summary>
        public string Episode
        {
            get
            {
                var groups = Parse(Number);
                return string.Join("", groups.Skip(1).Take(1));
            }
        }

        /// <summary>
        /// Gets the last 2 groups of 4 hex characters without dashes.
        /// </summary>
        public string Version
        {
            get
            {
                var groups = Parse(Number);
                return string.Join("", groups.Skip(4).Take(2));
            }
        }

        /// <summary>
        /// Gets the last 2 groups of 4 characters with dashes between each group.
        /// </summary>
        public string VersionFormatted
        {
            get
            {
                var groups = Parse(Number);
                return string.Join("-", groups.Skip(4).Take(2));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the ISAN's <see cref="Root"/> is valid and searchable (i.e., is not all zeros).
        /// </summary>
        public bool IsValid
        {
            get { return Root != ZeroRoot; }
        }

        public override string ToString()
        {
            return NumberFormatted;
        }

        /// <summary>
        /// Parses any valid ISAN number into 6 individual groups of 4 hex characters each.
        /// </summary>
        /// <param name="number">Any valid ISAN number</param>
        /// <returns>Enumerable containing 6 groups of 4 characters each</returns>
        private static string[] Parse([NotNull] string number)
        {
            var match = IsanRegex.Match(number);
            var groups = match.Groups;
            return new []
                {
                    groups[1].Value,
                    groups[2].Value,
                    groups[3].Value,
                    groups[4].Value,
                    groups[5].Value,
                    groups[6].Value
                };
        }

        /// <summary>
        /// Attempts to parse an ISAN number.
        /// </summary>
        /// <param name="number">Any valid ISAN number</param>
        /// <returns>A new ISAN object if the given number is a valid ISAN number; otherwise <c>null</c></returns>
        [CanBeNull]
        public static ISAN TryParse([CanBeNull] string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return null;

            if (!IsanRegex.IsMatch(number))
                return null;

            var parsed = string.Join("", Parse(number));

            return new ISAN { Number = parsed };
        }
    }
}
