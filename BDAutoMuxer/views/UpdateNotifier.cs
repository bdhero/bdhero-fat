using System;
using System.ComponentModel;
using System.Windows.Forms;
using BDAutoMuxerCore.Services;

namespace BDAutoMuxer.Views
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
        private readonly IUpdateService _updateService;

        private bool _isUpdateAvailable;

        private readonly Form _form;
        private readonly bool _notifyIfUpToDate;
        private readonly UpdateNotifierCompleteDelegate _onComplete;

        private UpdateNotifier(Form form, bool notifyIfUpToDate = false, UpdateNotifierCompleteDelegate onComplete = null)
        {
            _form = form;
            _notifyIfUpToDate = notifyIfUpToDate;
            _onComplete = onComplete;

            _updateService = UpdateServiceFactory.GetUpdateService();
            _updateService.UpdateStarted += message => MessageBox.Show(_form, message);
            _updateService.UpdateFinished += delegate(string message) { MessageBox.Show(_form, message); Application.Restart(); };

            DoWork += CheckForUpdate;
            RunWorkerCompleted += OnRunWorkerCompleted;
        }

        private void CheckForUpdate(object sender, DoWorkEventArgs e)
        {
            try
            {
                _isUpdateAvailable = _updateService.IsUpdateAvailable;
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
                if (_updateService.IsMandatory)
                {
                    MessageBox.Show(_form, _updateService.Message, "Mandatory update", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    _updateService.Update();
                }
                else if (DialogResult.Yes == MessageBox.Show(_form,  _updateService.Message, "Update available", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    _updateService.Update();
                }
            }
            else if (_notifyIfUpToDate)
            {
                var exception = (e.Result ?? _updateService.Result) as Exception;
                if (exception == null)
                {
                    MessageBox.Show(_form, _updateService.Message, "No updates available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string caption = string.Format("{0} Error", BDAutoMuxerSettings.AssemblyName);
                    string message = string.Format("{0}\n\n{1}", _updateService.Message, exception.Message);
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

    
}
