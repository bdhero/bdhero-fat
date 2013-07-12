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

        public int RunOrder { get { return 0; } }

        public EditPluginPreferenceHandler EditPreferences { get; private set; }

        private readonly IsanMetadataProvider _provider = new IsanMetadataProvider();

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
            var raw = job.Disc.Metadata.Raw;
            var derived = job.Disc.Metadata.Derived;

            _provider.Populate(raw.V_ISAN);

            var isan = raw.ISAN;
            if (isan != null && !string.IsNullOrWhiteSpace(isan.Title))
            {
                derived.SearchableTitles.Insert(0, new SearchQuery { Title = isan.Title, Year = isan.Year });
            }
        }
    }
}
