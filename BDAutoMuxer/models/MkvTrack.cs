using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BDAutoMuxer.controllers;

namespace BDAutoMuxer.models
{
    public enum MkvBitFlag
    {
        Unspecified,
        Yes,
        No
    }

    public abstract class MkvTrack
    {
        public bool IsAudio { get; protected set; }
        public bool IsVideo { get; protected set; }
        public bool IsSubtitle { get; protected set; }
        public bool IsChapter { get; protected set; }

        public int Id { get; protected set; }
        public string Format { get; protected set; }
        public string FormatInfo { get; protected set; }
        public string FormatProfile { get; protected set; }
        public string CodecId { get; protected set; }
        public TSStreamType StreamType { get; protected set; }
        public string OldTitle { get; protected set; }

        public string CodecName
        {
            get
            {
                switch (StreamType)
                {
                    case TSStreamType.MPEG1_VIDEO:
                        return "MPEG-1";
                    case TSStreamType.MPEG2_VIDEO:
                        return "MPEG-2";
                    case TSStreamType.AVC_VIDEO:
                        return "MPEG-4 AVC";
                    case TSStreamType.MVC_VIDEO:
                        return "MPEG-4 MVC";
                    case TSStreamType.VC1_VIDEO:
                        return "VC-1";
                    case TSStreamType.MPEG1_AUDIO:
                        return "MP1";
                    case TSStreamType.MPEG2_AUDIO:
                        return "MP2";
                    case TSStreamType.LPCM_AUDIO:
                        return "LPCM";
                    case TSStreamType.AC3_AUDIO:
                        return "Dolby Digital";
                    case TSStreamType.AC3_PLUS_AUDIO:
                    case TSStreamType.AC3_PLUS_SECONDARY_AUDIO:
                        return "Dolby Digital Plus";
                    case TSStreamType.AC3_TRUE_HD_AUDIO:
                        return "Dolby TrueHD";
                    case TSStreamType.DTS_AUDIO:
                        return "DTS";
                    case TSStreamType.DTS_HD_AUDIO:
                        return "DTS-HD High-Res";
                    case TSStreamType.DTS_HD_SECONDARY_AUDIO:
                        return "DTS Express";
                    case TSStreamType.DTS_HD_MASTER_AUDIO:
                        return "DTS-HD Master";
                    case TSStreamType.PRESENTATION_GRAPHICS:
                        return "Presentation Graphics";
                    case TSStreamType.INTERACTIVE_GRAPHICS:
                        return "Interactive Graphics";
                    case TSStreamType.SUBTITLE:
                        return "Subtitle";
                    default:
                        return "UNKNOWN";
                }
            }
        }
        public string CodecNameShort
        {
            get
            {

                switch (StreamType)
                {
                    case TSStreamType.MPEG1_VIDEO:
                        return "MPEG-1";
                    case TSStreamType.MPEG2_VIDEO:
                        return "MPEG-2";
                    case TSStreamType.AVC_VIDEO:
                        return "AVC";
                    case TSStreamType.MVC_VIDEO:
                        return "MVC";
                    case TSStreamType.VC1_VIDEO:
                        return "VC-1";
                    case TSStreamType.MPEG1_AUDIO:
                        return "MP1";
                    case TSStreamType.MPEG2_AUDIO:
                        return "MP2";
                    case TSStreamType.LPCM_AUDIO:
                        return "LPCM";
                    case TSStreamType.AC3_AUDIO:
                        return "AC3";
                    case TSStreamType.AC3_PLUS_AUDIO:
                    case TSStreamType.AC3_PLUS_SECONDARY_AUDIO:
                        return "AC3+";
                    case TSStreamType.AC3_TRUE_HD_AUDIO:
                        return "TrueHD";
                    case TSStreamType.DTS_AUDIO:
                        return "DTS";
                    case TSStreamType.DTS_HD_AUDIO:
                        return "DTS-HD HR";
                    case TSStreamType.DTS_HD_SECONDARY_AUDIO:
                        return "DTS Express";
                    case TSStreamType.DTS_HD_MASTER_AUDIO:
                        return "DTS-HD MA";
                    case TSStreamType.PRESENTATION_GRAPHICS:
                        return "PGS";
                    case TSStreamType.INTERACTIVE_GRAPHICS:
                        return "IGS";
                    case TSStreamType.SUBTITLE:
                        return "SUB";
                    default:
                        return "UNKNOWN";
                }
            }
        }

        public string NewTitle;
        public string Language;

        public MkvBitFlag IsDefault;
        public MkvBitFlag IsForced;

