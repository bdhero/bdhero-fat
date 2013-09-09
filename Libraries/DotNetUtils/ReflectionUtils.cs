using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetUtils
{
    public static class ReflectionUtils
    {
        public static string ToString(Object obj)
        {
            var fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
            return string.Join(Environment.NewLine, fields.Select(info => ToString(obj, info)));
        }

        private static string ToString(Object obj, FieldInfo info)
        {
            var value = (info.GetValue(obj) ?? "null").ToString();
            var lines = Regex.Split(value, @"[\n\r\f]+");
            if (lines.Count() > 1)
                value = Environment.NewLine + string.Join(Environment.NewLine, lines.Select(s => "    " + s));
            return string.Format("{0}: {1}", info.Name, value);
        }
    }
}
