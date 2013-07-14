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
                Codecs[codec.SerializableName] = codec.CommonName;
            }
        }

        public string GetCodecName(Codec codec)
        {
            return Codecs.ContainsKey(codec.SerializableName) ? Codecs[codec.SerializableName] : null;
        }
    }

    internal abstract class ReleaseMediumPreferences
    {
        public string Directory = @"%temp%\%volume%";
        public string FileName = "%title% [%res%]";
    }

    internal class MoviePreferences : ReleaseMediumPreferences
    {
        public new string FileName = "%title% (%year%) [%res%] [V- %vcodec%] [A- %acodec%] [%cut%] [V- %vlang%] [A- %alang%]";
    }

    internal class TVShowPreferences : ReleaseMediumPreferences
    {
        public new string FileName = "s%season%e%episode% - %title% [%res%]";

        /// <summary>
        /// Format string for <see cref="int.ToString(string)"/>.
        /// </summary>
        public string SeasonNumberFormat = "DD";

        /// <summary>
        /// Format string for <see cref="int.ToString(string)"/>.
        /// </summary>
        public string EpisodeNumberFormat = "DD";
    }
}
