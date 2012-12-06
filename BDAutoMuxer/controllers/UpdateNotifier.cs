using System;
using System.ComponentModel;
using System.Deployment.Application;
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
        public static bool IsClickOnce
        {
            get { return ApplicationDeployment.IsNetworkDeployed; }
        }

        private readonly IUpdateChecker _updateChecker;

        private bool _isUpdateAvailable;

        private readonly Form _form;
        private readonly bool _notifyIfUpToDate;
        private readonly UpdateNotifierCompleteDelegate _onComplete;

        private UpdateNotifier(Form form, bool notifyIfUpToDate = false, UpdateNotifierCompleteDelegate onComplete = null)
        {
            _form = form;
            _notifyIfUpToDate = notifyIfUpToDate;
            _onComplete = onComplete;

            _updateChecker = GetUpdateChecker(form);

            DoWork += CheckForUpdate;
            RunWorkerCompleted += OnRunWorkerCompleted;
        }

        private static IUpdateChecker GetUpdateChecker(Form form)
        {
            if (ApplicationDeployment.IsNetworkDeployed)
                return new ClickOnceUpdateChecker(form);
            return new GitHubUpdateChecker();
        }

        private void CheckForUpdate(object sender, DoWorkEventArgs e)
        {
            try
            {
                _isUpdateAvailable = _updateChecker.IsUpdateAvailable;
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_isUpdateAvailable)
            {
                if (_updateChecker.IsMandatory)
                {
                    MessageBox.Show(_form, _updateChecker.Message, "Mandatory update", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    _updateChecker.Update();
                }
                else if (DialogResult.Yes == MessageBox.Show(_form,  _updateChecker.Message, "Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    _updateChecker.Update();
                }
            }
            else if (_notifyIfUpToDate)
            {
                var exception = (e.Result ?? _updateChecker.Result) as Exception;
                if (exception == null)
                {
                    MessageBox.Show(_form, _updateChecker.Message, "No updates available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string caption = string.Format("{0} Error", BDAutoMuxerSettings.AssemblyName);
                    string message = string.Format("{0}\n\n{1}", _updateChecker.Message, exception.Message);
                    MessageBox.Show(_form, message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (_onComplete != null)
            {
                _onComplete();
            }
        }

        public static void CheckForUpdate(Form form, bool notifyIfUpToDate = false, UpdateNotifierCompleteDelegate onComplete = null)
        {
            var updateNotifier = new UpdateNotifier(form, notifyIfUpToDate, onComplete);
            updateNotifier.RunWorkerAsync();
        }
    }

    public interface IUpdateChecker
    {
        Version CurrentVersion { get; }
        Version AvailableVersion { get; }

        bool IsUpdateAvailable { get; }
        bool IsMandatory { get; }
        void Update();

        object Result { get; }
        string Message { get; }
    }

    /// <see cref="http://msdn.microsoft.com/en-us/library/ms404263.aspx"/>
    public class ClickOnceUpdateChecker : IUpdateChecker
    {
        private readonly Version _currentVersion;
        private Version _latestVersion;

        private ApplicationDeployment _ad;
        private UpdateCheckInfo _info;

        private readonly Form _form;

        public Version CurrentVersion { get { return _currentVersion; } }
        public Version AvailableVersion { get { return _latestVersion; } }

        public bool IsMandatory { get; private set; }

        public ClickOnceUpdateChecker(Form form)
        {
            _currentVersion = BDAutoMuxerSettings.AssemblyVersion;
            _form = form;
        }

        public bool IsUpdateAvailable
        {
            get
            {
                try
                {
                    return CheckForUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    Result = dde;
                    Message = "The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later.";
                }
                catch (InvalidDeploymentException ide)
                {
                    Result = ide;
                    Message =
                        "Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again.";
                }
                catch (InvalidOperationException ioe)
                {
                    // Strangely, this exception gets thrown when the user already has the latest version.
                    Result = ioe;
                    Message = "Unable to update the application.";
                }
                return false;
            }
        }

        private bool CheckForUpdate()
        {
            if (!ApplicationDeployment.IsNetworkDeployed)
                return false;

            _ad = ApplicationDeployment.CurrentDeployment;
            _info = _ad.CheckForDetailedUpdate();

            _latestVersion = _info.AvailableVersion;

            if (_info.UpdateAvailable)
            {
                if (_info.IsUpdateRequired)
                {
                    IsMandatory = true;
                    // Display a message that the app MUST reboot. Display the minimum required version.
                    Message =
                        string.Format(
                            "This application has detected a mandatory update from your current version to version {0}. The application will now install the update and restart.",
                            _info.MinimumRequiredVersion);
                }
                else
                {
                    Message = "An update is available. Would you like to update the application now?";
                }
                return true;
            }
            return false;
        }

        public void Update()
        {
            try
            {
                _ad.Update();
                MessageBox.Show(_form, "The application has been upgraded, and will now restart.");
                Application.Restart();
            }
            catch (DeploymentDownloadException dde)
            {
                Result = dde;
                Message = "Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later.";
            }
        }

        public object Result { get; private set; }
        public string Message { get; private set; }
    }

    public class GitHubUpdateChecker : IUpdateChecker
    {
        private const string DownloadUrl = "https://github.com/acdvorak/bdautomuxer/downloads";
        private const string VersionUrl = "https://raw.github.com/acdvorak/bdautomuxer/master/version.txt";
        private const string VersionRegex = @"^(\d+\.\d+\.\d+(?:\.\d+)?)\s+(\d{4}/\d{1,2}/\d{1,2})$";
        
        private readonly Version _currentVersion;
        private Version _latestVersion;
        private DateTime _latestDate;

        public Version CurrentVersion { get { return _currentVersion; } }
        public Version AvailableVersion { get { return _latestVersion; } }

        public bool IsMandatory { get; private set; }

        public GitHubUpdateChecker()
        {
            IsMandatory = false;
            _currentVersion = BDAutoMuxerSettings.AssemblyVersion;
        }

        public bool IsUpdateAvailable
        {
            get
            {
                try
                {
                    return CheckForUpdate();
                }
                catch (Exception e)
                {
                    Result = e;
                    Message = "An error occurred while checking for updates.";
                }
                return false;
            }
        }

        private bool CheckForUpdate()
        {
            var versionTxt = HttpRequest.Get(VersionUrl);

            if (!Regex.IsMatch(versionTxt, VersionRegex))
            {
                Message = string.Format("No version information found at {0}.", VersionUrl);
                return false;
            }

            var match = Regex.Match(versionTxt, VersionRegex);

            var strLatestVersion = match.Groups[1].Value;
            var strLatestDate = match.Groups[2].Value;

            if (Version.TryParse(strLatestVersion, out _latestVersion) && DateTime.TryParse(strLatestDate, out _latestDate))
            {
                if(_latestVersion > _currentVersion)
                {
                    Message =
                        string.Format(
                            "An updated version of {0} is available!\n\nYour version: {1}\nNew version: {2} ({3:d})\n\nWould you like to visit the download page?",
                            BDAutoMuxerSettings.AssemblyName, CurrentVersion, AvailableVersion, _latestDate);
                    return true;
                }
            }

            Message = string.Format("You are using the latest version of {0} ({1}).", BDAutoMuxerSettings.AssemblyName,
                                    BDAutoMuxerSettings.AssemblyVersionDisplay);

            return false;
        }

        public void Update()
        {
            Process.Start(DownloadUrl);
        }

        public object Result { get; private set; }
        public string Message { get; private set; }
    }
}