        private static readonly Regex TrackRegex = new Regex("<track\\s+type=\"(Audio|Video|Text|Menu)\"[^>]*>.*?</track>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        public static ICollection<MkvTrack> ParseMediaInfo(string mediaInfo)
        {
            // TODO: Is this check even necessary?
            if (!TrackRegex.IsMatch(mediaInfo))
                return new List<MkvTrack>();

            return
                TrackRegex.Matches(mediaInfo).Cast<Match>()
                          .Select(match => ParseTrack(match.Value, match.Groups[1].Value)).Where(track => track != null).ToList();
        }

        private static MkvTrack ParseTrack(string xml, string type)
        {
            var typeLower = type.ToLowerInvariant();
            if (typeLower == "audio")
                return MkvAudioTrack.Parse(xml);
            if (typeLower == "video")
                return MkvVideoTrack.Parse(xml);
            if (typeLower == "text")
                return MkvSubtitleTrack.Parse(xml);
            if (typeLower == "menu")
                return MkvChapterTrack.Parse(xml);
            return null;
        }

        protected static string GetString(string xml, string tagName, Regex regex = null)
        {
            var matcher = new Regex(string.Format("<{0}>(.*?)</{0}>", tagName), RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var value = matcher.IsMatch(xml) ? matcher.Match(xml).Groups[1].Value : null;

            if (value != null && regex != null && regex.IsMatch(value))
            {
                var match = regex.Match(value);
                return match.Groups.Count > 0 ? match.Groups[1].Value : match.Value;
            }

            return value;
        }

        protected static int GetInt(string xml, string tagName, Regex regex = null)
        {
            return ParseInt(GetString(xml, tagName, regex));
        }

        protected static double GetDouble(string xml, string tagName, Regex regex = null)
        {
            return ParseDouble(GetString(xml, tagName, regex));
        }

        protected static MkvBitFlag GetBitFlag(string xml, string tagName, Regex regex = null)
        {
            return ParseBitFlag(GetString(xml, tagName, regex));
        }

        protected static int ParseInt(string value)
        {
            int i;
            int.TryParse(value ?? "-1", out i);
            return i;
        }

        protected static double ParseDouble(string value)
        {
            double d;
            double.TryParse(value ?? "0", out d);
            return d;
        }

        protected static MkvBitFlag ParseBitFlag(string value)
        {
            var lower = (value ?? "").ToLowerInvariant();
            return lower == "yes" ? MkvBitFlag.Yes : (lower == "no" ? MkvBitFlag.No : MkvBitFlag.Unspecified);
        }

        protected static readonly IDictionary<string, double> SizeScales = new Dictionary<string, double>()
        {
            { "kib", 1024 },
            { "mib", 1048576 },
            { "gib", 1073741824 },
            { "tib", 1099511627776 }
        }; 

        protected static double ParseSize(string value, string scale)
        {
            var key = (scale ?? "").ToLowerInvariant();
            var multiplier = SizeScales.ContainsKey(key) ? SizeScales[key] : 1;
            return ParseDouble(value) * multiplier;
        }

        protected static void ParseCommon(MkvTrack track, string xml)
        {
            track.Id = GetInt(xml, "ID_String");
            track.Format = GetString(xml, "Format");
            track.FormatInfo = GetString(xml, "Format_Info");
            track.FormatProfile = GetString(xml, "Format_Profile");
            track.CodecId = GetString(xml, "CodecID");
            track.OldTitle = GetString(xml, "Title");
            track.NewTitle = GetString(xml, "Title");
            track.Language = GetString(xml, "Language_String");
            track.IsDefault = GetBitFlag(xml, "Default_String");
            track.IsForced = GetBitFlag(xml, "Forced_String");
        }
    }

    public class MkvAudioVideoTrack : MkvTrack
    {
        protected static readonly Regex StreamSizeRegex = new Regex(@"([0-9\.]+) ([a-zA-Z]{3}) \((\d+)%\)", RegexOptions.IgnoreCase);

        public string Duration { get; protected set; }
        public string Bitrate { get; protected set; }
        public double StreamSize { get; protected set; }
        public double StreamSizePercent { get; protected set; }
        
        protected static void ParseAudioVideo(MkvAudioVideoTrack track, string xml)
        {
            track.Duration = GetString(xml, "Duration_String");
            track.Bitrate = GetString(xml, "BitRate_String");

            var strStreamSize = GetString(xml, "StreamSize_String");
            if (strStreamSize != null && StreamSizeRegex.IsMatch(strStreamSize))
            {
                var streamSizeMatch = StreamSizeRegex.Match(strStreamSize);
                track.StreamSize = ParseSize(streamSizeMatch.Groups[1].Value, streamSizeMatch.Groups[2].Value);
                track.StreamSizePercent = ParseDouble(streamSizeMatch.Groups[3].Value) * .01;
            }
        }
    }

    public class MkvAudioTrack : MkvAudioVideoTrack
    {
        private static readonly Regex ChannelsRegex = new Regex(@"([0-9\.]+) channels", RegexOptions.IgnoreCase);
        
        public string BitrateMode { get; protected set; }
        public double Channels { get; protected set; }
        public string ChannelPositions { get; protected set; }
        public string SamplingRate { get; protected set; }
        public string BitDepth { get; protected set; }
        public string CompressionMode { get; protected set; }

        public MkvAudioTrack()
        {
            IsAudio = true;
        }

        public static MkvAudioTrack Parse(string xml)
        {
            var track = new MkvAudioTrack();

            ParseCommon(track, xml);
            ParseAudioVideo(track, xml);

            track.BitrateMode = GetString(xml, "BitRate_Mode_String");
            track.Channels = GetDouble(xml, "Channel_s__String", ChannelsRegex);
            track.ChannelPositions = GetString(xml, "ChannelPositions");
            track.SamplingRate = GetString(xml, "SamplingRate_String");
            track.BitDepth = GetString(xml, "BitDepth_String");
            track.CompressionMode = GetString(xml, "Compression_Mode_String");

            if (track.Channels == 6)
                track.Channels = 5.1;
            else if (track.Channels == 8)
                track.Channels = 7.1;

            switch (track.Format)
            {
                case "AC-3":
                    track.StreamType = TSStreamType.AC3_AUDIO;
                    break;
                case "TrueHD / AC-3":
                    track.StreamType = TSStreamType.AC3_TRUE_HD_AUDIO;
                    break;
                case "DTS":
                    switch (track.FormatProfile)
                    {
                        case "MA / Core":
                            track.StreamType = TSStreamType.DTS_HD_MASTER_AUDIO;
                            break;
                        default:
                            track.StreamType = TSStreamType.DTS_AUDIO;
                            break;
                    }
                    break;
                case "PCM":
                    track.StreamType = TSStreamType.LPCM_AUDIO;
                    break;
            }

            return track;
        }
    }

    public class MkvVideoTrack : MkvAudioVideoTrack
    {
        private static readonly Regex DimensionRegex = new Regex(@"([0-9\s]+) pixels", RegexOptions.IgnoreCase);

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public string AspectRatio { get; protected set; }
        public string FrameRateMode { get; protected set; }
        public string FrameRate { get; protected set; }
        public string ColorSpace { get; protected set; }
        public string ScanType { get; protected set; }

        public MkvVideoTrack()
        {
            IsVideo = true;
        }

        public static MkvVideoTrack Parse(string xml)
        {
            var track = new MkvVideoTrack();

            ParseCommon(track, xml);
            ParseAudioVideo(track, xml);

            var width = GetString(xml, "Width_String", DimensionRegex);
            var height = GetString(xml, "Height_String", DimensionRegex);

            if (width != null)
                track.Width = ParseInt(width.Replace(" ", ""));

            if (height != null)
                track.Height = ParseInt(height.Replace(" ", ""));

            track.AspectRatio = GetString(xml, "DisplayAspectRatio_String");
            track.FrameRateMode = GetString(xml, "FrameRate_Mode_String");
            track.FrameRate = GetString(xml, "FrameRate_String");
            track.ColorSpace = GetString(xml, "ColorSpace");
            track.ScanType = GetString(xml, "ScanType_String");

            switch (track.Format)
            {
                case "AVC":
                    track.StreamType = TSStreamType.AVC_VIDEO;
                    break;
                case "VC-1":
                    track.StreamType = TSStreamType.VC1_VIDEO;
                    break;
            }

            return track;
        }
    }

    public class MkvSubtitleTrack : MkvTrack
    {
        public MkvSubtitleTrack()
        {
            IsSubtitle = true;
        }

        public static MkvSubtitleTrack Parse(string xml)
        {
            var track = new MkvSubtitleTrack();

            ParseCommon(track, xml);

            if (track.Format == "PGS")
                track.StreamType = TSStreamType.PRESENTATION_GRAPHICS;

            return track;
        }
    }

    public class MkvChapterTrack : MkvTrack
    {
        public MkvChapterTrack()
        {
            IsChapter = true;
        }

        public static MkvChapterTrack Parse(string xml)
        {
            return new MkvChapterTrack();
        }
    }
}
