﻿using System;
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

        /// <summary>
        /// Gets or sets the path to the source Matroska file that will be modified by MkvPropEdit.
        /// The setter automatically adds the source path to the list of <see cref="NonInteractiveProcess.Arguments"/>;
        /// you should not add it manually.
        /// </summary>
        public string SourceFilePath
        {
            get { return _sourceFilePath; }
            set
            {
                if (_sourceFilePath != null)
                    throw new InvalidOperationException("Source file path cannot be set more than once.");
                if (Arguments.Any())
                    throw new InvalidOperationException("Source file must be the first argument in the list; another agument is already present.");
                _sourceFilePath = value;
                Arguments.Add(_sourceFilePath);
            }
        }
        private string _sourceFilePath;

        public MkvPropEdit()
        {
            ExePath = ExtractExe();
        }

        public MkvPropEdit SetChapters(IEnumerable<Chapter> chapters)
        {
            var chapterXmlPath = SaveChaptersToXml(chapters);
            Arguments.AddAll("--chapters", chapterXmlPath);
            return this;
        }

        public MkvPropEdit RemoveAllTags()
        {
            Arguments.AddAll("--tags", "all:");
            return this;
        }

        /// <summary>
        /// Automatically sets the "default track" flag to <code>true</code> for the first track of each type (video, audio, and subtitle),
        /// and the remaining tracks to <code>false</code>.
        /// </summary>
        public MkvPropEdit SetDefaultTracksAuto(List<Track> selectedTracks)
        {
            var numVideoTracks = selectedTracks.Count(track => track.IsVideo);
            var numAudioTracks = selectedTracks.Count(track => track.IsAudio);
            var numSubtitleTracks = selectedTracks.Count(track => track.IsSubtitle);

            for (var i = 1; i <= numVideoTracks; i++)
                SetDefaultTrackFlag("v", i, i == 1);
            for (var i = 1; i <= numAudioTracks; i++)
                SetDefaultTrackFlag("a", i, i == 1);
            for (var i = 1; i <= numSubtitleTracks; i++)
                SetDefaultTrackFlag("s", i, i == 1);

            return this;
        }

        /// <summary>
        /// Sets the "default track" flag of the specified track.
        /// </summary>
        /// <param name="trackType">"v", "a", or "s" for video, audio, and subtitle</param>
        /// <param name="indexOfType"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        public MkvPropEdit SetDefaultTrackFlag(string trackType, int indexOfType, bool isDefault)
        {
            Arguments.AddAll("--edit",
                             string.Format("track:{0}{1}", trackType, indexOfType),
                             "--set",
                             string.Format("flag-default={0}", isDefault ? 1 : 0));
            return this;
        }

        public MkvPropEdit AddAttachment([NotNull] string attachmentFilePath)
        {
            Arguments.AddAll("--add-attachment", attachmentFilePath);
            return this;
        }

        public MkvPropEdit DeleteAttachment()
        {
            Arguments.AddAll("--delete-attachment", "mime-type:image/jpeg");
            return this;
        }

        public MkvPropEdit AddCoverArt([CanBeNull] Image coverArt)
        {
            var coverImagePathLarge = ResizeCoverArt(coverArt, CoverArtSize.Large, "cover.jpg");
            var coverImagePathSmall = ResizeCoverArt(coverArt, CoverArtSize.Small, "small_cover.jpg");

            if (coverImagePathLarge != null)
                AddAttachment(coverImagePathLarge);

            if (coverImagePathSmall != null)
                AddAttachment(coverImagePathSmall);

            return this;
        }

        public MkvPropEdit SetMovieTitle([CanBeNull] string movieTitle)
        {
            if (!string.IsNullOrWhiteSpace(movieTitle))
                Arguments.AddAll("--edit", "segment_info", "--set", "title=" + movieTitle);
            return this;
        }

        public MkvPropEdit SetTrackProps(List<MkvTrackProps> trackProps)
        {
            for (var i = 0; i < trackProps.Count; i++)
            {
                var track = trackProps[i];

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

            return this;
        }

        /// <summary>
        /// Saves the given <paramref name="chapters"/> in Matroska XML format to a temporary file and returns the path to the file.
        /// </summary>
        /// <param name="chapters"></param>
        /// <returns>Full, absolute path to the chapter XML file</returns>
        [NotNull]
        private static string SaveChaptersToXml(IEnumerable<Chapter> chapters)
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
            var mkvPropEdit = new MkvPropEdit { SourceFilePath = mkvFilePath }
                .SetMovieTitle(movieTitle)
                .AddCoverArt(coverArt)
                .SetChapters(chapters)
                .SetTrackProps(trackProps);
            mkvPropEdit.Start();
        }

        private string ExtractExe()
        {
            var path = AssemblyUtils.GetTempFilePath(GetType(), MkvPropEditFilename);
            try { File.WriteAllBytes(path, BinTools.mkvpropedit_exe); } catch { }
            return path;
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
