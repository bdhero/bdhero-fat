@ECHO OFF

REM %CD% = C:\Projects\BDHero

call Build\Release\tools.bat

echo PATH=%PATH%

set ProjectUrl=http://bdhero.org/

call Build\Release\sign.bat "BDHero GUI" "%ProjectUrl%" Artifacts\Installer\ProgramFiles\*.exe

xcopy /Y Artifacts\Installer\ProgramFiles\*.exe Artifacts\Portable\

REM "%ProgramFiles%\BitRock InstallBuilder Enterprise 8.6.0\bin\builder-cli.exe" build "Installer.xml" windows

call %InnoSetup%\iscc "/sCustom=%SignTool%\signtool.exe $p" Build\InnoSetup\setup.iss

Build\Tools\Versioner\Versioner -v > VERSION.tmp
set /p Version= < VERSION.tmp
del VERSION.tmp

%SevenZip%\7za a -sfx7z.sfx -r Artifacts\bdhero-%Version%-sfx.exe .\Artifacts\Portable\*
%SevenZip%\7za a -r Artifacts\bdhero-%Version%.7z .\Artifacts\Portable\*
%SevenZip%\7za a -r Artifacts\bdhero-%Version%.zip .\Artifacts\Portable\*

call Build\Release\sign.bat "BDHero Portable (Self Extracting Archive)" "%ProjectUrl%" Artifacts\bdhero-%Version%-sfx.exe
