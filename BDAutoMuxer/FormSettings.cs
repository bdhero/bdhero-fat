//============================================================================
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
using BDAutoMuxer.Properties;
using BDAutoMuxer.controllers;
using BDAutoMuxer.models;
using BDAutoMuxer.views;
using ComboBox = System.Windows.Forms.ComboBox;
using ContextMenu = System.Windows.Forms.ContextMenu;
using MenuItem = System.Windows.Forms.MenuItem;
using ToolTip = System.Windows.Forms.ToolTip;

// ReSharper disable EmptyGeneralCatchClause
namespace BDAutoMuxer
{
    public partial class FormSettings : Form
    {
        private readonly Language[] _audienceLanguages;
        private readonly MIAudioCodec[] _bdInfoAudioCodecs;

        private bool _ignoreAudioCheckEvent;

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
            checkBoxSelectHighestChannelCount.Checked = BDAutoMuxerSettings.SelectHighestChannelCount;

            // Disable 
            checkBoxCheckForUpdates.Enabled = !UpdateNotifier.IsClickOnce;

            InitAudienceLanguage();
            InitPreferredAudioCodecs();
            InitSelectButton();

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

        private bool AreAllAudioCodecsChecked(ItemCheckEventArgs e = null)
        {
            var delta = e == null ? 0 : (e.NewValue == CheckState.Checked ? +1 : (e.NewValue == CheckState.Unchecked ? -1 : 0));
            return checkedListBoxAudioCodecs.CheckedItems.Count + delta == checkedListBoxAudioCodecs.Items.Count;
        }

        private void InitSelectButton()
        {
            var contextMenu = new ContextMenu();

            var selectAllMenuItem = new MenuItem("&All");
            var selectLosslessMenuItem = new MenuItem("&Lossless");
            var selectLossyMenuItem = new MenuItem("L&ossy");
            var selectNoneMenuItem = new MenuItem("&None");

            selectAllMenuItem.Click += SelectAllAudioCodecs;
            selectLosslessMenuItem.Click += SelectLosslessAudioCodecs;
            selectLossyMenuItem.Click += SelectLossyAudioCodecs;
            selectNoneMenuItem.Click += SelectNoAudioCodecs;

            contextMenu.MenuItems.Add(selectAllMenuItem);
            contextMenu.MenuItems.Add(selectLosslessMenuItem);
            contextMenu.MenuItems.Add(selectLossyMenuItem);
            contextMenu.MenuItems.Add(selectNoneMenuItem);

            splitButtonSelectAudioCodecs.ShowSplit = true;
            splitButtonSelectAudioCodecs.SplitMenu = contextMenu;
            splitButtonSelectAudioCodecs.Click += (sender, args) =>
                                                      {
                                                          _ignoreAudioCheckEvent = true;
                                                          if (AreAllAudioCodecsChecked())
                                                              SelectNoAudioCodecs();
                                                          else
                                                              SelectAllAudioCodecs();
                                                          _ignoreAudioCheckEvent = false;
                                                          AutoSetSelectButtonText();
                                                      };

            checkedListBoxAudioCodecs.ItemCheck += (sender, args) => AutoSetSelectButtonText(args);

            AutoSetSelectButtonText();
        }

        private void AutoSetSelectButtonText(ItemCheckEventArgs args = null)
        {
            if (_ignoreAudioCheckEvent) return;
            splitButtonSelectAudioCodecs.Text = AreAllAudioCodecsChecked(args) ? "Select &none" : "Select &all";
        }

        private void SelectAllAudioCodecs(object sender = null, EventArgs e = null)
        {
            for (var i = 0; i < checkedListBoxAudioCodecs.Items.Count; i++)
            {
                checkedListBoxAudioCodecs.SetItemChecked(i, true);
            }
        }

        private void SelectLosslessAudioCodecs(object sender = null, EventArgs e = null)
        {
            for (var i = 0; i < checkedListBoxAudioCodecs.Items.Count; i++)
            {
                var audioCodec = checkedListBoxAudioCodecs.Items[i] as MIAudioCodec;
                checkedListBoxAudioCodecs.SetItemChecked(i, audioCodec != null && audioCodec.Lossless);
            }
        }

        private void SelectLossyAudioCodecs(object sender = null, EventArgs e = null)
        {
            for (var i = 0; i < checkedListBoxAudioCodecs.Items.Count; i++)
            {
                var audioCodec = checkedListBoxAudioCodecs.Items[i] as MIAudioCodec;
                checkedListBoxAudioCodecs.SetItemChecked(i, audioCodec != null && audioCodec.Lossy);
            }
        }

        private void SelectNoAudioCodecs(object sender = null, EventArgs e = null)
        {
            for (var i = 0; i < checkedListBoxAudioCodecs.Items.Count; i++)
            {
                checkedListBoxAudioCodecs.SetItemChecked(i, false);
            }
        }

