﻿using System;
using System.Collections.Generic;
using System.IO;
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
            var pathSpecified = !string.IsNullOrWhiteSpace(job.OutputPath);
            if (pathSpecified && job.OutputPath.EndsWith(".mkv", StringComparison.InvariantCultureIgnoreCase))
                return;

            string firstVideoHeight = null;
            var firstVideoTrack = job.SelectedPlaylist.VideoTracks.FirstOrDefault(track => track.Keep);
            if (firstVideoTrack != null)
                firstVideoHeight = firstVideoTrack.VideoFormatDisplayable;

            var filename = string.Format(@"{0} [{1}].mkv", job.Disc.MovieTitle, firstVideoHeight);

            job.OutputPath = pathSpecified ? Path.Combine(job.OutputPath, filename) : filename;
        }
    }
}