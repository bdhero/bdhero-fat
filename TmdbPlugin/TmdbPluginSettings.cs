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
            apiKey = null;
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

