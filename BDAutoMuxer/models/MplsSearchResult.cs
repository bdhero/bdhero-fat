using System.Collections.Generic;

namespace BDAutoMuxer.models
{
    class MplsSearchResult
    {
        public bool success = false;
        public bool error = true;
        public List<MplsSearchResultError> errors = new List<MplsSearchResultError>();
        public ulong mainMplsFileSize = 0;
    }
}
