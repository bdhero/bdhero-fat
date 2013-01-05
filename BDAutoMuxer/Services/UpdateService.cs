﻿using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BDAutoMuxer.controllers;
using BDAutoMuxer.views;

namespace BDAutoMuxer.Services
{
    public interface IUpdateService
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
    public class ClickOnceUpdateService : IUpdateService
    {
        private readonly Version _currentVersion;
        private Version _latestVersion;

        private ApplicationDeployment _ad;
        private UpdateCheckInfo _info;

        private readonly Form _form;

        public Version CurrentVersion { get { return _currentVersion; } }
        public Version AvailableVersion { get { return _latestVersion; } }

        public bool IsMandatory { get; private set; }

        public ClickOnceUpdateService(Form form)
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
            if (!UpdateNotifier.IsClickOnce)
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
                _ad.UpdateCompleted += (sender, args) =>
                {
                    MessageBox.Show(_form, "The application has been updated and will now restart.");
                    Application.Restart();
                };
                _ad.UpdateAsync();
                MessageBox.Show(_form, "The application update is being downloading in the background.  You will be notified when it completes.");
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

        public bool IsMandatory { get; private set; }

        public GitHubUpdateService()
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
                if (_latestVersion > _currentVersion)
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