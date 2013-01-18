using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetUtils
{
    public static class AssemblyUtils
    {
        public static string GetAssemblyName(Type type)
        {
            return Assembly.GetAssembly(type).GetName().Name;
        }

        public static Version GetAssemblyVersion(Type type)
        {
            return Assembly.GetAssembly(type).GetName().Version;
        }

        public static string GetAssemblyVersionShort(Type type)
        {
            return Regex.Replace(GetAssemblyVersion(type).ToString(), @"(?:\.0)+$", "");
        }
    }
}
