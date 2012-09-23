using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BDAutoMuxer.views
{
    public static class FormUtils
    {
        /// <summary>
        /// Iterates through the Control's child Controls recursively and adds an event handler
        /// to each TextBox that allows the user to press CTRL + A to select all text.
        /// </summary>
        /// <param name="control"></param>
        public static void TextBox_EnableSelectAll(Control parentControl)
        {
            foreach (Control control in ControlFinder.Descendants<Control>(parentControl))
            {
                if (control is TextBox)
                {
                    (control as TextBox).KeyPress += TextBox_KeyPress;
                }
            }
        }

        /// <see cref="http://www.dzone.com/snippets/ctrl-shortcut-select-all-text"/>
        public static void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x1')
            {
                ((TextBox)sender).SelectAll();
                e.Handled = true;
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
                T childOfT = child as T;
                if (childOfT != null)
                {
                    yield return (T)childOfT;
                }

                if (child.HasChildren)
                {
                    foreach (T descendant in Descendants<T>(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }
    }
}
