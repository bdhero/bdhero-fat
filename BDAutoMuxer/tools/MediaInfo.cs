using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BDAutoMuxer.BDInfo;
using BDAutoMuxer.Properties;
using Newtonsoft.Json;

namespace BDAutoMuxer.tools
{
    public class MediaInfo
    {
        public static readonly MIConfig Config = new MIConfig();
        private static readonly Regex TrackRegex = new Regex(@"<(Video|Audio|Subtitle|Chapter)Track>\s*(.*?)\s*</\1Track>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex NumberedPattern = new Regex(@"^(.+)\.(\d+)(\.wav)$", RegexOptions.IgnoreCase);

        private readonly string _mediaFilePath;

        public MIFile File;
        public MIContainer Container;

        public bool IsNumbered { get { return _filePaths.Count > 1; } }
        public string DisplayableFilename { get; private set; }

        private readonly List<string> _filePaths = new List<string>();
        private readonly List<MITrack> _tracks = new List<MITrack>();
        private readonly List<MIVideoTrack> _videoTracks = new List<MIVideoTrack>();
        private readonly List<MIAudioTrack> _audioTracks = new List<MIAudioTrack>();
        private readonly List<MISubtitleTrack> _subtitleTracks = new List<MISubtitleTrack>();
        private readonly List<MIChapterTrack> _chapterTracks = new List<MIChapterTrack>();

        public IList<string> FilePaths { get { return _filePaths.AsReadOnly(); } }
        public IList<MITrack> Tracks { get { return _tracks.AsReadOnly(); } }
        public IList<MIVideoTrack> VideoTracks { get { return _videoTracks.AsReadOnly(); } }
        public IList<MIAudioTrack> AudioTracks { get { return _audioTracks.AsReadOnly(); } }
        public IList<MISubtitleTrack> SubtitleTracks { get { return _subtitleTracks.AsReadOnly(); } }
        public IList<MIChapterTrack> ChapterTracks { get { return _chapterTracks.AsReadOnly(); } }

        static MediaInfo()
        {
            Config.CLIPath = AbstractExternalTool.GetLibPathAbsolute("MediaInfo.exe");
            Config.CSVPath = AbstractExternalTool.GetLibPathAbsolute("MediaInfoXML.csv");
        }

        #region Testing

        public static void Test(string[] files = null)
        {
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

            if (files != null)
                mediaFilePaths = files;

            Console.WriteLine("Scanning {0} MPLS files...", mediaFilePaths.Length);

            var mediaInfos = mediaFilePaths.Select(filePath => new MediaInfo(filePath).Scan()).ToList();

            var miVideoTracks = mediaInfos.SelectMany(mi => mi.VideoTracks).ToList();
            var miAudioTracks = mediaInfos.SelectMany(mi => mi.AudioTracks).ToList();

            var videoCodecs = new Dictionary<MIFormat, string>();
            var audioCodecs = new Dictionary<MIFormat, string>();

            miVideoTracks.ForEach(track => videoCodecs[track.Format] = track.FilePath);
            miAudioTracks.ForEach(track => audioCodecs[track.Format] = track.FilePath);

            var strVideoCodecs = JsonConvert.SerializeObject(videoCodecs);
            var strAudioCodecs = JsonConvert.SerializeObject(audioCodecs);

            var text = string.Format("VIDEO\n=====\n\n{0}\n\nAUDIO\n=====\n\n{1}", strVideoCodecs, strAudioCodecs);

            var outputFile = @"E:\BD\BDAM.MediaInfo.txt";

            Console.WriteLine("\n\nWrote output to {0}", outputFile);

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
            var isTrash = fileInfo.Name.StartsWith("$");
            var isHiddenUnix = fileInfo.Name.StartsWith(".");
            return correctExtension && !(isTrash || isHiddenUnix);
        }

        #endregion

        public MediaInfo(string mediaFilePath)
        {
            _mediaFilePath = mediaFilePath;
        }

        private void GetFilePaths()
        {
            if (NumberedPattern.IsMatch(_mediaFilePath))
            {
                var match = NumberedPattern.Match(_mediaFilePath);
                var partialPath = match.Groups[1].Value;
                var extension = match.Groups[3].Value;

                for (var i = 1; i < 100; i++)
                {
                    var path = string.Format("{0}.{1}{2}", partialPath, i, extension);
                    if (System.IO.File.Exists(path))
                        _filePaths.Add(path);
                    else
                        break;
                }

                DisplayableFilename = string.Format("{0}.{{1...{1}}}{2}", Path.GetFileName(partialPath), _filePaths.Count, extension);
            }
            else
            {
                _filePaths.Add(_mediaFilePath);
                DisplayableFilename = Path.GetFileName(_mediaFilePath);
            }
        }

        public MediaInfo Scan()
        {
            if (!System.IO.File.Exists(_mediaFilePath))
                throw new FileNotFoundException("Media file not found", _mediaFilePath);

            var validationException = Config.ValidationException;
            if (validationException != null) throw validationException;

            if (!FromFilename())
                ParseXml();

            GetFilePaths();

            return this;
        }

        private bool FromFilename()
        {
            if (MICodecPGS.Matches(_mediaFilePath))
            {
                _subtitleTracks.Add(new MISubtitleTrack(_mediaFilePath, MICodec.PGS));
                return true;
            }
            if (MICodecVobSub.Matches(_mediaFilePath))
            {
                _subtitleTracks.Add(new MISubtitleTrack(_mediaFilePath, MICodec.VobSub));
                return true;
            }
            if (MICodecSRT.Matches(_mediaFilePath))
            {
                _subtitleTracks.Add(new MISubtitleTrack(_mediaFilePath, MICodec.SRT));
                return true;
            }
            return false;
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

        private void ParseXml()
        {
            var xmlResult = RunProcess(new List<string> {"--Full", "--Output=file://" + Config.CSVPath});
            if (xmlResult.Exception != null) throw xmlResult.Exception;
            if (xmlResult.StdErr.Length > 0)
                throw new Exception(string.Format("MediaInfo XML exception: {0}", xmlResult.StdErr));

            if (!xmlResult.StdOut.StartsWith("<?xml"))
                throw new Exception(string.Format("Expecting XML output; instead found \"{0}\"", xmlResult.StdOut));

            ParseXml(xmlResult.StdOut);

            if (_chapterTracks.Any())
            {
                var txtResult = RunProcess();
                if (txtResult.Exception != null) throw txtResult.Exception;
                if (txtResult.StdErr.Length > 0)
                    throw new Exception(string.Format("MediaInfo TXT exception: {0}", txtResult.StdErr));

                ParseTxt(txtResult.StdOut);
            }
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

    public class MIProcessResult
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

    public static class XmlUtil
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

    public class MIConfig
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

    #region Meta data

    public class MIFile
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

    public class MIContainer
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

    #endregion

    #region Tracks

    public abstract class MITrack
    {
        public bool IsVideo { get; protected set; }
        public bool IsAudio { get; protected set; }
        public bool IsSubtitle { get; protected set; }
        public bool IsChapter { get; protected set; }

        /// <summary>
        /// Shortcut for IsAudio || IsVideo || IsSubtitle.
        /// </summary>
        public bool IsAVS { get { return IsAudio || IsVideo || IsSubtitle; } }

        /// <summary>
        /// The order in which this track appears in the source media file, relative to other tracks of the same type (base = 0).
        /// </summary>
        public int StreamKindId { get; protected set; }

        /// <summary>
        /// Determines whether the track should be included (muxed) in the output file.
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

        public bool IsSelected;

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
            IsDefault = OldIsDefault = XmlUtil.GetBitFlag(xml, "Default") ?? false;
            IsForced = OldIsForced = XmlUtil.GetBitFlag(xml, "Forced") ?? false;

            return this;
        }
    }

    public abstract class MIAVSTrack : MITrack /*, INotifyPropertyListener */
    {
        /// <summary>
        /// The order in which this track appears in the source media file, irrespective of track type (base = 0).
        /// </summary>
        public int StreamOrder { get; protected set; }

        public MIFormat Format { get; protected set; }
        public long Duration { get; protected set; }
        public string DurationString { get; protected set; }
        public long StreamSize { get; protected set; }
        public double StreamSizePercent { get; protected set; }

        public double? FrameRate { get; protected set; }
        public string FrameRateString { get; protected set; }
        public long? FrameCount { get; protected set; }

        public MICodec Codec { get; protected set; }

        public override MITrack ReadFromXml(string xml)
        {
            base.ReadFromXml(xml);

            StreamOrder = XmlUtil.GetInt(xml, "StreamOrder");

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

            Codec = GetCodec(this);

            return this;
        }

        protected virtual string GetDefaultTitle()
        {
            return Codec.CommonName;
        }

        protected static MICodec GetCodec(MIAVSTrack track)
        {
            var format = track.Format;

            if (track.IsVideo)
            {
                if (MICodecAVC.Matches(format))
                    return MICodec.AVC;
                if (MICodecVC1.Matches(format))
                    return MICodec.VC1;
                if (MICodecMPEG1Video.Matches(format))
                    return MICodec.MPEG1Video;
                if (MICodecMPEG2Video.Matches(format))
                    return MICodec.MPEG2Video;
                return MICodec.UnknownVideo;
            }
            if (track.IsAudio)
            {
                // Dolby

                if (MICodecProLogic.Matches(format))
                    return MICodec.ProLogic;
                if (MICodecAC3.Matches(format))
                    return MICodec.AC3;
                if (MICodecAC3EX.Matches(format))
                    return MICodec.AC3EX;
                if (MICodecEAC3.Matches(format))
                    return MICodec.EAC3;
                if (MICodecTrueHD.Matches(format))
                    return MICodec.TrueHD;

                // DTS

                if (MICodecDTS.Matches(format))
                    return MICodec.DTS;
                if (MICodecDTSES.Matches(format))
                    return MICodec.DTSES;
                if (MICodecDTSExpress.Matches(format))
                    return MICodec.DTSExpress;
                if (MICodecDTSHDHRA.Matches(format))
                    return MICodec.DTSHDHRA;
                if (MICodecDTSHDMA.Matches(format))
                    return MICodec.DTSHDMA;

                // MPEG

                if (MICodecMP3.Matches(format))
                    return MICodec.MP3;
                if (MICodecAAC.Matches(format))
                    return MICodec.AAC;

                // MISC

                if (MICodecVorbis.Matches(format))
                    return MICodec.Vorbis;
                if (MICodecFLAC.Matches(format))
                    return MICodec.FLAC;

                // LPCM

                if (MICodecLPCM.Matches(format))
                    return MICodec.LPCM;

                return MICodec.UnknownAudio;
            }
            if (track.IsSubtitle)
            {
                var subTrack = track as MISubtitleTrack;
                if (subTrack == null) return MICodec.UnknownSubtitle;

                if (MICodecPGS.Matches(format) || MICodecPGS.Matches(subTrack.FilePath))
                    return MICodec.PGS;
                if (MICodecVobSub.Matches(format) || MICodecVobSub.Matches(subTrack.FilePath))
                    return MICodec.VobSub;
                if (MICodecSRT.Matches(format) || MICodecSRT.Matches(subTrack.FilePath))
                    return MICodec.SRT;

                return MICodec.UnknownSubtitle;
            }

            return MICodec.UnknownCodec;
        }
    }

    public class MIVideoTrack : MIAVSTrack
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

        /// <summary>
        /// "1080p", "1080i", "720p", "720i", "480p"
        /// </summary>
        public string DisplayResolution
        {
            get
            {
                var height = Width == 1920 ? 1080 : Width == 1280 ? 720 : 480;
                var scanType = IsProgressive ? "p" : "i";
                return string.Format("{0}{1}", height, scanType);
            }
        }

        public bool IsProgressive
        {
            get { return ScanType == "Progressive"; }
        }

        public bool IsInterlaced
        {
            get { return ScanType == "Interlaced"; }
        }

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

            Title = OldTitle = GetDefaultTitle();

            return this;
        }

        protected override string GetDefaultTitle()
        {
            return string.Format("{0} {1}", DisplayResolution, base.GetDefaultTitle());
        }
    }

