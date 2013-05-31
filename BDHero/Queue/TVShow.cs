using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.Queue
{
    public class TVShow : ReleaseMedium
    {
        public readonly IList<TVShowEpisode> Episodes = new List<TVShowEpisode>();

        public int SelectedEpisodeIndex;
    }
}
