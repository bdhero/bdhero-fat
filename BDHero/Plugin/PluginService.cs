using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using BDHero.Startup;
using DotNetUtils;
using DotNetUtils.Annotations;
using Ninject;

namespace BDHero.Plugin 
{
    /// <see cref="http://www.codeproject.com/Articles/6334/Plug-ins-in-C"/>
    [UsedImplicitly]
    public class PluginService : IPluginHost
    {
        private readonly ConcurrentDictionary<string, ProgressProvider> _progressProviders =
            new ConcurrentDictionary<string, ProgressProvider>();

        public readonly IList<IPlugin> Plugins = new List<IPlugin>();

        public IList<IPlugin> PluginsByType
        {
            get
            {
                var plugins = new List<IPlugin>();
                plugins.AddRange(DiscReaderPlugins);
                plugins.AddRange(MetadataProviderPlugins);
                plugins.AddRange(AutoDetectorPlugins);
                plugins.AddRange(NameProviderPlugins);
                plugins.AddRange(MuxerPlugins);
                plugins.AddRange(PostProcessorPlugins);
                return plugins;
            }
        }

        public IList<IDiscReaderPlugin>       DiscReaderPlugins       { get { return PluginsOfType<IDiscReaderPlugin>(); } }
        public IList<IMetadataProviderPlugin> MetadataProviderPlugins { get { return PluginsOfType<IMetadataProviderPlugin>(); } }
        public IList<IAutoDetectorPlugin>     AutoDetectorPlugins     { get { return PluginsOfType<IAutoDetectorPlugin>(); } }
        public IList<INameProviderPlugin>     NameProviderPlugins     { get { return PluginsOfType<INameProviderPlugin>(); } }
        public IList<IMuxerPlugin>            MuxerPlugins            { get { return PluginsOfType<IMuxerPlugin>(); } }
        public IList<IPostProcessorPlugin>    PostProcessorPlugins    { get { return PluginsOfType<IPostProcessorPlugin>(); } }

        private IList<T> PluginsOfType<T>() where T : IPlugin
        {
            return Plugins.OfType<T>().OrderBy(plugin => plugin.RunOrder).ToList();
        }

        public event PluginProgressHandler PluginProgressChanged;

        private readonly IDirectoryLocator _directoryLocator;
        private readonly IKernel _kernel;

        public PluginService(IDirectoryLocator directoryLocator, IKernel kernel)
        {
            _directoryLocator = directoryLocator;
            _kernel = kernel;
        }

        public void ReportProgress(IPlugin plugin, double percentComplete, string status)
        {
            var progressProvider = GetProgressProvider(plugin);

            progressProvider.Update(percentComplete, status);

            if (PluginProgressChanged != null)
                PluginProgressChanged(plugin, progressProvider);
        }

        public ProgressProvider GetProgressProvider(IPlugin plugin)
        {
            var progressProvider = _progressProviders.GetOrAdd(plugin.AssemblyInfo.Guid, guid => new ProgressProvider());
            progressProvider.Plugin = plugin;
            return progressProvider;
        }

        /// <summary>
        /// Searches the Application's Startup Directory for Plugins
        /// </summary>
        /// <param name="path">Full path to the root Plugins directory</param>
        public void LoadPlugins(string path)
        {
#if DEBUG
            if (_devPluginsLoaded)
                return;

            // If running in Visual Studio, load plugins from project bin dirs
            var installDir = AssemblyUtils.GetInstallDir();
            var vhostFiles = Directory.GetFiles(installDir, "*.vshost.exe", SearchOption.TopDirectoryOnly);
            if (vhostFiles.Any())
            {
                LoadDevPlugins();
                _devPluginsLoaded = true;
                return;
            }
#endif

            AddPluginsRecursive(path);
        }

#if DEBUG

        private bool _devPluginsLoaded;

        private void LoadDevPlugins()
        {
            var solutionDir = GetSolutionDirPath();
            var projects = new[]
                {
                    "AutoDetectorPlugin", "ChapterGrabberPlugin", "ChapterWriterPlugin", "DiscReaderPlugin",
                    "FFmpegMuxerPlugin", "FileNamerPlugin", "MKVMergeMuxerPlugin", "IsanPlugin", "TmdbPlugin"
                };
            foreach (var projectName in projects)
            {
                try
                {
                    var pluginDir = Path.Combine(solutionDir, "Plugins", projectName, "bin", "Debug");
                    AddPluginsRecursive(pluginDir);
                }
                catch
                {
                }
            }
        }

