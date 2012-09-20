using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO;

namespace BDAutoMuxer.controllers
{
    public class ChapterWriter
    {
        private TSPlaylistFile Playlist;
        private List<ChapterTimeSpan> Chapters;
        private ChapterTimeSpan Duration;
        private double FramesPerSecond;

        public ChapterWriter(TSPlaylistFile playlist)
        {
            this.Playlist = playlist;
            this.Chapters = new List<ChapterTimeSpan>();
            foreach (double d in playlist.Chapters)
            {
                this.Chapters.Add(new ChapterTimeSpan(d));
            }
            this.Duration = new ChapterTimeSpan(playlist.TotalLength);
        }

        public void ChangeFps(double fps)
        {
            List<ChapterTimeSpan> newChapters = new List<ChapterTimeSpan>();

            foreach (ChapterTimeSpan timeSpan in Chapters)
            {
                double frames = timeSpan.TotalSeconds * FramesPerSecond;
                newChapters.Add(new ChapterTimeSpan((long)Math.Round(frames / fps * TimeSpan.TicksPerSecond)));
            }

            double totalFrames = Duration.TotalSeconds * FramesPerSecond;
            Duration = new ChapterTimeSpan((long)Math.Round((totalFrames / fps) * TimeSpan.TicksPerSecond));
            FramesPerSecond = fps;
            Chapters = newChapters;
        }

        public void SaveText(string filename)
        {
            List<string> lines = new List<string>();
            int i = 0;
            foreach (ChapterTimeSpan c in Chapters)
            {
                i++;
                lines.Add("CHAPTER" + i.ToString("00") + "=" + c);
                lines.Add("CHAPTER" + i.ToString("00") + "NAME=" + "");
            }
            File.WriteAllLines(filename, lines.ToArray());
        }

        public void SaveCelltimes(string filename)
        {
            List<string> lines = new List<string>();
            foreach (ChapterTimeSpan c in Chapters)
            {
                lines.Add(((long)Math.Round(c.TotalSeconds * FramesPerSecond)).ToString());
            }
            File.WriteAllLines(filename, lines.ToArray());
        }

        public void SaveTsmuxerMeta(string filename)
        {
            string text = "--custom-" + Environment.NewLine + "chapters=";
            foreach (ChapterTimeSpan c in Chapters)
            {
                text += c + ";";
            }
            text = text.Substring(0, text.Length - 1);
            File.WriteAllText(filename, text);
        }

        public void SaveTimecodes(string filename)
        {
            List<string> lines = new List<string>();
            foreach (ChapterTimeSpan c in Chapters)
            {
                lines.Add(c.ToString());
            }
            File.WriteAllLines(filename, lines.ToArray());
        }

        private class ChapterTimeSpan
        {
            private TimeSpan timeSpan;

            public double TotalSeconds { get { return timeSpan.TotalSeconds; } }

            public ChapterTimeSpan(TimeSpan timeSpan)
            {
                this.timeSpan = timeSpan;
            }

            public ChapterTimeSpan(double d)
            {
                this.timeSpan = new TimeSpan((long)(d * (double)TimeSpan.TicksPerSecond));
            }

            public override string ToString()
            {
                return string.Format(
                        "{0}:{1}:{2}.{3}",
                        timeSpan.Hours.ToString("00"),
                        timeSpan.Minutes.ToString("00"),
                        timeSpan.Seconds.ToString("00"),
                        timeSpan.Milliseconds.ToString("000")
                    );
            }
        }

