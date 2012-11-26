using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using BDAutoMuxer.views;

namespace BDAutoMuxer.controllers
{
    class PlaylistFinder
    {
        public PlaylistScanResult ScanResult { get; private set; }

        public event EventHandler ScanSuccess;
        public event ErrorEventHandler ScanError;
        public event EventHandler ScanCompleted;

        private BackgroundWorker _initBDROMWorker;

        #region BDROM Initialization Worker

        public void InitBDROM(string path)
        {
            _initBDROMWorker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            _initBDROMWorker.DoWork += InitBDROMWork;
            _initBDROMWorker.ProgressChanged += InitBDROMProgress;
            _initBDROMWorker.RunWorkerCompleted += InitBDROMCompleted;
            _initBDROMWorker.RunWorkerAsync(path);
        }

        private void InitBDROMWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ScanResult = new PlaylistScanResult(e.Argument as string);

                var bdrom = ScanResult.BDROM;

                bdrom.StreamClipFileScanError += BDROM_StreamClipFileScanError;
                bdrom.StreamFileScanError += BDROM_StreamFileScanError;
                bdrom.PlaylistFileScanError += BDROM_PlaylistFileScanError;
                bdrom.Scan();

                e.Result = null;
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private bool BDROM_PlaylistFileScanError(TSPlaylistFile playlistFile, Exception ex)
        {
            DialogResult result = MessageBox.Show(string.Format(
                "An error occurred while scanning the playlist file {0}.\n\nThe disc may be copy-protected or damaged.\n\nDo you want to continue scanning the playlist files?", playlistFile.Name),
                "BDAutoMuxer Scan Error", MessageBoxButtons.YesNo);

            return result == DialogResult.Yes;
        }

        private bool BDROM_StreamFileScanError(TSStreamFile streamFile, Exception ex)
        {
            DialogResult result = MessageBox.Show(string.Format(
                "An error occurred while scanning the stream file {0}.\n\nThe disc may be copy-protected or damaged.\n\nDo you want to continue scanning the stream files?", streamFile.Name),
                "BDAutoMuxer Scan Error", MessageBoxButtons.YesNo);

            return result == DialogResult.Yes;
        }

        private bool BDROM_StreamClipFileScanError(TSStreamClipFile streamClipFile, Exception ex)
        {
            DialogResult result = MessageBox.Show(string.Format(
                "An error occurred while scanning the stream clip file {0}.\n\nThe disc may be copy-protected or damaged.\n\nDo you want to continue scanning the stream clip files?", streamClipFile.Name),
                "BDAutoMuxer Scan Error", MessageBoxButtons.YesNo);

            return result == DialogResult.Yes;
        }

        private void InitBDROMProgress(object sender, ProgressChangedEventArgs e)
        {
        }

        private void InitBDROMCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var exception = e.Result as Exception;

            if (exception != null)
            {
                MessageBox.Show(exception.Message, "BDAutoMuxer Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (ScanError != null)
                    ScanError.Invoke(this, new ErrorEventArgs(exception));

                if (ScanCompleted != null)
                    ScanCompleted.Invoke(this, EventArgs.Empty);

                return;
            }

            var bdrom = ScanResult.BDROM;

            /*
            textBoxSource.Text = BDROM.DirectoryRoot.FullName;

            textBoxDetails.Text += string.Format(
                "Detected BDMV Folder: {0} ({1}) {2}",
                BDROM.DirectoryBDMV.FullName,
                BDROM.VolumeLabel,
                Environment.NewLine);
            */

            var features = new List<string>();

            if (bdrom.Is50Hz)
            {
                features.Add("50Hz Content");
            }
            if (bdrom.IsBDPlus)
            {
                features.Add("BD+ Copy Protection");
            }
            if (bdrom.IsBDJava)
            {
                features.Add("BD-Java");
            }
            if (bdrom.Is3D)
            {
                features.Add("Blu-ray 3D");
            }
            if (bdrom.IsDBOX)
            {
                features.Add("D-BOX Motion Code");
            }
            if (bdrom.IsPSP)
            {
                features.Add("PSP Digital Copy");
            }
            if (features.Count > 0)
            {
                //textBoxDetails.Text += "Detected Features: " + string.Join(", ", features.ToArray()) + Environment.NewLine;
            }
            /*
            textBoxDetails.Text += string.Format(
                "Disc Size: {0:N0} bytes{1}",
                BDROM.Size,
                Environment.NewLine);
            */
            LoadPlaylists();
        }

        #endregion

        #region File/Stream Lists

