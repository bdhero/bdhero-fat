using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BDInfo;

namespace BDHero.BDROM
{
    public class DiscFileSystem
    {
        /// <summary>
        /// Root BD-ROM directory.
        /// </summary>
        /// <example>D:\</example>
        public DirectoryInfo DirectoryRoot;

        /// <summary>
        /// BDMV directory.
        /// </summary>
        /// <example>D:\BDMV</example>
        public DirectoryInfo DirectoryBDMV;

        /// <summary>
        /// Blu-ray Java objects.
        /// </summary>
        /// <example>D:\BDMV\BDJO</example>
        public DirectoryInfo DirectoryBDJO;

        /// <summary>
        /// Represents a BDMV/CLIPINF/XXXXX.CLPI file that contains information about
        /// the corresponding <see cref="TSStreamFile"/> (BDMV/STREAM/XXXXX.M2TS file).
        /// </summary>
        /// <example>D:\BDMV\CLIPINF</example>
        public DirectoryInfo DirectoryCLIPINF;

        /// <summary>
        /// Playlists (similar to DVD "titles").
        /// </summary>
        /// <example>D:\BDMV\PLAYLIST</example>
        public DirectoryInfo DirectoryPLAYLIST;

        /// <summary>
        /// PSP files.
        /// </summary>
        /// <example>D:\SNP</example>
        public DirectoryInfo DirectorySNP;

        /// <summary>
        /// Stream files containing tracks.
        /// </summary>
        /// <example>D:\BDMV\STREAM</example>
        public DirectoryInfo DirectorySTREAM;

        /// <summary>
        /// Interleaved file streams for Blu-ray 3D.
        /// </summary>
        /// <example>D:\BDMV\STREAM\SSIF</example>
        public DirectoryInfo DirectorySSIF;

        /// <summary>
        /// Blu-ray metadata XML files.
        /// </summary>
        /// <example>D:\BDMV\META\DL</example>
        public DirectoryInfo DirectoryBDMT;

        /// <summary>
        /// AACS encryption files, correcting for differences between encrypted, AnyDVD HD backups, and MakeMKV backups.
        /// </summary>
        /// <example><para><c>D:\AACS</c> (encrypted)</para></example>
        /// <example><para><c>D:\ANY!</c> (decrypted - AnyDVD HD)</para></example>
        /// <example><para><c>D:\MakeMKV\AACS</c> (decrypted - MakeMKV)</para></example>
        public DirectoryInfo DirectoryAACS;

        /// <summary>
        /// <c>ANY!</c> directory created by AnyDVD HD (renamed from <c>AACS</c> by AnyDVD).
        /// </summary>
        public DirectoryInfo DirectoryANY;

        /// <summary>
        /// <c>MakeMKV</c> directory created by MakeMKV.
        /// </summary>
        public DirectoryInfo DirectoryMAKEMKV;

        /// <summary>
        /// D-BOX <c>FilmIndex.xml</c> file.
        /// </summary>
        public FileInfo FileDBOX;

        /// <summary>
        /// <c>bdmt_xxx.xml</c> files from the <c>BDMV/META/DL</c> directory (<see cref="DirectoryBDMT"/>).
        /// </summary>
        public FileInfo[] FilesBDMT;
    }
}
