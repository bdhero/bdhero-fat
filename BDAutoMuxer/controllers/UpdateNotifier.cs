using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BDAutoMuxer.controllers
{
    public delegate void UpdateNotifierCompleteDelegate();

    /// <see cref="http://stackoverflow.com/a/11867784/467582"/>
    [System.ComponentModel.DesignerCategory("Code")]
    public class UpdateNotifier : BackgroundWorker
    {
        private static readonly string download_url = "https://github.com/acdvorak/bdautomuxer/downloads";
        private static readonly string version_url = "https://raw.github.com/acdvorak/bdautomuxer/master/version.txt";
        private static readonly string regex = @"^(\d+\.\d+\.\d+(?:\.\d+)?)\s+(\d{4}/\d{1,2}/\d{1,2})$";

        private Version currentVersion;
        private Version latestVersion;
        private DateTime latestDate;
        private bool isUpdateAvailable = false;

        public Version CurrentVersion { get { return currentVersion; } }
        public Version LatestVersion { get { return latestVersion; } }
        public DateTime LatestDate { get { return latestDate; } }
        public bool IsUpdateAvailable { get { return isUpdateAvailable; } }

        public UpdateNotifier()
        {
            this.currentVersion = BDAutoMuxerSettings.AssemblyVersion;
            this.DoWork += CheckForUpdate;
        }

        private void CheckForUpdate(object sender, DoWorkEventArgs e)
        {
            try
            {
                string versionTxt = HttpRequest.Get(version_url);

                if (Regex.IsMatch(versionTxt, regex))
                {
                    Match match = Regex.Match(versionTxt, regex);

                    string strLatestVersion = match.Groups[1].Value;
                    string strLatestDate = match.Groups[2].Value;

                    if (Version.TryParse(strLatestVersion, out latestVersion) && DateTime.TryParse(strLatestDate, out latestDate))
                    {
                        isUpdateAvailable = latestVersion > currentVersion;
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        public static void CheckForUpdate(Form form, bool notifyIfUpToDate = false, UpdateNotifierCompleteDelegate onComplete = null)
        {
            UpdateNotifier updateNotifier = new UpdateNotifier();
            updateNotifier.RunWorkerCompleted += (object sender, RunWorkerCompletedEventArgs e) =>
            {
                if (updateNotifier.IsUpdateAvailable)
                {
                    DialogResult result = MessageBox.Show(form,
                        string.Format("An updated version of {0} is available!\n\nYour version: {1}\nNew version: {2} ({3:d})\n\nWould you like to visit the download page?", BDAutoMuxerSettings.AssemblyName, updateNotifier.CurrentVersion, updateNotifier.LatestVersion, updateNotifier.LatestDate),
                        "Update available",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        Process.Start(download_url);
                    }
                }
                else if (notifyIfUpToDate)
                {
                    if (e.Result != null && e.Result is Exception)
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
