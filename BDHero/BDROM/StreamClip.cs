using System;
using System.IO;

namespace BDHero.BDROM
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
        public readonly int AngleIndex;
        public readonly TimeSpan Length;

        #endregion

        #region Constructors

        public StreamClip(FileInfo fileInfo, string filename, ulong filesize, int index, int angleIndex, double lengthSec)
        {
            FileInfo = fileInfo;
            Filename = filename;
            Filesize = filesize;
            Index = index;
            AngleIndex = angleIndex;
            Length = TimeSpan.FromMilliseconds(lengthSec * 1000);
        }

        public StreamClip(FileInfo fileInfo, string filename, ulong filesize, int index, int angleIndex, TimeSpan length)
        {
            FileInfo = fileInfo;
            Filename = filename;
            Filesize = filesize;
            Index = index;
            AngleIndex = angleIndex;
            Length = length;
        }

        #endregion
    }
}
