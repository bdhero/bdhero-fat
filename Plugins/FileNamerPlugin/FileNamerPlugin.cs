using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BDHero.JobQueue;
using DotNetUtils;

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

        // TODO: Write full implementation
        public void Rename(CancellationToken cancellationToken, Job job)
        {
            Host.ReportProgress(this, 0.0, "Auto-renaming output file...");

            var path = job.OutputPath;

            // User already specified filename
            var pathSpecified = !string.IsNullOrWhiteSpace(path);
            if (pathSpecified && FileUtils.ContainsFileName(path))
                path = Path.GetDirectoryName(path);

            string firstVideoHeight = null;
            var firstVideoTrack = job.SelectedPlaylist.VideoTracks.FirstOrDefault(track => track.Keep);
            if (firstVideoTrack != null)
                firstVideoHeight = firstVideoTrack.VideoFormatDisplayable;

            var directory = pathSpecified ? path : Environment.CurrentDirectory;
            var filename = string.Format(@"{0} [{1}].mkv", job.Disc.SanitizedTitle, firstVideoHeight);

            var movie = job.SelectedReleaseMedium as Movie;
            var tvShow = job.SelectedReleaseMedium as TVShow;

            if (movie != null)
            {
                filename = string.Format("{0} ({1}) [{2}].mkv",
                                         FileUtils.SanitizeFileName(movie.Title),
                                         movie.ReleaseYear,
                                         job.SelectedPlaylist.MaxVideoResolution);
            }
            else if (tvShow != null)
            {
                filename = string.Format("s{0}e{1} - {2} [{3}].mkv",
                                         tvShow.SelectedEpisode.SeasonNumber.ToString("00"),
                                         tvShow.SelectedEpisode.EpisodeNumber.ToString("00"),
                                         FileUtils.SanitizeFileName(tvShow.Title),
                                         job.SelectedPlaylist.MaxVideoResolution);
            }

            job.OutputPath = Path.Combine(directory, filename);

            Host.ReportProgress(this, 100.0, "Finished auto-renaming output file");
        }
    }
}
