using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TmdbPlugin
{   
    public class Settings        
    {
        public Settings()
        {
            apiKey = "b59b366b0f0a457d58995537d847409a";
            defaultLanguage = "en";
            url = "http://acdvorak.github.io/bdautomuxer/";
        }
        public string apiKey { get; set; }
        public string defaultLanguage { get; set; }
        public string url { get; set; }
    }
    
    public class PluginSettings
    {
        public Settings settings { get; set; }
    }
}