        private static string GetSolutionDirPath()
        {
            var curDir = Directory.GetCurrentDirectory();
            DirectoryInfo parent;
            while (!SolutionFileExists(curDir) && (parent = new DirectoryInfo(curDir).Parent) != null)
            {
                curDir = parent.FullName;
            }
            return SolutionFileExists(curDir) ? curDir : @"C:\Projects\bdhero";
        }

        private static bool SolutionFileExists(string dirPath)
        {
            return File.Exists(Path.Combine(dirPath, "BDHero.sln"));
        }

#endif

        /// <summary>
        /// Unloads and clears all available loaded plugins.
        /// </summary>
        public void UnloadPlugins()
        {
            foreach (var plugin in Plugins)
            {
                // Close all plugin instances
                // We call the plugins Dispose sub first incase it has to do
                // Its own cleanup stuff
                plugin.UnloadPlugin();
            }

            // Finally, clear our collection of available plugins
            Plugins.Clear();
        }

        /// <summary>
        /// Searches the given directory and its subdirectories recursively for Plugins
        /// </summary>
        /// <param name="pluginDir">Root directory to search for Plugins in</param>
        private void AddPluginsRecursive(string pluginDir)
        {
            if (!Directory.Exists(pluginDir))
                return;

            // Go through all the files in the plugin directory
            foreach (FileInfo file in Directory.GetFiles(pluginDir).Select(filePath => new FileInfo(filePath)).Where(IsPlugin))
            {
                AddPlugin(file.FullName);
            }

            foreach (string dir in Directory.GetDirectories(pluginDir))
            {
                AddPluginsRecursive(dir);
            }
        }

        private static bool IsPlugin(FileInfo file)
        {
            return file.Name.EndsWith("Plugin.dll", StringComparison.OrdinalIgnoreCase);
        }

        private void AddPlugin(string dllPath)
        {
            // Create a new assembly from the plugin file we're adding..
            Assembly pluginAssembly = Assembly.LoadFrom(dllPath);

            var guid = AssemblyUtils.Guid(pluginAssembly);

            var machineName = Path.GetFileNameWithoutExtension(dllPath) ?? pluginAssembly.GetName().Name ?? "";
            machineName = Regex.Replace(machineName, "Plugin$", "", RegexOptions.IgnoreCase);
            var configFileName = machineName + ".config.json";
            var configFilePath = Path.Combine(_directoryLocator.PluginConfigDir, machineName, configFileName);

            // Next we'll loop through all the Types found in the assembly
            foreach (Type pluginType in pluginAssembly.GetTypes().Where(IsValidPlugin))
            {
                // Create a new instance and store the instance in the collection for later use
                // We could change this later on to not load an instance.. we have 2 options
                // 1- Make one instance, and use it whenever we need it.. it's always there
                // 2- Don't make an instance, and instead make an instance whenever we use it, then close it
                // For now we'll just make an instance of all the plugins
                var newPlugin = (IPlugin) _kernel.Get(pluginType);

                // TODO: Store this in preferences file
                newPlugin.Enabled = true;

                var assemblyInfo = new PluginAssemblyInfo(dllPath,
                                                          AssemblyUtils.GetAssemblyVersion(pluginAssembly),
                                                          AssemblyUtils.GetLinkerTimestamp(pluginAssembly),
                                                          guid,
                                                          configFilePath);

                // Initialize the plugin
                newPlugin.LoadPlugin(this, assemblyInfo);

                // Add the new plugin to our collection here
                Plugins.Add(newPlugin);
            }
        }

        private static bool IsValidPlugin(Type pluginType)
        {
            return HasPublicVisibility(pluginType)
                && IsConcreteClass(pluginType)
                && ImplementsPluginInterface(pluginType)
                ;
        }

        private static bool HasPublicVisibility(Type pluginType)
        {
            return pluginType.IsPublic;
        }

        private static bool IsConcreteClass(Type pluginType)
        {
            return !pluginType.IsAbstract && !pluginType.IsInterface;
        }

        private static bool ImplementsPluginInterface(Type pluginType)
        {
            // Gets a type object of the interface we need the plugins to match
            Type typeInterface = pluginType.GetInterface(typeof(IPlugin).FullName);

            // Make sure the interface we want to use actually exists
            return typeInterface != null;
        }
    }
}


