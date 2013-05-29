using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProcessUtils;

namespace BDHero.Plugin
{
    /// <summary>
    /// Allows plugins to report their progress to the host.
    /// </summary>
    /// <param name="plugin">Plugin that is reporting its progress</param>
    /// <param name="progressState">Contains percentage completed, runtime, time remaining, etc.</param>
    public delegate void PluginProgressHandler(IPlugin plugin, ProgressState progressState);
}