        /*
        public static readonly XNamespace CgNs = "http://jvance.com/2008/ChapterGrabber";

        public static ChapterInfo Load(XmlReader r)
        {
            XDocument doc = XDocument.Load(r);
            return ChapterInfo.Load(doc.Root);
        }

        public static ChapterInfo Load(string filename)
        {
            XDocument doc = XDocument.Load(filename);
            return ChapterInfo.Load(doc.Root);
        }


        public static ChapterInfo Load(XElement root)
        {
            ChapterInfo ci = new ChapterInfo();
            ci.LangCode = (string)root.Attribute(XNamespace.Xml + "lang");
            ci.Extractor = (string)root.Attribute("extractor");

            if (root.Element(CgNs + "title") != null)
                ci.Title = (string)root.Element(CgNs + "title");

            XElement @ref = root.Element(CgNs + "ref");
            if (@ref != null)
            {
                ci.ChapterSetId = (int?)@ref.Element(CgNs + "chapterSetId");
                ci.ImdbId = (string)@ref.Element(CgNs + "imdbId");
                ci.MovieDbId = (int?)@ref.Element(CgNs + "movieDbId");
            }

            XElement src = root.Element(CgNs + "source");
            if (src != null)
            {
                ci.SourceName = (string)src.Element(CgNs + "name");
                if (src.Element(CgNs + "type") != null)
                    ci.SourceType = (string)src.Element(CgNs + "type");
                ci.SourceHash = (string)src.Element(CgNs + "hash");
                ci.FramesPerSecond = Convert.ToDouble(src.Element(CgNs + "fps").Value, new System.Globalization.NumberFormatInfo());
                ci.Duration = TimeSpan.Parse(src.Element(CgNs + "duration").Value);
            }

            ci.Chapters = root.Element(CgNs + "chapters").Elements(CgNs + "chapter")
              .Select(e => new ChapterEntry() { Name = (string)e.Attribute("name"), Time = TimeSpan.Parse((string)e.Attribute("time")) }).ToList();
            return ci;
        }

        public void Save(string filename)
        {
            ToXElement().Save(filename);
        }

        public void Save(XmlWriter x)
        {
            ToXElement().Save(x);
        }

        public XElement ToXElement()
        {
            var reference = new XElement(CgNs + "ref");
            if (ChapterSetId.HasValue) reference.Add(new XElement(CgNs + "chapterSetId", ChapterSetId));
            if (MovieDbId.HasValue) reference.Add(new XElement(CgNs + "movieDbId", MovieDbId));
            if (ImdbId != null) reference.Add(new XElement(CgNs + "imdbId", ImdbId));

            return new XElement(new XElement(CgNs + "chapterInfo",
              new XAttribute(XNamespace.Xml + "lang", LangCode),
              new XAttribute("version", "2"),
              Extractor != null ? new XAttribute("extractor", Extractor) : null,
              Title != null ? new XElement(CgNs + "title", Title) : null,
              reference.Elements().Count() > 0 ? reference : null,
              new XElement(CgNs + "source",
                new XElement(CgNs + "name", SourceName),
                SourceType != null ? new XElement(CgNs + "type", SourceType) : null,
                new XElement(CgNs + "hash", SourceHash),
                new XElement(CgNs + "fps", FramesPerSecond),
                new XElement(CgNs + "duration", Duration.ToString())),
              new XElement(CgNs + "chapters",
                Chapters.Select(c =>
                  new XElement(CgNs + "chapter",
                    new XAttribute("time", c.Time.ToString()),
                    new XAttribute("name", c.Name))))));
        }

        public void SaveXml(string filename)
        {
            new XDocument(new XElement("Chapters",
              new XElement("EditionEntry",
                new XElement("EditionFlagHidden", "0"),
                new XElement("EditionFlagDefault", "0"),
                //new XElement("EditionUID", "1"),
                Chapters.Select(c =>
                  new XElement("ChapterAtom",
                  new XElement("ChapterDisplay",
                    new XElement("ChapterString", c.Name),
                    new XElement("ChapterLanguage", LangCode == null ? "und" : LangCode)),
                  new XElement("ChapterTimeStart", c.Time.ToString()),
                  new XElement("ChapterFlagHidden", "0"),
                  new XElement("ChapterFlagEnabled", "1")))
                ))).Save(filename);

            //    <Chapters>
            //<EditionEntry>
            //  <EditionFlagHidden>0</EditionFlagHidden>
            //  <EditionFlagDefault>0</EditionFlagDefault>
            //  <EditionUID>62811788</EditionUID>
            //  <ChapterAtom>
            //    <ChapterDisplay>
            //      <ChapterString>Test1</ChapterString>
            //      <ChapterLanguage>und</ChapterLanguage>
            //    </ChapterDisplay>
            //    <ChapterUID>2401693056</ChapterUID>
            //    <ChapterTimeStart>00:01:40.000000000</ChapterTimeStart>
            //    <ChapterFlagHidden>0</ChapterFlagHidden>
            //    <ChapterFlagEnabled>1</ChapterFlagEnabled>
            //  </ChapterAtom>
        }
        */
    }
}
