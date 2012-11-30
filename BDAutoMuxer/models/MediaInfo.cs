﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BDAutoMuxer.controllers;
using BDAutoMuxer.tools;
using Newtonsoft.Json;

namespace BDAutoMuxer.models
{
    class MediaInfo
    {
        public static readonly MIConfig Config = new MIConfig();

        public static bool IsCorrectVersion { get { return Config.IsValid; } }

        private static readonly Regex TrackRegex = new Regex(@"<(Video|Audio|Subtitle|Chapter)Track>\s*(.*?)\s*</\1Track>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private readonly string _mediaFilePath;

        public MIFile File;
        public MIContainer Container;

        private readonly List<MITrack> _tracks = new List<MITrack>();

        private readonly List<MIVideoTrack> _videoTracks = new List<MIVideoTrack>();
        private readonly List<MIAudioTrack> _audioTracks = new List<MIAudioTrack>();
        private readonly List<MISubtitleTrack> _subtitleTracks = new List<MISubtitleTrack>();
        private readonly List<MIChapterTrack> _chapterTracks = new List<MIChapterTrack>();

        public IList<MITrack> Tracks { get { return _tracks.AsReadOnly(); } }

        public IList<MIVideoTrack> VideoTracks { get { return _videoTracks.AsReadOnly(); } }
        public IList<MIAudioTrack> AudioTracks { get { return _audioTracks.AsReadOnly(); } }
        public IList<MISubtitleTrack> SubtitleTracks { get { return _subtitleTracks.AsReadOnly(); } }
        public IList<MIChapterTrack> ChapterTracks { get { return _chapterTracks.AsReadOnly(); } }

        public static void Test()
        {
            MediaInfo.Config.CLIPath = @"C:\Tools\MediaInfo\MediaInfo.exe";
            MediaInfo.Config.CSVPath = @"C:\Projects\bdautomuxer\BDAutoMuxer\Resources\MediaInfo_XML.csv";

            /*

            var braveheart_mkv_path = @"Y:\BDAutoMuxer\BRAVEHEART_D1_AC\Braveheart (1995) [1080p].mkv";
            var pirates_m2ts_path =
                @"Y:\BDAutoMuxer\PIRATES3_FULL\Pirates of the Caribbean - At World's End (2007) [1080p].m2ts";
            var pirates_wav_path =
                @"Y:\BDAutoMuxer\PIRATES3_FULL\Pirates of the Caribbean - At World's End (2007) [1080p].track_4352.1.wav";
            var pirates_mkv_path = @"Y:\BDAutoMuxer\PIRATES3_SHORT\PIRATES3_00001.mmg.mkv";
            var thismeanswar_mka_path = @"Y:\BDAutoMuxer\THIS_MEANS_WAR\This Means War (2012) [1080p].mka";

            var start5 = DateTime.Now;

            var braveheart_mkv = new MediaInfo(braveheart_mkv_path).Scan();
            var pirates_m2ts = new MediaInfo(pirates_m2ts_path).Scan();
            var pirates_wav = new MediaInfo(pirates_wav_path).Scan();
            var pirates_mkv = new MediaInfo(pirates_mkv_path).Scan();
            var pirates_mka = new MediaInfo(thismeanswar_mka_path).Scan();

            var diff5 = DateTime.Now - start5;

            Console.WriteLine();
             
            */

//            var mediaFilePaths = FullDirList(new DirectoryInfo(@"E:\"));
            var mediaFilePaths = new[]
                                     {
                                         @"E:\BD\21_JUMP_STREET\BDMV\PLAYLIST\00001.MPLS",
                                         @"E:\BD\49123204_BLACK_HAWK_DOWN\BDMV\PLAYLIST\00009.MPLS",
                                         @"E:\BD\AMAZING_SPIDERMAN\BDMV\PLAYLIST\00001.MPLS",
                                         @"E:\BD\BEAUTYANDTHEBEAST\BDMV\PLAYLIST\00800.MPLS",
                                         @"E:\BD\BRAVE\BDMV\PLAYLIST\00800.MPLS",
                                         @"E:\BD\BRAVEHEART_D1_AC\BDMV\PLAYLIST\00004.MPLS",
                                         @"E:\BD\CONTACT\BDMV\PLAYLIST\00000.MPLS",
                                         @"E:\BD\DARK_SHADOWS\BDMV\PLAYLIST\00100.MPLS",
                                         @"E:\BD\DOOM\BDMV\PLAYLIST\00000.MPLS",
                                         @"E:\BD\FIVE_YEAR_ENGAGEMENT_RENTAL\BDMV\PLAYLIST\00800.MPLS",
                                         @"E:\BD\GALAXY_QUEST_AC\BDMV\PLAYLIST\00006.MPLS",
                                         @"E:\BD\GOOD_WILL_HUNTING\BDMV\PLAYLIST\00800.MPLS",
                                         @"E:\BD\INCREDIBLES\BDMV\PLAYLIST\00800.MPLS",
                                         @"E:\BD\LOCKOUT\BDMV\PLAYLIST\00001.MPLS",
                                         @"E:\BD\MARVELS_THE_AVENGERS\BDMV\PLAYLIST\00800.MPLS",
                                         @"E:\BD\MIRROR_MIRROR_BD\BDMV\PLAYLIST\00000.MPLS",
                                         @"E:\BD\MUMMY_RETURNS\BDMV\PLAYLIST\00010.MPLS",
                                         @"E:\BD\PANS_LABYRINTH\BDMV\PLAYLIST\00062.MPLS",
                                         @"E:\BD\PRINCESSANDTHEFROG\BDMV\PLAYLIST\00289.MPLS",
                                         @"E:\BD\RED_BIRD_2D_WW\BDMV\PLAYLIST\00802.MPLS",
                                         @"E:\BD\SALT\BDMV\PLAYLIST\00001.MPLS",
                                         @"E:\BD\SAVING_PRIVATE_RYAN_1\BDMV\PLAYLIST\01022.MPLS",
                                         @"E:\BD\SCARFACE\BDMV\PLAYLIST\00800.MPLS",
                                         @"E:\BD\STARTSHIP_TROOPERS_1_P1\BDMV\PLAYLIST\00000.MPLS",
                                         @"E:\BD\SWATH_RENTAL\BDMV\PLAYLIST\00801.MPLS",
                                         @"E:\BD\TANGLED\BDMV\PLAYLIST\00800.MPLS",
                                         @"E:\BD\THE_COUNT_OF_MONTE_CRISTO\BDMV\PLAYLIST\00001.MPLS",
                                         @"E:\BD\THIS_MEANS_WAR\BDMV\PLAYLIST\00800.MPLS",
                                         @"E:\BD\TOY_STORY\BDMV\PLAYLIST\00337.MPLS",
                                         @"E:\BD\TOY_STORY_3_DISC_1\BDMV\PLAYLIST\00800.MPLS",
                                         @"E:\BD\WALL-E BD\BDMV\PLAYLIST\00089.MPLS"
                                     };

            Console.WriteLine(string.Format("Scanning {0} MPLS files...", mediaFilePaths.Length));

            var startAll = DateTime.Now;

            var i = 0;

            var mediaInfos = mediaFilePaths.Select(filePath => new MediaInfo(filePath).Scan(i++)).ToList();

            var diffAll = DateTime.Now - startAll;

            var miVideoTracks = mediaInfos.SelectMany(mi => mi.VideoTracks).ToList();
            var miAudioTracks = mediaInfos.SelectMany(mi => mi.AudioTracks).ToList();

            var videoCodecs = miVideoTracks.ToDictionary(videoTrack => videoTrack.Format, videoTrack => videoTrack.FilePath);
            var audioCodecs = miAudioTracks.ToDictionary(audioTrack => audioTrack.Format, audioTrack => audioTrack.FilePath);

            var strVideoCodecs = JsonConvert.SerializeObject(videoCodecs);
            var strAudioCodecs = JsonConvert.SerializeObject(audioCodecs);

            var text = string.Format("VIDEO\n=====\n\n{0}\n\nAUDIO\n=====\n\n{1}", strVideoCodecs, strAudioCodecs);

            var outputFile = @"E:\BD\BDAM.MediaInfo.txt";

            Console.WriteLine("\n\nWrote output to " + outputFile);

            System.IO.File.WriteAllText(outputFile, text);

            Console.WriteLine();
        }

        public static List<FileInfo> FullDirList(DirectoryInfo dir)
        {
            var filesFound = FindFilesRecursive(dir);
            filesFound.Sort((a, b) => string.CompareOrdinal(a.FullName, b.FullName));
            return filesFound;
        }

        private static List<FileInfo> FindFilesRecursive(DirectoryInfo dir)
        {
            var filesFound = new List<FileInfo>();
            

            try
            {
                filesFound.AddRange(dir.GetFiles().Where(IncludeFile));
            }
            catch { }

            try
            {
                foreach (var d in dir.GetDirectories())
                {
                    filesFound.AddRange(FindFilesRecursive(d));
                }
            }
            catch { }

            return filesFound;
        }

        private static readonly string[] Extensions = new[] { ".vob", /*".mpls",*/ ".mkv", ".mka", ".mks", ".mp3" };

        private static bool IncludeFile(FileInfo fileInfo)
        {
            var correctExtension = Extensions.Contains(fileInfo.Extension.ToLower());
            var isArchive = (fileInfo.Attributes & FileAttributes.Archive) == FileAttributes.Archive;
            var isHiddenWindows = (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
            var isTrash = fileInfo.Name.StartsWith("$");
            var isHiddenUnix = fileInfo.Name.StartsWith(".");
            return correctExtension && !(isTrash || isHiddenUnix);
        }

        public MediaInfo(string mediaFilePath)
        {
            _mediaFilePath = mediaFilePath;
        }

        public MediaInfo Scan(int i)
        {
            if (!System.IO.File.Exists(_mediaFilePath))
                throw new FileNotFoundException("Media file not found", _mediaFilePath);

            var validationException = Config.ValidationException;
            if (validationException != null) throw validationException;

            Console.WriteLine("{0}) Scanning {1}...", i, _mediaFilePath);

            var xmlResult = RunProcess(new List<string>() { "--Output=file://" + Config.CSVPath });
            if (xmlResult.Exception != null) throw xmlResult.Exception;
            if (xmlResult.StdErr.Length > 0) throw new Exception(string.Format("MediaInfo XML exception: {0}", xmlResult.StdErr));

            if (!xmlResult.StdOut.StartsWith("<?xml"))
                throw new Exception(string.Format("Expecting XML output; instead found \"{0}\"", xmlResult.StdOut));

            ParseXml(xmlResult.StdOut);

            if (_chapterTracks.Any())
            {
                var txtResult = RunProcess();
                if (txtResult.Exception != null) throw txtResult.Exception;
                if (txtResult.StdErr.Length > 0) throw new Exception(string.Format("MediaInfo TXT exception: {0}", txtResult.StdErr));

                ParseTxt(txtResult.StdOut);
            }

            return this;
        }

        private MIProcessResult RunProcess(IEnumerable<string> args = null)
        {
            if (args == null) args = new string[0];

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Config.CLIPath,
                    Arguments = new Args(new List<string>(args) { _mediaFilePath }).ToString(),
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            var stdout = new StringBuilder();
            var stderr = new StringBuilder();

            Exception exception = null;

            try
            {
                process.Start();

                while (!process.StandardOutput.EndOfStream)
                {
                    stdout.AppendLine(process.StandardOutput.ReadLine());
                }

                while (!process.StandardError.EndOfStream)
                {
                    stderr.AppendLine(process.StandardError.ReadLine());
                }
            }
            catch (Exception e)
            {
                exception = e;
            }
            finally
            {
                try { process.Kill(); }
                catch {}
            }

            return new MIProcessResult(stdout.ToString(), stderr.ToString(), exception);
        }

        private void ParseXml(string xml)
        {
            var fileSectionRegex = new Regex(@"<File>\s*(.+?)\s*</File>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var containerSectionRegex = new Regex(@"<Container>\s*(.+?)\s*</Container>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var tracksSectionRegex = new Regex(@"<Tracks>\s*(.+?)\s*</Tracks>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (string.IsNullOrWhiteSpace(xml) || !fileSectionRegex.IsMatch(xml) ||
                !containerSectionRegex.IsMatch(xml) || !tracksSectionRegex.IsMatch(xml))
                throw new Exception("Unable to locate track section in MediaInfo XML");

            var fileSection = fileSectionRegex.Match(xml).Groups[1].Value;
            var containerSection = containerSectionRegex.Match(xml).Groups[1].Value;
            var tracksSection = tracksSectionRegex.Match(xml).Groups[1].Value;

            File = new MIFile(fileSection);
            Container = new MIContainer(containerSection);

            var videoSectionRegex = new Regex(@"<Video>\s*(.+?)\s*</Video>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var audioSectionRegex = new Regex(@"<Audio>\s*(.+?)\s*</Audio>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var subtitleSectionRegex = new Regex(@"<Subtitle>\s*(.+?)\s*</Subtitle>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var chapterSectionRegex = new Regex(@"<Chapter>\s*(.+?)\s*</Chapter>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            _videoTracks.AddRange(ParseTracks(videoSectionRegex, tracksSection).Select(trackXml => new MIVideoTrack(_mediaFilePath).ReadFromXml(trackXml) as MIVideoTrack));
            _audioTracks.AddRange(ParseTracks(audioSectionRegex, tracksSection).Select(trackXml => new MIAudioTrack(_mediaFilePath).ReadFromXml(trackXml) as MIAudioTrack));
            _subtitleTracks.AddRange(ParseTracks(subtitleSectionRegex, tracksSection).Select(trackXml => new MISubtitleTrack(_mediaFilePath).ReadFromXml(trackXml) as MISubtitleTrack));
            _chapterTracks.AddRange(ParseTracks(chapterSectionRegex, tracksSection).Select(trackXml => new MIChapterTrack().ReadFromXml(trackXml) as MIChapterTrack));

            _tracks.AddRange(_videoTracks);
            _tracks.AddRange(_audioTracks);
            _tracks.AddRange(_subtitleTracks);
            _tracks.AddRange(_chapterTracks);
        }

        private void ParseTxt(string txt)
        {
            // Normalize all newlines to Unix-style
            txt = new Regex(@"\r\n|\r").Replace(txt, "\n");

            var newlineMultiRegex = new Regex(@"\n{2,}");

            var menuSections = newlineMultiRegex.Split(txt).Where(section => section.StartsWith("Menu")).ToList();

            if (menuSections.Count != _chapterTracks.Count)
                throw new Exception(string.Format("Expecting {0} Menu sections in TXT output, but instead found {1}", ChapterTracks.Count, menuSections.Count));

            for (var i = 0; i < menuSections.Count; i++)
            {
                var menuSection = menuSections[i];
                var chapterTrack = _chapterTracks[i];
                chapterTrack.ReadFromTxt(menuSection);
            }
        }

        private List<string> ParseTracks(Regex trackTypeSectionRegex, string tracksSection)
        {
            if (!trackTypeSectionRegex.IsMatch(tracksSection))
                return new List<string>();
            var trackMatches = TrackRegex.Matches(trackTypeSectionRegex.Match(tracksSection).Groups[1].Value);
            var tracks = (from Match trackMatch in trackMatches select Value(trackMatch)).ToList();
            return tracks;
        }

        private static string Value(Match trackMatch)
        {
            var value = trackMatch.Groups[2].Value;
            return value;
        }
    }

    class MIProcessResult
    {
        public readonly string StdOut;
        public readonly string StdErr;
        public readonly Exception Exception;
        public MIProcessResult(string stdOut, string stdErr, Exception exception)
        {
            StdOut = stdOut;
            StdErr = stdErr;
            Exception = exception;
        }
    }

    class XmlUtil
    {
        public static string GetString(string xml, string tagName, Regex regex = null)
        {
            var matcher = new Regex(String.Format("<{0}>(.*?)</{0}>", tagName), RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var value = matcher.IsMatch(xml) ? matcher.Match(xml).Groups[1].Value : null;

            if (value != null && regex != null && regex.IsMatch(value))
            {
                var match = regex.Match(value);
                return match.Groups.Count > 0 ? match.Groups[1].Value : match.Value;
            }

            return value;
        }

        public static int GetInt(string xml, string tagName, Regex regex = null)
        {
            return ParseInt(GetString(xml, tagName, regex));
        }

        public static int? GetIntNullable(string xml, string tagName, Regex regex = null)
        {
            return ParseIntNullable(GetString(xml, tagName, regex));
        }

        public static long GetLong(string xml, string tagName, Regex regex = null)
        {
            return ParseLong(GetString(xml, tagName, regex));
        }

        public static long? GetLongNullable(string xml, string tagName, Regex regex = null)
        {
            return ParseLongNullable(GetString(xml, tagName, regex));
        }

        public static double GetDouble(string xml, string tagName, Regex regex = null)
        {
            return ParseDouble(GetString(xml, tagName, regex));
        }

        public static double? GetDoubleNullable(string xml, string tagName, Regex regex = null)
        {
            return ParseDoubleNullable(GetString(xml, tagName, regex));
        }

        public static bool? GetBitFlag(string xml, string tagName, Regex regex = null)
        {
            return ParseBitFlag(GetString(xml, tagName, regex));
        }

        public static int ParseInt(string value)
        {
            int i;
            Int32.TryParse(value ?? "0", out i);
            return i;
        }

        public static int? ParseIntNullable(string value)
        {
            int i;
            if (Int32.TryParse(value ?? "", out i))
                return i;
            return null;
        }

        public static long ParseLong(string value)
        {
            long i;
            Int64.TryParse(value ?? "0", out i);
            return i;
        }

        public static long? ParseLongNullable(string value)
        {
            long i;
            if (Int64.TryParse(value ?? "", out i))
                return i;
            return null;
        }

        public static double ParseDouble(string value)
        {
            double d;
            Double.TryParse(value ?? "0", out d);
            return d;
        }

        public static double? ParseDoubleNullable(string value)
        {
            double d;
            if (Double.TryParse(value ?? "", out d))
                return d;
            return null;
        }

        public static bool? ParseBitFlag(string value)
        {
            var lower = (value ?? "").ToLowerInvariant();
            if (lower == "yes")
                return true;
            if (lower == "no")
                return false;
            return null;
        }
    }

    class MIConfig
    {
        private string _cliPath;

        public string CLIPath {
            get { return _cliPath; }
            set
            {
                _cliPath = value;

                if (_cliPath == null) return;

                Process process = null;

                try
                {
                    if (!File.Exists(_cliPath)) return;

                    process = new Process
                                  {
                                      StartInfo = new ProcessStartInfo
                                                      {
                                                          FileName = _cliPath,
                                                          Arguments = "--version",
                                                          UseShellExecute = false,
                                                          CreateNoWindow = true,
                                                          ErrorDialog = false,
                                                          RedirectStandardError = true,
                                                          RedirectStandardOutput = true,
                                                          WindowStyle = ProcessWindowStyle.Hidden
                                                      }
                                  };

                    process.Start();

                    var versionRegex = new Regex(@"MediaInfoLib - v(\w+\.\w+\.\w+)", RegexOptions.IgnoreCase);

                    var lineNum = 0;

                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();

                        if (lineNum++ == 0 && line != null && !line.Contains("MediaInfo Command line"))
                            return;

                        Version version;

                        if (line != null && versionRegex.IsMatch(line) &&
                            Version.TryParse(versionRegex.Match(line).Groups[1].Value, out version))
                        {
                            Version = version;
                            return;
                        }
                    }

                    while (!process.StandardError.EndOfStream)
                    {
                        process.StandardError.ReadLine();
                        return;
                    }
                }
                catch
                {
                }
                finally
                {
                    try
                    {
                        if (process != null)
                            process.Kill();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public string CSVPath;

        public Version Version { get; protected set; }
        public readonly Version MinVersion = new Version(0, 7, 60);
        public readonly Version MaxVersion = new Version(0, 7, 999999999);

        public MIConfig(string cliPath = null, string csvPath = null)
        {
            CLIPath = cliPath;
            CSVPath = csvPath;
        }

        public bool IsCLIPathValid
        {
            get { return File.Exists(CLIPath); }
        }

        public bool IsCSVPathValid
        {
            get { return File.Exists(CSVPath); }
        }

        public bool IsVersionValid
        {
            get { return Version >= MinVersion && Version <= MaxVersion; }
        }

        public bool IsValid
        {
            get
            {
                try
                {
                    return IsCLIPathValid && IsCSVPathValid && IsVersionValid;
                }
                catch
                {
                    return false;
                }
            }
        }

        public Exception ValidationException
        {
            get
            {
                if (!IsCLIPathValid)
                    return new FileNotFoundException("MediaInfo.exe CLI not found", CLIPath);

                if (!IsCSVPathValid)
                    return new FileNotFoundException("MediaInfo CSV file not found", CSVPath);

                if (!IsVersionValid)
                    return new Exception(string.Format("Unsupported MediaInfo.exe CLI version: {0}.  Expecting {1} <= Version <= {2}.", Version, MinVersion, MaxVersion));

                return null;
            }
        }
    }

    class MIFile
    {
        public string Folder { get; protected set; }
        public string Filename { get; protected set; }
        public string Extension { get; protected set; }
        public string Path { get; protected set; }
        public long Size { get; protected set; }

        public MIFile(string xml)
        {
            Folder = XmlUtil.GetString(xml, "Folder");
            Filename = XmlUtil.GetString(xml, "Filename");
            Extension = XmlUtil.GetString(xml, "Extension");
            Path = XmlUtil.GetString(xml, "Path");
            Size = XmlUtil.GetLong(xml, "Size");
        }
    }

    class MIContainer
    {
        public string Title { get; protected set; }
        public string Format { get; protected set; }
        public long Duration { get; protected set; }
        public string DurationString { get; protected set; }
        public long? Size { get; protected set; }

        public MIContainer(string xml)
        {
            Title = XmlUtil.GetString(xml, "Title");
            Format = XmlUtil.GetString(xml, "Format");
            Duration = XmlUtil.GetLong(xml, "Duration");
            DurationString = XmlUtil.GetString(xml, "DurationString");
            Size = XmlUtil.GetLongNullable(xml, "Size");
        }
    }

    abstract class MICodec
    {
        public abstract bool IsAudio { get; }
        public abstract bool IsVideo { get; }
        public abstract bool IsSubtitle { get; }

        /// <summary>
        /// Shown in MediaInfo.  Also stored in MKV headers.
        /// </summary>
        /// <example>A_AC3</example>
        /// <example>A_DTS</example>
        /// <example>V_MPEG4/ISO/AVC</example>
        public abstract string CodecId { get; }

        public abstract string FullName { get; }
        public abstract string ShortName { get; }
        public abstract string MicroName { get; }

        public virtual string AltFullName { get { return null; } }
        public virtual string AltShortName { get { return null; } }
        public virtual string AltMicroName { get { return null; } }

        public abstract bool Lossy { get; }
        public abstract bool Lossless { get; }
    }

    abstract class MIAudioCodec : MICodec
    {
        public override bool IsAudio
        {
            get { return true; }
        }
        public override bool IsVideo
        {
            get { return false; }
        }
        public override bool IsSubtitle
        {
            get { return false; }
        }
    }

    abstract class MIVideoCodec : MICodec
    {
        public override bool IsAudio
        {
            get { return false; }
        }
        public override bool IsVideo
        {
            get { return true; }
        }
        public override bool IsSubtitle
        {
            get { return false; }
        }
    }

    abstract class MISubtitleCodec : MICodec
    {
        public override bool IsAudio
        {
            get { return false; }
        }
        public override bool IsVideo
        {
            get { return false; }
        }
        public override bool IsSubtitle
        {
            get { return true; }
        }
    }

    class MICodecProLogic : MIAudioCodec
    {
        public override string CodecId
        {
            get { return "A_AC3"; }
        }

        public override string FullName
        {
            get { return "Dolby Pro Logic"; }
        }

        public override string ShortName
        {
            get { return "Pro Logic"; }
        }

        public override string MicroName
        {
            get { return "DPL"; }
        }

        /// <summary>
        /// "Dolby Pro Logic" is actually the name of the Decoder; "Dolby Surround" is the name of the Encoder.
        /// </summary>
        public override string AltFullName
        {
            get { return "Dolby Surround"; }
        }

        public override bool Lossy
        {
            get { return true; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "AC-3" && format.Profile == "Dolby Digital";
        }
    }

    class MICodecAC3 : MIAudioCodec
    {
        public override string CodecId
        {
            get { return "A_AC3"; }
        }

        public override string FullName
        {
            get { return "Dolby Digital"; }
        }

        public override string ShortName
        {
            get { return "AC-3"; }
        }

        public override string MicroName
        {
            get { return "AC3"; }
        }

        public override string AltFullName
        {
            get { return "ATSC A/52"; }
        }

        public override string AltShortName
        {
            get { return "A/52"; }
        }

        public override string AltMicroName
        {
            get { return "DD"; }
        }

        public override bool Lossy
        {
            get { return true; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "AC-3" && !MICodecProLogic.Matches(format);
        }
    }

//    class MICodec 

    class MIFormat
    {
        public string Id { get; protected set; }
        public string Info { get; protected set; }
        public string Version { get; protected set; }
        public string Profile { get; protected set; }
        public string Compresion { get; protected set; }
        public string Settings { get; protected set; }

        public MIFormat(string id, string info, string version, string profile, string compression, string settings)
        {
            Id = id;
            Info = info;
            Version = version;
            Profile = profile;
            Compresion = compression;
            Settings = settings;
        }

        protected bool Equals(MIFormat other)
        {
            return
                string.Equals(Id, other.Id) &&
                string.Equals(Info, other.Info) &&
                string.Equals(Version, other.Version) &&
                string.Equals(Profile, other.Profile) &&
                string.Equals(Compresion, other.Compresion) &&
                string.Equals(Settings, other.Settings);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MIFormat) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Info != null ? Info.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Version != null ? Version.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Profile != null ? Profile.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Compresion != null ? Compresion.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Settings != null ? Settings.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Id, Profile);
        }
    }

    abstract class MITrack
    {
        public bool IsVideo { get; protected set; }
        public bool IsAudio { get; protected set; }
        public bool IsSubtitle { get; protected set; }
        public bool IsChapter { get; protected set; }

        public int StreamKindId { get; protected set; }

        /// <summary>
        /// Determines whether the track is included in (muxed into) the output file.
        /// </summary>
        public bool Mux = true;

        protected string OldTitle;
        public string Title;

        protected Language OldLanguage;
        public Language Language;

        protected bool? OldIsDefault;
        public bool? IsDefault;

        protected bool? OldIsForced;
        public bool? IsForced;

        public virtual bool HasChanged
        {
            get
            {
                return
                    OldTitle != Title ||
                    OldLanguage != Language ||
                    OldIsDefault != IsDefault ||
                    OldIsForced != IsForced
                    ;
            }
        }

        public virtual MITrack ReadFromXml(string xml)
        {
            StreamKindId = XmlUtil.GetInt(xml, "StreamKindId");

            Title = OldTitle = XmlUtil.GetString(xml, "Title");
            Language = OldLanguage = Language.GetLanguage(XmlUtil.GetString(xml, "LanguageIso2"));
            IsDefault = OldIsDefault = XmlUtil.GetBitFlag(xml, "Default");
            IsForced = OldIsForced = XmlUtil.GetBitFlag(xml, "Forced");

            return this;
        }
    }

    abstract class MIAVSTrack : MITrack /*, INotifyPropertyListener */
    {
        public MIFormat Format { get; protected set; }
        public long Duration { get; protected set; }
        public string DurationString { get; protected set; }
        public long StreamSize { get; protected set; }
        public double StreamSizePercent { get; protected set; }

        public double? FrameRate { get; protected set; }
        public string FrameRateString { get; protected set; }
        public long? FrameCount { get; protected set; }

        public override MITrack ReadFromXml(string xml)
        {
            base.ReadFromXml(xml);

            Format = 
                new MIFormat(
                    XmlUtil.GetString(xml, "Format"),
                    XmlUtil.GetString(xml, "FormatInfo"),
                    XmlUtil.GetString(xml, "FormatVersion"),
                    XmlUtil.GetString(xml, "FormatProfile"),
                    XmlUtil.GetString(xml, "FormatCompression"),
                    XmlUtil.GetString(xml, "FormatSettings")
                );
            Duration = XmlUtil.GetLong(xml, "Duration");
            DurationString = XmlUtil.GetString(xml, "DurationString");
            StreamSize = XmlUtil.GetLong(xml, "StreamSize");
            StreamSizePercent = XmlUtil.GetDouble(xml, "StreamSizePercent");

            FrameRate = XmlUtil.GetDoubleNullable(xml, "FrameRate");
            FrameRateString = XmlUtil.GetString(xml, "FrameRateString");
            FrameCount = XmlUtil.GetLongNullable(xml, "FrameCount");

            return this;
        }
    }

    class MIVideoTrack : MIAVSTrack
    {
        public int BitDepth { get; protected set; }
        public string BitDepthString { get; protected set; }
        public string BitRate { get; protected set; }
        public string BitRateMode { get; protected set; }
        public string BitRateModeString { get; protected set; }

        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public double PixelAspectRatio { get; protected set; }
        public double DisplayAspectRatio { get; protected set; }
        public string Chroma { get; protected set; }
        public string ScanType { get; protected set; }

        public string FilePath { get; protected set; }

        public MIVideoTrack(string filePath)
        {
            IsVideo = true;
            FilePath = filePath;
        }

        public override MITrack ReadFromXml(string xml)
        {
            base.ReadFromXml(xml);

            BitDepth = XmlUtil.GetInt(xml, "BitDepth");
            BitDepthString = XmlUtil.GetString(xml, "BitDepthString");
            BitRate = XmlUtil.GetString(xml, "BitRate");
            BitRateMode = XmlUtil.GetString(xml, "BitRateMode");
            BitRateModeString = XmlUtil.GetString(xml, "BitRateModeString");

            Width = XmlUtil.GetInt(xml, "Width");
            Height = XmlUtil.GetInt(xml, "Height");

            PixelAspectRatio = XmlUtil.GetDouble(xml, "PixelAspectRatio");
            DisplayAspectRatio = XmlUtil.GetDouble(xml, "DisplayAspectRatio");
            Chroma = XmlUtil.GetString(xml, "Chroma");
            ScanType = XmlUtil.GetString(xml, "ScanType");

            return this;
        }
    }

    class MIAudioTrack : MIAVSTrack
    {
        public int BitDepth { get; protected set; }
        public string BitDepthString { get; protected set; }
        public string BitRate { get; protected set; }
        public string BitRateMode { get; protected set; }
        public string BitRateModeString { get; protected set; }

        public double Channels { get; protected set; }
        public string ChannelsString { get; protected set; }

        /// <summary>
        /// Number of channels per spatial location, in Front/Side/Back format
        /// </summary>
        /// <example>3/2/0.1</example>
        public string ChannelPositions { get; protected set; }

        /// <summary>
        /// Spatial layout of channels in human-readable form
        /// </summary>
        /// <example>Front: L C R, Side: L R, LFE</example>
        public string ChannelPositionsString { get; protected set; }

        public int SamplingRate { get; protected set; }
        public string SamplingRateString { get; protected set; }
        public long SampleCount { get; protected set; }

        public string FilePath { get; protected set; }

        public MIAudioTrack(string filePath)
        {
            IsAudio = true;
            FilePath = filePath;
        }

        public override MITrack ReadFromXml(string xml)
        {
            base.ReadFromXml(xml);

            BitDepth = XmlUtil.GetInt(xml, "BitDepth");
            BitDepthString = XmlUtil.GetString(xml, "BitDepthString");
            BitRate = XmlUtil.GetString(xml, "BitRate");
            BitRateMode = XmlUtil.GetString(xml, "BitRateMode");
            BitRateModeString = XmlUtil.GetString(xml, "BitRateModeString");

            Channels = XmlUtil.GetDouble(xml, "Channels");
            ChannelsString = XmlUtil.GetString(xml, "ChannelsString");

            ChannelPositions = XmlUtil.GetString(xml, "ChannelPositions");
            ChannelPositionsString = XmlUtil.GetString(xml, "ChannelPositionsString");

            SamplingRate = XmlUtil.GetInt(xml, "SamplingRate");
            SamplingRateString = XmlUtil.GetString(xml, "SamplingRateString");
            SampleCount = XmlUtil.GetLong(xml, "SampleCount");

            return this;
        }
    }

    /// <summary>
    /// MediaInfo refers to these as "Text".
    /// </summary>
    class MISubtitleTrack : MIAVSTrack
    {
        public int? Width { get; protected set; }
        public int? Height { get; protected set; }

        public string FilePath { get; protected set; }

        public MISubtitleTrack(string filePath)
        {
            IsSubtitle = true;
            FilePath = filePath;
        }

        public override MITrack ReadFromXml(string xml)
        {
            base.ReadFromXml(xml);

            Width = XmlUtil.GetIntNullable(xml, "Width");
            Height = XmlUtil.GetIntNullable(xml, "Height");

            return this;
        }
    }

    /// <summary>
    /// MediaInfo refers to these as "Menu"; "Chapters" is something different (not sure what "Chapters" means to MediaInfo)
    /// </summary>
    class MIChapterTrack : MITrack
    {
        private readonly List<MIChapter> _chapters = new List<MIChapter>();

        public IList<MIChapter> Chapters
        {
            get { return _chapters.AsReadOnly(); }
        }

        public MIChapterTrack()
        {
            IsChapter = true;
        }

        public void ReadFromTxt(string menuSectionText)
        {
            _chapters.AddRange(menuSectionText.Split('\n').Where(MIChapter.IsChapter).Select(MIChapter.Parse));
        }

        public override string ToString()
        {
            return string.Format("{0} chapters [ {1} ]", _chapters.Count, string.Join(", ", _chapters));
        }
    }
    
    class MIChapter
    {
        private static readonly Regex ChapterLineRegex = new Regex(@"^(\d{2}):(\d{2}):(\d{2})\.(\d{3})[ \t]*:[ \t]*(?:([a-z]{2}):)?(.*)$");

        public TimeSpan Offset { get; protected set; }

        public string OffsetString
        {
            get
            {
                // 00:00:00.000
                return string.Format("{0}:{1}:{2}.{3}",
                    Offset.Hours.ToString("00"),
                    Offset.Minutes.ToString("00"),
                    Offset.Seconds.ToString("00"),
                    Offset.Milliseconds.ToString("000"));
            }
        }

        public Language Language;
        public string Title;

        public MIChapter(int hours, int minutes, int seconds, int milliseconds, Language language, string title)
        {
            Offset = new TimeSpan(0, hours, minutes, seconds, milliseconds);
            Language = language;
            Title = title;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(OffsetString);
            if (Language != null)
                sb.AppendFormat(" - {0}", Language.ISO_639_2);
            if (!string.IsNullOrWhiteSpace(Title))
                sb.AppendFormat(" - {0}", Title);
            return sb.ToString();
        }

        public static bool IsChapter(string line)
        {
            return ChapterLineRegex.IsMatch(line);
        }

        public static MIChapter Parse(string line)
        {
            if (!IsChapter(line)) return null;

            var groups = ChapterLineRegex.Match(line).Groups;

            var chapter = new MIChapter
                (
                    XmlUtil.ParseInt(groups[1].Value),
                    XmlUtil.ParseInt(groups[2].Value),
                    XmlUtil.ParseInt(groups[3].Value),
                    XmlUtil.ParseInt(groups[4].Value),
                    !string.IsNullOrWhiteSpace(groups[5].Value) ? Language.GetLanguage(groups[5].Value) : null,
                    !string.IsNullOrWhiteSpace(groups[6].Value) ? groups[6].Value : null
                );

            return chapter;
        }
    }
}
