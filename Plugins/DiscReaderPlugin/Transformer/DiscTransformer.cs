using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BDHero.BDROM;
using DotNetUtils.Extensions;
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
                        PrimaryLanguage = bdrom.DiscLanguage,
                        Playlists = PlaylistTransformer.Transform(tsPlaylistFiles)
                    };

            DiscFileSystemTransformer.Transform(bdrom, disc);
            DiscFeaturesTransformer.Transform(disc);
            DiscMetadataTransformer.Transform(disc);

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
            var derived = disc.Metadata.Derived;
            var validBdmtTitles = derived.ValidBdmtTitles;
            if (validBdmtTitles.ContainsKey(disc.PrimaryLanguage))
            {
                derived.SearchableTitle = validBdmtTitles[disc.PrimaryLanguage];
            }
            else
            {
                derived.SearchableTitle = derived.DBOXTitleSanitized ?? derived.VolumeLabelSanitized;
            }
        }

        #endregion
    }

    
}