    public class MIAudioTrack : MIAVSTrack
    {
        public int BitDepth { get; protected set; }
        public string BitDepthString { get; protected set; }
        public string BitRate { get; protected set; }
        public string BitRateMode { get; protected set; }
        public string BitRateModeString { get; protected set; }

        /// <summary>
        /// Integer value reported by MediaInfo as "Channels" (8 = 7.1, 6 = 5.1, etc.)
        /// </summary>
        public int ChannelsInt { get; protected set; }

        /// <summary>
        /// Decimal value calculated from ChannelPositions.
        /// 8.1, 7.1, 5.1, 2.1, 2.0, etc.
        /// </summary>
        public double Channels
        {
            get { return ChannelPositions != null ? ChannelPositions.Split('/').Sum(s => Double.Parse(s)) : -1; }
        }

        /// <summary>
        /// E.G., "6 channels"
        /// </summary>
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

            ChannelsInt = XmlUtil.GetInt(xml, "Channels");
            ChannelsString = XmlUtil.GetString(xml, "ChannelsString");

            ChannelPositions = XmlUtil.GetString(xml, "ChannelPositions");
            ChannelPositionsString = XmlUtil.GetString(xml, "ChannelPositionsString");

            SamplingRate = XmlUtil.GetInt(xml, "SamplingRate");
            SamplingRateString = XmlUtil.GetString(xml, "SamplingRateString");
            SampleCount = XmlUtil.GetLong(xml, "SampleCount");

