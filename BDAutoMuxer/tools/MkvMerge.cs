using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace BDAutoMuxer.tools
{
    /// <see cref="http://stackoverflow.com/a/11867784/467582"/>
// ReSharper disable LocalizableElement
// ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
// ReSharper restore RedundantNameQualifier
// ReSharper restore LocalizableElement
    class MkvMerge : AbstractExternalTool
    {
        private readonly string _inputM2TsPath;
        private readonly string _inputMkvPath;
        private readonly string _inputChaptersPath;
        private readonly string _outputMkvPath;
        private readonly bool _keepM2TsAudio;

        protected override string Name { get { return "MkvMerge"; } }
        protected override string Filename { get { return "mkvmerge.exe"; } }

        public MkvMerge(string inputM2TsPath, string inputMkvPath, string inputChaptersPath, string outputMkvPath, bool keepM2TsAudio = true)
        {
            _inputM2TsPath = inputM2TsPath;
            _inputMkvPath = inputMkvPath;
            _inputChaptersPath = inputChaptersPath;
            _outputMkvPath = outputMkvPath;
            _keepM2TsAudio = keepM2TsAudio;

            DoWork += Mux;
        }

        private void Mux(object sender, DoWorkEventArgs e)
        {
            ExtractResources();

            var inputM2TsFlags = _keepM2TsAudio ? null : "--no-audio";
            var inputMkvFlags = _keepM2TsAudio ? "--no-audio" : null;

            var args = new Args();

            // Chapter file
            args.AddIfAllNonEmpty("--chapters", _inputChaptersPath);

            // Output file
            args.AddAll("-o", _outputMkvPath);

            // Input M2TS file
            args.AddNonEmpty("--no-video", inputM2TsFlags, _inputM2TsPath);

            // Input MKV file
            args.AddNonEmpty(inputMkvFlags, _inputMkvPath);
            
            Execute(args, sender, e);
        }

        protected override void ExtractResources()
        {
            ExtractResource(Filename);
        }

        protected override void HandleOutputLine(string line, object sender, DoWorkEventArgs e)
        {
            const string progressRegex = @"^Progress: ([\d\.]+)\%";
            const string errorRegex = @"^Error:";

            if (Regex.IsMatch(line, progressRegex))
            {
                var match = Regex.Match(line, progressRegex);
                Double.TryParse(match.Groups[1].Value, out progress);
            }
            else if (Regex.IsMatch(line, errorRegex))
            {
                isError = true;
                errorMessages.Add(line);
            }
        }
    }
}
