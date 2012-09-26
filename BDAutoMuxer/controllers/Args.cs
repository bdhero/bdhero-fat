using System.Collections.Generic;
using System.Linq;

namespace BDAutoMuxer.controllers
{
    public class Args : List<string>
    {
        public Args(params string[] args)
        {
            AddAll(args);
        }

        public void AddAll(params string[] args)
        {
            AddRange(args);
        }

        public void AddAll(IEnumerable<string> args)
        {
            AddRange(args);
        }

        public void AddNonEmpty(params string[] args)
        {
            AddAll(args.Where(arg => !string.IsNullOrEmpty(arg)));
        }

        public void AddIfAllNonEmpty(params string[] args)
        {
            if (args.Any(string.IsNullOrEmpty))
                return;

            AddAll(args);
        }
    }
}
