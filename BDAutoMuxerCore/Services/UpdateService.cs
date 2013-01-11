using System;
using System.Deployment.Application;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BDAutoMuxerCore.Services
{
    public interface IUpdateService
    {
        Version CurrentVersion { get; }
        Version AvailableVersion { get; }

        /// <summary>
        /// Invoked when the application has started downloading a ClickOnce update.
        /// </summary>
        event UpdateStartedDelegate UpdateStarted;

        /// <summary>
        /// Invoked when the application has finished downloading a ClickOnce update.
        /// </summary>
        event UpdateStartedDelegate UpdateFinished;

        bool IsUpdateAvailable { get; }
        bool IsMandatory { get; }
        void Update();

        object Result { get; }
        string Message { get; }
    }

    public static class UpdateServiceFactory
    {
        public static IUpdateService GetUpdateService()
        {
            if (ClickOnceUpdateService.IsClickOnce)
                return new ClickOnceUpdateService();
            return new GitHubUpdateService();
        }
    }

    /// <see cref="http://msdn.microsoft.com/en-us/library/ms404263.aspx"/>
    public class ClickOnceUpdateService : IUpdateService
    {
        public static bool IsClickOnce
        {
            get { return ApplicationDeployment.IsNetworkDeployed; }
        }

        private readonly Version _currentVersion;
        private Version _latestVersion;

        private ApplicationDeployment _ad;
        private UpdateCheckInfo _info;

        public Version CurrentVersion { get { return _currentVersion; } }
        public Version AvailableVersion { get { return _latestVersion; } }

        public event UpdateStartedDelegate UpdateStarted;
        public event UpdateStartedDelegate UpdateFinished;

        public bool IsMandatory { get; private set; }

        public ClickOnceUpdateService()
        {
            _currentVersion = AssemblyUtils.AssemblyVersion;
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
            if (!IsClickOnce)
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
                        String.Format(
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
                _ad.UpdateCompleted += (sender, args) => NotifyFinished("The application has been updated and will now restart.");
                _ad.UpdateAsync();
                NotifyStarted("The application update is being downloading in the background.  You will be notified when it completes.");
            }
            catch (DeploymentDownloadException dde)
            {
                Result = dde;
                Message = "Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later.";
            }
        }

        private void NotifyStarted(string message)
        {
            if (UpdateStarted != null)
                UpdateStarted(message);
        }

        private void NotifyFinished(string message)
        {
            if (UpdateFinished != null)
                UpdateFinished(message);
        }

        public object Result { get; private set; }
        public string Message { get; private set; }
    }

    public class GitHubUpdateService : IUpdateService
    {
        private const string DownloadUrl = "https://github.com/acdvorak/bdautomuxer/downloads";
        private const string VersionUrl = "https://raw.github.com/acdvorak/bdautomuxer/master/version.txt";
        private const string VersionRegex = @"^(\d+\.\d+\.\d+(?:\.\d+)?)\s+(\d{4}/\d{1,2}/\d{1,2})$";

        private readonly Version _currentVersion;
        private Version _latestVersion;
        private DateTime _latestDate;

        public Version CurrentVersion { get { return _currentVersion; } }
        public Version AvailableVersion { get { return _latestVersion; } }

        public event UpdateStartedDelegate UpdateStarted;
        public event UpdateStartedDelegate UpdateFinished;

        public bool IsMandatory { get; private set; }

        public GitHubUpdateService()
        {
            IsMandatory = false;
            _currentVersion = AssemblyUtils.AssemblyVersion;
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
                if (_latestVersion > _currentVersion)
                {
                    Message =
                        string.Format(
                            "An updated version of {0} is available!\n\nYour version: {1}\nNew version: {2} ({3:d})\n\nWould you like to visit the download page?",
                            AssemblyUtils.AssemblyName, CurrentVersion, AvailableVersion, _latestDate);
                    return true;
                }
            }

            Message = string.Format("You are using the latest version of {0} ({1}).", AssemblyUtils.AssemblyName,
                                    AssemblyUtils.AssemblyVersionDisplay);

            return false;
        }

        public void Update()
        {
            Process.Start(DownloadUrl);
        }

        public object Result { get; private set; }
        public string Message { get; private set; }
    }

    public delegate void UpdateStartedDelegate(string message);
    public delegate void UpdateFinishedDelegate(string message);
}
