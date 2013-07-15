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
using DotNetUtils;
using DotNetUtils.Extensions;
using I18N;

namespace BDHero.Plugin.FileNamer
{
    internal partial class FormFileNamerPreferences : Form
    {
        private const string DateFormatUrl = "http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.100).aspx";

        private readonly Preferences _userPrefs;
        private readonly Preferences _prefsCopy;

        private readonly Job _movieJob;
        private readonly FileNamer _movieNamer;

        private readonly Job _tvShowJob;
        private readonly FileNamer _tvShowNamer;

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
            _userPrefs = prefs;
            _prefsCopy = _userPrefs.Clone();

            _movieJob = MockJobFactory.CreateMovieJob();
            _movieNamer = new FileNamer(_movieJob, _prefsCopy);

            _tvShowJob = MockJobFactory.CreateTVShowJob();
            _tvShowNamer = new FileNamer(_tvShowJob, _prefsCopy);

            InitializeComponent();
            Load += OnLoad;
            this.EnableSelectAll();
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            InitValues();
            InitReplaceSpaces();
            InitCodecListView();
            InitTextBoxEvents();
            InitComboBoxEvents();
            InitLinks();
            Rename();
        }

        #endregion

        #region Initialization

        private static string NF2S(string numberFormat)
        {
            return numberFormat == "D2" ? "01" : "1";
        }

        private static string S2NF(string text)
        {
            return text == "01" ? "D2" : "D1";
        }

        private void InitValues()
        {
            checkBoxReplaceSpaces.Checked = _prefsCopy.ReplaceSpaces;
            textBoxReplaceSpacesWith.Enabled = _prefsCopy.ReplaceSpaces;
            textBoxReplaceSpacesWith.Text = _prefsCopy.ReplaceSpacesWith;

            textBoxMovieDirectory.Text = _prefsCopy.Movies.Directory;
            textBoxMovieFileName.Text = _prefsCopy.Movies.FileName;

            textBoxTVShowDirectory.Text = _prefsCopy.TVShows.Directory;
            textBoxTVShowFileName.Text = _prefsCopy.TVShows.FileName;
            comboBoxSeasonNumberFormat.SelectedItem = NF2S(_prefsCopy.TVShows.SeasonNumberFormat);
            comboBoxEpisodeNumberFormat.SelectedItem = NF2S(_prefsCopy.TVShows.EpisodeNumberFormat);
            textBoxTVShowReleaseDateFormat.Text = _prefsCopy.TVShows.ReleaseDateFormat;
        }

        private void InitReplaceSpaces()
        {
            checkBoxReplaceSpaces.CheckedChanged += CheckBoxReplaceSpacesOnCheckedChanged;
            textBoxReplaceSpacesWith.TextChanged += TextBoxReplaceSpacesWithOnTextChanged;

            CheckBoxReplaceSpacesOnCheckedChanged();
            TextBoxReplaceSpacesWithOnTextChanged();
        }

        private void InitCodecListView()
        {
            listViewCodecNames.AfterLabelEdit += ListViewCodecNamesOnAfterLabelEdit;
            PopulateCodecListView();
            listViewCodecNames.SetSortColumn(columnHeaderNumber.Index);
        }

