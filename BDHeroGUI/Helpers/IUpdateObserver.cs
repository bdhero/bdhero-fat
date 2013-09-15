using System;
using DotNetUtils.Net;
using Updater;

namespace BDHeroGUI.Helpers
{
    public interface IUpdateObserver
    {
        void OnBeforeCheckForUpdate();
        void OnBeforeDownloadUpdate(Update update);
        void OnUpdateDownloadProgressChanged(Update update, FileDownloadProgress progress);
        void OnUpdateException(Exception exception);
        void OnUpdateReadyToInstall(Update update);
        void OnNoUpdateAvailable();
        bool ShouldInstallUpdate(Update update);
        void OnBeforeInstallUpdate(Update update);
    }
}