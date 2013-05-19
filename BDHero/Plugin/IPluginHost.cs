using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BDHero.Plugin
{
    public interface IPluginHost
    {
        /// <summary>
        /// Allows plugins to report their progress to the host.
        /// </summary>
        /// <param name="plugin">Plugin that is reporting its progress</param>
        /// <param name="progressPercent">0.0 to 1.0</param>
        void ReportProgress(IPlugin plugin, double progressPercent);
    }
}
