using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TmdbPlugin
{
    class TmdbPluginSettings
    {
        public class Setting
        {
            public string apiKey { get; set { apiKey = null; } }
            public string defaultLanguage { get; set { defaultLanguage = "en"; } }
            public string url { get; set { url = "http://acdvorak.github.io/bdautomuxer/"; } }
        }

        public class PluginSettings
        {
            public List<Setting> settings { get; set; }
        }
    }
}
