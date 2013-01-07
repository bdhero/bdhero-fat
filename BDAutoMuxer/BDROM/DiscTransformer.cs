using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using BDAutoMuxer.BDInfo;
using BDAutoMuxer.controllers;

namespace BDAutoMuxer.BDROM
{
    public partial class Disc
    {
        public static Disc Transform(BDInfo.BDROM bdrom)
        {
            var disc = new Disc
                           {
                               VolumeLabel = bdrom.VolumeLabel,
                               MetaTitle = bdrom.DiscName,
                               PrimaryLanguage = bdrom.DiscLanguage,
                               Playlists = Playlist.Transform(bdrom.PlaylistFiles)
                           };

            // Data gathering
            disc.TransformPrimaryLanguage();
            disc.TransformLanguageList();
            disc.TransformTitle();
            disc.TransformDuplicatePlaylists(bdrom);
            disc.TransformBogusPlaylists();
            disc.TransformPlaylistQuality();

            // Auto-configuration
            disc.DetectPlaylistTypes();
            disc.DetectMainFeaturePlaylistTrackTypes();
            disc.DetectCommentaryPlaylistTrackTypes();
            disc.DetectSpecialFeaturePlaylistTrackTypes();

            return disc;
        }

        #region Data Gathering

        private void TransformPrimaryLanguage()
        {
            if (PrimaryLanguage != null) return;

            PrimaryLanguage = Playlists.SelectMany(playlist => playlist.AudioTracks)
                                       .Select(track => track.Language)
                                       .GroupBy(language => language)
                                       .OrderByDescending(grouping => grouping.Count())
                                       .Select(grouping => grouping.Key)
                                       .FirstOrDefault();
        }

        private void TransformLanguageList()
        {
            // Sort languages alphabetically
            var languagesWithDups =
                Playlists
                    .SelectMany(playlist => playlist.Tracks)
                    .Select(track => track.Language)
                    .Where(language => language != null && language != Language.Undetermined);
            Languages = new HashSet<Language>(languagesWithDups).OrderBy(language => language.Name).ToList();

            if (PrimaryLanguage == null || PrimaryLanguage == Language.Undetermined) return;

            // Move primary language to the beginning of the list
            Languages.Remove(PrimaryLanguage);
            Languages.Insert(0, PrimaryLanguage);
        }

        private void TransformTitle()
        {
            MovieTitle = MetaTitle.Trim();

            if (!string.IsNullOrWhiteSpace(MovieTitle))
            {
                MovieTitle = Regex.Replace(MovieTitle, @" - Blu-ray.*", "", RegexOptions.IgnoreCase);
                MovieTitle = Regex.Replace(MovieTitle, @" \(?Disc \w+(?: of \w+)?\)?", "", RegexOptions.IgnoreCase);
                MovieTitle = Regex.Replace(MovieTitle, @"\s*[[(].*", "", RegexOptions.IgnoreCase);
                MovieTitle = MovieTitle.Trim();
            }

            if (Regex.Replace(MovieTitle, @"\W", "").ToLowerInvariant() == "bluray")
            {
                MovieTitle = "";
            }

            // TMDb chokes on dashes
            MovieTitle = Regex.Replace(MovieTitle, @"-+", " ");

            // No valid bdmt_<lang>.xml value found; use volume label as fallback
            if (string.IsNullOrWhiteSpace(MovieTitle))
            {
                MovieTitle = VolumeLabel;
                MovieTitle = Regex.Replace(MovieTitle, @"^\d{6,}_", "");
                MovieTitle = Regex.Replace(MovieTitle, @"_+", " ");
            }

            MovieTitle = Regex.Replace(MovieTitle, @"^(.*), (A|An|The)$", "$2 $1", RegexOptions.IgnoreCase);
        }

        private void TransformDuplicatePlaylists(BDInfo.BDROM bdrom)
        {
            var bdamPlaylistMap = Playlists.ToDictionary(playlist => playlist.Filename);
            var bdinfoDuplicateMap = bdrom.PlaylistFiles.Values.ToMultiValueDictionary(GetTSPlaylistFileDupKey);

            var dups = (from key in bdinfoDuplicateMap.Keys
                        where bdinfoDuplicateMap[key].Count > 1
                        select bdinfoDuplicateMap[key].OrderByDescending(file => file.HiddenTrackCount))
                .SelectMany(sortedFiles => sortedFiles.Skip(1));

            foreach (var tsPlaylistFile in dups)
            {
                tsPlaylistFile.IsDuplicate = true;
                bdamPlaylistMap[tsPlaylistFile.Name].IsDuplicate = true;
            }
        }

