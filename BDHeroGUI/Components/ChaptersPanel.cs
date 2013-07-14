﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using DotNetUtils.Annotations;
using DotNetUtils.Extensions;

namespace BDHeroGUI.Components
{
    public partial class ChaptersPanel : UserControl
    {
        #region Properties

        public Playlist Playlist
        {
            get { return _playlist; }
            set { LoadChapters(_playlist = value); }
        }

        private Playlist _playlist;

        public ChapterSearchResult SelectedSearchResult
        {
            get
            {
                int index = SelectedSearchResultIndex;
                if (index == -1)
                    return null;
                return comboBoxSearchResults.Items[index] as ChapterSearchResult;
            }
        }

        public int SelectedSearchResultIndex
        {
            get { return comboBoxSearchResults.SelectedIndex; }
        }

        #endregion

        #region Constructor and OnLoad

        public ChaptersPanel()
        {
            InitializeComponent();

            comboBoxSearchResults.DisplayMember = "Title";
            comboBoxSearchResults.SelectedIndexChanged += ComboBoxSearchResultsOnSelectedIndexChanged;

            listViewChapters.KeyDown += ListViewChaptersOnKeyDown;
            listViewChapters.DoubleClick += ListViewChaptersOnDoubleClick;
            listViewChapters.AfterLabelEdit += ListViewChaptersOnAfterLabelEdit;

            Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            listViewChapters.SetSortColumn(columnHeaderIndex.Index);
        }

        #endregion

        private void LoadChapters(Playlist playlist)
        {
            comboBoxSearchResults.Items.Clear();

            if (playlist == null)
                return;

            comboBoxSearchResults.Items.Add(new ChapterSearchResult { Title = "Default", Chapters = CopyChapters(playlist.Chapters) });

            foreach (var result in playlist.ChapterSearchResults)
            {
                comboBoxSearchResults.Items.Add(result);
            }

            comboBoxSearchResults.SelectedIndex = playlist.ChapterSearchResults.Count > 1 ? 1 : 0;
        }

        private IList<Chapter> CopyChapters(IList<Chapter> chapters)
        {
            return
                chapters.Select(
                    chapter =>
                    new Chapter(chapter.Number, chapter.StartTime.TotalSeconds)
                        {
                            Title = chapter.Title,
                            Language = chapter.Language
                        }).ToList();
        }

        #region UI event handlers

        private void EditSelectedListViewItem()
        {
            var items = listViewChapters.SelectedItems.OfType<ListViewItem>().ToArray();
            if (!items.Any())
                return;
            items.First().BeginEdit();
        }

        private void ListViewChaptersOnKeyDown(object sender, KeyEventArgs args)
        {
            if (args.KeyCode == Keys.F2)
            {
                EditSelectedListViewItem();
            }
        }

        private void ListViewChaptersOnDoubleClick(object sender, EventArgs eventArgs)
        {
            EditSelectedListViewItem();
        }

        private void ListViewChaptersOnAfterLabelEdit(object sender, LabelEditEventArgs args)
        {
            Playlist.Chapters[args.Item].Title = args.Label;

            if (SelectedSearchResult != null)
            {
                SelectedSearchResult.Chapters[args.Item].Title = args.Label;
            }
        }

        private void ComboBoxSearchResultsOnSelectedIndexChanged(object sender, EventArgs args)
        {
            listViewChapters.Items.Clear();

            if (SelectedSearchResult == null)
                return;

            // Mark selected search result as such
            Playlist.ChapterSearchResults.ForEach(result => result.IsSelected = (result == SelectedSearchResult));

            // If "Default" is selected, reset chapter titles to null, which sets them to "Chapter 1", "Chapter 2", etc.
            ReplaceChapters(Playlist.Chapters, SelectedSearchResultIndex > 0 ? SelectedSearchResult.Chapters : null);
        }

        #endregion

        private void ReplaceChapters(IList<Chapter> chapters, [CanBeNull] IList<Chapter> searchResult)
        {
            var i = 0;
            foreach (var chapter in chapters)
            {
                // If "Default" is selected, reset chapter titles to null, which sets them to "Chapter 1", "Chapter 2", etc.
                chapter.Title = searchResult != null ? searchResult[i].Title : null;
                listViewChapters.Items.Add(ToListItem(chapter));
                i++;
            }
            listViewChapters.AutoSizeColumns();
        }

        private ListViewItem ToListItem(Chapter chapter)
        {
            var item = new ListViewItem(chapter.Title) { Tag = chapter };
            var subitems = new[]
                {
                    new ListViewItem.ListViewSubItem(item, chapter.StartTime.ToStringMedium()) { Tag = chapter.StartTime },
                    new ListViewItem.ListViewSubItem(item, chapter.Number.ToString("D")) { Tag = chapter.Number }
                };
            item.SubItems.AddRange(subitems);
            return item;
        }
    }
}