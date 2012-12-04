﻿//============================================================================
// BDInfo - Blu-ray Video and Audio Analysis Tool
// Copyright © 2010 Cinema Squid
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//=============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Forms;
using BDAutoMuxer.controllers;
using BDAutoMuxer.models;
using BDAutoMuxer.views;
using ComboBox = System.Windows.Forms.ComboBox;
using ToolTip = System.Windows.Forms.ToolTip;

// ReSharper disable EmptyGeneralCatchClause
namespace BDAutoMuxer
{
    public partial class FormSettings : Form
    {
        private readonly Language[] _audienceLanguages;
        private readonly MIAudioCodec[] _bdInfoAudioCodecs;

        public FormSettings()
        {
            InitializeComponent();

            _audienceLanguages = Language.AllLanguages.OrderBy(lang => lang.ISO_639_2).ToArray();
            _bdInfoAudioCodecs = MICodec.AudioCodecs.Where(codec => codec.StreamType != TSStreamType.Unknown && codec.IsMuxable).ToArray();

            checkBoxFilterLoopingPlaylists.Checked = BDAutoMuxerSettings.FilterLoopingPlaylists;
            checkBoxFilterShortPlaylists.Checked = BDAutoMuxerSettings.FilterShortPlaylists;
            textBoxFilterShortPlaylistsValue.Text = BDAutoMuxerSettings.FilterShortPlaylistsValue.ToString();
            checkBoxKeepStreamOrder.Checked = BDAutoMuxerSettings.KeepStreamOrder;
            checkBoxEnableSSIF.Checked = BDAutoMuxerSettings.EnableSSIF;
            textBoxApiKey.Text = BDAutoMuxerSettings.ApiKey;
            checkBoxCheckForUpdates.Checked = BDAutoMuxerSettings.CheckForUpdates;

            InitAudienceLanguage();
            InitPreferredAudioCodecs();

            FormUtils.TextBox_EnableSelectAll(this);
        }

        private void InitAudienceLanguage()
        {
            comboBoxAudienceLanguage.DataSource = _audienceLanguages;
            comboBoxAudienceLanguage.DisplayMember = "UIDisplayName";

            var selectedIndex = comboBoxAudienceLanguage.Items.IndexOf(BDAutoMuxerSettings.AudienceLanguage);
            if (selectedIndex > -1)
                comboBoxAudienceLanguage.SelectedIndex = selectedIndex;

            comboBoxAudienceLanguage.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxAudienceLanguage.AutoCompleteSource = AutoCompleteSource.ListItems;

            comboBoxAudienceLanguage.Validating += AudienceLanguageValidating;
        }

        private void AudienceLanguageValidating(object sender, CancelEventArgs cancelEventArgs)
        {
            var text = comboBoxAudienceLanguage.Text;
            if (comboBoxAudienceLanguage.Items.OfType<Language>().All(lang => lang.UIDisplayName != text))
                cancelEventArgs.Cancel = true;
        }

        private void InitPreferredAudioCodecs()
        {
            checkedListBoxAudioCodecs.DataSource = _bdInfoAudioCodecs;
            checkedListBoxAudioCodecs.DisplayMember = "FullNameDisambig";

            foreach (var index in BDAutoMuxerSettings.PreferredAudioCodecs.Select(audioCodec => checkedListBoxAudioCodecs.Items.IndexOf(audioCodec)).Where(index => index > -1))
            {
                checkedListBoxAudioCodecs.SetItemChecked(index, true);
            }
        }

        private Language SelectedAudienceLanguage
        {
            get { return comboBoxAudienceLanguage.SelectedValue as Language; }
        }

