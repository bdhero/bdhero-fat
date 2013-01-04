﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDAutoMuxer.BDInfo;

namespace BDAutoMuxer.BDROM
{
    public class Chapter
    {
        public TimeSpan Start { get; private set; }
        public Language Language;
        public string Title;

        public Json ToJson()
        {
            return new Json
                       {
                           start = Start,
                           iso639_2 = Language.ISO_639_2,
                           title = Title
                       };
        }

        public class Json
        {
            public TimeSpan start;
            public string iso639_2;
            public string title;

            public Chapter FromJson()
            {
                return new Chapter {Start = start, Language = Language.GetLanguage(iso639_2), Title = title};
            }
        }
    }
}
