﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.JobQueue;

namespace BDHero.Plugin.MKVMergeMuxer
{
    public class MkvMergePlugin : IMuxerPlugin
    {
        public IPluginHost Host { get; private set; }
        public PluginAssemblyInfo AssemblyInfo { get; private set; }

        public string Name { get { return "mkvmerge (mkvtoolnix)"; } }

        public event EditPluginPreferenceHandler EditPreferences;

        public MatroskaFeatures SupportedFeatures
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

        public void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo)
        {
            Host = host;
            AssemblyInfo = assemblyInfo;
        }

        public void UnloadPlugin()
        {
        }

        public void Mux(Job job)
        {
        }
    }
}
