using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BDHero.JobQueue;
using ProcessUtils;

namespace BDHero.Plugin.FFmpegMuxer
{
    public class FFmpegPlugin : IMuxerPlugin
    {
        public IPluginHost Host { get; set; }
        public string Name { get { return "FFmpeg"; } }

        public event PluginProgressHandler ProgressUpdated;
        public event EditPluginPreferenceHandler EditPreferences;

        public MatroskaFeatures SupportedFeatures
        {
            get { return MatroskaFeatures.LPCM; }
        }

        private readonly AutoResetEvent _mutex = new AutoResetEvent(false);

        public void LoadPlugin()
        {
        }

        public void UnloadPlugin()
        {
        }

        public void Mux(Job job)
        {
            var ffmpeg = new FFmpeg(job, job.SelectedPlaylist, job.OutputPath);
            ffmpeg.ProgressUpdated += OnProgressUpdated;
            ffmpeg.Exited += FFmpegOnExited;
            ffmpeg.StartAsync();
            WaitForThreadToExit();
        }

        private void OnProgressUpdated(ProgressState progressState)
        {
            if (ProgressUpdated != null)
                ProgressUpdated(this, progressState);
        }

        private void FFmpegOnExited(NonInteractiveProcessState state, int exitCode, TimeSpan runTime)
        {
            Console.WriteLine("FFmpeg exited with state: {0}", state);
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
