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
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BDAutoMuxer.controllers;
using BDAutoMuxer.views;

// ReSharper disable EmptyGeneralCatchClause
namespace BDAutoMuxer
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();

            checkBoxFilterLoopingPlaylists.Checked = BDAutoMuxerSettings.FilterLoopingPlaylists;
            checkBoxFilterShortPlaylists.Checked = BDAutoMuxerSettings.FilterShortPlaylists;
            textBoxFilterShortPlaylistsValue.Text = BDAutoMuxerSettings.FilterShortPlaylistsValue.ToString();
            checkBoxKeepStreamOrder.Checked = BDAutoMuxerSettings.KeepStreamOrder;
            checkBoxEnableSSIF.Checked = BDAutoMuxerSettings.EnableSSIF;
            textBoxApiKey.Text = BDAutoMuxerSettings.ApiKey;
            checkBoxCheckForUpdates.Checked = BDAutoMuxerSettings.CheckForUpdates;

            FormUtils.TextBox_EnableSelectAll(this);
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
