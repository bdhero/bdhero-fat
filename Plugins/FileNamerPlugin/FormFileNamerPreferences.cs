using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using BDHero.JobQueue;
using BDInfo;
using DotNetUtils.Extensions;
using I18N;

namespace BDHero.Plugin.FileNamer
{
    internal partial class FormFileNamerPreferences : Form
    {
        private readonly Preferences _prefs;
        private readonly Job _movieJob;
        private readonly FileNamer _movieNamer;

        #region Properties

        private Codec SelectedCodec
        {
            get
            {
                var indices = listViewCodecNames.SelectedIndices;
                var index = indices.Count > 0 ? indices[0] : -1;
                if (index > -1)
                {
                    var codec = listViewCodecNames.Items[index].Tag as Codec;
                    return codec;
                }
                return null;
            }
        }

        #endregion

        #region Constructor and OnLoad

        public FormFileNamerPreferences(Preferences prefs)
        {
            _prefs = prefs;
            _movieJob = CreateMovieJob();
            _movieNamer = new FileNamer(_movieJob, _prefs);

            InitializeComponent();
            Load += OnLoad;
            this.EnableSelectAll();
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            InitReplaceSpaces();
            InitCodecListView();
            InitTextBoxes();
        }


        private void InitTextBoxes()
        {
            textBoxMovieDirectory.TextChanged += (s, e) => OnChanged(_movieNamer);
            textBoxMovieFileName.TextChanged += (s, e) => OnChanged(_movieNamer);

//            textBoxTVShowDirectory.TextChanged += OnChanged;
//            textBoxTVShowFileName.TextChanged += OnChanged;

            Rename();
        }

        private void OnChanged(FileNamer movieNamer /* , FileNamer tvShowNamer */)
        {
            // Movies

            _prefs.Movies.Directory = textBoxMovieDirectory.Text;
            _prefs.Movies.FileName = textBoxMovieFileName.Text;

            var moviePath = movieNamer.GetPath();

            textBoxMovieDirectoryExample.Text = moviePath.Directory;
            textBoxMovieFileNameExample.Text = moviePath.FileName;

            // TV Shows

//            var tvShowPath = tvShowNamer.GetPath();
//            textBoxTVShowDirectoryExample.Text = Path.GetDirectoryName(tvShowPath);
//            textBoxTVShowFileNameExample.Text = Path.GetFileName(tvShowPath);
        }

        private static Job CreateMovieJob()
        {
            var metadata = new DiscMetadata
                {
                    Derived = new DiscMetadata.DerivedMetadata
                        {
                            VolumeLabel = "EMPIRE_STRIKES_BACK"
                        }
                };
            var disc = new Disc
                {
                    Metadata = metadata,
                    Playlists = new List<Playlist>
                        {
                            new Playlist
                                {
                                    Tracks = new List<Track>
                                        {
                                            new Track
                                                {
                                                    IsVideo = true,
                                                    Codec = Codec.AVC,
                                                    Type = TrackType.MainFeature,
                                                    VideoFormat = TSVideoFormat.VIDEOFORMAT_1080p,
                                                    AspectRatio = TSAspectRatio.ASPECT_16_9,
                                                    Index = 0,
                                                    IndexOfType = 0,
                                                    IsBestGuess = true,
                                                    Keep = true,
                                                    Language = Language.English
                                                },
                                            new Track
                                                {
                                                    IsAudio = true,
                                                    Codec = Codec.DTSHDMA,
                                                    Type = TrackType.MainFeature,
                                                    ChannelCount = 6.1,
                                                    Index = 1,
                                                    IndexOfType = 0,
                                                    IsBestGuess = true,
                                                    Keep = true,
                                                    Language = Language.English
                                                },
                                            new Track
                                                {
                                                    IsSubtitle = true,
                                                    Codec = Codec.PGS,
                                                    Type = TrackType.MainFeature,
                                                    Index = 2,
                                                    IndexOfType = 0,
                                                    IsBestGuess = true,
                                                    Keep = true,
                                                    Language = Language.English
                                                },
                                        }
                                }
                        }
                };
            var job = new Job(disc)
                {
                    ReleaseMediumType = ReleaseMediumType.Movie,
                    SearchQuery = new SearchQuery()
                        {
                            Title = "Star Wars: Episode V - The Empire Strikes Back",
                            Year = 1980,
                            Language = Language.English
                        }
                };
            job.Movies.Add(new Movie
                {
                    IsSelected = true,
                    Title = "Star Wars: Episode V - The Empire Strikes Back",
                    ReleaseYear = 1980,
                    Id = 1891,
                    Url = "http://www.themoviedb.org/movie/1891-star-wars-episode-v-the-empire-strikes-back"
                });
            return job;
        }

