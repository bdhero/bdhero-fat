using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

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

        public MkvMerge(/*BDROM BDROM, */string inputM2tsPath, string inputMkvPath, string inputChaptersPath, string outputMkvPath, bool keepM2tsAudio = true)
            : base()
        {
            //this.BDROM = BDROM;
            this.inputM2tsPath = inputM2tsPath;
            this.inputMkvPath = inputMkvPath;
            this.inputChaptersPath = inputChaptersPath;
            this.outputMkvPath = outputMkvPath;
            this.keepM2tsAudio = keepM2tsAudio;

            this.DoWork += Mux;
        }

        private void Mux(object sender, DoWorkEventArgs e)
        {
            //outputFilePath = e.Argument as string;

            ExtractResources();

            string inputM2tsFlags = keepM2tsAudio ? null : "--no-audio";
            string inputMkvFlags = keepM2tsAudio ? "--no-audio" : null;

            // TODO: Don't pass empty args
            Execute(new List<string>() { "--chapters", inputChaptersPath, "-o", outputMkvPath, "--no-video", inputM2tsFlags, inputM2tsPath, inputMkvFlags, inputMkvPath }, sender, e);
        }

        protected override void ExtractResources()
        {
            exe_path = this.ExtractResource(Filename);
        }

        protected override void HandleOutputLine(string line, object sender, DoWorkEventArgs e)
        {
            string regex = @"^Progress: ([\d\.]+)\%";
            if (Regex.IsMatch(line, regex))
            {
                Match match = Regex.Match(line, regex);
                Double.TryParse(match.Groups[1].Value, out progress);
            }
            // TODO: Check for errors here
        }
    }
}
