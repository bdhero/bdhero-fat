# BDAutoMuxer

## What Is BDAutoMuxer?

BDAutoMuxer is an intelligent, automatic tool for detecting main movie playlist(s) on Blu-ray Discs™ and muxing them to M2TS / MKV containers.

## Our Goal

Our goal is simple:

1.  Pick the **right** playlist **every time** with minimal effort
2.  Make it easy for users to rip only the languages, subtitles, and audio codecs they want
3.  Never require manual intervention from more than one user for a given disc

## The Problem

From a technical perspective, the Blu-ray Disc™ standard is a total disaster.  It's arbitrarily and unnecessarily complex and gives third party tools almost no useful information to work with.

The major issues we intend to solve are:

1.  **Confusing Playlists**

    Modern Blu-rays contain numerous _almost_-identical playlists that have the same run time and file size, but with slight non-obvious variations, making it difficult (not to mention time-consuming) to identify the correct playlist(s) to rip.  "The Hunger Games", for instance, has _**62**_ different movie-length playlists -- but only _**one**_ of them is actually the main movie!

2.  **Multiple Video Languages**

    Animated films (particularly Disney movies) often have 3-4 nearly-identical playlists containing the same audio and subtitle tracks, but with brief video sequences rendered in different languages (e.g., the opening title sequence, newspaper headlines).  Inexperienced individuals may simply pick one playlist at random and rip it, only to discover the next day that they wasted 12 hours encoding the French render of "Toy Story" instead of the English one.

3.  **No Meta Information**

    The icing on the steamy pile of poo cake that is the Blu-ray Disc™ standard is the fact that there is absolutely _**zero**_ meta information _anywhere_ on the disc to indicate:

    1. Which playlist is the main movie
    2. The video render language
    3. The purpose of each audio track (primary audio, commentary, SAP)
    4. The "cut" or edition of the film (e.g., theatrical, special, extended)
    5. Chapter names
    6. Even the name and year of the movie*!

    _\* Most newer discs actually **do** contain the movie name and occasionally the year, but they vary greatly in how they format it, and they typically tack a bunch of useless crap on the end; e.g., "Amazing Spider-Man, The - Blu-ray™", "Beauty and the Beast (Disc 1) - Blu-ray™".  Although human beings have no trouble extracting the movie name, parsing these strings **correctly** in software (and getting it right 100% of the time) is a bit trickier.  Not impossible, but definitely a pain in the ass._

## The Solution

BDAutoMuxer achieves our goals and solves the aforementioned issues with the following features:

1.  **Online Database**

    BDAutoMuxer leverages a small but growing database of Blu-ray Disc metadata contributed by other BDAutoMuxer users** that includes main movie playlists, video render languages, cut / edition / commentary information, and [TMDb][tmdb] IDs.

    _\* For the time being, only project maintainers are allowed to submit to the database.  Our goal is to ensure 100% accuracy of all database entries, and doing so requires great care and extensive knowledge of Blu-ray ripping.  In the future we may open up submissions to trusted committers and AVS / Doom9 forum users, but for now it's restricted to the developers only._

2.  **Intelligent Detection of Main-Movie Playlist(s)**

    Even in offline mode, BDAutoMuxer will do 90% of the work for you and auto-detect the main movie playlist(s).

3.  **Smart Output File Naming**

    BDAutoMuxer detects the movie name and release year of each disc and allows you to insert useful metadata into the output path, including:

    ##### Placeholders

        %volume% - AMAZING_SPIDERMAN; CONTACT
        %title% - The Amazing Spider-Man; Contact
        %year% - 2012; 1997
        %res% - 1080p; 720p
        %vcodec% - AVC; VC-1
        %acodec% - DTS-HD MA; TrueHD
        %channels% - 7.1; 5.1

    ##### Example

        %TEMP%\%volume%\%title% (%year%) [%res%] [%vcodec%] [%acodec%]
        =>
        C:\Users\BobaFett\AppData\Local\Temp\CONTACT\Contact (1997) [1080p] [AVC] [DTS-HD MA].m2ts

# Screenshots

## 1 - Disc tab

![Disc tab screenshot](https://raw.github.com/acdvorak/bdautomuxer/master/Screenshots/1-Disc.png)

## 2 - Output tab

![Output tab screenshot](https://raw.github.com/acdvorak/bdautomuxer/master/Screenshots/2-Output.png)

## 3 - Progress tab

![Progress tab screenshot](https://raw.github.com/acdvorak/bdautomuxer/master/Screenshots/3-Progress.png)

# Credit

BDAutoMuxer would not be possible without the **exceptional** work of these amazing free and open source projects:

*  **BDInfo**

    BDAutoMuxer is a fork of the **fantastic** [BDInfo][bdinfo] tool by [CinemaSquid][cinemasquid].

*  **tsMuxeR**

    BD-ROM to M2TS muxing is made possible by [tsMuxeR][tsmuxer], a (sadly) discontinued product from SmartLabs.

*  **mkvmerge**

    Remuxing from M2TS to MKV is done with [mkvmerge][mkvmerge], part of the **outstanding** [MKVToolNix][mkvtoolnix] suite.

*  **BDAutoMuxer**

    BDAutoMuxer is developed and maintained by [Andy Dvorak][andydvorak] and [Chris Spencer][chrisspencer].

# License

*  The BDInfo source code is licensed under the [LGPL 2.1][lgpl].
*  BDAutoMuxer is currently licensed under the [LGPL 2.1][lgpl] as well, but this is under review and we may release it under a different license in the future.

[tmdb]: http://www.themoviedb.org/
[bdinfo]: http://www.cinemasquid.com/blu-ray/tools/bdinfo
[cinemasquid]: http://www.cinemasquid.com/
[tsmuxer]: http://www.videohelp.com/tools/tsMuxeR
[mkvmerge]: http://www.bunkus.org/videotools/mkvtoolnix/doc/mkvmerge.html
[mkvtoolnix]: http://www.bunkus.org/videotools/mkvtoolnix/
[andydvorak]: https://github.com/acdvorak
[chrisspencer]: https://github.com/Chris-Spencer
[lgpl]: http://www.gnu.org/licenses/old-licenses/lgpl-2.1.html