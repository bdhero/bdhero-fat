﻿using System;
using System.Collections.Generic;
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

            listViewChapters.AfterLabelEdit += ListViewChaptersOnAfterLabelEdit;
            listViewChapters.ItemChecked += ListViewChaptersOnItemChecked;

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
            {
                comboBoxSearchResults.Enabled = false;
                ComboBoxSearchResultsOnSelectedIndexChanged();
                return;
            }

            comboBoxSearchResults.Enabled = true;

            // TODO: Move this elsewhere so we don't erase the user's custom titles when they re-select "Default"
            comboBoxSearchResults.Items.Add(new ChapterSearchResult { Title = "Default", Chapters = CopyChapters(playlist.Chapters) });

            foreach (var result in playlist.ChapterSearchResults)
            {
                comboBoxSearchResults.Items.Add(result);
            }

            comboBoxSearchResults.SelectedIndex = playlist.ChapterSearchResults.Any() ? 1 : 0;
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

        private void ComboBoxSearchResultsOnSelectedIndexChanged(object sender = null, EventArgs args = null)
        {
            listViewChapters.SuspendDrawing();
            listViewChapters.Items.Clear();

            if (SelectedSearchResult != null)
            {
                // Mark selected search result as such
                Playlist.ChapterSearchResults.ForEach(result => result.IsSelected = (result == SelectedSearchResult));

                // If "Default" is selected, reset chapter titles to null, which sets them to "Chapter 1", "Chapter 2", etc.
                ReplaceChapters(Playlist.Chapters, SelectedSearchResultIndex > 0 ? SelectedSearchResult.Chapters : null);
            }

            listViewChapters.ResumeDrawing();
        }

        private void ListViewChaptersOnAfterLabelEdit(object sender, LabelEditEventArgs args)
        {
            var index = args.Item;
            var text = args.Label;

            // The new text to associate with the ListViewItem or null if the text is unchanged.
            // http://msdn.microsoft.com/en-us/library/system.windows.forms.labelediteventargs.label(v=vs.100).aspx
            if (text == null) { return; }

            Playlist.Chapters[index].Title = text;

            if (SelectedSearchResult != null)
            {
                SelectedSearchResult.Chapters[index].Title = text;
            }
        }

        private void ListViewChaptersOnItemChecked(object sender, ItemCheckedEventArgs args)
        {
            var index = args.Item.Index;
            var isChecked = args.Item.Checked;

            Playlist.Chapters[index].Keep = isChecked;

            if (SelectedSearchResult != null)
            {
                SelectedSearchResult.Chapters[index].Keep = isChecked;
            }
        }

        #endregion

        private void ReplaceChapters(IList<Chapter> playlistChapters, [CanBeNull] IList<Chapter> searchResult)
        {
            for (var i = 0; i < playlistChapters.Count; i++)
            {
                var playlistChapter = playlistChapters[i];

                // If "Default" is selected, reset chapter titles to null, which sets them to "Chapter 1", "Chapter 2", etc.
                playlistChapter.Title = searchResult != null ? searchResult[i].Title : null;
                playlistChapter.Keep = searchResult == null || searchResult[i].Keep;

                listViewChapters.Items.Add(ToListItem(playlistChapter));
            }
            listViewChapters.AutoSizeColumns();
        }

        private ListViewItem ToListItem(Chapter chapter)
        {
            var item = new ListViewItem(chapter.Title) { Tag = chapter, Checked = chapter.Keep };
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
