using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using BDHero.JobQueue;

namespace BDHero.Plugin.FileNamer
{
    public class FileNamerPlugin : INameProviderPlugin
    {
        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "BDHero File Namer"; } }

        public bool Enabled { get; set; }

        public Icon Icon { get { return null; } }

        public int RunOrder { get { return 0; } }

        public EditPluginPreferenceHandler EditPreferences { get; private set; }

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        // TODO: Write full implementation
        public void Rename(CancellationToken cancellationToken, Job job)
        {
            Host.ReportProgress(this, 0.0, "Auto-renaming output file...");

            var prefs = new Preferences();
            var namer = new FileNamer(job, prefs);

            job.OutputPath = namer.GetPath();

            Host.ReportProgress(this, 100.0, "Finished auto-renaming output file");
        }
    }
}
