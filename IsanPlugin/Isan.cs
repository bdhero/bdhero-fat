using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DotNetUtils.Annotations;

namespace IsanPlugin
{
    /// <summary>
    /// Immutable.
    /// </summary>
    /// TODO: Write unit tests
    public class Isan
    {
        private const string ZeroRoot = "000000000000";
        private static readonly Regex IsanRegex = new Regex(@"\b([0-9a-f]{4})([0-9a-f]{4})([0-9a-f]{4})([0-9a-f]{4})[a-z]?([0-9a-f]{4})([0-9a-f]{4})[a-z]?\b", RegexOptions.IgnoreCase);

        public readonly string Number;
        public readonly string NumberFormatted;

        public string NumberFullFormatted;

        public readonly string Root;
        public readonly string RootFormatted;

        public readonly string Episode;

        public readonly string Version;
        public readonly string VersionFormatted;

        public string Title;
        public int? Year;
        public int? LengthMin;

        public bool IsSearchable { get { return Root != ZeroRoot; } }

        protected Isan(string n1, string n2, string n3, string n4, string n5, string n6)
        {
            Root = string.Format("{0}{1}{2}", n1, n2, n3);
            RootFormatted = string.Format("{0}-{1}-{2}", n1, n2, n3);

            Episode = n4;

            Version = string.Format("{0}{1}", n5, n6);
            VersionFormatted = string.Format("{0}-{1}", n5, n6);

            Number = string.Format("{0}{1}{2}", Root, Episode, Version);
            NumberFormatted = string.Format("{0}-{1}-{2}", RootFormatted, Episode, VersionFormatted);
        }

        public static bool IsIsan(string number)
        {
            number = number.Replace("-", "");
            return IsanRegex.IsMatch(number);
        }

        [CanBeNull]
        public static Isan TryParse(string number)
        {
            if (!IsIsan(number))
                return null;

            var n = Parse(number);
            
            return new Isan(n[0], n[1], n[2], n[3], n[4], n[5]);
        }

        protected static string[] Parse(string number)
        {
            number = number.Replace("-", "");
            return !IsIsan(number) ? null : IsanRegex.Match(number).Groups.OfType<Group>().Skip(1).Select(@group => @group.Value).ToArray();
        }
    }
}
