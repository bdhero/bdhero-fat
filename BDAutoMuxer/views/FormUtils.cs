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
        /// <param name="parentControl"></param>
        public static void TextBox_EnableSelectAll(Control parentControl)
        {
            foreach (var control in parentControl.Descendants<Control>().OfType<TextBox>())
            {
                (control as TextBox).KeyPress += TextBox_KeyPress;
            }
        }

        /// <see cref="http://www.dzone.com/snippets/ctrl-shortcut-select-all-text"/>
        public static void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\x1') return;
            ((TextBox)sender).SelectAll();
            e.Handled = true;
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
}
