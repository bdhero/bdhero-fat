@ECHO OFF

REM %CD% = C:\Projects\BDHero

git reset --hard HEAD
git checkout master
git status
git log -n 5