        private void SaveSettings(object sender = null, EventArgs e = null)
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
            BDAutoMuxerSettings.SelectHighestChannelCount = checkBoxSelectHighestChannelCount.Checked;
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
        private static readonly Settings UserSettings = Settings.Default;
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
                return  UserSettings.GetType().Assembly.GetName().Name;
            }
        }

        public static Version AssemblyVersion
        {
            get
            {
                return UserSettings.GetType().Assembly.GetName().Version;
            }
        }

        public static string AssemblyVersionDisplay
        {
            get
            {
                return Regex.Replace(AssemblyVersion.ToString(), @"^(\d+\.\d+\.\d+)\.\d+$", "$1");
            }
        }

        public static bool UpgradeRequired
        {
            get
            {
                try { return UserSettings.UpgradeRequired; }
                catch { return true; }
            }

            set
            {
                try { UserSettings.UpgradeRequired = value; }
                catch { }
            }
        }

        public static bool EnableSSIF
        {
            get
            {
                try { return UserSettings.EnableSSIF; }
                catch { return true; }
            }

            set
            {
                try { UserSettings.EnableSSIF = value; }
                catch { }
            }
        }

        public static bool FilterLoopingPlaylists
        {
            get
            {
                try { return UserSettings.FilterLoopingPlaylists; }
                catch { return false; }
            }

            set
            {
                try { UserSettings.FilterLoopingPlaylists = value; }
                catch { }
            }
        }

        public static bool FilterShortPlaylists
        {
            get
            {
                try { return UserSettings.FilterShortPlaylists; }
                catch { return false; }
            }

            set
            {
                try { UserSettings.FilterShortPlaylists = value; }
                catch { }
            }
        }

        public static int FilterShortPlaylistsValue
        {
            get
            {
                try { return UserSettings.FilterShortPlaylistsValue; }
                catch { return 0; }
            }

            set
            {
                try { UserSettings.FilterShortPlaylistsValue = value; }
                catch { }
            }
        }

        public static bool KeepStreamOrder
        {
            get
            {
                try { return UserSettings.KeepStreamOrder; }
                catch { return false; }
            }

            set
            {
                try { UserSettings.KeepStreamOrder = value; }
                catch { }
            }
        }

        public static string ApiKey
        {
            get
            {
                try { return UserSettings.ApiKey; }
                catch { return ""; }
            }

            set
            {
                try { UserSettings.ApiKey = value; }
                catch { }
            }
        }

        public static string LastPath
        {
            get
            {
                try { return UserSettings.LastPath; }
                catch { return ""; }
            }

            set
            {
                try { UserSettings.LastPath = value; }
                catch { }
            }
        }

        public static string OutputDir
        {
            get
            {
                try { return UserSettings.OutputDir; }
                catch { return ""; }
            }

            set
            {
                try { UserSettings.OutputDir = value; }
                catch { }
            }
        }

        public static string OutputFileName
        {
            get
            {
                try { return UserSettings.OutputFileName; }
                catch { return ""; }
            }

            set
            {
                try { UserSettings.OutputFileName = value; }
                catch { }
            }
        }

        public static bool DetailsWindowMaximized
        {
            get
            {
                try { return UserSettings.DetailsWindowMaximized; }
                catch { return false; }
            }

            set
            {
                try { UserSettings.DetailsWindowMaximized = value; }
                catch { }
            }
        }

        public static Size DetailsWindowSize
        {
            get
            {
                try { return UserSettings.DetailsWindowSize; }
                catch { return Size.Empty; }
            }

            set
            {
                try { UserSettings.DetailsWindowSize = value; }
                catch { }
            }
        }

        public static Point DetailsWindowLocation
        {
            get
            {
                try { return UserSettings.DetailsWindowLocation; }
                catch { return Point.Empty; }
            }

            set
            {
                try { UserSettings.DetailsWindowLocation = value; }
                catch { }
            }
        }

        public static bool CheckForUpdates
        {
            get
            {
                try { return UserSettings.CheckForUpdates; }
                catch { return true; }
            }

            set
            {
                try { UserSettings.CheckForUpdates = value; }
                catch { }
            }
        }

        public static bool ReplaceSpaces
        {
            get
            {
                try { return UserSettings.ReplaceSpaces; }
                catch { return false; }
            }

            set
            {
                try { UserSettings.ReplaceSpaces = value; }
                catch { }
            }
        }

        public static string ReplaceSpacesWith
        {
            get
            {
                try { return UserSettings.ReplaceSpacesWith; }
                catch { return ""; }
            }

            set
            {
                try { UserSettings.ReplaceSpacesWith = value; }
                catch { }
            }
        }

        public static bool DemuxLPCM
        {
            get
            {
                try { return UserSettings.DemuxLPCM; }
                catch { return true; }
            }

            set
            {
                try { UserSettings.DemuxLPCM = value; }
                catch { }
            }
        }

        public static bool DemuxSubtitles
        {
            get
            {
                try { return UserSettings.DemuxSubtitles; }
                catch { return true; }
            }

            set
            {
                try { UserSettings.DemuxSubtitles = value; }
                catch { }
            }
        }

        public static Language AudienceLanguage
        {
            get
            {
                Language lang = null;
                try { lang = Language.GetLanguage(UserSettings.AudienceLanguage); }
                catch {}
                return lang ?? Language.CurrentUILanguage;
            }

            set
            {
                try { UserSettings.AudienceLanguage = value != null ? value.ISO_639_2 : ""; }
                catch { }
            }
        }

        public static ISet<MIAudioCodec> PreferredAudioCodecs
        {
            get
            {
                string str = null;

                try { str = UserSettings.PreferredAudioCodecs; }
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

                try { UserSettings.PreferredAudioCodecs = serialized; }
                catch { }
            }
        }

        public static bool SelectHighestChannelCount
        {
            get
            {
                try { return UserSettings.SelectHighestChannelCount; }
                catch { return true; }
            }

            set
            {
                try { UserSettings.SelectHighestChannelCount = value; }
                catch { }
            }
        }

        /// <summary>
        /// Migrates a previous version's user.config file to the current version's user.config if necessary.
        /// </summary>
        public static void UpgradeFromPreviousVersion()
        {
            // ClickOnce automatically upgrades user.config settings when it auto-updates the application.
            if (UpdateNotifier.IsClickOnce || !UpgradeRequired)
                return;
            try
            {
                UserSettings.Upgrade();
                UpgradeRequired = false;
                SaveSettings();
            }
            catch { }
        }

        public static void SaveSettings()
        {
            try
            {
                UserSettings.Save();
            }
            catch { }
        }
    }
}
