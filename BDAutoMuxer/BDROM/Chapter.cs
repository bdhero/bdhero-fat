﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDAutoMuxer.BDInfo;

namespace BDAutoMuxer.BDROM
{
    public class Chapter
    {
        public int Number { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public Language Language;
        public string Title;

        public Chapter()
        {
        }

        public Chapter(int number, double value, Language language = null)
        {
            Number = number;
            StartTime = new TimeSpan((long)(value * TimeSpan.TicksPerSecond));
            Language = language;
        }

        /// <summary>
        /// StartTime in Matroska XML format (e.g., "HH:MM:SS:mmm"
        /// </summary>
        public string StartTimeXmlFormat
        {
            get
            {
                return string.Format(
                        "{0}:{1}:{2}.{3}",
                        StartTime.Hours.ToString("00"),
                        StartTime.Minutes.ToString("00"),
                        StartTime.Seconds.ToString("00"),
                        StartTime.Milliseconds.ToString("000")
                    );
            }
        }

        public static IList<Chapter> Transform(IEnumerable<double> doubles)
        {
            return doubles.Select((t, i) => new Chapter(i + 1, t)).ToList();
        }

        public Json ToJsonObject()
        {
            return new Json
                       {
                           start = StartTime,
                           iso639_2 = Language.ISO_639_2,
                           title = Title
                       };
        }

        public class Json
        {
            public TimeSpan start;
            public string iso639_2;
            public string title;

            public Chapter ToChapter()
            {
                return new Chapter {StartTime = start, Language = Language.FromCode(iso639_2), Title = title};
            }
        }
    }
}