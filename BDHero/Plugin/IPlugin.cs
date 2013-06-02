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
        IPluginHost Host { get; }

        /// <summary>
        /// Full path to the plugin DLL.
        /// </summary>
        string Location { get; }

        /// <summary>
        /// GUID of the plugin assembly DLL.
        /// </summary>
        string Guid { get; }

        /// <summary>
        /// Human-friendly name of the plugin that will be displayed in the UI.
        /// </summary>
        string Name { get; }

        event PluginProgressHandler ProgressUpdated;
        event EditPluginPreferenceHandler EditPreferences;

        /// <summary>
        /// Invoked when the application first starts up and loads the plugin assembly.
        /// </summary>
        /// <param name="host">Host object that loaded the plugin</param>
        /// <param name="location">Full path to the plugin DLL</param>
        /// <param name="guid">The plugin assembly DLL's GUID</param>
        /// <exception cref="PluginException"></exception>
        void LoadPlugin(IPluginHost host, string location, string guid);

        /// <summary>
        /// Invoked when the application is about to exit.
        /// </summary>
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
