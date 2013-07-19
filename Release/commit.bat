@ECHO OFF

REM %CD% = C:\Projects\BDHero

REM Capture stdout from Versioner.exe and store it in a variable
REM See http://stackoverflow.com/a/108511/467582
FOR /F "delims=" %%i IN ('Release\Versioner --version') DO set NewVersion=%%i

git status

git add BDHero.xml
git add BDHero\Properties\AssemblyInfo.cs
git add BDHeroCLI\Properties\AssemblyInfo.cs
git add BDHeroGUI\Properties\AssemblyInfo.cs

git status

set Message=Bumped BDHero version to %NewVersion%

REM TODO: Check if tag already exists
echo "TODO: Check if tag already exists"

git commit -m "%Message%"
git tag -a v%NewVersion% -m v%NewVersion%
git push -u origin master --tags

git status
