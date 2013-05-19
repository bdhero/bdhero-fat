using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DotNetUtils;

namespace BDHero.Plugin 
{
    /// <see cref="http://www.codeproject.com/Articles/6334/Plug-ins-in-C"/>
	public class PluginService : IPluginHost
	{
		private Types.AvailablePlugins _colAvailablePlugins = new Types.AvailablePlugins();
		
		/// <summary>
		/// A Collection of all Plugins Found and Loaded by the FindPlugins() Method
		/// </summary>
		public Types.AvailablePlugins AvailablePlugins
		{
			get {return _colAvailablePlugins;}
			set {_colAvailablePlugins = value;}
		}
		
		/// <summary>
		/// Searches the Application's Startup Directory for Plugins
		/// </summary>
		public void FindPlugins()
		{
			// First empty the collection, we're reloading them all
			_colAvailablePlugins.Clear();

		    var programDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
		    var programDataPluginDir = Path.Combine(programDataRoot, AssemblyUtils.GetAssemblyName(), "Plugins");

//            AddPlugins(AppDomain.CurrentDomain.BaseDirectory);
            AddPlugins(programDataPluginDir);
		}

		/// <summary>
		/// Searches the passed path for Plugins
		/// </summary>
		/// <param name="path">Directory to search for Plugins in</param>
		public void AddPlugins(string path)
		{
			// Go through all the files in the plugin directory
			foreach (string fileOn in Directory.GetFiles(path))
			{
				FileInfo file = new FileInfo(fileOn);
				
				// Preliminary check, must be .dll
				if (file.Extension.Equals(".plugin.dll"))
				{	
					// Add the 'plugin'
					this.AddPlugin(fileOn);				
				}
			}
		}
		
		/// <summary>
		/// Unloads and Closes all AvailablePlugins
		/// </summary>
		public void ClosePlugins()
		{
			foreach (Types.AvailablePlugin pluginOn in _colAvailablePlugins)
			{
				// Close all plugin instances
				// We call the plugins Dispose sub first incase it has to do 
				// Its own cleanup stuff
				pluginOn.Instance.UnloadPlugin(); 
				
				// After we give the plugin a chance to tidy up, get rid of it
				pluginOn.Instance = null;
			}
			
			// Finally, clear our collection of available plugins
			_colAvailablePlugins.Clear();
		}
		
		private void AddPlugin(string fileName)
		{
			// Create a new assembly from the plugin file we're adding..
			Assembly pluginAssembly = Assembly.LoadFrom(fileName);
			
			// Next we'll loop through all the Types found in the assembly
			foreach (Type pluginType in pluginAssembly.GetTypes())
			{
				if (pluginType.IsPublic) //Only look at public types
				{
					if (!pluginType.IsAbstract)  //Only look at non-abstract types
					{
						// Gets a type object of the interface we need the plugins to match
                        Type typeInterface = pluginType.GetInterface(typeof(IPlugin).FullName);
						
						// Make sure the interface we want to use actually exists
						if (typeInterface != null)
						{
							//Create a new available plugin since the type implements the IPlugin interface
							Types.AvailablePlugin newPlugin = new Types.AvailablePlugin();
							
							// Set the filename where we found it
							newPlugin.AssemblyPath = fileName;
							
							// Create a new instance and store the instance in the collection for later use
							// We could change this later on to not load an instance.. we have 2 options
							// 1- Make one instance, and use it whenever we need it.. it's always there
							// 2- Don't make an instance, and instead make an instance whenever we use it, then close it
							// For now we'll just make an instance of all the plugins
							newPlugin.Instance = (IPlugin)Activator.CreateInstance(pluginAssembly.GetType(pluginType.ToString()));
							
							// Set the Plugin's host to this class which inherited IPluginHost
							newPlugin.Instance.Host = this;

							// Call the initialization sub of the plugin
							newPlugin.Instance.LoadPlugin();
							
							// Add the new plugin to our collection here
							this._colAvailablePlugins.Add(newPlugin);
							
							// cleanup a bit
							newPlugin = null;
						}	
						
						typeInterface = null; //Mr. Clean			
					}				
				}			
			}
			
			pluginAssembly = null; // more cleanup
		}

        public void ReportProgress(IPlugin plugin, double progressPercent)
	    {
	        throw new NotImplementedException();
	    }
	}

	namespace Types
	{
		/// <summary>
		/// Collection for AvailablePlugin Type
		/// </summary>
		public class AvailablePlugins : System.Collections.CollectionBase
		{
			// A Simple Home-brew class to hold some info about our Available Plugins
			
			/// <summary>
			/// Add a Plugin to the collection of Available plugins
			/// </summary>
			/// <param name="pluginToAdd">The Plugin to Add</param>
			public void Add(Types.AvailablePlugin pluginToAdd)
			{
				this.List.Add(pluginToAdd); 
			}
		
			/// <summary>
			/// Remove a Plugin to the collection of Available plugins
			/// </summary>
			/// <param name="pluginToRemove">The Plugin to Remove</param>
			public void Remove(Types.AvailablePlugin pluginToRemove)
			{
				this.List.Remove(pluginToRemove);
			}
		
			/// <summary>
			/// Finds a plugin in the available Plugins
			/// </summary>
			/// <param name="pluginNameOrPath">The name or File path of the plugin to find</param>
			/// <returns>Available Plugin, or null if the plugin is not found</returns>
			public Types.AvailablePlugin Find(string pluginNameOrPath)
			{
				Types.AvailablePlugin toReturn = null;
			
				// Loop through all the plugins
				foreach (Types.AvailablePlugin pluginOn in this.List)
				{
					// Find the one with the matching name or filename
					if ((pluginOn.Instance.Name.Equals(pluginNameOrPath)) || pluginOn.AssemblyPath.Equals(pluginNameOrPath))
					{
						toReturn = pluginOn;
						break;		
					}
				}
				return toReturn;
			}
		}
		
		/// <summary>
		/// Data Class for Available Plugin.  Holds and instance of the loaded Plugin, as well as the Plugin's Assembly Path
		/// </summary>
		public class AvailablePlugin
		{
			// This is the actual AvailablePlugin object.. 
			// Holds an instance of the plugin to access
			// Also holds assembly path... not really necessary
			private IPlugin myInstance = null;
			private string myAssemblyPath = "";
			
			public IPlugin Instance
			{
				get {return myInstance;}
				set	{myInstance = value;}
			}
			public string AssemblyPath
			{
				get {return myAssemblyPath;}
				set {myAssemblyPath = value;}
			}
		}
	}	
}


