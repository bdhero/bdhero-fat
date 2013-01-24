using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using BDAutoMuxerCore.Annotations;
using BDAutoMuxerCore.BDROM;
using DotNetUtils;
using MediaInfoWrapper;
using ProcessUtils;

namespace BDAutoMuxerCore.Tools
{
    public class MkvPropEdit : NonInteractiveProcess
    {
        private const string MkvPropEditFilename = "mkvpropedit.exe";

        [NotNull]
        private readonly string _mkvFilePath;

        [CanBeNull]
        private readonly string _movieTitle;

        private readonly List<MkvTrackProps> _trackProps;
        private readonly List<Chapter> _chapters;

        public MkvPropEdit(
            [NotNull] string mkvFilePath,
            [CanBeNull] string movieTitle,
            [CanBeNull] Image coverArt,
            IEnumerable<MkvTrackProps> trackProps,
            IEnumerable<Chapter> chapters)
        {
            _mkvFilePath = mkvFilePath;
            _movieTitle = movieTitle;
            _trackProps = trackProps.ToList();
            _chapters = chapters.ToList();

            ExePath = AssemblyUtils.GetTempFilePath(GetType(), MkvPropEditFilename);
            ExtractResources();

            Arguments.Add(_mkvFilePath);

            var coverImagePathLarge = ResizeCoverArt(coverArt, CoverArtSize.Large, "cover.jpg");
            var coverImagePathSmall = ResizeCoverArt(coverArt, CoverArtSize.Small, "small_cover.jpg");

            if (coverImagePathLarge != null)
                Arguments.AddAll("--add-attachment", coverImagePathLarge);

            if (coverImagePathSmall != null)
                Arguments.AddAll("--add-attachment", coverImagePathSmall);

            var chapterXmlPath = SaveChapters(_chapters);

            Arguments.AddAll("--chapters", chapterXmlPath);
            Arguments.AddAll("--edit", "segment_info");

            if (!string.IsNullOrWhiteSpace(_movieTitle))
                Arguments.AddAll("--set", "title=" + _movieTitle);

            for (var i = 0; i < _trackProps.Count; i++)
            {
                var track = _trackProps[i];

                Arguments.AddAll("--edit", "track:" + (i + 1));
                Arguments.AddAll("--set", "name=" + track.Name);

                if (track.Default.HasValue)
                    Arguments.AddAll("--set", "flag-default=" + (track.Default == true ? 1 : 0));
                else
                    Arguments.AddAll("--delete", "flag-default");

                if (track.Forced.HasValue)
                    Arguments.AddAll("--set", "flag-forced=" + (track.Forced == true ? 1 : 0));
                else
                    Arguments.AddAll("--delete", "flag-forced");

                Arguments.AddAll("--set", "language=" + track.Language.ISO_639_2);
            }
        }

        /// <summary>
        /// Saves the given <paramref name="chapters"/> in Matroska XML format to a temporary file and returns the path to the file.
        /// </summary>
        /// <param name="chapters"></param>
        /// <returns>Full, absolute path to the chapter XML file</returns>
        [NotNull]
        private static string SaveChapters(IEnumerable<Chapter> chapters)
        {
            var path = AssemblyUtils.GetTempFilePath(typeof(MkvPropEdit), "chapters.xml");
            ChapterWriterV2.SaveAsXml(chapters, path);
            return path;
        }

        /// <summary>
        /// Resizes the cover art image to the appropriate dimensions and saves it to a temporary file with the given filename.
        /// </summary>
        /// <param name="image">Full size cover art image from TMDb.  If <paramref name="image"/> is null, this method will return null.</param>
        /// <param name="width">120 for small or 600 for large</param>
        /// <param name="filename">cover.{jpg,png} or small_cover.{jpg,png}</param>
        /// <returns>Full, absolute path to the resized image on disk if <paramref name="image"/> is not null; otherwise null.</returns>
        [NotNull]
        private string ResizeCoverArt([CanBeNull] Image image, CoverArtSize width, [NotNull] string filename)
        {
            if (image == null) return null;
            var ext = Path.GetExtension(filename.ToLowerInvariant());
            var format = ext == ".png" ? ImageFormat.Png : ImageFormat.Jpeg;
            var path = AssemblyUtils.GetTempFilePath(GetType(), filename);
            ScaleImage(image, (int)width, int.MaxValue).Save(path, format);
            return path;
        }

        [NotNull]
        private static Image ScaleImage([NotNull] Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public static void Test(string mkvFilePath = null)
        {
            mkvFilePath = mkvFilePath ?? @"Y:\BDAM\out\progress\BLACK_HAWK_DOWN_00000.mpls.propedit2.mkv";
            var movieTitle = "Black Hawk Down";
            var coverArt = Image.FromFile(@"Y:\BDAM\cover-art\black-hawk-down\full.jpg");
            var language = Language.FromCode("eng");
            var trackProps = new List<MkvTrackProps>
                                 {
                                     new MkvTrackProps { Name = "1080p MPEG-2", Language = language, Default = true, Forced = false },
                                     new MkvTrackProps { Name = "Main Movie: AC-3 (5.1 ch)", Language = language, Default = false, Forced = false },
                                     new MkvTrackProps { Name = "Main Movie: PCM (5.1 ch)", Language = language, Default = true, Forced = false },
                                     new MkvTrackProps { Name = "Commentary: AC-3 (2.0 ch)", Language = language, Default = false, Forced = false },
                                     new MkvTrackProps { Name = "Commentary: AC-3 (2.0 ch)", Language = language, Default = false, Forced = false },
                                     new MkvTrackProps { Name = "Commentary: AC-3 (2.0 ch)", Language = language, Default = false, Forced = false },
                                     new MkvTrackProps { Name = "Main Movie: HDMV PGS", Language = language, Default = true, Forced = false },
                                     new MkvTrackProps { Name = "Main Movie: HDMV PGS", Language = language, Default = false, Forced = false },
                                     new MkvTrackProps { Name = "Unknown: HDMV PGS", Language = language, Default = false, Forced = false }
                                 };
            var chapters = new List<Chapter>
                               {
                                   new Chapter(1, TimeSpan.FromMinutes(5)),
                                   new Chapter(2, TimeSpan.FromMinutes(10)),
                                   new Chapter(3, TimeSpan.FromMinutes(15)),
                                   new Chapter(4, TimeSpan.FromMinutes(20)),
                                   new Chapter(5, TimeSpan.FromMinutes(25))
                               };
            var mkvPropEdit = new MkvPropEdit(mkvFilePath, movieTitle, coverArt, trackProps, chapters);
            mkvPropEdit.Start();
        }

        private void ExtractResources()
        {
            try
            {
                File.WriteAllBytes(ExePath, BinTools.mkvpropedit);
            }
            catch { }
        }
    }

    public enum CoverArtSize
    {
        Small = 120,
        Large = 600
    }

    public class MkvTrackProps
    {
        public string Name;
        public bool? Default;
        public bool? Forced;
        public Language Language;
    }
}
