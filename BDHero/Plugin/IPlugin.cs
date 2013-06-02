using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BDHero.BDROM;
using BDHero.JobQueue;

namespace BDHero.Plugin
{
    public delegate void EditPluginPreferenceHandler(IWin32Window owner);

    public interface IPlugin
    {
        IPluginHost Host { get; set; }
        string Name { get; }

        event PluginProgressHandler ProgressUpdated;
        event EditPluginPreferenceHandler EditPreferences;

        /// <exception cref="PluginException"></exception>
        void LoadPlugin();
        /// <exception cref="PluginException"></exception>
        void UnloadPlugin();
    }

    public interface IDiscReaderPlugin : IPlugin
    {
        /// <exception cref="PluginException"></exception>
        Disc ReadBDROM(string bdromPath);
    }

    public interface IMetadataProviderPlugin : IPlugin
    {
        /// <exception cref="PluginException"></exception>
        void GetMetadata(Job job);
    }

    public interface IAutoDetectorPlugin : IPlugin
    {
        /// <exception cref="PluginException"></exception>
        void AutoDetect(Job job);
    }

    public interface INameProviderPlugin : IPlugin
    {
        /// <exception cref="PluginException"></exception>
        void Rename(Job job);
    }

    public interface IMuxerPlugin : IPlugin
    {
        MatroskaFeatures SupportedFeatures { get; }

        /// <exception cref="PluginException"></exception>
        void Mux(Job job);
    }

    public interface IPostProcessorPlugin : IPlugin
    {
        /// <exception cref="PluginException"></exception>
        void PostProcess(Job job);
    }

    [Flags]
    public enum MatroskaFeatures
    {
        None        = 0x00,
        Chapters    = 0x01,
        CoverArt    = 0x02,
        LPCM        = 0x04,
        DefaultFlag = 0x08,
        ForcedFlag  = 0x10,
    }
}
