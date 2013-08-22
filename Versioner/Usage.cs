﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 10.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Versioner
{
    using System;
    
    
    #line 1 "C:\Projects\bdhero\Versioner\Usage.tt"
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "10.0.0.0")]
    public partial class Usage : UsageBase
    {
        public virtual string TransformText()
        {
            this.Write("USAGE:\r\n    ");
            
            #line 4 "C:\Projects\bdhero\Versioner\Usage.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_exeName));
            
            #line default
            #line hidden
            this.Write(" [OPTIONS]\r\n\r\nDESCRIPTION:\r\n    Utility to update BDHero version numbers.  Allows" +
                    " incremental \"bumps\",\r\n    custom version numbers, and synchronization (ensuring" +
                    " that version numbers\r\n    are in sync across all files).\r\n\r\n    The \"current\" v" +
                    "ersion number is read from the AssemblyVersion or\r\n    AssemblyFileVersion attri" +
                    "butes in BDHero/Properties/AssemblyInfo.cs,\r\n    whichever appears first in the " +
                    "file.\r\n\r\n    If invoked without any arguments, this utility will synchronize the" +
                    "\r\n    current version number across all files.\r\n\r\nOPTIONS:\r\n    -h, --help, /?\r\n" +
                    "        Display this message and exit.\r\n\r\n    --workspace=SOLUTION_DIR\r\n        " +
                    "Absolute path to the Visual Studio root solution directory.\r\n        If not spec" +
                    "ified, defaults to the current working directory (%CD%).\r\n\r\n    --test\r\n        " +
                    "Perform a test run.  Output the result of any other options,\r\n        but do not" +
                    " actually write changes to disk.\r\n\r\n    --test-with=CURRENT_VERSION\r\n        Sam" +
                    "e as --test, but uses CURRENT_VERSION for the current version\r\n        number in" +
                    "stead of reading it from disk.\r\n\r\n    -v, --version\r\n    -p, --print\r\n        Pr" +
                    "int the current BDHero version number to stdout and exit.\r\n\r\n    --id, --version" +
                    "-id\r\n        Print the current BDHero version number ID to stdout and exit.\r\n   " +
                    "     The version ID is a signed integer representation of the version\r\n        n" +
                    "umber suitable for use in the <versionId> tag of a\r\n        BitRock InstallBuild" +
                    "er update.xml file.\r\n\r\n        The exact format of the ID is the integer value o" +
                    "f the\r\n        concatenation of each version group in two digit form.\r\n\r\n       " +
                    " Examples:\r\n\r\n            1.2.3.4   =   1020304\r\n            0.8.0.1   =     800" +
                    "01\r\n            0.8.0.10  =     80010\r\n           20.8.0.10  =  20080010\r\n\r\n    " +
                    "--strategy=STRATEGY\r\n        Determines how {0} updates version numbers in the s" +
                    "olution.\", exe\r\n\r\n        STRATEGY must be one of the following:\r\n\r\n            " +
                    "\"_._._.x\": Incremental: bug fix\r\n                       (Version.Revision)\r\n    " +
                    "        \"_._.x._\": Incremental: minor feature/enhancement\r\n                     " +
                    "  (Version.Build)\r\n            \"_.x._._\": Incremental: full release\r\n           " +
                    "            (Version.Minor)\r\n            \"x._._._\": Incremental: major milestone" +
                    "\r\n                       (Version.Major)\r\n            \"x.x.x.x\": Non-incremental" +
                    ": use custom version number\r\n                       (see --custom)\r\n            " +
                    "\"_._._._\": None: don\'t increment the version number; leave it as is\r\n           " +
                    "            and synchronize the value across all files\r\n                       (" +
                    "default behavior)\r\n\r\n    --custom=VERSION_NUMBER\r\n        Use a custom version n" +
                    "umber instead of incrementing the current number.\r\n\r\n    --infinite, --no-limit\r" +
                    "\n        Don\'t limit version number groups (major, minor, build, revision) to\r\n " +
                    "       0-9 when incrementing; if a group\'s current value is 9, allow it to go\r\n " +
                    "       to 10 instead of setting it to zero and incrementing the next most\r\n     " +
                    "   significant group.\r\n\r\n        Examples:\r\n\r\n            > Versioner\r\n         " +
                    "   6.7.8.9 => 6.7.8.9 (same as --strategy=_._._._)\r\n\r\n            > Versioner --" +
                    "strategy=_._._.x\r\n            6.7.8.9 => 6.7.9.0\r\n\r\n            > Versioner --st" +
                    "rategy=_._._.x --infinite\r\n            6.7.8.9 => 6.7.8.10\r\n\r\n            > Vers" +
                    "ioner --strategy=_._._.x\r\n            1.9.9.9 => 2.0.0.0\r\n\r\n            > Versio" +
                    "ner --strategy=_._._.x --infinite\r\n            1.9.9.9 => 1.9.9.10");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "10.0.0.0")]
    public class UsageBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
