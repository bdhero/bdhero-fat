using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using BDHero.Queue;

namespace BDHero.Plugin
{
    public delegate void EditPluginPreferenceHandler(IWin32Window owner);

    public interface IPlugin
    {
        IPluginHost Host { get; set; }
        string Name { get; }

        event PluginProgressHandler ProgressUpdated;
        event EditPluginPreferenceHandler EditPreferences;

        void LoadPlugin();
        void UnloadPlugin();
    }

    public interface IDiscReaderPlugin : IPlugin
    {
        Disc ReadBDROM(string bdromPath);
    }

    public interface IMetadataProviderPlugin : IPlugin
    {
        void GetMetadata(Disc disc);
    }

    public interface IAutoDetectorPlugin : IPlugin
    {
        void AutoDetect(Job job);
    }

    public interface INameProviderPlugin : IPlugin
    {
        void Rename(Job job);
    }

    public interface IMuxerPlugin : IPlugin
    {
        void Mux(Job job);
    }

    public interface IPostProcessorPlugin : IPlugin
    {
        void PostProcess(Job job);
    }
}