        #endregion

        #region Initialization

        private void InitReplaceSpaces()
        {
            checkBoxReplaceSpaces.CheckedChanged += CheckBoxReplaceSpacesOnCheckedChanged;
            textBoxReplaceSpacesWith.TextChanged += TextBoxReplaceSpacesWithOnTextChanged;

            checkBoxReplaceSpaces.Checked = _prefs.ReplaceSpaces;
            textBoxReplaceSpacesWith.Text = _prefs.ReplaceSpacesWith;

            CheckBoxReplaceSpacesOnCheckedChanged();
            TextBoxReplaceSpacesWithOnTextChanged();
        }

        private void InitCodecListView()
        {
            listViewCodecNames.AfterLabelEdit += ListViewCodecNamesOnAfterLabelEdit;
            listViewCodecNames.SuspendDrawing();

            var codecs = Codec.MuxableBDCodecs.Where(codec => _prefs.Codecs.ContainsKey(codec.SerializableName)).ToArray();

            var i = 1;
            foreach (var codec in codecs)
            {
                var label = _prefs.Codecs[codec.SerializableName];
                var group = codec.IsVideo
                                ? listViewCodecNames.Groups["listViewGroupVideo"]
                                : codec.IsAudio
                                      ? listViewCodecNames.Groups["listViewGroupAudio"]
                                      : listViewCodecNames.Groups["listViewGroupSubtitles"];
                var item = new ListViewItem(label, group) { Tag = codec };
                var subitems = new[]
                    {
                        new ListViewItem.ListViewSubItem(item, codec.FullNameDisambig),
                        new ListViewItem.ListViewSubItem(item, i.ToString("D")) { Tag = i }
                    };
                item.SubItems.AddRange(subitems);
                listViewCodecNames.Items.Add(item);
                i++;
            }

            listViewCodecNames.ResumeDrawing();
            listViewCodecNames.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewCodecNames.SetSortColumn(columnHeaderNumber.Index);
        }

        private void ListViewCodecNamesOnAfterLabelEdit(object sender, LabelEditEventArgs args)
        {
            if (args.CancelEdit || args.Label == null)
                return;

            var index = args.Item;
            var label = args.Label;

            if (SelectedCodec != null)
            {
                _prefs.Codecs[SelectedCodec.SerializableName] = label;
            }

            Rename();
        }

        #endregion

        #region UI events

        private void CheckBoxReplaceSpacesOnCheckedChanged(object sender = null, EventArgs eventArgs = null)
        {
            textBoxReplaceSpacesWith.Enabled = checkBoxReplaceSpaces.Checked;
            _prefs.ReplaceSpaces = checkBoxReplaceSpaces.Checked;
            Rename();
        }

        private void TextBoxReplaceSpacesWithOnTextChanged(object sender = null, EventArgs eventArgs = null)
        {
            using (Graphics g = textBoxReplaceSpacesWith.CreateGraphics())
            {
                var before = textBoxReplaceSpacesWith.Width;
                var text = textBoxReplaceSpacesWith.Text + "MM"; // add 2 letters for padding
                var size = g.MeasureString(text, textBoxReplaceSpacesWith.Font);
                var after = (int)Math.Ceiling(size.Width);
                var delta = after - before;
                textBoxReplaceSpacesWith.Width += delta;
            }
            _prefs.ReplaceSpacesWith = textBoxReplaceSpacesWith.Text;
            Rename();
        }

        private void Rename()
        {
            OnChanged(_movieNamer);
//            OnChanged(_tvShowNamer);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        #endregion
    }
}
