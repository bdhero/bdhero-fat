using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using BDHero.Plugin.DiscReader.Transformer;
using BDInfo;
using ProcessUtils;

namespace BDHero.Plugin.DiscReader
{
    public class DiscReader : IDiscReaderPlugin
    {
        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "BDHero Disc Reader"; } }

        public event PluginProgressHandler ProgressUpdated;
        public event EditPluginPreferenceHandler EditPreferences;

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public Disc ReadBDROM(string bdromPath)
        {
            var bdrom = new BDInfo.BDROM(bdromPath);
            bdrom.ScanProgress += BDROMOnScanProgress;
            bdrom.Scan();
            var disc = DiscTransformer.Transform(bdrom);
            return disc;
        }

        private void BDROMOnScanProgress(BDROMScanProgressState bdromState)
        {
#if false
            Console.WriteLine("BDROM: {0}: scanning {1} of {2} ({3}%).  Total: {4} of {5} ({6}%).",
                bdromState.FileType, bdromState.CurFileOfType, bdromState.NumFilesOfType, bdromState.TypeProgress.ToString("0.00"),
                bdromState.CurFileOverall, bdromState.NumFilesOverall, bdromState.OverallProgress.ToString("0.00"));
#endif

            var progressState = new ProgressState
                {
                    PercentComplete = bdromState.OverallProgress,
                    ProcessState = bdromState.OverallProgress >= 100.0 ? NonInteractiveProcessState.Completed : NonInteractiveProcessState.Running
                };

            if (ProgressUpdated != null)
                ProgressUpdated(this, progressState);

            Host.ReportProgress(this, progressState);
        }
    }
}
