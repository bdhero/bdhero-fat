using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.JobQueue;

namespace BDHero.Plugin
{
    /// <summary>
    /// Allows plugins to report their progress to the host.
    /// </summary>
    /// <param name="plugin">Plugin that is reporting its progress</param>
    /// <param name="progressProvider">Provides plugin status information</param>
    public delegate void PluginProgressHandler(IPlugin plugin, ProgressProvider progressProvider);
}
