﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MediaInfoWrapper.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class MediaInfoCoreFiles {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MediaInfoCoreFiles() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MediaInfoWrapper.Resources.MediaInfoCoreFiles", typeof(MediaInfoCoreFiles).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        internal static byte[] MediaInfo_DLL {
            get {
                object obj = ResourceManager.GetObject("MediaInfo_DLL", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        internal static byte[] MediaInfo_EXE {
            get {
                object obj = ResourceManager.GetObject("MediaInfo_EXE", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ;
        ///;
        ///;Bug: &quot;Page_Begin&quot;, &quot;Page_Middle&quot; and &quot;Page_End&quot; sections are picked on lines 10, 11 and 12 regardless what is there. So it is better to leave them there.
        ///;Bug: \r\n is not turned into a newline on &quot;Page&quot; entries.
        ///;Bug: &quot;Image&quot; sections are not active, but should.
        ///;
        ///;
        ///;
        ///Page;(unused)\r\n
        ///Page_Begin;
        ///Page_Middle;
        ///Page_End;&lt;/Tracks&gt;&lt;/MediaInfo&gt;
        ///;
        ///File;(unused)\r\n
        ///File_Begin;
        ///File_Middle;(unused)\r\n
        ///File_End;
        ///;
        ///General;&lt;?xml version=&quot;1.0&quot;?&gt;\r\n&lt;MediaInfo&gt;\r\n    &lt;File&gt;\r\n        &lt;Folde [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MediaInfoXML_CSV {
            get {
                return ResourceManager.GetString("MediaInfoXML_CSV", resourceCulture);
            }
        }
    }
}
