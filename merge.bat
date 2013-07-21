@ECHO OFF

set ConfigurationName=%1
set SolutionDir=%2

IF "%ConfigurationName%"=="" set ConfigurationName=Debug
IF NOT "%SolutionDir%"=="" cd "%SolutionDir%"

set PluginDir=Artifacts\Installer\Plugins
set ConfigDir=Artifacts\Installer\Config
set ProgramFilesDir=Artifacts\Installer\ProgramFiles
set PortableDir=Artifacts\Portable

echo %ConfigurationName%

REM Clean up old merged files
rmdir /S /Q Artifacts

REM Delete empty dummy DLL
del /F Packager\bin\%ConfigurationName%\Packager.dll

REM Copy core BDHero EXEs, DLLs, and config files to Artifacts dir
xcopy /Y Packager\bin\%ConfigurationName%\*.exe %ProgramFilesDir%\
xcopy /Y Packager\bin\%ConfigurationName%\*.dll %ProgramFilesDir%\
xcopy /Y Packager\bin\%ConfigurationName%\Config %ConfigDir%\

REM Copy required plugins

xcopy /Y Plugins\DiscReaderPlugin\bin\%ConfigurationName%\DiscReaderPlugin.dll %PluginDir%\Required\DiscReader\
xcopy /Y Plugins\DiscReaderPlugin\bin\%ConfigurationName%\INIFileParser.dll %PluginDir%\Required\DiscReader\

xcopy /Y Plugins\TmdbPlugin\bin\%ConfigurationName%\TmdbPlugin.dll %PluginDir%\Required\Tmdb\
xcopy /Y Plugins\TmdbPlugin\bin\%ConfigurationName%\RestSharp.dll %PluginDir%\Required\Tmdb\
xcopy /Y Plugins\TmdbPlugin\bin\%ConfigurationName%\WatTmdb.dll %PluginDir%\Required\Tmdb\

xcopy /Y Plugins\ChapterGrabberPlugin\bin\%ConfigurationName%\ChapterGrabberPlugin.dll %PluginDir%\Required\ChapterGrabber\

xcopy /Y Plugins\IsanPlugin\bin\%ConfigurationName%\IsanPlugin.dll %PluginDir%\Required\Isan\
xcopy /Y Plugins\IsanPlugin\bin\%ConfigurationName%\CsQuery.dll %PluginDir%\Required\Isan\

xcopy /Y Plugins\AutoDetectorPlugin\bin\%ConfigurationName%\AutoDetectorPlugin.dll %PluginDir%\Required\AutoDetector\

xcopy /Y Plugins\FileNamerPlugin\bin\%ConfigurationName%\FileNamerPlugin.dll %PluginDir%\Required\FileNamer\

xcopy /Y Plugins\FFmpegMuxerPlugin\bin\%ConfigurationName%\FFmpegMuxerPlugin.dll %PluginDir%\Required\FFmpegMuxer\
xcopy /Y Plugins\FFmpegMuxerPlugin\bin\%ConfigurationName%\*.exe %PluginDir%\Required\FFmpegMuxer\

REM Copy everything to Portable directory

xcopy /Y /S %PluginDir% %PortableDir%\Plugins\
xcopy /Y %ConfigDir% %PortableDir%\Config\
xcopy /Y %ProgramFilesDir%\* %PortableDir%\
