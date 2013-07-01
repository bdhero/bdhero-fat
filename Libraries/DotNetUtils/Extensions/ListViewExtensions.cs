using System;
using System.Linq;
using System.Windows.Forms;

namespace DotNetUtils.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ListView"/>s.
    /// </summary>
    public static class ListViewExtensions
    {
        /// <summary>
        /// Automatically resizes each column to fit its header <b>and</b> contents, whichever is wider.
        /// </summary>
        /// <param name="listView"></param>
        public static void AutoSizeColumns(this ListView listView)
        {
            // Resize each column to its header text
            foreach (ColumnHeader columnHeader in listView.Columns)
            {
                columnHeader.Width = -2;
            }

            var before = listView.Columns.OfType<ColumnHeader>().Select(header => header.Width).ToArray();

            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            var after = listView.Columns.OfType<ColumnHeader>().Select(header => header.Width).ToArray();

            var i = 0;
            foreach (var column in listView.Columns.OfType<ColumnHeader>())
            {
                column.Width = Math.Max(before[i], after[i]);
                i++;
            }
        }

        /// <summary>
        /// Selects all <see cref="ListViewItem"/>s.
        /// </summary>
        /// <param name="listView"></param>
        public static void SelectAll(this ListView listView)
        {
            SelectWhere(listView, item => true);
        }

        /// <summary>
        /// Deselects all <see cref="ListViewItem"/>s.
        /// </summary>
        /// <param name="listView"></param>
        public static void SelectNone(this ListView listView)
        {
            SelectWhere(listView, item => false);
        }

        /// <summary>
        /// Selects all <see cref="ListViewItem"/>s whose <see cref="ListViewItem.Tag"/> is contained within the list of <paramref name="tagValues"/>.
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="tagValues"></param>
        public static void SelectWithTag(this ListView listView, params object[] tagValues)
        {
            SelectWhere(listView, item => tagValues.Contains(item.Tag));
        }

        /// <summary>
        /// Selects all <see cref="ListViewItem"/>s for which the given <paramref name="condition"/> returns <c>true</c>.
        /// </summary>
        /// <param name="listView"></param>
        /// <param name="condition"></param>
        public static void SelectWhere(this ListView listView, Func<ListViewItem, bool> condition)
        {
            listView.Items.OfType<ListViewItem>().ForEach(item => item.Selected = condition(item));
        }
    }
}