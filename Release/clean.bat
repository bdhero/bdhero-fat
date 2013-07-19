@ECHO OFF

cd ..

echo %CD%

git reset --hard HEAD

del /F /Q Setup\*.zip
del /F /Q Setup\*.exe
