using System.Collections.Generic;
using System.Linq;

namespace MediaInfoWrapper
{
    /// <summary>
    /// Helper class for constructing a list of arguments to be passed to a command line application.
    /// </summary>
    public class CLIArguments : List<string>
    {
        private const string DoubleQuote = "\"";
        private const string DoubleQuoteEscaped = "\\\"";

        /// <summary>
        /// Gets or sets whether null arguments should be skipped by ToString().
        /// </summary>
        public bool SkipNullArgs = true;

        public CLIArguments(params string[] args)
        {
            AddAll(args);
        }

        public CLIArguments(IEnumerable<string> args)
        {
            AddAll(args);
        }

        /// <summary>
        /// Adds all arguments, regardless of whether they are null or empty.
        /// </summary>
        /// <param name="args"></param>
        public void AddAll(params string[] args)
        {
            AddRange(args);
        }

        /// <summary>
        /// Adds all arguments, regardless of whether they are null or empty.
        /// </summary>
        /// <param name="args"></param>
        public void AddAll(IEnumerable<string> args)
        {
            AddRange(args);
        }

        /// <summary>
        /// Adds all non-null, non-empty arguments (i.e., skips null/empty arguments).
        /// </summary>
        /// <param name="args"></param>
        public void AddNonEmpty(params string[] args)
        {
            AddAll(args.Where(arg => !string.IsNullOrEmpty(arg)));
        }

        /// <summary>
        /// Adds all arguments iff all arguments are non-null and non-empty.  If any arguments are null or empty, nothing is added.
        /// </summary>
        /// <param name="args"></param>
        public void AddIfAllNonEmpty(params string[] args)
        {
            if (args.Any(string.IsNullOrEmpty))
                return;

            AddAll(args);
        }

        private bool KeepArg(string rawArg)
        {
            return rawArg != null || !SkipNullArgs;
        }

        public static string Escape(string rawArg)
        {
            return string.Format("{0}", rawArg).Replace(DoubleQuote, DoubleQuoteEscaped);
        }

        public static string ForCommandLine(string rawArg)
        {
            return rawArg.Contains(" ") ? string.Format("{0}{1}{0}", DoubleQuote, Escape(rawArg)) : rawArg;
        }

        public override string ToString()
        {
            return string.Join(" ", this.Where(KeepArg).Select(ForCommandLine));
        }
    }
}
