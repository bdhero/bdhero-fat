using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BrightIdeasSoftware;

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
            var titleCase = textInfo.ToTitleCase(textInfo.ToLower(str));
            return titleCase;
        }
    }

    /// <summary>
    /// Dictionary that supports keys with a list of values (one-to-many).
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MultiValueDictionary<TKey, TValue> : Dictionary<TKey, IList<TValue>>
    {
        /// <summary>
        /// Adds the specified value to the list at the specified key.
        /// If the key is not already present in the dictionary, it is added automatically.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            if (!ContainsKey(key))
                this[key] = new List<TValue>();
            this[key].Add(value);
        }
    }

    public static class ToMultiValueDictionaryExtension
    {
        /// <summary>
        /// Converts the Enumerable to a MultiValueDictionary using the specified key provider function.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="values"></param>
        /// <param name="keyProvider"></param>
        /// <returns></returns>
        public static MultiValueDictionary<TKey, TValue> ToMultiValueDictionary<TKey, TValue>(this IEnumerable<TValue> values, Func<TValue, TKey> keyProvider)
        {
            var dic = new MultiValueDictionary<TKey, TValue>();
            foreach (var value in values)
            {
                dic.Add(keyProvider(value), value);
            }
            return dic;
        }
    }

    public static class ObjectListViewExtension
    {
        public static void AutoResizeColumnsSmart(this ObjectListView olv, int paddingRight = 0)
        {
            foreach (var column in olv.AllColumns)
            {
                AutoResizeColumn(column, paddingRight);
            }
            var lastColumn = olv.AllColumns.LastOrDefault();
            if (lastColumn != null)
            {
                lastColumn.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                lastColumn.Width += paddingRight;
            }
        }

        private static void AutoResizeColumn(OLVColumn column, int paddingRight = 0)
        {
            column.AutoResize(ColumnHeaderAutoResizeStyle.None);
            var noneWidth = column.Width;
            column.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            var headerWidth = column.Width;
            column.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            var contentWidth = column.Width;
            column.Width = Math.Max(headerWidth, contentWidth) + paddingRight;
        }
    }
}
