using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.Queue;
using ProcessUtils;

namespace BDHero.Plugin.FFmpegMuxer
{
    public class FFmpegPlugin : IMuxerPlugin
    {
        public IPluginHost Host { get; set; }
        public string Name { get { return "FFmpeg"; } }

        public event PluginProgressHandler ProgressUpdated;
        public event EditPluginPreferenceHandler EditPreferences;

        public void LoadPlugin()
        {
        }

        public void UnloadPlugin()
        {
        }

        public void Mux(Job job)
        {
            var ffmpeg = new FFmpeg(job.Disc, job.SelectedPlaylist, job.OutputPath);
            ffmpeg.ProgressUpdated += OnProgressUpdated;
            ffmpeg.ProgressUpdated += Console.WriteLine;
            ffmpeg.Exited += (state, code, time) => Console.WriteLine(state);
            ffmpeg.StartAsync();
        }

        private void OnProgressUpdated(ProgressState progressState)
        {
            if (ProgressUpdated != null)
                ProgressUpdated(this, progressState);
        }
    }
}
