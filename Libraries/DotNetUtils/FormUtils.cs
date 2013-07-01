using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DotNetUtils
{
    public static class FormUtils
    {
        /// <summary>
        /// Iterates through the Control's child Controls recursively and adds an event handler
        /// to each TextBox that allows the user to press CTRL + A to select all text.
        /// </summary>
        /// <param name="parentControl"></param>
        public static void TextBox_EnableSelectAll(Control parentControl)
        {
            var controls = parentControl.Descendants<Control>().ToList();
            foreach (var control in controls.OfType<TextBox>())
            {
                control.KeyPress += TextBox_KeyPress;
            }
            foreach (var control in controls.OfType<ListView>())
            {
                control.KeyPress += ListView_KeyPress;
            }
        }

        /// <see cref="http://www.dzone.com/snippets/ctrl-shortcut-select-all-text"/>
        public static void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\x1') return;
            var textBox = sender as TextBox;
            if (textBox == null) return;

            textBox.SelectAll();
            e.Handled = true;
        }

        /// <see cref="http://www.dzone.com/snippets/ctrl-shortcut-select-all-text"/>
        public static void ListView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\x1') return;
            var listView = sender as ListView;
            if (listView == null) return;

            foreach (var x in listView.Items.OfType<ListViewItem>())
            {
                x.Selected = true;
            }

            e.Handled = true;
        }

        public static void EnableControls(TabPage page, bool enable)
        {
            EnableControls(page.Controls, enable);
        }

        public static void EnableControls(Control.ControlCollection ctls, bool enable)
        {
            foreach (Control ctl in ctls)
            {
                ctl.Enabled = enable;
                EnableControls(ctl.Controls, enable);
            }
        }
    }

    public static class ControlFinder
    {
        /// <see cref="http://stackoverflow.com/a/2735242/467582"/>
        public static IEnumerable<T> Descendants<T>(this Control control) where T : class
        {
            foreach (Control child in control.Controls)
            {
                var childOfT = child as T;

                if (childOfT != null)
                {
                    yield return childOfT;
                }

                if (!child.HasChildren) continue;

                foreach (var descendant in Descendants<T>(child))
                {
                    yield return descendant;
                }
            }
        }
    }

    public static class ListViewExtensions
    {
        public static void AutoSizeColumns(this ListView listView)
        {
            // Loop through and size each column header to fit the column header text.
            foreach (ColumnHeader ch in listView.Columns)
            {
                ch.Width = -2;
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
    }

    public static class ControlExtensions
    {
        /// <summary>
        /// Sets whether the control is first drawn to a buffer rather than directly to the screen,
        /// which can reduce flicker.  If this method is called while the user is in a Terminal session,
        /// double buffering will be automatically disabled to improve performance.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="doubleBuffered">
        /// If <c>true</c>, the control is first drawn to a buffer rather than directly to the screen,
        /// which can reduce flicker.  If you set this property to <c>true</c>, you should also set
        /// the <see cref="ControlStyles.AllPaintingInWmPaint"/> flag to true.
        /// </param>
        /// <seealso cref="http://stackoverflow.com/a/77233/467582"/>
        public static void SetDoubleBuffered(this Control control, bool doubleBuffered)
        {
            System.Reflection.PropertyInfo prop =
                  typeof(Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            // Taxes: Remote Desktop Connection and painting
            // http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            var @value = doubleBuffered && !SystemInformation.TerminalServerSession;

            prop.SetValue(control, @value, null);

            // Alternative:
//            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}
