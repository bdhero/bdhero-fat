using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.Plugin
{
    public interface IPluginHost
    {
        /// <summary>
        /// Invoked whenever a plugin's state or progress changes.
        /// </summary>
        event PluginProgressHandler PluginProgressChanged;

        /// <summary>
        /// Allows plugins to report their progress to the host.
        /// </summary>
        /// <param name="plugin">Plugin that is reporting its progress</param>
        /// <param name="percentComplete">0.0 to 100.0</param>
        /// <param name="status">Description of what the plugin is currently doing (e.g., "Parsing 00850.MPLS", "Muxing to MKV @ 00:45:19")</param>
        void ReportProgress(IPlugin plugin, double percentComplete, string status);

        ProgressProvider GetProgressProvider(IPlugin plugin);
    }
}