        private ISet<MIAudioCodec> SelectedAudioCodecs
        {
            get { return new HashSet<MIAudioCodec>(checkedListBoxAudioCodecs.CheckedItems.OfType<MIAudioCodec>()); }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            BDAutoMuxerSettings.KeepStreamOrder = checkBoxKeepStreamOrder.Checked;
            BDAutoMuxerSettings.FilterLoopingPlaylists = checkBoxFilterLoopingPlaylists.Checked;
            BDAutoMuxerSettings.FilterShortPlaylists = checkBoxFilterShortPlaylists.Checked;
            BDAutoMuxerSettings.EnableSSIF = checkBoxEnableSSIF.Checked;
            BDAutoMuxerSettings.ApiKey = textBoxApiKey.Text;
            BDAutoMuxerSettings.CheckForUpdates = checkBoxCheckForUpdates.Checked;
            int filterShortPlaylistsValue;
            if (int.TryParse(textBoxFilterShortPlaylistsValue.Text, out filterShortPlaylistsValue))
            {
                BDAutoMuxerSettings.FilterShortPlaylistsValue = filterShortPlaylistsValue;
            }
            BDAutoMuxerSettings.AudienceLanguage = SelectedAudienceLanguage;
            BDAutoMuxerSettings.PreferredAudioCodecs = SelectedAudioCodecs;
            BDAutoMuxerSettings.SaveSettings();
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonCheckForUpdates_Click(object sender, EventArgs e)
        {
            buttonCheckForUpdates.Enabled = false;
            UpdateNotifier.CheckForUpdate(this, true, delegate() { buttonCheckForUpdates.Enabled = true; });
        }
    }

    public static class BDAutoMuxerSettings
    {
        private static readonly HashSet<MIAudioCodec> AudioCodecsHD = new HashSet<MIAudioCodec>() { MICodec.LPCM, MICodec.DTSHDMA, MICodec.TrueHD };
        private const string HDOnlyValue = "hd_only";
        private const string AllValueText = "all";
        private const string AllValueChar = "*";

        public static string ConfigDir
        {
            get
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                return Path.GetDirectoryName(config.FilePath);
            }
        }

        public static string AssemblyName
        {
            get
            {
                Assembly asm = Properties.Settings.Default.GetType().Assembly;
                return asm.GetName().Name;
            }
        }

        public static Version AssemblyVersion
        {
            get
            {
                return Properties.Settings.Default.GetType().Assembly.GetName().Version;
            }
        }

        public static string AssemblyVersionDisplay
        {
            get
            {
                return Regex.Replace(AssemblyVersion.ToString(), @"^(\d+\.\d+\.\d+)\.\d+$", "$1");
            }
        }

        public static bool EnableSSIF
        {
            get
            {
                try { return Properties.Settings.Default.EnableSSIF; }
                catch { return true; }
            }

            set
            {
                try { Properties.Settings.Default.EnableSSIF = value; }
                catch { }
            }
        }

        public static bool FilterLoopingPlaylists
        {
            get
            {
                try { return Properties.Settings.Default.FilterLoopingPlaylists; }
                catch { return false; }
            }

            set
            {
                try { Properties.Settings.Default.FilterLoopingPlaylists = value; }
                catch { }
            }
        }

        public static bool FilterShortPlaylists
        {
            get
            {
                try { return Properties.Settings.Default.FilterShortPlaylists; }
                catch { return false; }
            }

            set
            {
                try { Properties.Settings.Default.FilterShortPlaylists = value; }
                catch { }
            }
        }

        public static int FilterShortPlaylistsValue
        {
            get
            {
                try { return Properties.Settings.Default.FilterShortPlaylistsValue; }
                catch { return 0; }
            }

            set
            {
                try { Properties.Settings.Default.FilterShortPlaylistsValue = value; }
                catch { }
            }
        }

        public static bool KeepStreamOrder
        {
            get
            {
                try { return Properties.Settings.Default.KeepStreamOrder; }
                catch { return false; }
            }

            set
            {
                try { Properties.Settings.Default.KeepStreamOrder = value; }
                catch { }
            }
        }

        public static string ApiKey
        {
            get
            {
                try { return Properties.Settings.Default.ApiKey; }
                catch { return ""; }
            }

            set
            {
                try { Properties.Settings.Default.ApiKey = value; }
                catch { }
            }
        }

        public static string LastPath
        {
            get
            {
                try { return Properties.Settings.Default.LastPath; }
                catch { return ""; }
            }

            set
            {
                try { Properties.Settings.Default.LastPath = value; }
                catch { }
            }
        }

        public static string OutputDir
        {
            get
            {
                try { return Properties.Settings.Default.OutputDir; }
                catch { return ""; }
            }

            set
            {
                try { Properties.Settings.Default.OutputDir = value; }
                catch { }
            }
        }

