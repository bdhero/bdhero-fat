using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BDAutoMuxer.controllers
{
    public delegate void UpdateNotifierCompleteDelegate();

    /// <see cref="http://stackoverflow.com/a/11867784/467582"/>
// ReSharper disable LocalizableElement
// ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
// ReSharper restore RedundantNameQualifier
// ReSharper restore LocalizableElement
    public class UpdateNotifier : BackgroundWorker
    {
        private const string DownloadUrl = "https://github.com/acdvorak/bdautomuxer/downloads";
        private const string VersionUrl = "https://raw.github.com/acdvorak/bdautomuxer/master/version.txt";
        private const string VersionRegex = @"^(\d+\.\d+\.\d+(?:\.\d+)?)\s+(\d{4}/\d{1,2}/\d{1,2})$";

        private readonly Version _currentVersion;
        private Version _latestVersion;
        private DateTime _latestDate;

        public Version CurrentVersion { get { return _currentVersion; } }
        public Version LatestVersion { get { return _latestVersion; } }
        public DateTime LatestDate { get { return _latestDate; } }
        public bool IsUpdateAvailable { get; private set; }

        public UpdateNotifier()
        {
            _currentVersion = BDAutoMuxerSettings.AssemblyVersion;
            DoWork += CheckForUpdate;
        }

        private void CheckForUpdate(object sender, DoWorkEventArgs e)
        {
            try
            {
                var versionTxt = HttpRequest.Get(VersionUrl);

                if (!Regex.IsMatch(versionTxt, VersionRegex)) return;

                var match = Regex.Match(versionTxt, VersionRegex);

                var strLatestVersion = match.Groups[1].Value;
                var strLatestDate = match.Groups[2].Value;

                if (Version.TryParse(strLatestVersion, out _latestVersion) && DateTime.TryParse(strLatestDate, out _latestDate))
                {
                    IsUpdateAvailable = _latestVersion > _currentVersion;
                }
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        public static void CheckForUpdate(Form form, bool notifyIfUpToDate = false, UpdateNotifierCompleteDelegate onComplete = null)
        {
            var updateNotifier = new UpdateNotifier();

            updateNotifier.RunWorkerCompleted += (sender, e) =>
            {
                if (updateNotifier.IsUpdateAvailable)
                {
                    var result = MessageBox.Show(form,
                        string.Format("An updated version of {0} is available!\n\nYour version: {1}\nNew version: {2} ({3:d})\n\nWould you like to visit the download page?", BDAutoMuxerSettings.AssemblyName, updateNotifier.CurrentVersion, updateNotifier.LatestVersion, updateNotifier.LatestDate),
                        "Update available",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        Process.Start(DownloadUrl);
                    }
                }
                else if (notifyIfUpToDate)
                {
                    if (e.Result is Exception)
                    {
                        MessageBox.Show(form,
                            string.Format("{0}", ((Exception)e.Result).Message),
                            "BDAutoMuxer Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show(form,
                            string.Format("You are using the latest version of {0} ({1}).", BDAutoMuxerSettings.AssemblyName, BDAutoMuxerSettings.AssemblyVersionDisplay),
                            "No updates available",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                if (onComplete != null)
                {
                    onComplete();
                }
            };
            updateNotifier.RunWorkerAsync();
        }
    }
}
