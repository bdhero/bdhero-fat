using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDAutoMuxer.BDInfo;
using BDAutoMuxer.BDROM;
using BDAutoMuxer.controllers;
using BrightIdeasSoftware;

namespace BDAutoMuxer
{
    public partial class FormPlaylistsTest : Form
    {
        #region Private Fields

        private readonly Disc _disc;

        #endregion

        #region Private Playlist Properties and Methods

        private Playlist SelectedPlaylist
        {
            get { return objectListViewPlaylists.SelectedObject as Playlist; }
        }

        private List<Playlist> PlaylistsDisplayable
        {
            get { return _disc.Playlists.Where(ShowPlaylist).ToList(); }
        }

        private bool ShowPlaylist(Playlist playlist)
        {
            if (playlist.Type == TrackType.Commentary && !checkBoxHideCommentary.Checked)
                return false;
            if (playlist.Type == TrackType.SpecialFeature && !checkBoxHideSpecialFeatures.Checked)
                return false;
            if (playlist.Type == TrackType.Misc && !checkBoxHideMisc.Checked)
                return false;
            if (playlist.IsBogus && checkBoxHideBogus.Checked)
                return false;
            if (!playlist.IsMaxQuality && checkBoxHideLowQuality.Checked)
                return false;
            return true;
        }

        #endregion

        #region Private Tracks Properties and Methods

        private List<Track> TracksDisplayable
        {
            get { return SelectedPlaylist != null ? SelectedPlaylist.Tracks.Where(ShowTrack).ToList() : new List<Track>(); }
        }

        private bool ShowTrack(Track track)
        {
            return true;
        }

        #endregion

        public FormPlaylistsTest(BDInfo.BDROM bdrom)
        {
            InitializeComponent();

            _disc = Disc.Transform(bdrom);

            objectListViewPlaylists.CellEditStarting += HandleCellEditStarting;
            objectListViewTracks.CellEditStarting += HandleCellEditStarting;

            UpdatePlaylists();
        }

        private void HandleCellEditStarting(object sender, CellEditEventArgs e)
        {
            ComboBox combo = new ComboBox();
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.Bounds = e.CellBounds;
//            combo.FlatStyle = FlatStyle.Flat;

            if (e.Value is TrackTypeDisplayable)
            {
                var type = (TrackTypeDisplayable) e.Value;
                var trackTypeDisplayables = TrackTypeDisplayable.List.ToArray();
                combo.DataSource = trackTypeDisplayables;
                combo.ValueMember = "Value";
                combo.DisplayMember = "Displayable";
                e.Control = combo;
            }
            if (e.Value is LanguageComboBoxWrapper)
            {
                var language = e.Value as LanguageComboBoxWrapper;
                var languageComboBoxWrappers = _disc.Languages.Select(lang => lang.ComboBoxWrapper).ToArray();
                combo.DataSource = languageComboBoxWrappers;
                combo.ValueMember = "Value";
                combo.DisplayMember = "Displayable";
                e.Control = combo;
            }
        }

        private void UpdatePlaylists(object sender = null, EventArgs e = null)
        {
            // Set data model and sorting
            objectListViewPlaylists.SetObjects(PlaylistsDisplayable);
            objectListViewPlaylists.ListViewItemSorter =
                new ColumnComparer(playlistTypeColumn, SortOrder.Ascending,
                                   playlistWarningsColumn, SortOrder.Ascending);

            // Resize columns
            objectListViewPlaylists.AutoResizeColumnsSmart(SystemInformation.VerticalScrollBarWidth * 2);
        }

        private void SelectedPlaylistChanged(object sender = null, EventArgs e = null)
        {
            if (SelectedPlaylist == null || objectListViewPlaylists.SelectedObjects.Count != 1)
            {
                objectListViewTracks.ClearObjects();
                return;
            }

            // Set data model
            objectListViewTracks.SetObjects(TracksDisplayable);

            // Resize columns
            objectListViewTracks.AutoResizeColumnsSmart(SystemInformation.VerticalScrollBarWidth * 2);
        }
    }
}
