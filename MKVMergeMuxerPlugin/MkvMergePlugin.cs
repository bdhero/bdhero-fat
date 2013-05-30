using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.Queue;

namespace BDHero.Plugin.MKVMergeMuxer
{
    public class MkvMergePlugin : IMuxerPlugin
    {
        public IPluginHost Host { get; set; }
        public string Name { get { return "mkvmerge (mkvtoolnix)"; } }

        public event PluginProgressHandler ProgressUpdated;
        public event EditPluginPreferenceHandler EditPreferences;

        public MatroskaFeatures MatroskaFeatures
        {
            get
            {
                return MatroskaFeatures.Chapters
                     | MatroskaFeatures.CoverArt
                     | MatroskaFeatures.DefaultFlag
                     | MatroskaFeatures.ForcedFlag
                    ;
            }
        }

        public void LoadPlugin()
        {
        }

        public void UnloadPlugin()
        {
        }

        public void Mux(Job job)
        {
        }
    }
}
