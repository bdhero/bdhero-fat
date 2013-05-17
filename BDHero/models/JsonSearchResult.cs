using System.Collections.Generic;

namespace BDHero.models
{
// ReSharper disable InconsistentNaming
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
// ReSharper restore InconsistentNaming
}
