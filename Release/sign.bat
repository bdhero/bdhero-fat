@ECHO OFF

REM Path to the code signing certificate private key (.pfx) file
IF "%CodeSigningCertPK%"=="" echo Environment variable %%CodeSigningCertPK%% is missing && GOTO END

REM Password for the PK
IF "%CodeSigningCertPW%"=="" echo Environment variable %%CodeSigningCertPK%% is missing && GOTO END

set ExeDescription=%1
set WebsiteUrl=%2
set PathToExe=%3

IF %ExeDescription%=="" echo Argument %%1 (ExeDescription) is missing && GOTO USAGE
IF %WebsiteUrl%=="" echo Argument %%2 (WebsiteUrl) is missing && GOTO USAGE
IF %PathToExe%=="" echo Argument %%3 (PathToExe) is missing && GOTO USAGE

REM set WindowsSdkDir=%ProgramFiles(x86)%\Microsoft SDKs\Windows\v7.0A\Bin
REM cd %WindowsSdkDir%\Bin

REM set /P PFXPass=Enter PFX Password: 
REM signtool sign /p %PFXPASS% /f C:\src\mycert.pfx /d "FiddlerCap" /du "http://www.fiddlercap.com/" /t http://timestamp.comodoca.com/authenticode /v FiddlerCap-en.msi 
REM set PFXPass=blank

REM signtool sign /v /f certfile.pfx /d "${project.fullName}" /du "https://github.com/bdhero/bdhero" "${installer_pathname}"

REM Usage: sign.bat "Project Name" "http://link.to/website" "path/to.exe"
signtool sign /v /p %CodeSigningCertPW% /f %CodeSigningCertPK% /d %ExeDescription% /du %WebsiteUrl% /t http://timestamp.comodoca.com/authenticode %PathToExe%

GOTO END

:USAGE
echo.
echo USAGE: sign.bat EXE_DESCRIPTION WEBSITE_URL PATH_TO_EXE
goto END

:END
