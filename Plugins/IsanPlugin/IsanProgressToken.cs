using System.Threading;
using BDHero.Plugin;

namespace IsanPlugin
{
    /// <summary>
    /// <para>
    /// Wrapper for an <see cref="IPluginHost"/>, <see cref="IPlugin"/>, and <see cref="CancellationToken"/>
    /// that allows all 3 to be passed around as a single object instead of being passed separately.
    /// </para>
    /// <para>
    /// It also allows <see cref="IsanMetadataProvider"/> to report its progress and cancel its search
    /// without having to know anything about <see cref="IPluginHost"/>, <see cref="IPlugin"/>, or <see cref="CancellationToken"/>.
    /// </para>
    /// </summary>
    public class IsanProgressToken
    {
        private readonly CancellationToken _token;

        private readonly IPlugin _plugin;
        private readonly IPluginHost _host;

        /// <summary>
        /// Gets whether the user has requested to cancel the current operation.
        /// </summary>
        public bool IsCancelled { get { return _token.IsCancellationRequested; } }

        /// <summary>
        /// Constructs a new IsanProgressToken that does <strong>not</strong> report its progress
        /// and <strong>cannot</strong> be cancelled.
        /// </summary>
        public IsanProgressToken() : this(null, null, CancellationToken.None)
        {
        }

        /// <summary>
        /// Constructs a new IsanProgressToken that does reports its progress to the <paramref name="host"/>
        /// and may be cancelled via the given <paramref name="token"/>.
        /// </summary>
        public IsanProgressToken(IPluginHost host, IPlugin plugin, CancellationToken token)
        {
            _host = host;
            _token = token;
            _plugin = plugin;
        }

        public void ReportProgress(double percentComplete, string status)
        {
            if (_host != null)
                _host.ReportProgress(_plugin, percentComplete, status);
        }
    }
}