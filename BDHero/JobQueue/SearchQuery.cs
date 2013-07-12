using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.JobQueue
{
    public class SearchQuery
    {
        public string Title;
        public int? Year;

        public override string ToString()
        {
            return Year.HasValue
                       ? string.Format("{0} ({1})", Title, Year)
                       : Title;
        }
    }
}
