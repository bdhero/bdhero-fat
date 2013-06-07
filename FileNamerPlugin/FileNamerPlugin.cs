using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BDHero.JobQueue;

namespace BDHero.Plugin.FileNamer
{
    public class FileNamerPlugin : INameProviderPlugin
    {
        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "BDHero File Namer"; } }

        public event EditPluginPreferenceHandler EditPreferences;

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public void Rename(Job job)
        {
            Host.ReportProgress(this, 0.0, "Auto-renaming output file...");

            var pathSpecified = !string.IsNullOrWhiteSpace(job.OutputPath);
            if (pathSpecified && job.OutputPath.EndsWith(".mkv", StringComparison.InvariantCultureIgnoreCase))
                return;

            string firstVideoHeight = null;
            var firstVideoTrack = job.SelectedPlaylist.VideoTracks.FirstOrDefault(track => track.Keep);
            if (firstVideoTrack != null)
                firstVideoHeight = firstVideoTrack.VideoFormatDisplayable;

            var directory = pathSpecified ? job.OutputPath : Environment.CurrentDirectory;
            var filename = string.Format(@"{0} [{1}].mkv", job.Disc.SanitizedTitle, firstVideoHeight);
            job.OutputPath = Path.Combine(directory, filename);

            Host.ReportProgress(this, 100.0, "Finished auto-renaming output file");
        }
    }
}
