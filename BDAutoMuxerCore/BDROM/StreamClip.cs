using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BDAutoMuxerCore.BDInfo;

namespace BDAutoMuxerCore.BDROM
{
    /// <summary>
    /// Represents a .M2TS file on a BD-ROM.
    /// </summary>
    public class StreamClip
    {
        #region Readonly fields

        public readonly FileInfo FileInfo;
        public readonly String Filename;
        public readonly ulong Filesize;
        public readonly int Index;
        public readonly TimeSpan Length;

        #endregion

        #region Constructors

        public StreamClip(FileInfo fileInfo, string filename, ulong filesize, int index, double lengthSec)
        {
            FileInfo = fileInfo;
            Filename = filename;
            Filesize = filesize;
            Index = index;
            Length = TimeSpan.FromMilliseconds(lengthSec * 1000);
        }

        public StreamClip(FileInfo fileInfo, string filename, ulong filesize, int index, TimeSpan length)
        {
            FileInfo = fileInfo;
            Filename = filename;
            Filesize = filesize;
            Index = index;
            Length = length;
        }

        #endregion

        #region Transformers

        public static List<StreamClip> Transform(IEnumerable<TSStreamClip> tsStreamClips)
        {
            return tsStreamClips.Select(Transform).ToList();
        }

        private static StreamClip Transform(TSStreamClip tsStreamClip, int index)
        {
            return new StreamClip(tsStreamClip.StreamFile.FileInfo, tsStreamClip.Name, tsStreamClip.FileSize, index, tsStreamClip.Length);
        }

        #endregion
    }
}
