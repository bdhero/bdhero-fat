using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDInfo.models
{
    class JsonPlaylist
    {
        public string filename;
        public ulong filesize;
        public double length;
        public string ISO_639_2;

        public bool is_main = false;
        public bool is_theatrical = false;
        public bool is_special = false;
        public bool is_extended = false;
        public bool is_unrated = false;
        public bool is_commentary = false;
    }
}
