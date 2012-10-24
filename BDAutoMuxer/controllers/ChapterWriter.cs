﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Text;

namespace BDAutoMuxer.controllers
{
    public class ChapterWriter
    {
        private List<ChapterTimeSpan> _chapters;
        private ChapterTimeSpan _duration;
        private double _framesPerSecond;

        public ChapterWriter(TSPlaylistFile playlist)
        {
            _chapters = playlist.Chapters.Select(d => new ChapterTimeSpan(d)).ToList();
            _duration = new ChapterTimeSpan(playlist.TotalLength);
        }

        public void ChangeFps(double newFps)
        {
            _chapters = _chapters.Select(cts => ChangeFps(cts, newFps)).ToList();
            _duration = ChapterTimeSpan.FromFrameRate(_duration.TotalSeconds, _framesPerSecond, newFps);
            _framesPerSecond = newFps;
        }

        private ChapterTimeSpan ChangeFps(ChapterTimeSpan timeSpan, double fps)
        {
            return ChapterTimeSpan.FromFrameRate(timeSpan.TotalSeconds, _framesPerSecond, fps);
        }

        public void SaveText(string filename)
        {
            var lines = new List<string>();
            var i = 0;
            foreach (var c in _chapters)
            {
                i++;
                var istr = i.ToString("00");
                lines.Add("CHAPTER" + istr + "=" + c);
                lines.Add("CHAPTER" + istr + "NAME=" + "Chapter " + istr);
            }
            File.WriteAllLines(filename, lines.ToArray());
        }

        public void SaveXML(string filename)
        {
            const string lng = "eng";
            var i = 0;

            var writer = new XmlTextWriter(filename, Encoding.GetEncoding("ISO-8859-1"));
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
                writer.WriteDocType("Chapters", null, "matroskachapters.dtd", null);
                writer.WriteStartElement("Chapters");
                    foreach (var c in _chapters)
                    {
                        i++;
                        var istr = i.ToString("00");
                        writer.WriteStartElement("EditionEntry");
                            writer.WriteStartElement("ChapterAtom");
                                writer.WriteStartElement("ChapterTimeStart");
                                writer.WriteString(c.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("ChapterDisplay");
                                    writer.WriteStartElement("ChapterString");
                                    writer.WriteString("Chapter " + istr);
                                    writer.WriteEndElement();
                                    writer.WriteStartElement("ChapterLanguage");
                                    writer.WriteString(lng);
                                    writer.WriteEndElement();
                                writer.WriteEndElement();
                            writer.WriteEndElement();
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                writer.WriteEndDocument();
            writer.Close();  
        }

        public void SaveCellTimes(string filename)
        {
            File.WriteAllLines(filename, _chapters.Select(GetCellTime).ToArray());
        }

        private string GetCellTime(ChapterTimeSpan c)
        {
            return ((long)Math.Round(c.TotalSeconds * _framesPerSecond)).ToString(CultureInfo.InvariantCulture);
        }

        public void SaveTsMuxerMeta(string filename)
        {
            var text = "--custom-" + Environment.NewLine + "chapters=";
            text = _chapters.Aggregate(text, (current, c) => current + (c + ";"));
            text = text.Substring(0, text.Length - 1);
            File.WriteAllText(filename, text);
        }

        public void SaveTimecodes(string filename)
        {
            File.WriteAllLines(filename, _chapters.Select(c => c.ToString()).ToArray());
        }

        private class ChapterTimeSpan
        {
            private TimeSpan _timeSpan;

            public double TotalSeconds { get { return _timeSpan.TotalSeconds; } }

            public ChapterTimeSpan(double d)
            {
                _timeSpan = new TimeSpan((long)(d * TimeSpan.TicksPerSecond));
            }

            public override string ToString()
            {
                return string.Format(
                        "{0}:{1}:{2}.{3}",
                        _timeSpan.Hours.ToString("00"),
                        _timeSpan.Minutes.ToString("00"),
                        _timeSpan.Seconds.ToString("00"),
                        _timeSpan.Milliseconds.ToString("000")
                    );
            }

            public static ChapterTimeSpan FromFrameRate(double seconds, double oldFps, double newFps)
            {
                return FromFrameRate(seconds * oldFps, newFps);
            }

            public static ChapterTimeSpan FromFrameRate(double frames, double newFps)
            {
                return new ChapterTimeSpan((long)Math.Round(frames / newFps * TimeSpan.TicksPerSecond));
            }
        }
    }
}
