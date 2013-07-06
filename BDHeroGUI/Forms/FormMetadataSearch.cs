﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BDHeroGUI.Forms
{
    public partial class FormMetadataSearch : Form
    {
        /// <summary>
        /// Gets or sets the text of the search query TextBox.
        /// </summary>
        public string SearchQuery
        {
            get { return textBoxSearchQuery.Text; }
            set { textBoxSearchQuery.Text = value; }
        }

        /// <summary>
        /// Maximum width to auto-resize.
        /// </summary>
        private int _maxAutoResizeWidth;

        /// <summary>
        /// Determines whether the form should auto-resize itself when the value of the TextBox changes due to user input.
        /// </summary>
        private bool _autoResize = true;

        /// <summary>
        /// Flag set by <see cref="AutoResize"/> to notify <see cref="OnResize"/> to ignore resize events generated by auto-resizing the form.
        /// </summary>
        private bool _ignoreResize;

        public FormMetadataSearch(string searchQuery)
        {
            InitializeComponent();

            SearchQuery = searchQuery;

            Load += OnLoad;
            Resize += OnResize;
            textBoxSearchQuery.TextChanged += (sender, args) => AutoResize();
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            _maxAutoResizeWidth = Screen.FromControl(this).WorkingArea.Width / 2;

            AutoResize();

            MinimumSize = new Size(Width, Height);
            MaximumSize = new Size(int.MaxValue, Height);
        }

        private void OnResize(object sender, EventArgs eventArgs)
        {
            if (!_autoResize)
                return;

            // User manually resized the form; stop auto-resizing
            if (!_ignoreResize)
                _autoResize = false;

            _ignoreResize = false;
        }

        /// <summary>
        /// Automatically resizes the form to best fit the text in the TextBox.
        /// Respects upper and lower limits to prevent the form from resizing
        /// too small or taking up the entire width of the screen.
        /// </summary>
        private void AutoResize()
        {
            if (!_autoResize)
                return;

            _ignoreResize = true;

            using (Graphics g = CreateGraphics())
            {
                var before = textBoxSearchQuery.Width;
                var size = g.MeasureString(textBoxSearchQuery.Text + "MM", textBoxSearchQuery.Font);
                var after = (int)Math.Ceiling(size.Width);
                var delta = after - before;
                var width = Width + delta;
                Width = Math.Min(width, _maxAutoResizeWidth);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
