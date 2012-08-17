using System;
using System.Collections.Generic;
using System.Text;

namespace BDInfo.models
{
    class MplsSearchResult
    {
        public bool success = false;
        public bool error = true;
        public List<MplsSearchResultError> errors = new List<MplsSearchResultError>();
        public ulong mainMplsFileSize = 0;
    }
}
