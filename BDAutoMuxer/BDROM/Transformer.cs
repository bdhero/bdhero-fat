using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using BDAutoMuxer.BDInfo;

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

            disc.TransformLanguages();
            disc.TransformTitle();
            disc.TransformFeatureLengthPlaylists();
            disc.TransformPlaylistQuality();
            disc.TransformDuplicatePlaylists();

            return disc;
        }

        private void TransformLanguages()
        {
            // Sort languages alphabetically
            Languages = new HashSet<Language>(Playlists.SelectMany(playlist => playlist.Tracks).Select(track => track.Language)).OrderBy(language => language.Name).ToList();

            // Move primary language to the beginning of the list
            if (PrimaryLanguage != null && PrimaryLanguage != Language.Undetermined && Languages.Count > 1)
            {
                Languages.Remove(PrimaryLanguage);
                Languages.Insert(0, PrimaryLanguage);
            }
        }

        private void TransformTitle()
        {
            MovieTitle = MetaTitle.Trim();

            if (!string.IsNullOrWhiteSpace(MovieTitle))
            {
                MovieTitle = Regex.Replace(MovieTitle, @" - Blu-ray.*", "", RegexOptions.IgnoreCase);
                MovieTitle = Regex.Replace(MovieTitle, @" \(?Disc \d+\)?", "", RegexOptions.IgnoreCase);
                MovieTitle = Regex.Replace(MovieTitle, @"\s*[[(].*", "", RegexOptions.IgnoreCase);
                MovieTitle = MovieTitle.Trim();
            }

            if (Regex.Replace(MovieTitle, @"\W", "").ToLowerInvariant() == "bluray")
            {
                MovieTitle = "";
            }

            MovieTitle = Regex.Replace(MovieTitle, @"-+", " ");

            if (string.IsNullOrWhiteSpace(MovieTitle))
            {
                var cultureInfo = Thread.CurrentThread.CurrentCulture;
                var textInfo = cultureInfo.TextInfo;

                MovieTitle = VolumeLabel;
                MovieTitle = Regex.Replace(MovieTitle, @"^\d{6,}_", "");
                MovieTitle = Regex.Replace(MovieTitle, @"_+", " ");
                MovieTitle = textInfo.ToTitleCase(MovieTitle);
            }

            MovieTitle = Regex.Replace(MovieTitle, @"^(.*), (A|An|The)$", "$2 $1", RegexOptions.IgnoreCase);
        }

        private void TransformFeatureLengthPlaylists()
        {
            var maxLengthPlaylist = Playlists.OrderByDescending(p => p.LengthSec).FirstOrDefault();
            if (maxLengthPlaylist == null) return;
            var maxLength = maxLengthPlaylist.LengthSec;
            foreach (var playlist in Playlists.Where(playlist => playlist.LengthSec > maxLength * 0.9))
            {
                playlist.Type = TrackType.MainFeature;
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

            var lowQualityPlaylists = Playlists.Where(playlist => playlist.MaxAudioChannels < maxChannels || playlist.MaxVideoResolution < maxHeight);

            foreach (var playlist in lowQualityPlaylists)
            {
                playlist.IsLowQuality = true;
            }
        }

        private void TransformDuplicatePlaylists()
        {
        }
    }
}
