using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetUtils;
using ProcessUtils;

namespace BDHero.Plugin 
{
    /// <see cref="http://www.codeproject.com/Articles/6334/Plug-ins-in-C"/>
    public class PluginService : IPluginHost
    {
        public readonly IList<IPlugin> Plugins = new List<IPlugin>();

        public IList<IDiscReaderPlugin>       DiscReaderPlugins       { get { return Plugins.OfType<IDiscReaderPlugin>().ToList(); } }
        public IList<IMetadataProviderPlugin> MetadataProviderPlugins { get { return Plugins.OfType<IMetadataProviderPlugin>().ToList(); } }
        public IList<IAutoDetectorPlugin>     AutoDetectorPlugins     { get { return Plugins.OfType<IAutoDetectorPlugin>().ToList(); } }
        public IList<INameProviderPlugin>     NameProviderPlugins     { get { return Plugins.OfType<INameProviderPlugin>().ToList(); } }
        public IList<IMuxerPlugin>            MuxerPlugins            { get { return Plugins.OfType<IMuxerPlugin>().ToList(); } }
        public IList<IPostProcessorPlugin>    PostProcessorPlugins    { get { return Plugins.OfType<IPostProcessorPlugin>().ToList(); } }

        public event PluginProgressHandler PluginProgress;

        public void ReportProgress(IPlugin plugin, ProgressState progressState)
        {
            if (PluginProgress != null)
                PluginProgress(plugin, progressState);
        }

        /// <summary>
        /// Searches the Application's Startup Directory for Plugins
        /// </summary>
        public void LoadPlugins()
        {
            // First empty the collection, we're reloading them all
            ClosePlugins();

            var programDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var programDataPluginDir = GetPluginDir(programDataRoot);

            var appDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appDataPluginDir = GetPluginDir(appDataRoot);

//            AddPluginsRecursive(AppDomain.CurrentDomain.BaseDirectory);
            AddPluginsRecursive(programDataPluginDir);
            AddPluginsRecursive(appDataPluginDir);

#if DEBUG
            var slnDir = @"C:\Projects\bdhero";
            var projects = new[] { "AutoDetectorPlugin", "ChapterGrabberPlugin", "DiscReaderPlugin", "FFmpegMuxerPlugin", "MKVMergeMuxerPlugin" };
            foreach (var projectName in projects)
            {
                try
                {
                    var pluginDir = Path.Combine(slnDir, projectName, "bin", "Debug");
                    AddPluginsRecursive(pluginDir);
                }
                catch (Exception e)
                {
                    
                }
            }
#endif
        }

        private static string GetPluginDir(string root)
        {
            return Path.Combine(root, AssemblyUtils.GetAssemblyName(), "Plugins");
        }

        /// <summary>
        /// Searches the passed path for Plugins
        /// </summary>
        /// <param name="pluginDir">Directory to search for Plugins in</param>
        public void AddPluginsRecursive(string pluginDir)
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
            return file.Extension.Equals(".plugin.dll") || file.Name.EndsWith("Plugin.dll", StringComparison.OrdinalIgnoreCase);
        }

        public void AddPlugin(string fileName)
        {
            // Create a new assembly from the plugin file we're adding..
            Assembly pluginAssembly = Assembly.LoadFrom(fileName);

            // Next we'll loop through all the Types found in the assembly
            foreach (Type pluginType in pluginAssembly.GetTypes())
            {
                if (pluginType.IsPublic) // Only look at public types
                {
                    if (!pluginType.IsAbstract)  // Only look at non-abstract types
                    {
                        // Gets a type object of the interface we need the plugins to match
                        Type typeInterface = pluginType.GetInterface(typeof(IPlugin).FullName);

                        // Make sure the interface we want to use actually exists
                        if (typeInterface != null)
                        {
                            // Create a new instance and store the instance in the collection for later use
                            // We could change this later on to not load an instance.. we have 2 options
                            // 1- Make one instance, and use it whenever we need it.. it's always there
                            // 2- Don't make an instance, and instead make an instance whenever we use it, then close it
                            // For now we'll just make an instance of all the plugins
                            var newPlugin = (IPlugin)Activator.CreateInstance(pluginAssembly.GetType(pluginType.ToString()));

                            // Set the Plugin's host to this class which inherited IPluginHost
                            newPlugin.Host = this;

                            // Call the initialization sub of the plugin
                            newPlugin.LoadPlugin();

                            // Add the new plugin to our collection here
                            Plugins.Add(newPlugin);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Unloads and Closes all AvailablePlugins
        /// </summary>
        public void ClosePlugins()
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
    }
}


