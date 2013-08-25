@ECHO OFF

REM %CD% = C:\Projects\BDHero

call Build\Release\tools.bat

echo PATH=%PATH%

set ProjectUrl=http://bdhero.org/
set MirrorUrl=http://dl.cdn.bdhero.org/

call Build\Release\sign.bat "BDHero GUI" "%ProjectUrl%" Artifacts\Installer\ProgramFiles\*.exe

xcopy /Y Artifacts\Installer\ProgramFiles\*.exe Artifacts\Portable\

REM "%ProgramFiles%\BitRock InstallBuilder Enterprise 8.6.0\bin\builder-cli.exe" build "Installer.xml" windows

call %InnoSetup%\iscc "/sCustom=%SignTool%\signtool.exe $p" Build\InnoSetup\setup.iss

%SevenZip%\7za a -sfx7z.sfx -r "%SfxPath%" .\Artifacts\Portable\*
%SevenZip%\7za a -r "%SevenZipPath%" .\Artifacts\Portable\*
%SevenZip%\7za a -r "%ZipPath%" .\Artifacts\Portable\*

call Build\Release\sign.bat "BDHero Portable (Self Extracting Archive)" "%ProjectUrl%" "%SfxPath%"
