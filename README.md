# BDAutoMuxer

## What Is BDAutoMuxer?

BDAutoMuxer is an intelligent, automatic tool for detecting main movie playlist(s) on Blu-ray Discs™ and muxing them to M2TS / MKV containers.

### The Problem

Many modern Blu-rays contain numerous _almost_-identical playlists that have the same run time and file size, but with slight non-obvious variations, making it difficult (not to mention time-consuming) to identify the correct playlist(s) to rip.  "The Hunger Games", for instance, has _**62**_ different movie-length playlists -- but only _**one**_ of them is actually the main movie!

Animated films (particularly Disney movies) often have 3-4 nearly-identical playlists containing the same audio and subtitle tracks, but with brief video sequences rendered in different languages (e.g., the opening title sequence, newspaper headlines).  Inexperienced individuals may simply pick one playlist at random and rip it, only to discover the next day that they wasted 12 hours encoding the French render of "Toy Story" instead of the English one.

The icing on the steamy pile of poo cake that is the Blu-ray Disc™ standard is the fact that there is absolutely _**zero**_ meta information _anywhere_ on the disc to indicate A) which playlist is the main movie, B) which language the video is rendered in, C) what the purpose of each audio track is (main audio, commentary, SAP), D) the "cut" or edition of the film (e.g., theatrical, special, extended), E) the names of the chapters, or even F) the name and year of the movie*!

_\* Most newer discs actually **do** contain the movie name and occasionally the year, but they vary greatly in how they format it, and they typically tack a bunch of useless crap on the end; e.g., "Amazing Spider-Man, The - Blu-ray™", "Beauty and the Beast (Disc 1) - Blu-ray™".  Though trivial for human beings, parsing them **correctly** in software (and getting it right 100% of the time) is a bit trickier.  Not impossible, but definitely a pain in the ass._

### The Solution

BDAutoMuxer solves _most_ of these issues with the following features:

1.  Automatic filtering of main-movie playlists
2.  Online database of video languages
3.  Auto-file naming

## Screenshots

### 1 - Disc tab

![Disc tab screenshot](https://raw.github.com/acdvorak/bdautomuxer/master/Screenshots/1-Disc.png)

### 2 - Output tab

![Output tab screenshot](https://raw.github.com/acdvorak/bdautomuxer/master/Screenshots/2-Output.png)

### 3 - Progress tab

![Progress tab screenshot](https://raw.github.com/acdvorak/bdautomuxer/master/Screenshots/3-Progress.png)