﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BDHero.JobQueue;
using DotNetUtils;
using ProcessUtils;

namespace BDHero.Plugin.FFmpegMuxer
{
    public class FFmpegPlugin : IMuxerPlugin
    {
        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "FFmpeg"; } }

        public event EditPluginPreferenceHandler EditPreferences;

        public MatroskaFeatures SupportedFeatures
        {
            get { return MatroskaFeatures.LPCM; }
        }

        private readonly AutoResetEvent _mutex = new AutoResetEvent(false);

        private PluginException _exception;

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public void Mux(Job job)
        {
            Host.ReportProgress(this, 0.0, "Starting FFmpeg process...");

            _exception = null;

            var ffmpeg = new FFmpeg(job, job.SelectedPlaylist, job.OutputPath);
            ffmpeg.ProgressUpdated += state => OnProgressUpdated(ffmpeg, state);
            ffmpeg.Exited += FFmpegOnExited;
            ffmpeg.StartAsync();
            WaitForThreadToExit();

            if (_exception != null)
                throw _exception;
        }

        private void OnProgressUpdated(FFmpeg ffmpeg, ProgressState progressState)
        {
            var status = string.Format("Muxing to MKV with FFmpeg: {0} - {1} @ {2} fps",
                TimeSpan.FromMilliseconds(ffmpeg.CurOutTimeMs),
                FileUtils.BytesToString(ffmpeg.CurSize),
                ffmpeg.CurFps.ToString("0.0"));
            Host.ReportProgress(this, progressState.PercentComplete, status);
        }

        private void FFmpegOnExited(NonInteractiveProcessState state, int exitCode, TimeSpan runTime)
        {
            if (state != NonInteractiveProcessState.Completed)
            {
                _exception = new PluginException(string.Format("FFmpeg exited with state: {0}", state), PluginExceptionSeverity.Fatal);
            }
            SignalThreadExited();
        }

        private void WaitForThreadToExit()
        {
            _mutex.WaitOne();
        }

        private void SignalThreadExited()
        {
            _mutex.Set();
        }
    }
}
