using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using BDAutoMuxer.controllers;

namespace BDAutoMuxer.tools
{
    /// <see cref="http://stackoverflow.com/a/11867784/467582"/>
    [System.ComponentModel.DesignerCategory("Code")]
    class MkvMerge : AbstractExternalTool
    {
        private string exe_path;

        private string inputM2tsPath;
        private string inputMkvPath;
        private string inputChaptersPath;
        private string outputMkvPath;
        private bool keepM2tsAudio;

        protected override string Name { get { return "MkvMerge"; } }
        protected override string Filename { get { return "mkvmerge.exe"; } }

        public MkvMerge(string inputM2tsPath, string inputMkvPath, string inputChaptersPath, string outputMkvPath, bool keepM2tsAudio = true)
            : base()
        {
            this.inputM2tsPath = inputM2tsPath;
            this.inputMkvPath = inputMkvPath;
            this.inputChaptersPath = inputChaptersPath;
            this.outputMkvPath = outputMkvPath;
            this.keepM2tsAudio = keepM2tsAudio;

            this.DoWork += Mux;
        }

        private void Mux(object sender, DoWorkEventArgs e)
        {
            ExtractResources();

            string inputM2tsFlags = keepM2tsAudio ? null : "--no-audio";
            string inputMkvFlags = keepM2tsAudio ? "--no-audio" : null;

            var args = new Args();

            // Chapter file
            args.AddIfAllNonEmpty("--chapters", inputChaptersPath);

            // Output file
            args.AddAll("-o", outputMkvPath);

            // Input M2TS file
            args.AddNonEmpty("--no-video", inputM2tsFlags, inputM2tsPath);

            // Input MKV file
            args.AddNonEmpty(inputMkvFlags, inputMkvPath);
            
            Execute(args, sender, e);
        }

        protected override void ExtractResources()
        {
            exe_path = this.ExtractResource(Filename);
        }

        protected override void HandleOutputLine(string line, object sender, DoWorkEventArgs e)
        {
            string progressRegex = @"^Progress: ([\d\.]+)\%";
            string errorRegex = @"^Error:";

            if (Regex.IsMatch(line, progressRegex))
            {
                Match match = Regex.Match(line, progressRegex);
                Double.TryParse(match.Groups[1].Value, out progress);
            }
            else if (Regex.IsMatch(line, errorRegex))
            {
                this.isError = true;
                this.errorMessages.Add(line);
            }
        }
    }
}