            Title = OldTitle = GetDefaultTitle();

            return this;
        }

        protected override string GetDefaultTitle()
        {
            return string.Format("{0} ({1} ch)", base.GetDefaultTitle(), Channels.ToString("0.0"));
        }
    }

    /// <summary>
    /// MediaInfo refers to these as "Text".
    /// </summary>
    public class MISubtitleTrack : MIAVSTrack
    {
        public int? Width { get; protected set; }
        public int? Height { get; protected set; }

        public string FilePath { get; protected set; }

        public MISubtitleTrack(string filePath, MISubtitleCodec codec = null)
        {
            IsSubtitle = true;
            FilePath = filePath;
            Codec = codec ?? MICodec.UnknownSubtitle;
        }

        public override MITrack ReadFromXml(string xml)
        {
            base.ReadFromXml(xml);

            Width = XmlUtil.GetIntNullable(xml, "Width");
            Height = XmlUtil.GetIntNullable(xml, "Height");

            Title = OldTitle = GetDefaultTitle();

            return this;
        }
    }

    /// <summary>
    /// MediaInfo refers to these as "Menu"; "Chapters" is something different (not sure what "Chapters" means to MediaInfo)
    /// </summary>
    public class MIChapterTrack : MITrack
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

    public class MIChapter
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

    #endregion

