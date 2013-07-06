using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BDHero.Exceptions;
using BDHero.JobQueue;
using BDHero.Plugin;

namespace BDHero
{
    public interface IController
    {
        #region Properties

        Job Job { get; }

        #endregion

        #region Events

        /// <summary>
        /// Invoked immediately before the scanning stage starts.
        /// </summary>
        event EventHandler ScanStarted;

        /// <summary>
        /// Invoked when the scanning stage completes successfully.
        /// </summary>
        event EventHandler ScanSucceeded;

        /// <summary>
        /// Invoked when the scanning stage aborts because of an error.
        /// </summary>
        event EventHandler ScanFailed;

        /// <summary>
        /// Invoked when the scanning stage completes, regardless of whether the it succeeded or failed.
        /// This event always occurs after the <see cref="ScanSucceeded"/> and <see cref="ScanFailed"/> events.
        /// </summary>
        event EventHandler ScanCompleted;

        /// <summary>
        /// Invoked immediately before the conversion stage starts.
        /// </summary>
        event EventHandler ConvertStarted;

        /// <summary>
        /// Invoked when the conversion stage completes successfully.
        /// </summary>
        event EventHandler ConvertSucceeded;

        /// <summary>
        /// Invoked when the conversion stage aborts because of an error.
        /// </summary>
        event EventHandler ConvertFailed;

        /// <summary>
        /// Invoked when the conversion stage completes, regardless of whether the it succeeded or failed.
        /// This event always occurs after the <see cref="ConvertSucceeded"/> and <see cref="ConvertFailed"/> events.
        /// </summary>
        event EventHandler ConvertCompleted;

        /// <summary>
        /// Invoked whenever an <see cref="IPlugin"/>'s state or progress changes.
        /// </summary>
        event PluginProgressHandler PluginProgressUpdated;

        /// <summary>
        /// Invoked when an exception is thrown by an <see cref="IPlugin"/>.
        /// </summary>
        event UnhandledExceptionEventHandler UnhandledException;

        #endregion

        #region Methods

        /// <summary>
        /// Sets the <c>TaskScheduler</c> that will be used to invoke event callbacks.
        /// This ensures that events are always invoked from the appropriate thread.
        /// </summary>
        /// <param name="scheduler">Scheduler to use for event callbacks.  If none is specified, the calling thread's scheduler will be used.</param>
        void SetEventScheduler(TaskScheduler scheduler = null);

        void RenameSync(string mkvPath);

        /// <summary>
        /// Scans a BD-ROM, retrieves metadata, auto-detects the type of each playlist and track, and renames tracks and output file names.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="bdromPath">Path to the BD-ROM directory</param>
        /// <param name="mkvPath">Optional path to the output directory or MKV file</param>
        /// <returns><c>true</c> if the scan succeeded; otherwise <c>false</c></returns>
        Task<bool> CreateScanTask(CancellationToken cancellationToken, string bdromPath, string mkvPath = null);

        /// <summary>
        /// Muxes the BD-ROM to MKV and runs any post-processing plugins.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="mkvPath">Optional path to the MKV output file or directory (if overridden by the user)</param>
        /// <returns><c>true</c> if all muxing plugins succeeded; otherwise <c>false</c></returns>
        Task<bool> CreateConvertTask(CancellationToken cancellationToken, string mkvPath = null);

        #endregion
    }
}
