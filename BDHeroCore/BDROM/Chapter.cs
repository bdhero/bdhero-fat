﻿using System;
using System.Collections.Generic;
using System.Linq;
using MediaInfoWrapper;

namespace BDHeroCore.BDROM
{
    public class Chapter
    {
        private string _title;

        #region DB Fields

        public int Number { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public Language Language;
        public string Title
        {
            get { return _title ?? "Chapter " + Number; }
            set { _title = value; }
        }

        #endregion

        #region Constructors

        public Chapter()
        {
        }

        public Chapter(int number, double seconds, Language language = null)
        {
            Number = number;
            StartTime = new TimeSpan((long)(seconds * TimeSpan.TicksPerSecond));
            Language = language;
        }

        public Chapter(int number, TimeSpan startTime, Language language = null)
        {
            Number = number;
            StartTime = startTime;
            Language = language;
        }

        #endregion

        #region Non-DB Properties (StartTimeXmlFormat)

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

        #endregion

        #region Transformer

        public static IList<Chapter> Transform(IEnumerable<double> chaptersInSeconds)
        {
            return chaptersInSeconds.Select((t, i) => new Chapter(i + 1, t)).ToList();
        }

        #endregion

        #region JSON Conversion

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

        #endregion
    }
}