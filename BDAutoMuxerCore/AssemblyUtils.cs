using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BDAutoMuxerCore
{
    public static class AssemblyUtils
    {
        public static string AssemblyName
        {
            get
            {
                return Assembly.GetAssembly(typeof(AssemblyUtils)).GetName().Name;
            }
        }

        public static Version AssemblyVersion
        {
            get
            {
                return Assembly.GetAssembly(typeof(AssemblyUtils)).GetName().Version;
            }
        }

        public static string AssemblyVersionDisplay
        {
            get
            {
                return Regex.Replace(AssemblyVersion.ToString(), @"\.0$", "");
            }
        }
    }
}