        private void PopulateCodecListView()
        {
            listViewCodecNames.SuspendDrawing();
            listViewCodecNames.Items.Clear();

            var codecs = Codec.MuxableBDCodecs.Where(codec => _prefsCopy.Codecs.ContainsKey(codec.SerializableName)).ToArray();

            var i = 1;
            foreach (var codec in codecs)
            {
                var label = _prefsCopy.Codecs[codec.SerializableName];
                var group = codec.IsVideo
                                ? listViewCodecNames.Groups["listViewGroupVideo"]
                                : codec.IsAudio
                                      ? listViewCodecNames.Groups["listViewGroupAudio"]
                                      : listViewCodecNames.Groups["listViewGroupSubtitles"];
                var item = new ListViewItem(label, @group) {Tag = codec};
                var subitems = new[]
                    {
                        new ListViewItem.ListViewSubItem(item, codec.FullNameDisambig),
                        new ListViewItem.ListViewSubItem(item, i.ToString("D")) {Tag = i}
                    };
                item.SubItems.AddRange(subitems);
                listViewCodecNames.Items.Add(item);
                i++;
            }

            listViewCodecNames.ResumeDrawing();
            listViewCodecNames.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void InitTextBoxEvents()
        {
            textBoxMovieDirectory.TextChanged += (s, e) => Rename();
            textBoxMovieFileName.TextChanged += (s, e) => Rename();

            textBoxTVShowDirectory.TextChanged += (s, e) => Rename();
            textBoxTVShowFileName.TextChanged += (s, e) => Rename();

            textBoxTVShowReleaseDateFormat.TextChanged += textBoxTVShowReleaseDateFormat_TextChanged;
        }

        private void InitComboBoxEvents()
        {
            comboBoxSeasonNumberFormat.SelectedIndexChanged += (s, e) => Rename();
            comboBoxEpisodeNumberFormat.SelectedIndexChanged += (s, e) => Rename();
        }

        private void InitLinks()
        {
            new ToolTip().SetToolTip(linkLabelTVShowReleaseDateFormat, DateFormatUrl);
        }

        #endregion

        #region UI events

        private void Rename()
        {
            OnChanged(_movieNamer, _tvShowNamer);
        }

        private void OnChanged(FileNamer movieNamer, FileNamer tvShowNamer)
        {
            // Movies

            _prefsCopy.Movies.Directory = textBoxMovieDirectory.Text;
            _prefsCopy.Movies.FileName = textBoxMovieFileName.Text;

            var moviePath = movieNamer.GetPath();

            textBoxMovieDirectoryExample.Text = moviePath.Directory;
            textBoxMovieFileNameExample.Text = moviePath.FileName;

            // TV Shows

            _prefsCopy.TVShows.Directory = textBoxTVShowDirectory.Text;
            _prefsCopy.TVShows.FileName = textBoxTVShowFileName.Text;
            _prefsCopy.TVShows.SeasonNumberFormat = S2NF(comboBoxSeasonNumberFormat.SelectedItem as string);
            _prefsCopy.TVShows.EpisodeNumberFormat = S2NF(comboBoxEpisodeNumberFormat.SelectedItem as string);
            _prefsCopy.TVShows.ReleaseDateFormat = textBoxTVShowReleaseDateFormat.Text;

            var tvShowPath = tvShowNamer.GetPath();

            textBoxTVShowDirectoryExample.Text = tvShowPath.Directory;
            textBoxTVShowFileNameExample.Text = tvShowPath.FileName;
        }

        private void CheckBoxReplaceSpacesOnCheckedChanged(object sender = null, EventArgs eventArgs = null)
        {
            textBoxReplaceSpacesWith.Enabled = checkBoxReplaceSpaces.Checked;
            _prefsCopy.ReplaceSpaces = checkBoxReplaceSpaces.Checked;
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
            _prefsCopy.ReplaceSpacesWith = textBoxReplaceSpacesWith.Text;
            Rename();
        }

        private void ListViewCodecNamesOnAfterLabelEdit(object sender, LabelEditEventArgs args)
        {
            if (args.CancelEdit || args.Label == null)
                return;

            var index = args.Item;
            var label = args.Label;

            if (SelectedCodec != null)
            {
                _prefsCopy.Codecs[SelectedCodec.SerializableName] = label;
            }

            Rename();
        }

        private void textBoxTVShowReleaseDateFormat_TextChanged(object sender, EventArgs e)
        {
            var format = textBoxTVShowReleaseDateFormat.Text;
            try
            {
                var str = DateTime.Now.ToString(format);
                _prefsCopy.TVShows.ReleaseDateFormat = format;
                Rename();
            }
            catch
            {
                // TODO: Tell user the format is wrong
            }
        }

        private void linkLabelTVShowReleaseDateFormat_Click(object sender, EventArgs e)
        {
            FileUtils.OpenUrl(DateFormatUrl);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // TODO: Auto enable/disable this button when settings have changed
            _userPrefs.CopyFrom(_prefsCopy);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            _prefsCopy.CopyFrom(new Preferences());
            Reset();
        }

        private void Reset()
        {
            InitValues();
            PopulateCodecListView();
            Rename();
        }

        #endregion
    }
}
