using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotNetUtils
{
    public static class AppUtils
    {
        #region Assembly Attribute Accessors

        /// <summary>
        /// Gets the human-friendly name of the application.
        /// </summary>
        /// <example><code>"BDHero GUI"</code></example>
        public static string AppName
        {
            get
            {
                var assembly = AssemblyUtils.AssemblyOrDefault();
                object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(assembly.CodeBase);
            }
        }

        /// <summary>
        /// Gets the internal name of the application assembly.
        /// </summary>
        /// <example><code>"bdhero-gui"</code></example>
        public static string AssemblyName
        {
            get { return AssemblyUtils.GetAssemblyName(); }
        }

        /// <summary>
        /// Gets the application's version number.
        /// </summary>
        public static Version AppVersion
        {
            get { return AssemblyUtils.GetAssemblyVersion(); }
        }

        /// <summary>
        /// Gets the human-friendly description of the application.
        /// </summary>
        /// <example><code>"BDHero graphical interface"</code></example>
        public static string AppDescription
        {
            get
            {
                object[] attributes = AssemblyUtils.AssemblyOrDefault().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary>
        /// Gets the product name of the application.
        /// </summary>
        /// <example><code>"BDHero"</code></example>
        public static string ProductName
        {
            get
            {
                object[] attributes = AssemblyUtils.AssemblyOrDefault().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        /// <summary>
        /// Gets the application's copyright string.
        /// </summary>
        /// <example><code>"Copyright © 2013"</code></example>
        public static string Copyright
        {
            get
            {
                object[] attributes = AssemblyUtils.AssemblyOrDefault().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// Gets the name of the company that developed the application.
        /// </summary>
        /// <example><code>"BDHero"</code></example>
        public static string Company
        {
            get
            {
                object[] attributes = AssemblyUtils.AssemblyOrDefault().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        /// <summary>
        /// Gets the date and time the application was built.
        /// </summary>
        public static DateTime BuildDate
        {
            get { return AssemblyUtils.GetLinkerTimestamp(); }
        }

        #endregion
    }
}
