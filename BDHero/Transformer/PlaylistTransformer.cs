using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.Transformer
{
#if false
    class PlaylistTransformer
    {
        /// <summary>
        /// Returns a List of TSPlaylistFile objects from the given Dictionary.
        /// </summary>
        public static List<TSPlaylistFile> Transform(IEnumerable<KeyValuePair<string, TSPlaylistFile>> tsPlaylistFiles)
        {
            return tsPlaylistFiles.Select(pair => pair.Value).ToList();
        }

        /// <summary>
        /// Transforms a List of TSPlaylistFile objects into a List of Playlist objects.
        /// </summary>
        /// <param name="playlistFiles"></param>
        /// <returns></returns>
        public static List<Playlist> Transform(IEnumerable<TSPlaylistFile> playlistFiles)
        {
            return playlistFiles.OrderBy(file => file.Name).Select(Transform).ToList();
        }

        public static Playlist Transform(TSPlaylistFile playlistFile)
        {
            return new Playlist
            {
                Filename = playlistFile.Name,
                FullPath = playlistFile.FullName,
                Filesize = playlistFile.FileSize,
                Length = TimeSpan.FromMilliseconds(playlistFile.TotalLength * 1000),
                StreamClips = StreamClip.Transform(playlistFile.StreamClips),
                Tracks = Track.Transform(playlistFile.SortedStreams),
                Chapters = Chapter.Transform(playlistFile.Chapters),
                HasDuplicateStreamClips = playlistFile.HasDuplicateClips,
                HasLoops = playlistFile.HasLoops
            };
        }
    }
#endif
}