        private void LoadPlaylists()
        {
            var bdrom = ScanResult.BDROM;

            if (bdrom == null) return;

            var groups = new List<List<TSPlaylistFile>>();

            var sortedPlaylistFiles = new TSPlaylistFile[bdrom.PlaylistFiles.Count];
            bdrom.PlaylistFiles.Values.CopyTo(sortedPlaylistFiles, 0);
            Array.Sort(sortedPlaylistFiles, ComparePlaylistFiles);

            foreach (TSPlaylistFile playlist1 in sortedPlaylistFiles)
            {
                if (!playlist1.IsValid) continue;

                int matchingGroupIndex = 0;
                for (int groupIndex = 0; groupIndex < groups.Count; groupIndex++)
                {
                    List<TSPlaylistFile> group = groups[groupIndex];
                    foreach (TSPlaylistFile playlist2 in @group.Where(playlist2 => playlist2.IsValid))
                    {
                        foreach (TSStreamClip clip1 in playlist1.StreamClips)
                        {
                            if (playlist2.StreamClips.Any(clip2 => clip1.Name == clip2.Name))
                            {
                                matchingGroupIndex = groupIndex + 1;
                            }
                            if (matchingGroupIndex > 0) break;
                        }
                        if (matchingGroupIndex > 0) break;
                    }
                    if (matchingGroupIndex > 0) break;
                }
                if (matchingGroupIndex > 0)
                {
                    groups[matchingGroupIndex - 1].Add(playlist1);
                }
                else
                {
                    groups.Add(new List<TSPlaylistFile> { playlist1 });
                    //matchingGroupIndex = groups.Count;
                }
                //playlistGroup[playlist1.Name] = matchingGroupIndex;
            }

            var sortedPlaylists = ScanResult.SortedPlaylists;
            var languages = ScanResult.Languages;

            foreach (List<TSPlaylistFile> @group in groups)
            {
                @group.Sort(ComparePlaylistFiles);
                sortedPlaylists.AddRange(@group.Where(playlist => playlist.IsValid));
            }

            languages.AddRange(sortedPlaylists.SelectMany(playlistFile => (from stream in playlistFile.SortedStreams
                                                                           where stream.LanguageCode != null
                                                                           select Language.GetLanguage(stream.LanguageCode))));

            FindMainPlaylist();
        }

        private void FindMainPlaylist()
        {
            var mainPlaylists = ScanResult.MainPlaylists;
            var sortedPlaylists = ScanResult.SortedPlaylists;

            mainPlaylists.Clear();

            if (sortedPlaylists.Count == 0) return;

            double maxlength = sortedPlaylists[0].TotalLength;

            foreach (TSPlaylistFile playlist in sortedPlaylists.Where(playlist => playlist.TotalLength > maxlength * 0.9))
            {
                playlist.IsFeatureLength = true;
                mainPlaylists.Add(playlist);
            }

            var duplicateMap = new Dictionary<string, IList<TSPlaylistFile>>();
            foreach (TSPlaylistFile mainPlaylist in mainPlaylists)
            {
                IList<string> streamClips = mainPlaylist.StreamClips.Select(clip => clip.Name + "/" + clip.Length + "/" + clip.FileSize).ToList();

                string key = mainPlaylist.TotalLength + "/" + mainPlaylist.FileSize + "=[" + string.Join(",", streamClips) + "]";

                if (!duplicateMap.ContainsKey(key))
                    duplicateMap[key] = new List<TSPlaylistFile>();

                duplicateMap[key].Add(mainPlaylist);
            }

            foreach (string key in duplicateMap.Keys)
            {
                // Sort
                IList<TSPlaylistFile> sorted = duplicateMap[key].OrderBy(x => x.HiddenTrackCount).ToList();

                // Mark [1, ... n] as duplicates
                if (sorted.Count > 0)
                {
                    foreach (TSPlaylistFile playlist in duplicateMap[key].Skip(1))
                    {
                        playlist.IsDuplicate = true;
                    }
                }
            }

            // DONE!

            if (ScanSuccess != null)
                ScanSuccess.Invoke(this, EventArgs.Empty);

            if (ScanCompleted != null)
                ScanCompleted.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Cancel

        public void CancelScan()
        {
            if (_initBDROMWorker != null && _initBDROMWorker.IsBusy)
            {
                _initBDROMWorker.CancelAsync();
            }
        }

        #endregion

        #region Comparison

        private static int ComparePlaylistFiles(TSPlaylistFile x, TSPlaylistFile y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return 1;
            }
            if (y == null)
            {
                return -1;
            }
            if (x.TotalLength > y.TotalLength)
            {
                return -1;
            }
            if (y.TotalLength > x.TotalLength)
            {
                return 1;
            }
            return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }

        public static int ComparePlaylistFilesForMainMovie(TSPlaylistFile x, TSPlaylistFile y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            if (x == null)
            {
                return 1;
            }
            if (y == null)
            {
                return -1;
            }
            if (x.HiddenTrackCount < y.HiddenTrackCount)
            {
                return -1;
            }
            if (y.HiddenTrackCount < x.HiddenTrackCount)
            {
                return 1;
            }
            return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }

        #endregion
    }

    public class PlaylistScanResult
    {
        public readonly BDROM BDROM;

        public readonly ISet<Language> Languages = new HashSet<Language>();
        public readonly List<TSPlaylistFile> SortedPlaylists = new List<TSPlaylistFile>();
        public readonly List<TSPlaylistFile> MainPlaylists = new List<TSPlaylistFile>();

        public PlaylistScanResult(string path)
        {
            BDROM = new BDROM(path);
        }
    }

    /// <see cref="http://stackoverflow.com/a/2984664/467582"/>
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> newItems)
        {
            foreach (T item in newItems)
            {
                collection.Add(item);
            }
        }
    }
}
