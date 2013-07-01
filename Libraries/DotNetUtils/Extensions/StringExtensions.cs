using System;
using System.Threading;

namespace DotNetUtils.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="String"/>s.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the string to Title Case (a.k.a., Proper Case).
        /// </summary>
        public static String ToTitle(this String str)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;
            var titleCase = textInfo.ToTitleCase(textInfo.ToLower(str));
            return titleCase;
        }
    }
}