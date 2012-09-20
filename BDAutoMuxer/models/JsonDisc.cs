using System.Collections.Generic;

namespace BDAutoMuxer.models
{
    public class JsonDisc
    {
        public string disc_name;
        public string volume_label;
        public string ISO_639_2;
        
        public int tmdb_id;
        public string movie_title;
        public int? year;

        public IList<JsonPlaylist> playlists;
    }
}
