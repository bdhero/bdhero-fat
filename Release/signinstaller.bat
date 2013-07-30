@ECHO OFF

REM Usage: signinstaller.bat "BDHero Installer" "https://github.com/bdhero/bdhero" PROJECT_ROOT_DIR

set Description=%1
set HomepageUrl=%2
set ProjectRootDir=%3

for /f "usebackq delims=|" %%f in (`dir /b "%ProjectRootDir%\Artifacts\*.exe"`) do (
    echo %%f
    %ProjectRootDir%\Release\sign.bat %Description% %HomepageUrl% %ProjectRootDir%\Artifacts\%%f
)