        public static string OutputFileName
        {
            get
            {
                try { return Properties.Settings.Default.OutputFileName; }
                catch { return ""; }
            }

            set
            {
                try { Properties.Settings.Default.OutputFileName = value; }
                catch { }
            }
        }

        public static bool DetailsWindowMaximized
        {
            get
            {
                try { return Properties.Settings.Default.DetailsWindowMaximized; }
                catch { return false; }
            }

            set
            {
                try { Properties.Settings.Default.DetailsWindowMaximized = value; }
                catch { }
            }
        }

        public static Size DetailsWindowSize
        {
            get
            {
                try { return Properties.Settings.Default.DetailsWindowSize; }
                catch { return Size.Empty; }
            }

            set
            {
                try { Properties.Settings.Default.DetailsWindowSize = value; }
                catch { }
            }
        }

        public static Point DetailsWindowLocation
        {
            get
            {
                try { return Properties.Settings.Default.DetailsWindowLocation; }
                catch { return Point.Empty; }
            }

            set
            {
                try { Properties.Settings.Default.DetailsWindowLocation = value; }
                catch { }
            }
        }

        public static bool CheckForUpdates
        {
            get
            {
                try { return Properties.Settings.Default.CheckForUpdates; }
                catch { return true; }
            }

            set
            {
                try { Properties.Settings.Default.CheckForUpdates = value; }
                catch { }
            }
        }

        public static bool ReplaceSpaces
        {
            get
            {
                try { return Properties.Settings.Default.ReplaceSpaces; }
                catch { return false; }
            }

            set
            {
                try { Properties.Settings.Default.ReplaceSpaces = value; }
                catch { }
            }
        }

        public static string ReplaceSpacesWith
        {
            get
            {
                try { return Properties.Settings.Default.ReplaceSpacesWith; }
                catch { return ""; }
            }

            set
            {
                try { Properties.Settings.Default.ReplaceSpacesWith = value; }
                catch { }
            }
        }

        public static bool DemuxLPCM
        {
            get
            {
                try { return Properties.Settings.Default.DemuxLPCM; }
                catch { return true; }
            }

            set
            {
                try { Properties.Settings.Default.DemuxLPCM = value; }
                catch { }
            }
        }

        public static bool DemuxSubtitles
        {
            get
            {
                try { return Properties.Settings.Default.DemuxSubtitles; }
                catch { return true; }
            }

            set
            {
                try { Properties.Settings.Default.DemuxSubtitles = value; }
                catch { }
            }
        }

        public static Language AudienceLanguage
        {
            get
            {
                Language lang = null;
                try { lang = Language.GetLanguage(Properties.Settings.Default.AudienceLanguage); }
                catch {}
                return lang ?? Language.CurrentUILanguage;
            }

            set
            {
                try { Properties.Settings.Default.AudienceLanguage = value != null ? value.ISO_639_2 : ""; }
                catch { }
            }
        }

        public static ISet<MIAudioCodec> PreferredAudioCodecs
        {
            get
            {
                string str = null;

                try { str = Properties.Settings.Default.PreferredAudioCodecs; }
                catch { }

                str = str != null ? str.ToLowerInvariant() : null;

                // All audio codecs
                if (str == AllValueText || str == AllValueChar)
                    return new HashSet<MIAudioCodec>(MICodec.MuxableBDAudioCodecs);

                // HD only
                if (str == HDOnlyValue)
                    return AudioCodecsHD;

                return new HashSet<MIAudioCodec>(MICodec.DeserializeCodecs<MIAudioCodec>(str));
            }

            set
            {
                string serialized;
                var valueCount = value.Count();
                var allAudio = new HashSet<MIAudioCodec>(MICodec.MuxableBDAudioCodecs);

                // All audio codecs
                if (allAudio.Intersect(value).Count() == valueCount && valueCount == allAudio.Count())
                    serialized = AllValueText;

                // HD only
                else if (AudioCodecsHD.Intersect(value).Count() == valueCount && valueCount == AudioCodecsHD.Count())
                    serialized = HDOnlyValue;

                // Custom
                else
                    serialized = MICodec.SerializeCodecs(value);

                try { Properties.Settings.Default.PreferredAudioCodecs = serialized; }
                catch { }
            }
        }

        public static void SaveSettings()
        {
            try
            {
                Properties.Settings.Default.Save();
            }
            catch { }
        }
    }
}
