@ECHO OFF

REM %CD% = C:\Projects\BDHero

set ProjectUrl=http://bdhero.org/

call Release\sign.bat "BDHero GUI" "%ProjectUrl%" Artifacts\Installer\ProgramFiles\*.exe

xcopy /Y Artifacts\Installer\ProgramFiles\*.exe Artifacts\Portable\

REM "%ProgramFiles%\BitRock InstallBuilder Enterprise 8.6.0\bin\builder-cli.exe" build "Installer.xml" windows

iscc "/sCustom=signtool.exe $p" Installer\InnoSetup\setup.iss

Release\Versioner.exe -v > VERSION.tmp
set /p Version= < VERSION.tmp
del VERSION.tmp

7za a -sfx7z.sfx -r Artifacts\bdhero-%Version%-sfx.exe .\Artifacts\Portable\*
7za a -r Artifacts\bdhero-%Version%.7z .\Artifacts\Portable\*
7za a -r Artifacts\bdhero-%Version%.zip .\Artifacts\Portable\*

call Release\sign.bat "BDHero Portable (Self Extracting Archive)" "%ProjectUrl%" Artifacts\bdhero-%Version%-sfx.exe
