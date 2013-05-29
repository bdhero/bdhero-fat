using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.Queue;

namespace BDHero.Plugin.FileNamer
{
    public class FileNamerPlugin : INameProviderPlugin
    {
        public IPluginHost Host { get; set; }
        public string Name { get { return "BDHero File Namer"; } }

        public event PluginProgressHandler ProgressUpdated;
        public event EditPluginPreferenceHandler EditPreferences;

        public void LoadPlugin()
        {
        }

        public void UnloadPlugin()
        {
        }

        public void Rename(Job job)
        {
            if (string.IsNullOrWhiteSpace(job.OutputPath))
            {
                var firstVideoTrack = job.SelectedPlaylist.VideoTracks.FirstOrDefault(track => track.Keep);
                if (firstVideoTrack != null)
                {
                    job.OutputPath = string.Format(@"X:\BDHero\{0} [{1}].mkv", job.Disc.MovieTitle, firstVideoTrack.VideoFormatDisplayable);
                }
            }
        }
    }
}
