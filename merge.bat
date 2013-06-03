@ECHO OFF

set ConfigurationName=%1
set SolutionDir=%2

IF "%ConfigurationName%"=="" set ConfigurationName=Debug
IF NOT "%SolutionDir%"=="" cd "%SolutionDir%"

echo %ConfigurationName%

mkdir Setup\ProgramFiles\Config

xcopy /Y BDHeroCLI\bin\%ConfigurationName%\bdhero-cli.exe Setup\ProgramFiles\
xcopy /Y BDHeroCLI\bin\%ConfigurationName%\*.dll Setup\ProgramFiles\
xcopy /Y BDHeroCLI\bin\%ConfigurationName%\Config Setup\ProgramFiles\Config\

xcopy /Y DiscReaderPlugin\bin\%ConfigurationName%\DiscReaderPlugin.dll Setup\ProgramFiles\Plugins\Required\DiscReader\

xcopy /Y TmdbPlugin\bin\%ConfigurationName%\TmdbPlugin.dll Setup\ProgramFiles\Plugins\Required\Tmdb\
xcopy /Y TmdbPlugin\bin\%ConfigurationName%\RestSharp.dll Setup\ProgramFiles\Plugins\Required\Tmdb\
xcopy /Y TmdbPlugin\bin\%ConfigurationName%\WatTmdb.dll Setup\ProgramFiles\Plugins\Required\Tmdb\

xcopy /Y AutoDetectorPlugin\bin\%ConfigurationName%\AutoDetectorPlugin.dll Setup\ProgramFiles\Plugins\Required\AutoDetector\

xcopy /Y FileNamerPlugin\bin\%ConfigurationName%\FileNamerPlugin.dll Setup\ProgramFiles\Plugins\Required\FileNamer\

xcopy /Y ChapterGrabberPlugin\bin\%ConfigurationName%\ChapterGrabberPlugin.dll Setup\ProgramFiles\Plugins\Required\ChapterGrabber\

xcopy /Y FFmpegMuxerPlugin\bin\%ConfigurationName%\FFmpegMuxerPlugin.dll Setup\ProgramFiles\Plugins\Required\FFmpegMuxer\
xcopy /Y FFmpegMuxerPlugin\bin\%ConfigurationName%\ffmpeg.exe Setup\ProgramFiles\Plugins\Required\FFmpegMuxer\
