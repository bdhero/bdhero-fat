﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BDInfo;

namespace BDHero.BDROM
{
    /// <summary>
    /// Contains important files and directories on the BD-ROM necessary for BDHero auto-detection.
    /// </summary>
    public class DiscFileSystem
    {
        /// <summary>
        /// Contains important directories on the BD-ROM.
        /// </summary>
        public DiscDirectories Directories;

        /// <summary>
        /// Contains important files on the BD-ROM necessary for BDHero auto-detection.
        /// </summary>
        public DiscFiles Files;

        /// <summary>
        /// Contains important directories on the BD-ROM.
        /// </summary>
        public class DiscDirectories
        {
            /// <summary>
            /// Root BD-ROM directory.
            /// </summary>
            /// <example>D:\</example>
            public DirectoryInfo Root;

            /// <summary>
            /// BDMV directory.
            /// </summary>
            /// <example>D:\BDMV</example>
            public DirectoryInfo BDMV;

            /// <summary>
            /// Blu-ray Java objects.
            /// </summary>
            /// <example>D:\BDMV\BDJO</example>
            public DirectoryInfo BDJO;

            /// <summary>
            /// Represents a BDMV/CLIPINF/XXXXX.CLPI file that contains information about
            /// the corresponding <see cref="TSStreamFile"/> (BDMV/STREAM/XXXXX.M2TS file).
            /// </summary>
            /// <example>D:\BDMV\CLIPINF</example>
            public DirectoryInfo CLIPINF;

            /// <summary>
            /// Playlists (similar to DVD "titles").
            /// </summary>
            /// <example>D:\BDMV\PLAYLIST</example>
            public DirectoryInfo PLAYLIST;

            /// <summary>
            /// PSP files.
            /// </summary>
            /// <example>D:\SNP</example>
            public DirectoryInfo SNP;

            /// <summary>
            /// Stream files containing tracks.
            /// </summary>
            /// <example>D:\BDMV\STREAM</example>
            public DirectoryInfo STREAM;

            /// <summary>
            /// Interleaved file streams for Blu-ray 3D.
            /// </summary>
            /// <example>D:\BDMV\STREAM\SSIF</example>
            public DirectoryInfo SSIF;

            /// <summary>
            /// Blu-ray metadata XML files.
            /// </summary>
            /// <example>D:\BDMV\META\DL</example>
            public DirectoryInfo BDMT;

            /// <summary>
            /// AACS encryption files, correcting for differences between encrypted, AnyDVD HD backups, and MakeMKV backups.
            /// </summary>
            /// <example><para><c>D:\AACS</c> (encrypted)</para></example>
            /// <example><para><c>D:\ANY!</c> (decrypted - AnyDVD HD)</para></example>
            /// <example><para><c>D:\MakeMKV\AACS</c> (decrypted - MakeMKV)</para></example>
            public DirectoryInfo AACS;

            /// <summary>
            /// <c>ANY!</c> directory created by AnyDVD HD (renamed from <c>AACS</c> by AnyDVD).
            /// </summary>
            public DirectoryInfo ANY;

            /// <summary>
            /// <c>MakeMKV</c> directory created by MakeMKV.
            /// </summary>
            public DirectoryInfo MAKEMKV;
        }

        /// <summary>
        /// Contains important files on the BD-ROM necessary for BDHero auto-detection.
        /// </summary>
        public class DiscFiles
        {
            /// <summary>
            /// AACS <c>mcmf.xml</c> file containing the BD's <see cref="ISAN"/>.
            /// </summary>
            public FileInfo MCMF;

            /// <summary>
            /// D-BOX <c>FilmIndex.xml</c> file.
            /// </summary>
            public FileInfo DBOX;

            /// <summary>
            /// <c>bdmt_xxx.xml</c> files from the <c>BDMV/META/DL</c> directory (<see cref="DiscDirectories.BDMT"/>).
            /// </summary>
            public FileInfo[] BDMT;
        }
    }
}
