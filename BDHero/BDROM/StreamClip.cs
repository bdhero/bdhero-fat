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
    }
}
