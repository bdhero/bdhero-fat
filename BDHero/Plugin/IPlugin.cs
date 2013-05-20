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

        void LoadPlugin();
        void UnloadPlugin();

        event EditPluginPreferenceHandler EditPreferences;
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
        void AutoDetect(Disc disc);
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
