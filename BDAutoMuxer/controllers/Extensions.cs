using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BDAutoMuxer.controllers
{
    /// <see cref="http://stackoverflow.com/a/2984664/467582"/>
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
            {
                collection.Add(item);
            }
        }
    }

    public static class StringExtensions
    {
        /// <summary>
        /// Converts the string to Title Case (a.k.a., Proper Case).
        /// </summary>
        public static String ToTitle(this String str)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(str);
        }
    }
}
