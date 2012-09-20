using System;
using System.Collections.Generic;
using System.Text;

namespace BDAutoMuxer.models
{
    public class JsonSearchResult
    {
        public bool success = false;
        public bool error = true;
        public IList<JsonDisc> discs = new List<JsonDisc>();
        public IList<JsonSearchResultError> errors = new List<JsonSearchResultError>();
    }

    public class JsonSearchResultError
    {
        public string textStatus = null;
        public string errorMessage = null;
    }
}