        private static string GetTSPlaylistFileDupKey(TSPlaylistFile playlistFile)
        {
            IList<string> streamClips = playlistFile.StreamClips.Select(clip => clip.Name + "/" + clip.Length + "/" + clip.FileSize).ToList();
            var key = playlistFile.TotalLength + "/" + playlistFile.FileSize + "=[" + string.Join(",", streamClips) + "]";
            return key;
        }

        private void TransformBogusPlaylists()
        {
            foreach (var playlist in Playlists)
            {
                playlist.IsBogus = playlist.IsDuplicate || playlist.HasDuplicateStreamClips ||
                                   playlist.HasHiddenFirstTracks;
            }
        }

        private void TransformPlaylistQuality()
        {
            var bestAudioPlaylist = Playlists.OrderByDescending(p => p.MaxAudioChannels).FirstOrDefault();
            if (bestAudioPlaylist == null) return;

            var bestVideoPlaylist = Playlists.OrderByDescending(p => p.MaxVideoResolution).FirstOrDefault();
            if (bestVideoPlaylist == null) return;

            var maxChannels = bestAudioPlaylist.MaxAudioChannels;
            var maxHeight = bestVideoPlaylist.MaxVideoResolution;

            var maxQualityPlaylists = Playlists.Where(playlist => playlist.MaxAudioChannels == maxChannels && playlist.MaxVideoResolution == maxHeight);

            foreach (var playlist in maxQualityPlaylists)
            {
                playlist.IsMaxQuality = true;
            }
        }

        #endregion

        #region Auto Detection

        private void DetectPlaylistTypes()
        {
            var maxLength =
                Playlists
                    .OrderByDescending(p => p.LengthSec)
                    .Select(playlist => playlist.LengthSec)
                    .FirstOrDefault();

            foreach (var playlist in Playlists)
            {
                if (playlist.IsMainFeaturePlaylist(maxLength))
                    playlist.Type = TrackType.MainFeature;

                if (playlist.IsSpecialFeaturePlaylist(maxLength))
                    playlist.Type = TrackType.SpecialFeature;
            }
        }

        private void DetectMainFeaturePlaylistTrackTypes()
        {
            foreach (var playlist in Playlists.Where(playlist => playlist.IsMainFeature))
            {
                // Additional video tracks are PiP commentary
                foreach (var videoTrack in playlist.VideoTracks.Skip(1))
                {
                    videoTrack.Type = TrackType.Commentary;
                }

                var audioLanguages = playlist.AudioTracks.Select(track => track.Language);

                // Detect type of audio tracks (per-language)
                foreach (var audioLanguage in audioLanguages)
                {
                    var lang = audioLanguage;
                    var audioTracksWithLang = playlist.AudioTracks.Where(track => track.Language == lang).ToList();

                    // Detect type of audio tracks
                    for (var i = 0; i < audioTracksWithLang.Count; i++)
                    {
                        var audioTrack = audioTracksWithLang[i];

                        if (audioTrack.IsMainFeatureAudioTrack(i))
                            audioTrack.Type = TrackType.MainFeature;

                        if (audioTrack.IsCommentaryAudioTrack(i))
                            audioTrack.Type = TrackType.Commentary;
                    }
                }

                // Subtitle track types cannot be inferred programmatically;
                // assume "Main Feature" and the user can override if necessary.
                foreach (var subtitleTrack in playlist.SubtitleTracks)
                {
                    subtitleTrack.Type = TrackType.MainFeature;
                }
            }
        }

        private void DetectCommentaryPlaylistTrackTypes()
        {
            var tracks = Playlists.Where(playlist => playlist.IsVideoCommentary).SelectMany(playlist => playlist.Tracks);
            foreach (var track in tracks)
            {
                track.Type = TrackType.Commentary;
            }
        }

        private void DetectSpecialFeaturePlaylistTrackTypes()
        {
            var tracks = Playlists.Where(playlist => playlist.IsSpecialFeature).SelectMany(playlist => playlist.Tracks);
            foreach (var track in tracks)
            {
                track.Type = TrackType.SpecialFeature;
            }
        }

        #endregion
    }

    
}