    public class MIFormat
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
            var sb = new StringBuilder();
            sb.Append(Id);
            if (!string.IsNullOrWhiteSpace(Profile))
                sb.AppendFormat(" - {0}", Profile);
            if (!string.IsNullOrWhiteSpace(Version))
                sb.AppendFormat(" ({0})", Version);
            return sb.ToString();
        }
    }

    #region Abstract Codecs

    public abstract class MICodec
    {
        #region Individual Codecs

        #region Video

        public static readonly MICodecAVC AVC = new MICodecAVC();
        public static readonly MICodecVC1 VC1 = new MICodecVC1();
        public static readonly MICodecMPEG1Video MPEG1Video = new MICodecMPEG1Video();
        public static readonly MICodecMPEG2Video MPEG2Video = new MICodecMPEG2Video();
        public static readonly MICodecUnknownVideo UnknownVideo = new MICodecUnknownVideo();

        #endregion

        #region Audio

        public static readonly MICodecProLogic ProLogic = new MICodecProLogic();
        public static readonly MICodecAC3 AC3 = new MICodecAC3();
        public static readonly MICodecAC3EX AC3EX = new MICodecAC3EX();
        public static readonly MICodecEAC3 EAC3 = new MICodecEAC3();
        public static readonly MICodecTrueHD TrueHD = new MICodecTrueHD();

        public static readonly MICodecDTS DTS = new MICodecDTS();
        public static readonly MICodecDTSES DTSES = new MICodecDTSES();
        public static readonly MICodecDTSExpress DTSExpress = new MICodecDTSExpress();
        public static readonly MICodecDTSHDHRA DTSHDHRA = new MICodecDTSHDHRA();
        public static readonly MICodecDTSHDMA DTSHDMA = new MICodecDTSHDMA();

        public static readonly MICodecMP3 MP3 = new MICodecMP3();
        public static readonly MICodecAAC AAC = new MICodecAAC();

        public static readonly MICodecVorbis Vorbis = new MICodecVorbis();
        public static readonly MICodecFLAC FLAC = new MICodecFLAC();

        public static readonly MICodecLPCM LPCM = new MICodecLPCM();

        public static readonly MICodecUnknownAudio UnknownAudio = new MICodecUnknownAudio();

        #endregion

        #region Subtitle

        public static readonly MICodecPGS PGS = new MICodecPGS();
        public static readonly MICodecVobSub VobSub = new MICodecVobSub();
        public static readonly MICodecSRT SRT = new MICodecSRT();

        public static readonly MICodecUnknownSubtitle UnknownSubtitle = new MICodecUnknownSubtitle();

        #endregion

        #region Unknown

        public static readonly MIUnknownCodec UnknownCodec = new MIUnknownCodec();

        #endregion

        #endregion

        #region Codec Lists

        public static readonly List<MIVideoCodec> VideoCodecs = new List<MIVideoCodec>()
                                                                    {
                                                                        AVC,
                                                                        VC1,
                                                                        MPEG1Video,
                                                                        MPEG2Video
                                                                    };

        public static readonly List<MIAudioCodec> AudioCodecs = new List<MIAudioCodec>()
                                                                    {
                                                                        // LPCM
                                                                        LPCM,
                                                                        // DTS
                                                                        DTSHDMA,
                                                                        DTSHDHRA,
                                                                        DTSES,
                                                                        DTS,
                                                                        DTSExpress,
                                                                        // Dolby
                                                                        TrueHD,
                                                                        EAC3,
                                                                        AC3EX,
                                                                        AC3,
                                                                        ProLogic,
                                                                        // MPEG
                                                                        AAC,
                                                                        MP3,
                                                                        // Misc
                                                                        FLAC,
                                                                        Vorbis
                                                                    };

        public static readonly List<MIAudioCodec> MuxableBDAudioCodecs = new List<MIAudioCodec>()
                                                                    {
                                                                        // Lossless - All
                                                                        LPCM,
                                                                        DTSHDMA,
                                                                        TrueHD,
                                                                        // Lossy - DTS
                                                                        DTSHDHRA,
                                                                        DTS,
                                                                        // Lossy - Dolby
                                                                        EAC3,
                                                                        AC3
                                                                    };

        public static readonly List<MISubtitleCodec> SubtitleCodecs = new List<MISubtitleCodec>()
                                                                          {
                                                                              PGS,
                                                                              VobSub,
                                                                              SRT
                                                                          };

        public static readonly List<MICodec> AllCodecs = new List<MICodec>()
                                                             {
                                                                 AVC,
                                                                 VC1,
                                                                 MPEG1Video,
                                                                 MPEG2Video,
                                                                 ProLogic,
                                                                 AC3,
                                                                 AC3EX,
                                                                 EAC3,
                                                                 TrueHD,
                                                                 DTS,
                                                                 DTSES,
                                                                 DTSExpress,
                                                                 DTSHDHRA,
                                                                 DTSHDMA,
                                                                 MP3,
                                                                 AAC,
                                                                 Vorbis,
                                                                 FLAC,
                                                                 LPCM,
                                                                 PGS
                                                             };

        #endregion

        public abstract bool IsAudio { get; }
        public abstract bool IsVideo { get; }
        public abstract bool IsSubtitle { get; }

        public virtual bool IsKnown { get { return true; } }

        public bool IsOfficialBlurayCodec { get { return IsRequiredBlurayCodec || IsOptionalBlurayCodec; } }
        public virtual bool IsRequiredBlurayCodec { get { return false; } }
        public virtual bool IsOptionalBlurayCodec { get { return false; } }

        public bool IsOfficialDVDCodec { get { return IsRequiredDVDCodec || IsOptionalDVDCodec; } }
        public virtual bool IsRequiredDVDCodec { get { return false; } }
        public virtual bool IsOptionalDVDCodec { get { return false; } }

        /// <summary>
        /// Serializable codec ID in all uppercase (e.g., "V_H264")
        /// </summary>
        public abstract string SerializableName { get; }

        public abstract TSStreamType StreamType { get; }

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

        public abstract string Description { get; }

        public virtual string CommonName { get { return ShortName; } }

        public virtual IEnumerable<string> AltDisplayNames
        {
            get
            {
                return new List<string>(new[] {AltFullName, AltShortName}.Where(s => s != null));
            }
        }

        /// <summary>
        /// Can this codec be muxed by standard, freely available consumer software?
        /// </summary>
        public virtual bool IsMuxable { get { return true; } }

        /// <summary>
        /// The full name of the codec plus any alternate names to avoid confusion (e.g., "Dolby Digital (AC-3)").
        /// </summary>
        public virtual string FullNameDisambig { get { return FullName; } }

        public abstract string TypeDisplay { get; }

        public abstract Image Logo { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(CodecId);
            sb.AppendFormat(" - {0}", FullName);
            if (!string.IsNullOrWhiteSpace(AltFullName))
                sb.AppendFormat(" ({0})", AltFullName);
            if (!IsMuxable)
                sb.Append(" (NOT muxable)");
            return sb.ToString();
        }

        public override int GetHashCode()
        {
            return SerializableName.GetHashCode();
        }

        public static MICodec FromSerializableName(string serializedName)
        {
            serializedName = serializedName.ToUpperInvariant();
            return AllCodecs.FirstOrDefault(codec => codec.SerializableName == serializedName) ?? UnknownCodec;
        }

        public static string SerializeCodecs<T>(ICollection<T> codecs) where T : MICodec
        {
            return string.Join(";", new HashSet<T>(codecs).Select(codec => codec.SerializableName));
        }

        public static ISet<T> DeserializeCodecs<T>(string codecs) where T : MICodec
        {
            var codecNames = (codecs ?? "").Trim().Split(';').Where(serializedName => !string.IsNullOrWhiteSpace(serializedName));
            return new HashSet<T>(codecNames.Select(DeserializeCodec<T>));
        }

        public static T DeserializeCodec<T>(string serializableName) where T : MICodec
        {
            serializableName = (serializableName ?? "").Trim().ToUpperInvariant();
            var matchingCodec = AllCodecs.FirstOrDefault(codec => codec.SerializableName == serializableName);
            return (matchingCodec ?? UnknownCodec) as T;
        }
    }

    public abstract class MIAudioCodec : MICodec
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

        public override string TypeDisplay
        {
            get { return "Audio"; }
        }

        public virtual MIAudioCodec Core { get { return null; } }

        public bool Lossy { get { return !Lossless; } }
        public abstract bool Lossless { get; }
    }

    public abstract class MIVideoCodec : MICodec
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
        public override string TypeDisplay
        {
            get { return "Video"; }
        }
    }

    public abstract class MISubtitleCodec : MICodec
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
        public override string TypeDisplay
        {
            get { return "Subtitle"; }
        }
    }

    #endregion

    #region Video Codecs

    /// <summary>
    /// H.264/MPEG-4 AVC
    /// </summary>
    public class MICodecAVC : MIVideoCodec
    {
        public override string SerializableName
        {
            get { return "V_H264"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.AVC_VIDEO; }
        }

        public override string CodecId
        {
            get { return "V_MPEG4/ISO/AVC"; }
        }

        public override string FullName
        {
            get { return "H.264/MPEG-4 AVC"; }
        }

        public override string ShortName
        {
            get { return "H.264/AVC"; }
        }

        public override string MicroName
        {
            get { return "H.264"; }
        }

        public override string AltFullName
        {
            get { return "MPEG-4 Part 10"; }
        }

        public override string AltMicroName
        {
            get { return "AVC"; }
        }

        public override string CommonName
        {
            get { return MicroName; }
        }

        public override IEnumerable<string> AltDisplayNames
        {
            get { return new[] { AltMicroName, ShortName, AltFullName }; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "The de facto standard for high quality HD video at reasonable file sizes.  Enjoys wide player support and is HandBrake's default video codec."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_h264; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "AVC";
        }
    }

    /// <summary>
    /// VC-1
    /// </summary>
    public class MICodecVC1 : MIVideoCodec
    {
        public override string SerializableName
        {
            get { return "V_VC1"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.VC1_VIDEO; }
        }

        public override string CodecId
        {
            // TODO: Or is it "V_MS/VFW/WVC1"?
            get { return "V_VC1"; }
        }

        public override string FullName
        {
            get { return "VC-1"; }
        }

        public override string ShortName
        {
            get { return "VC-1"; }
        }

        public override string MicroName
        {
            get { return "VC-1"; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Microsoft's Blu-ray video codec.  Much less common than H.264/AVC."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_vc1; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "VC-1";
        }
    }

    /// <summary>
    /// MPEG-1 Video (non-Blu-ray)
    /// </summary>
    public class MICodecMPEG1Video : MIVideoCodec
    {
        public override string SerializableName
        {
            get { return "V_MPEG1"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.MPEG1_VIDEO; }
        }

        public override string CodecId
        {
            get { return "V_MPEG1"; }
        }

        public override string FullName
        {
            get { return "MPEG-1 Video"; }
        }

        public override string ShortName
        {
            get { return "MPEG-1 Video"; }
        }

        public override string MicroName
        {
            get { return "M1V"; }
        }

        public override string AltFullName
        {
            get { return "MPEG-1 Part 2"; }
        }

        public override string CommonName
        {
            get { return "MPEG"; }
        }

        public override bool IsRequiredDVDCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Video portion of the MPEG-1 standard.  Most commonly found on DVDs and standalone video files (e.g., .mpg, .mpeg)."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_mpeg1_video; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "MPEG Video" && format.Version == "Version 1";
        }
    }

    /// <summary>
    /// MPEG-2 Video
    /// </summary>
    public class MICodecMPEG2Video : MIVideoCodec
    {
        public override string SerializableName
        {
            get { return "V_MPEG2"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.MPEG2_VIDEO; }
        }

        public override string CodecId
        {
            get { return "V_MPEG2"; }
        }

        public override string FullName
        {
            get { return "H.262/MPEG-2 Part 2"; }
        }

        public override string ShortName
        {
            get { return "MPEG-2 Video"; }
        }

        public override string MicroName
        {
            get { return "M2V"; }
        }

        public override string AltShortName
        {
            get { return "MPEG-2 Part 2"; }
        }

        public override string AltMicroName
        {
            get { return "H.262"; }
        }

        public override IEnumerable<string> AltDisplayNames
        {
            get { return new[] { AltMicroName, AltShortName }; }
        }

        public override string Description
        {
            get { return "Video portion of the MPEG-2 standard."; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override bool IsRequiredDVDCodec
        {
            get { return true; }
        }

        public override Image Logo
        {
            get { return Resources.logo_mpeg2_video; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "MPEG Video" && format.Version == "Version 2";
        }
    }

    public class MICodecUnknownVideo : MISubtitleCodec
    {
        public override string SerializableName
        {
            get { return "V_UNKNOWN"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "V_UNKNOWN"; }
        }

        public override string FullName
        {
            get { return "Unknown Video Codec"; }
        }

        public override string ShortName
        {
            get { return "Unknown Video"; }
        }

        public override string MicroName
        {
            get { return "Unknown Video"; }
        }

        public override string Description
        {
            get { return "Unknown video format."; }
        }

        public override Image Logo
        {
            get { return null; }
        }

        public override bool IsKnown
        {
            get { return false; }
        }
    }

    #endregion

    #region Audio Codecs

    #region Dolby

    /// <summary>
    /// Dolby Pro Logic
    /// </summary>
    public class MICodecProLogic : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_AC3_PL"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.Unknown; }
        }

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
            get { return "PL"; }
        }

        /// <summary>
        /// "Dolby Pro Logic" is actually the name of the Decoder; "Dolby Surround" is the name of the Encoder.
        /// </summary>
        public override string AltFullName
        {
            get { return "Dolby Surround"; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override string Description
        {
            get { return "Dolby Stereo + 2 matrixed channels (front center and rear center), resulting in 4.0 channel output.  Backwards compatible with older stereo systems."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_dolby_pro_logic; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "AC-3" && format.Profile == "Dolby Digital";
        }
    }

    /// <summary>
    /// Dolby Digital
    /// </summary>
    public class MICodecAC3 : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_AC3"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.AC3_AUDIO; }
        }

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

        public override string AltMicroName
        {
            get { return "DD"; }
        }

        public override string FullNameDisambig
        {
            get { return string.Format("{0} ({1})", base.FullNameDisambig, ShortName); }
        }

        public override IEnumerable<string> AltDisplayNames
        {
            get { return new[] {MicroName}; }
        }

        public override string Description
        {
            get { return "Standard Dolby Digital.  One of the most common audio codecs for consumer video (Blu-ray, DVD, and TV)."; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override bool IsRequiredDVDCodec
        {
            get { return true; }
        }

        public override Image Logo
        {
            get { return Resources.logo_dolby_digital; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "AC-3" && string.IsNullOrEmpty(format.Profile);
        }
    }

    /// <summary>
    /// Dolby Digital EX
    /// </summary>
    public class MICodecAC3EX : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_AC3_EX"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "A_AC3"; }
        }

        public override string FullName
        {
            get { return "Dolby Digital EX"; }
        }

        public override string ShortName
        {
            get { return "AC-3 EX"; }
        }

        public override string MicroName
        {
            get { return "AC3 EX"; }
        }

        public override string AltMicroName
        {
            get { return "DD EX"; }
        }

        public override string CommonName
        {
            get { return FullName; }
        }

        public override IEnumerable<string> AltDisplayNames
        {
            get { return new[] {MicroName}; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override MIAudioCodec Core
        {
            get { return AC3; }
        }

        public override string Description
        {
            get { return "Extension of AC-3 (Dolby Digital) that adds 1 or 2 matrixed rear channels, creating 6.1 or 7.1 channel output.  Backwards compatible with regular AC-3."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_dolby_digital_ex; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "AC-3" && format.Profile == "MA";
        }
    }

    /// <summary>
    /// Dolby Digital Plus
    /// </summary>
    public class MICodecEAC3 : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_AC3_PLUS"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.AC3_PLUS_AUDIO; }
        }

        public override string CodecId
        {
            get { return "A_EAC3"; }
        }

        public override string FullName
        {
            get { return "Dolby Digital Plus"; }
        }

        public override string ShortName
        {
            get { return "E-AC-3"; }
        }

        public override string MicroName
        {
            get { return "EAC3"; }
        }

        public override string AltMicroName
        {
            get { return "AC3+"; }
        }

        public override string FullNameDisambig
        {
            get { return string.Format("{0} ({1} / {2})", base.FullNameDisambig, AltMicroName, ShortName); }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsOptionalBlurayCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Enhanced version of AC-3.  Not backwards compatible with regular AC-3.  Typically used for secondary audio (commentary) tracks on Blu-ray discs."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_dolby_digital_plus; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "E-AC-3";
        }
    }

    /// <summary>
    /// Dolby TrueHD
    /// </summary>
    public class MICodecTrueHD : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_TRUEHD"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.AC3_TRUE_HD_AUDIO; }
        }

        public override string CodecId
        {
            get { return "A_TRUEHD"; }
        }

        public override string FullName
        {
            get { return "Dolby TrueHD"; }
        }

        public override string ShortName
        {
            get { return "TrueHD"; }
        }

        public override string MicroName
        {
            get { return "TrueHD"; }
        }

        public override bool Lossless
        {
            get { return true; }
        }

        public override MIAudioCodec Core
        {
            get { return AC3; }
        }

        public override bool IsOptionalBlurayCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Lossless audio encoding with a core AC-3 (Dolby Digital) stream for backwards compatibility with existing AC-3 hardware."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_dolby_truehd; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id.StartsWith("TrueHD");
        }
    }

    #endregion

    #region DTS

    /// <summary>
    /// Standard DTS
    /// </summary>
    public class MICodecDTS : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_DTS"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.DTS_AUDIO; }
        }

        public override string CodecId
        {
            get { return "A_DTS"; }
        }

        public override string FullName
        {
            get { return "DTS Digital Surround"; }
        }

        public override string ShortName
        {
            get { return "DTS"; }
        }

        public override string MicroName
        {
            get { return "DTS"; }
        }

        public override string FullNameDisambig
        {
            get { return string.Format("{0} ({1})", base.FullNameDisambig, ShortName); }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override bool IsOptionalDVDCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Standard DTS.  One of the most common audio codecs for consumer video (Blu-ray and DVD)."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_dts; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "DTS" && string.IsNullOrEmpty(format.Profile);
        }
    }

    /// <summary>
    /// DTS Extended Surround
    /// </summary>
    public class MICodecDTSES : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_DTS_ES"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "A_DTS"; }
        }

        public override string FullName
        {
            get { return "DTS Extended Surround"; }
        }

        public override string ShortName
        {
            get { return "DTS-ES"; }
        }

        public override string MicroName
        {
            get { return "DTS-ES"; }
        }

        public override string FullNameDisambig
        {
            get { return string.Format("{0} ({1})", base.FullNameDisambig, ShortName); }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsOptionalDVDCodec
        {
            get { return true; }
        }

        public override MIAudioCodec Core
        {
            get { return DTS; }
        }

        public override string Description
        {
            get { return "Regular DTS Digital Surround plus an additional discrete or matrix-encoded rear channel.  Backwards compatible with regular DTS."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_dts_es; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "DTS" && format.Profile == "ES";
        }
    }

    /// <summary>
    /// DTS Express
    /// </summary>
    public class MICodecDTSExpress : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_DTS_EXPRESS"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.DTS_HD_SECONDARY_AUDIO; }
        }

        public override string CodecId
        {
            get { return "A_DTS"; }
        }

        public override string FullName
        {
            get { return "DTS Express"; }
        }

        public override string ShortName
        {
            get { return "DTS Express"; }
        }

        public override string MicroName
        {
            get { return "DTS Express"; }
        }

        public override string Description
        {
            get { return "Low bit-rate audio codec used for Blu-ray secondary audio and BD Live.  Might not be muxable with current freely available software."; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsOptionalBlurayCodec
        {
            get { return true; }
        }

        public override bool IsMuxable
        {
            get { return false; }
        }

        public override Image Logo
        {
            get { return Resources.logo_dts_express; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "DTS" && format.Profile == "Express";
        }
    }

    /// <summary>
    /// DTS-HD High Resolution Audio
    /// </summary>
    public class MICodecDTSHDHRA : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_DTS_HD_HRA"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.DTS_HD_AUDIO; }
        }

        public override string CodecId
        {
            get { return "A_DTS"; }
        }

        public override string FullName
        {
            get { return "DTS-HD High Resolution Audio"; }
        }

        public override string ShortName
        {
            get { return "DTS-HD Hi-Res"; }
        }

        public override string MicroName
        {
            get { return "DTS-HD HR"; }
        }

        public override string CommonName
        {
            get { return MicroName; }
        }

        public override IEnumerable<string> AltDisplayNames
        {
            get { return new[] {ShortName}; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override bool IsOptionalBlurayCodec
        {
            get { return true; }
        }

        public override MIAudioCodec Core
        {
            get { return DTS; }
        }

        public override string Description
        {
            get { return "Extension of regular DTS Digital Surround with higher quality.  Contains backwards compatible DTS Digital Surround core."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_dts_hd_hra; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "DTS" && format.Profile == "HRA / Core";
        }
    }

    /// <summary>
    /// DTS-HD Master Audio
    /// </summary>
    public class MICodecDTSHDMA : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_DTS_HD_MA"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.DTS_HD_MASTER_AUDIO; }
        }

        public override string CodecId
        {
            get { return "A_DTS"; }
        }

        public override string FullName
        {
            get { return "DTS-HD Master Audio"; }
        }

        public override string ShortName
        {
            get { return "DTS-HD Master"; }
        }

        public override string MicroName
        {
            get { return "DTS-HD MA"; }
        }

        public override string CommonName
        {
            get { return MicroName; }
        }

        public override bool Lossless
        {
            get { return true; }
        }

        public override bool IsOptionalBlurayCodec
        {
            get { return true; }
        }

        public override MIAudioCodec Core
        {
            get { return DTS; }
        }

        public override string Description
        {
            get { return "Lossless extension to regular DTS Digital Surround.  Contains backwards compatible DTS Digital Surround core."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_dts_hd_ma; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "DTS" && format.Profile == "MA / Core";
        }
    }

    #endregion

    #region Misc Audio

    public class MICodecVorbis : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_VORBIS"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "A_VORBIS"; }
        }

        public override string FullName
        {
            get { return "Ogg Vorbis"; }
        }

        public override string ShortName
        {
            get { return "Vorbis"; }
        }

        public override string MicroName
        {
            get { return "OGA"; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override string Description
        {
            get { return "Free / open source audio codec."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_vorbis; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "Vorbis";
        }
    }

    public class MICodecFLAC : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_FLAC"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "A_FLAC"; }
        }

        public override string FullName
        {
            get { return "Free Lossless Audio Codec"; }
        }

        public override string ShortName
        {
            get { return "FLAC"; }
        }

        public override string MicroName
        {
            get { return "FLAC"; }
        }

        public override bool Lossless
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Open audio format with royalty-free licensing and a free software reference implementation."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_flac; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "FLAC";
        }
    }

    #endregion

    #region MPEG Audio

    // TODO: Investigate "MPEG-1 Audio Layer I", "MPEG-1 Audio Layer II", and "MPEG-2 Audio Layer II (MP2)"

    public class MICodecMP3 : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_MP3"; }
        }

        public override TSStreamType StreamType
        {
            // TODO: Should this return TSStreamType.MPEG1_AUDIO
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "A_MPEG/L3"; }
        }

        public override string FullName
        {
            get { return "MPEG Audio Layer III"; }
        }

        public override string ShortName
        {
            get { return "MPEG Layer 3"; }
        }

        public override string MicroName
        {
            get { return "MP3"; }
        }

        public override string AltFullName
        {
            get { return "MPEG-1 Part 3"; }
        }

        public override string CommonName
        {
            get { return MicroName; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override string Description
        {
            get { return "Audio portion of the MPEG-1 and MPEG-2 standards."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_mp3; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "MPEG Audio" && format.Profile == "Layer 3";
        }
    }

    public class MICodecAAC : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_AAC"; }
        }

        public override TSStreamType StreamType
        {
            // TODO: Should this return TSStreamType.MPEG1_AUDIO
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "A_AAC"; }
        }

        public override string FullName
        {
            get { return "Advanced Audio Coding"; }
        }

        public override string ShortName
        {
            get { return "AAC"; }
        }

        public override string MicroName
        {
            get { return "AAC"; }
        }

        public override string AltFullName
        {
            get { return "MPEG-2 Part 7"; }
        }

        public override string Description
        {
            get { return "Successor to MP3 with better sound quality at similar bit rates."; }
        }

        public override bool Lossless
        {
            get { return false; }
        }

        public override Image Logo
        {
            get { return Resources.logo_aac; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "AAC";
        }
    }

    #endregion

    #region LPCM

    public class MICodecLPCM : MIAudioCodec
    {
        public override string SerializableName
        {
            get { return "A_LPCM"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.LPCM_AUDIO; }
        }

        public override string CodecId
        {
            // TODO: throw new NotImplementedException("Could be either A_PCM/INT/LIT, A_PCM/INT/BIG, or A_PCM/FLOAT/IEEE");
            get { return "A_PCM"; }
        }

        public override string FullName
        {
            get { return "LPCM"; }
        }

        public override string ShortName
        {
            get { return "LPCM"; }
        }

        public override string MicroName
        {
            get { return "PCM"; }
        }

        public override string AltFullName
        {
            get { return "Linear pulse-code modulation"; }
        }

        public override string FullNameDisambig
        {
            get { return string.Format("{0} (uncompressed)", base.FullNameDisambig); }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override bool IsRequiredDVDCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Uncompressed studio-quality audio.  Not directly supported as input from M2TS containers by mkvmerge; must first be demuxed to .WAV files with eac3to or tsMuxeR (see https://trac.bunkus.org/ticket/763)."; }
        }

        public override bool Lossless
        {
            get { return true; }
        }

        public override Image Logo
        {
            get { return Resources.logo_lpcm; }
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "PCM";
        }
    }

    #endregion

    #region Unknown

    public class MICodecUnknownAudio : MISubtitleCodec
    {
        public override string SerializableName
        {
            get { return "A_UNKNOWN"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "A_UNKNOWN"; }
        }

        public override string FullName
        {
            get { return "Unknown Audio Codec"; }
        }

        public override string ShortName
        {
            get { return "Unknown Audio"; }
        }

        public override string MicroName
        {
            get { return "Unknown Audio"; }
        }

        public override string Description
        {
            get { return "Unknown audio format."; }
        }

        public override Image Logo
        {
            get { return null; }
        }

        public override bool IsKnown
        {
            get { return false; }
        }
    }

    #endregion

    #endregion

    #region Subtitle Codecs

    /*
    .sup = S_HDMV/PGS  - Blu-ray
    .idx = S_VOBSUB    - DVD      (.sub = companion file)
    .srt = S_TEXT/UTF8 - Matroska
     */

    public class MICodecPGS : MISubtitleCodec
    {
        public override string SerializableName
        {
            get { return "S_PGS"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.PRESENTATION_GRAPHICS; }
        }

        public override string CodecId
        {
            get { return "S_HDMV/PGS"; }
        }

        public override string FullName
        {
            get { return "Presentation Graphics Stream"; }
        }

        public override string ShortName
        {
            get { return "PGS"; }
        }

        public override string MicroName
        {
            get { return "PGS"; }
        }

        public override bool IsRequiredBlurayCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "The official subtitle format for Blu-ray discs.  Stored as bitmap images rather than plain text to reduce strain on player hardware.  Can be converted to plain text with OCR software (e.g., BDSup2Sub)."; }
        }

        public override Image Logo
        {
            get { return Resources.logo_pgs; }
        }

        public static bool Matches(string filename)
        {
            return new Regex(@"(^|\.)sup$", RegexOptions.IgnoreCase).IsMatch(filename);
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "PGS";
        }
    }

    public class MICodecVobSub : MISubtitleCodec
    {
        public override string SerializableName
        {
            get { return "S_VOBSUB"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "S_VOBSUB"; }
        }

        public override string FullName
        {
            get { return "VobSub"; }
        }

        public override string ShortName
        {
            get { return "VobSub"; }
        }

        public override string MicroName
        {
            get { return "VobSub"; }
        }

        public override bool IsRequiredDVDCodec
        {
            get { return true; }
        }

        public override string Description
        {
            get { return "Official DVD subtitle format.  *.sub file = subtitle bitmaps (images); *.idx file = timecodes (plain text)"; }
        }

        public override Image Logo
        {
            get { return null; }
        }

        public static bool Matches(string filename)
        {
            return new Regex(@"(^|\.)(idx|sup)$", RegexOptions.IgnoreCase).IsMatch(filename);
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "VobSub";
        }
    }

    public class MICodecSRT : MISubtitleCodec
    {
        public override string SerializableName
        {
            get { return "S_SRT"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "S_TEXT/UTF"; }
        }

        public override string FullName
        {
            get { return "SRT"; }
        }

        public override string ShortName
        {
            get { return "SRT"; }
        }

        public override string MicroName
        {
            get { return "SRT"; }
        }

        public override string Description
        {
            get { return "Plain text Matroska subtitle format."; }
        }

        public override Image Logo
        {
            get { return null; }
        }

        public static bool Matches(string filename)
        {
            return new Regex(@"(^|\.)(srt)$", RegexOptions.IgnoreCase).IsMatch(filename);
        }

        public static bool Matches(MIFormat format)
        {
            return format.Id == "UTF-8";
        }
    }

    public class MICodecUnknownSubtitle : MISubtitleCodec
    {
        public override string SerializableName
        {
            get { return "S_UNKNOWN"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "S_UNKNOWN"; }
        }

        public override string FullName
        {
            get { return "Unknown Subtitle Codec"; }
        }

        public override string ShortName
        {
            get { return "Unknown Sub"; }
        }

        public override string MicroName
        {
            get { return "Unknown Sub"; }
        }

        public override string Description
        {
            get { return "Unknown subtitle format."; }
        }

        public override Image Logo
        {
            get { return null; }
        }

        public override bool IsKnown
        {
            get { return false; }
        }
    }

    #endregion

    #region Unknown Codec

    public class MIUnknownCodec : MICodec
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
            get { return false; }
        }

        public override bool IsKnown
        {
            get { return false; }
        }

        public override string TypeDisplay
        {
            get { return "Unknown"; }
        }

        public override string SerializableName
        {
            get { return "UNKNOWN"; }
        }

        public override TSStreamType StreamType
        {
            get { return TSStreamType.Unknown; }
        }

        public override string CodecId
        {
            get { return "UNKNOWN"; }
        }

        public override string FullName
        {
            get { return "Unknown Codec"; }
        }

        public override string ShortName
        {
            get { return "Unknown"; }
        }

        public override string MicroName
        {
            get { return "Unknown"; }
        }

        public override string Description
        {
            get { return "Unknown format."; }
        }

        public override Image Logo
        {
            get { return null; }
        }
    }

    #endregion

}
