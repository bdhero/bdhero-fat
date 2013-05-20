using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BDHero.BDROM;
using DotNetUtils;
using I18N;

namespace BDHero.Plugin.DiscReader.Transformer
{
    static class DiscTransformer
    {
        public static Disc Transform(BDInfo.BDROM bdrom)
        {
            var tsPlaylistFiles = PlaylistTransformer.Transform(bdrom.PlaylistFiles);

            var disc =
                new Disc
                    {
                        VolumeLabel = bdrom.VolumeLabel,
                        MetaTitle = bdrom.DiscName,
                        PrimaryLanguage = bdrom.DiscLanguage,
                        Playlists = PlaylistTransformer.Transform(tsPlaylistFiles)
                    };

            // Data gathering
            TransformPrimaryLanguage(disc);
            TransformVideoLanguages(disc);
            TransformLanguageList(disc);
            TransformTitle(disc);

            return disc;
        }

        #region Data Gathering

        private static void TransformPrimaryLanguage(Disc disc)
        {
            if (disc.PrimaryLanguage != null) return;

            disc.PrimaryLanguage = disc.Playlists.SelectMany(playlist => playlist.AudioTracks)
                                       .Select(track => track.Language)
                                       .GroupBy(language => language)
                                       .OrderByDescending(grouping => grouping.Count())
                                       .Select(grouping => grouping.Key)
                                       .FirstOrDefault();
        }

        private static void TransformVideoLanguages(Disc disc)
        {
            if (disc.PrimaryLanguage == null) return;

            foreach (var videoTrack in disc.Playlists.SelectMany(playlist => playlist.VideoTracks))
            {
                videoTrack.Language = disc.PrimaryLanguage;
            }
        }

        private static void TransformLanguageList(Disc disc)
        {
            var playlists = disc.Playlists;
            var languages = disc.Languages;
            var primaryLanguage = disc.PrimaryLanguage;

            // Sort languages alphabetically
            var languagesWithDups =
                    playlists
                        .SelectMany(playlist => playlist.Tracks)
                        .Select(track => track.Language)
                        .Where(language => language != null && language != Language.Undetermined);

            languages.Clear();
            languages.AddRange(new HashSet<Language>(languagesWithDups).OrderBy(language => language.Name));

            if (primaryLanguage == null || primaryLanguage == Language.Undetermined) return;

            // Move primary language to the beginning of the list
            languages.Remove(primaryLanguage);
            languages.Insert(0, primaryLanguage);
        }

        private static void TransformTitle(Disc disc)
        {
            var metaTitle = disc.MetaTitle;
            var volumeLabel = disc.VolumeLabel;

            var movieTitle = (metaTitle ?? "").Trim();

            if (!string.IsNullOrWhiteSpace(movieTitle))
            {
                movieTitle = Regex.Replace(movieTitle, @" - Blu-ray.*", "", RegexOptions.IgnoreCase);
                movieTitle = Regex.Replace(movieTitle, @" \(?Disc \w+(?: of \w+)?\)?", "", RegexOptions.IgnoreCase);
                movieTitle = Regex.Replace(movieTitle, @"\s*[[(].*", "", RegexOptions.IgnoreCase);
                movieTitle = movieTitle.Trim();
            }

            if (Regex.Replace(movieTitle, @"\W", "").ToLowerInvariant() == "bluray")
            {
                movieTitle = "";
            }

            // TMDb chokes on dashes
            movieTitle = Regex.Replace(movieTitle, @"-+", " ");

            // No valid bdmt_<lang>.xml value found; use volume label as fallback
            if (string.IsNullOrWhiteSpace(movieTitle))
            {
                movieTitle = volumeLabel;
                movieTitle = Regex.Replace(movieTitle, @"^\d{6,}_", ""); // e.g., "01611720_GOODFELLAS" => "GOODFELLAS"
                movieTitle = Regex.Replace(movieTitle, @"_NA$", ""); // remove trailing region codes (NA = North America)
                movieTitle = Regex.Replace(movieTitle, @"_+", " ");
            }

            movieTitle = Regex.Replace(movieTitle, @"^(.*), (A|An|The)$", "$2 $1", RegexOptions.IgnoreCase);

            disc.MovieTitle = movieTitle;
        }

        #endregion
    }

    
}
