using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using DotNetUtils.Extensions;

namespace BDHero.Plugin.FileNamer
{
    internal partial class FormFileNamerPreferences : Form
    {
        private readonly Preferences _prefs;

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
            InitializeComponent();
            Load += OnLoad;
            this.EnableSelectAll();
        }

        private void OnLoad(object sender, EventArgs eventArgs)
        {
            InitReplaceSpaces();
            InitCodecListView();
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
            var index = args.Item;
            var label = args.Label;

            if (SelectedCodec != null)
            {
                _prefs.Codecs[SelectedCodec.SerializableName] = label;
            }
        }

        #endregion

        #region UI events

        private void CheckBoxReplaceSpacesOnCheckedChanged(object sender = null, EventArgs eventArgs = null)
        {
            textBoxReplaceSpacesWith.Enabled = checkBoxReplaceSpaces.Checked;
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
