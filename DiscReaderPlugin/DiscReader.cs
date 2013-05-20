using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using BDHero.Plugin.DiscReader.Transformer;
using BDInfo;

namespace BDHero.Plugin.DiscReader
{
    public class DiscReader : IDiscReaderPlugin
    {
        public IPluginHost Host { get; set; }
        public string Name { get { return "BDHero Disc Reader"; } }

        public event EditPluginPreferenceHandler EditPreferences;

        public void LoadPlugin()
        {
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

        private void BDROMOnScanProgress(BDROMScanProgressState state)
        {
            Console.WriteLine("BDROM: {0}: scanning {1} of {2} ({3}%).  Total: {4} of {5} ({6}%).",
                state.FileType, state.CurFileOfType, state.NumFilesOfType, state.TypeProgress.ToString("0.00"),
                state.CurFileOverall, state.NumFilesOverall, state.OverallProgress.ToString("0.00"));

            Host.ReportProgress(this, state.OverallProgress);
        }
    }
}
