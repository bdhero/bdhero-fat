using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using BDHero.JobQueue;
using BDHero.Plugin;

namespace IsanPlugin
{
    public class IsanPlugin : IMetadataProviderPlugin
    {
        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "ISAN Metadata Provider"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get; private set; }

        public EditPluginPreferenceHandler EditPreferences { get; private set; }

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public void GetMetadata(CancellationToken cancellationToken, Job job)
        {
//            IsanMetadataProvider.Populate(job.Disc.Metadata.Raw.V_ISAN);
        }
    }
}
