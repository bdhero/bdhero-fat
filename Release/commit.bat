@ECHO OFF

REM %CD% = C:\Projects\BDHero

REM Capture stdout from Versioner.exe and store it in a variable
REM See http://stackoverflow.com/a/108511/467582
FOR /F "delims=" %%i IN ('Release\Versioner --version') DO set NewVersion=%%i

git status

echo git add BDHero.xml
echo git add BDHero\Properties\AssemblyInfo.cs
echo git add BDHeroCLI\Properties\AssemblyInfo.cs
echo git add BDHeroGUI\Properties\AssemblyInfo.cs

git status

set Message=Bumped BDHero version number to %NewVersion%

REM TODO: Check if tag already exists
echo "TODO: Check if tag already exists"

echo git commit -m "%Message%"
echo git tag -a v%NewVersion%
echo git push origin --tags

git status
