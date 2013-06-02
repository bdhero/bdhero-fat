using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BDHero.Plugin
{
    /// <summary>
    /// Exception thrown by an <see cref="IPlugin"/>.
    /// </summary>
    public class PluginException : Exception
    {
        /// <summary>
        /// Severity of the exception.
        /// </summary>
        public PluginExceptionSeverity Severity { get; private set; }

        public PluginException(string message, PluginExceptionSeverity severity)
            : base(message)
        {
            Severity = severity;
        }

        public PluginException(string message, Exception innerException, PluginExceptionSeverity severity)
            : base(message, innerException)
        {
            Severity = severity;
        }
    }

    /// <summary>
    /// Severity of an exception thrown by an <see cref="IPlugin"/>.
    /// </summary>
    public enum PluginExceptionSeverity
    {
        /// <summary>
        /// Fatal error that leaves the plugin in an unusable, unrecoverable state.  The plugin is unable to function properly until the error is fixed.
        /// </summary>
        /// <example>An <see cref="IOException"/> is thrown while attempting to read or deserialize the plugin's config file.</example>
        Fatal,

        /// <summary>
        /// Non-fatal error that reduces or eliminates the usefulness of the plugin, but which the plugin can recover from, leaving it in a working, usable state.
        /// </summary>
        /// <example>
        /// The user is behind a corporate proxy or filewall, and HTTP requests to TMDb return a 401 or 403 status.  The TMDb plugin is therefore
        /// unable to retrieve search results for a movie, which means it cannot provide any metadata (movie title, release date, or cover art).
        /// </example>
        Error
    }
}
