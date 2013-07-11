using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;

namespace DotNetUtils.Controls
{
    public class ListView2 : ListView
    {
        private readonly ListViewColumnSorter _columnSorter = new ListViewColumnSorter();

        public ListView2()
        {
            FullRowSelect = true;
            GridLines = true;
            View = View.Details;
            AllowColumnReorder = true;
            HideSelection = false;
            MultiSelect = false;
            ShowItemToolTips = true;

            this.SetDoubleBuffered(true);

            ListViewItemSorter = _columnSorter;
            ColumnClick += (_, e) => SetSortColumn(e.Column);

            var isResizing = false;

            // Automatically resize the last column to take up all remaining free space
            Resize += delegate
                {
                    // listView.AutoSizeLastColumn() calls listView.ResumeDrawing(), which raises the Resize event.
                    // To prevent multiple recursive invocations of the Resize event, we make sure it's not already in progress.
                    if (isResizing) return;
                    isResizing = true;
                    this.AutoSizeLastColumn();
                    isResizing = false;
                };
        }

        public void SetSortColumn(int columnIndex)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (columnIndex == _columnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                _columnSorter.Order = _columnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _columnSorter.SortColumn = columnIndex;
                _columnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            Sort();
            this.SetSortIcon(_columnSorter.SortColumn, _columnSorter.Order);
        }

        /// <summary>
        /// Gets the first column on the left based on <see cref="ColumnHeader.DisplayIndex"/>.
        /// </summary>
        public ColumnHeader FirstDisplayedColumn
        {
            [CanBeNull]
            get
            {
                var columnHeaders = Columns.OfType<ColumnHeader>().ToArray();
                var minDisplayIndex = columnHeaders.Min(header => header.DisplayIndex);
                var firstColumn = columnHeaders.FirstOrDefault(header => header.DisplayIndex == minDisplayIndex);
                return firstColumn;
            }
        }

        /// <summary>
        /// Gets the last column on the right based on <see cref="ColumnHeader.DisplayIndex"/>.
        /// </summary>
        public ColumnHeader LastDisplayedColumn
        {
            [CanBeNull]
            get
            {
                var columnHeaders = Columns.OfType<ColumnHeader>().ToArray();
                var maxDisplayIndex = columnHeaders.Max(header => header.DisplayIndex);
                var lastColumn = columnHeaders.LastOrDefault(header => header.DisplayIndex == maxDisplayIndex);
                return lastColumn;
            }
        }
    }
}
