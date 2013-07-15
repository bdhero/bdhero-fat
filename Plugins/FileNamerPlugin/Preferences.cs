using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;

namespace BDHero.Plugin.FileNamer
{
    internal class Preferences
    {
        public MoviePreferences Movies = new MoviePreferences();
        public TVShowPreferences TVShows = new TVShowPreferences();

        public IDictionary<string, string> Codecs = new Dictionary<string, string>();

        public bool ReplaceSpaces = false;
        public string ReplaceSpacesWith = ".";

        public Preferences()
        {
            foreach (var codec in Codec.MuxableBDCodecs)
            {
                Codecs[codec.SerializableName] = codec.FileName;
            }
        }

        public string GetCodecName(Codec codec)
        {
            return Codecs.ContainsKey(codec.SerializableName) ? Codecs[codec.SerializableName] : null;
        }

        public Preferences CopyFrom(Preferences other)
        {
            Movies.CopyFrom(other.Movies);
            TVShows.CopyFrom(other.TVShows);

            Codecs.Clear();
            foreach (var k in other.Codecs.Keys)
            {
                Codecs[k] = other.Codecs[k];
            }

            ReplaceSpaces = other.ReplaceSpaces;
            ReplaceSpacesWith = other.ReplaceSpacesWith;

            return this;
        }

        public Preferences Clone()
        {
            return new Preferences().CopyFrom(this);
        }
    }

    internal abstract class ReleaseMediumPreferences
    {
        public string Directory = @"%temp%\%volume%";
        public string FileName = @"%title% [%res%]";

        public void CopyFrom(ReleaseMediumPreferences other)
        {
            Directory = other.Directory;
            FileName = other.FileName;
        }
    }

    internal class MoviePreferences : ReleaseMediumPreferences
    {
        public new string FileName = @"%title% (%year%) [%res%] [%vcodec%] [%acodec% %channels%]";

        public void CopyFrom(MoviePreferences other)
        {
            base.CopyFrom(other);
            FileName = other.FileName;
        }
    }

    internal class TVShowPreferences : ReleaseMediumPreferences
    {
        public new string Directory = @"%temp%\%title%\Season %season%";
        public new string FileName = @"s%season%e%episode% - %episodetitle% [%res%]";

        /// <summary>
        /// Format string for <see cref="int.ToString(string)"/>.
        /// </summary>
        public string SeasonNumberFormat = "D2";

        /// <summary>
        /// Format string for <see cref="int.ToString(string)"/>.
        /// </summary>
        public string EpisodeNumberFormat = "D2";

        /// <summary>
        /// Format string for <see cref="DateTime.ToString(string)"/>.
        /// </summary>
        public string ReleaseDateFormat = "yyyy-MM-dd";

        public void CopyFrom(TVShowPreferences other)
        {
            base.CopyFrom(other);
            Directory = other.Directory;
            FileName = other.FileName;
            SeasonNumberFormat = other.SeasonNumberFormat;
            EpisodeNumberFormat = other.EpisodeNumberFormat;
            ReleaseDateFormat = other.ReleaseDateFormat;
        }
    }
}
