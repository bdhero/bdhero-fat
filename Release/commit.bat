@ECHO OFF

cd ..

echo %CD%

echo git add BDHero.xml
echo git add BDHero\Properties\AssemblyInfo.cs
echo git add BDHeroCLI\Properties\AssemblyInfo.cs
echo git add BDHeroGUI\Properties\AssemblyInfo.cs

set Message=Bumped BDHero version number to %NewVersion%

echo git commit -m "%Message%"
echo git tag -a v%NewVersion% -m "%Message%"
echo git push origin --tags
