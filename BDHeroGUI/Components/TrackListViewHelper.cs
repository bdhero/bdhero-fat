using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using DotNetUtils.Controls;
using DotNetUtils.Extensions;
using I18N;

namespace BDHeroGUI.Components
{
    class TrackListViewHelper
    {
        public Playlist Playlist
        {
            get { return _playlist; }
            set
            {
                _playlist = value;

                _listView.Items.Clear();

                if (_playlist == null) return;

                var items = Transform(_playlist.Tracks);
                _listView.Items.AddRange(items);
                _listView.AutoSizeColumns();
            }
        }

        private Playlist _playlist;

        private readonly ListView2 _listView;
        private readonly Func<Track, bool> _filter;
        private readonly Func<Track, ICollection<ListViewCell>> _transform;

        public TrackListViewHelper(ListView2 listView, Func<Track, bool> filter, Func<Track, ICollection<ListViewCell>> transform)
        {
            _listView = listView;
            _filter = filter;
            _transform = transform;

            _listView.MultiSelect = true;
            _listView.ItemCheck += ListViewOnItemCheck;
            _listView.ItemChecked += ListViewOnItemChecked;

            InitContextMenu();
        }

        public void OnLoad(object sender = null, EventArgs eventArgs = null)
        {
            _listView.SetSortColumn(_listView.FirstDisplayedColumn.Index);
            _listView.AutoSizeColumns();
        }

        private void InitContextMenu()
        {
            _listView.MouseClick += ListViewOnMouseClick;
        }

        private void ListViewOnMouseClick(object sender, MouseEventArgs args)
        {
            if (args.Button != MouseButtons.Right)
                return;

            var pos = args.Location;
            var listViewItem = _listView.GetItemAt(pos.X, pos.Y);

            if (listViewItem == null)
                return;

            var track = listViewItem.Tag as Track;

            if (track == null)
                return;

            var menu = new ContextMenuStrip();

            AddLanguagesMenuItem(listViewItem, track, menu);

            foreach (var trackType in Enum.GetValues(typeof (TrackType)).OfType<TrackType>())
            {
                var type = trackType;
                var menuItem = new ToolStripMenuItem(trackType.ToString());
                if (track.Type == trackType)
                {
                    menuItem.Checked = true;
                    menuItem.Enabled = false;
                }
                menuItem.Click += (s, e) => TrackTypeMenuItemOnClick(listViewItem, track, type);
                menu.Items.Add(menuItem);
            }

            menu.Show(_listView, args.Location);
        }

        private void AddLanguagesMenuItem(ListViewItem listViewItem, Track track, ContextMenuStrip menu)
        {
            // Only allow users to change the language on video tracks
            if (!track.IsVideo)
                return;

            var langs = new HashSet<Language>();
            foreach (var track2 in Playlist.Tracks)
            {
                langs.Add(track2.Language);
            }

            var languagesMenuItem = new ToolStripMenuItem("Language");
            foreach (var language in langs.OrderBy(language => language.Name))
            {
                var lang = language;
                var langMenuItem = new ToolStripMenuItem(language.Name);

                langMenuItem.Click += (s, e) => LanguageMenuItemOnClick(listViewItem, track, lang);

                if (track.Language.Equals(language))
                {
                    langMenuItem.Checked = true;
                    langMenuItem.Enabled = false;
                }

                languagesMenuItem.DropDownItems.Add(langMenuItem);
            }

            menu.Items.Add(languagesMenuItem);
            menu.Items.Add("-");
        }

        private void LanguageMenuItemOnClick(ListViewItem listViewItem, Track track, Language language)
        {
            track.Language = language;
            var listViewSubItems = listViewItem.SubItems.OfType<ListViewItem.ListViewSubItem>().ToArray();
            var languageSubItems = listViewSubItems.Where(subItem => subItem.Tag is Language).ToArray();
            foreach (var subItem in languageSubItems)
            {
                subItem.Tag = language;
                subItem.Text = language.Name;
            }
        }

        private void TrackTypeMenuItemOnClick(ListViewItem listViewItem, Track track, TrackType trackType)
        {
            track.Type = trackType;
            foreach (var subItem in listViewItem.SubItems.OfType<ListViewItem.ListViewSubItem>().Where(subItem => subItem.Tag is TrackType))
            {
                subItem.Tag = trackType;
                subItem.Text = trackType.ToString();
            }
        }

        private void ListViewOnItemCheck(object sender, ItemCheckEventArgs e)
        {
            var track = _listView.Items[e.Index].Tag as Track;
            if (track == null) return;
            if (ShouldDisable(track))
            {
                e.NewValue = CheckState.Unchecked;
            }
        }

        private static void ListViewOnItemChecked(object sender, ItemCheckedEventArgs args)
        {
            var track = args.Item.Tag as Track;
            if (track != null)
            {
                track.Keep = args.Item.Checked;
            }
        }

        private static bool IsBestChoice(Track track)
        {
            return track.IsBestGuess;
        }

        private static bool ShouldDisable(Track track)
        {
            return !track.Codec.IsKnown || !track.Codec.IsMuxable;
        }

        private static bool ShouldMarkHidden(Track track)
        {
            return track.IsHidden;
        }

        private static void MarkBestChoice(ListViewItem item)
        {
            item.MarkBestChoice();
            item.AppendToolTip("Best choice based on your preferences");
        }

        private static void VisuallyDisable(ListViewItem item)
        {
            item.VisuallyDisable();
            item.AppendToolTip("Unsupported codec: cannot be muxed");
        }

        private static void MarkHidden(ListViewItem item)
        {
            item.MarkHidden();
            item.AppendToolTip("Hidden track");
        }

        private ListViewItem[] Transform(IEnumerable<Track> tracks)
        {
            return tracks.Where(_filter).Select(delegate(Track track)
                {
                    var cells = _transform(track);
                    var firstCell = cells.First();
                    var subCells = cells.Skip(1);

                    var item = new ListViewItem(firstCell.Text)
                        {
                            Checked = track.Keep,
                            Tag = track,
                            UseItemStyleForSubItems = true
                        };

                    item.SubItems.AddRange(subCells.Select(cell => CreateListViewSubItem(item, cell)).ToArray());

                    if (IsBestChoice(track))
                        MarkBestChoice(item);

                    if (ShouldDisable(track))
                        VisuallyDisable(item);

                    if (ShouldMarkHidden(track))
                        MarkHidden(item);

                    return item;
                }).ToArray();
        }

        private static ListViewItem.ListViewSubItem CreateListViewSubItem(ListViewItem item, ListViewCell cell)
        {
            return new ListViewItem.ListViewSubItem(item, cell.Text) {Tag = cell.Tag};
        }
    }
}
