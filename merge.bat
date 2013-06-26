@ECHO OFF

set ConfigurationName=%1
set SolutionDir=%2

IF "%ConfigurationName%"=="" set ConfigurationName=Debug
IF NOT "%SolutionDir%"=="" cd "%SolutionDir%"

echo %ConfigurationName%

REM Clean up old merged files
rmdir /S /Q Setup\ProgramFiles

REM Delete empty dummy DLL
del /F Packager\bin\%ConfigurationName%\Packager.dll

REM Copy core BDHero EXEs, DLLs, and config files to Setup dir
xcopy /Y Packager\bin\%ConfigurationName%\*.exe Setup\ProgramFiles\
xcopy /Y Packager\bin\%ConfigurationName%\*.dll Setup\ProgramFiles\
xcopy /Y Packager\bin\%ConfigurationName%\Config Setup\ProgramFiles\Config\

REM Copy required plugins

xcopy /Y Plugins\DiscReaderPlugin\bin\%ConfigurationName%\DiscReaderPlugin.dll Setup\ProgramFiles\Plugins\Required\DiscReader\

xcopy /Y Plugins\TmdbPlugin\bin\%ConfigurationName%\TmdbPlugin.dll Setup\ProgramFiles\Plugins\Required\Tmdb\
xcopy /Y Plugins\TmdbPlugin\bin\%ConfigurationName%\RestSharp.dll Setup\ProgramFiles\Plugins\Required\Tmdb\
xcopy /Y Plugins\TmdbPlugin\bin\%ConfigurationName%\WatTmdb.dll Setup\ProgramFiles\Plugins\Required\Tmdb\

xcopy /Y Plugins\AutoDetectorPlugin\bin\%ConfigurationName%\AutoDetectorPlugin.dll Setup\ProgramFiles\Plugins\Required\AutoDetector\

xcopy /Y Plugins\FileNamerPlugin\bin\%ConfigurationName%\FileNamerPlugin.dll Setup\ProgramFiles\Plugins\Required\FileNamer\

xcopy /Y Plugins\ChapterGrabberPlugin\bin\%ConfigurationName%\ChapterGrabberPlugin.dll Setup\ProgramFiles\Plugins\Required\ChapterGrabber\

xcopy /Y Plugins\FFmpegMuxerPlugin\bin\%ConfigurationName%\FFmpegMuxerPlugin.dll Setup\ProgramFiles\Plugins\Required\FFmpegMuxer\
xcopy /Y Plugins\FFmpegMuxerPlugin\bin\%ConfigurationName%\*.exe Setup\ProgramFiles\Plugins\Required\FFmpegMuxer\
