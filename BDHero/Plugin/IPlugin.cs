using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        /// Human-friendly name of the plugin that will be displayed in the UI.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Contains information about the plugin DLL and config file.
        /// </summary>
        PluginAssemblyInfo AssemblyInfo { get; }

        event PluginProgressHandler ProgressUpdated;
        event EditPluginPreferenceHandler EditPreferences;

        /// <summary>
        /// Invoked when the application first starts up and loads the plugin assembly.
        /// </summary>
        /// <param name="host">Host object that loaded the plugin</param>
        /// <param name="assemblyInfo">Contains information about the plugin DLL and config file</param>
        /// <exception cref="PluginException"></exception>
        void LoadPlugin(IPluginHost host, PluginAssemblyInfo assemblyInfo);

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

    /// <summary>
    /// Contains information about the plugin DLL and config file.
    /// </summary>
    public class PluginAssemblyInfo
    {
        /// <summary>
        /// Full path to the plugin DLL.
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// Version of the plugin assembly DLL.
        /// </summary>
        public Version Version { get; private set; }

        /// <summary>
        /// GUID of the plugin assembly DLL.
        /// </summary>
        public string Guid { get; private set; }

        /// <summary>
        /// Returns the full path to the plugin's JSON config file.
        /// </summary>
        /// <remarks>The path to the config file is the same as that of the DLL, except with a ".config.json" extension instead of ".dll".</remarks>
        public string SettingsFile { get { return new Regex(@"\.dll$", RegexOptions.IgnoreCase).Replace(Location, ".config.json"); } }

        public PluginAssemblyInfo(string location, Version version, string guid)
        {
            Location = location;
            Version = version;
            Guid = guid;
        }
    }
}
