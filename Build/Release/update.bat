@ECHO OFF

REM %CD% = C:\Projects\BDHero

call Build\Release\tools.bat

Build\Tools\UMGen --mirror "%MirrorUrl%" --windows --setup "%SetupPath%" --sfx "%SfxPath%" --7z "%SevenZipPath%" --zip "%ZipPath%" --output update.json
