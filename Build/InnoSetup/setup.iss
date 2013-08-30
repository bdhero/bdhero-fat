; http://www.codeproject.com/Articles/20868/NET-Framework-1-1-2-0-3-5-Installer-for-InnoSetup

#include "dependencies.iss"

[Setup]

;#define DebugMode

#define MyAppName "BDHero"
#define MyAppMachineName "bdhero"
#define MyAppVersion "0.8.2.6"
#define MyAppPublisher "BDHero"
#define MyAppURL "http://bdhero.org/"
#define MyAppExeName "bdhero-gui.exe"

#define CodeSigningCertPK GetEnv('CodeSigningCertPK')
#define CodeSigningCertPW GetEnv('CodeSigningCertPW')

#define InstallerArtifactDir "..\..\Artifacts\Installer"
#define DeleteFileFlags "uninsrestartdelete ignoreversion"
#define DeleteDirFlags "uninsrestartdelete ignoreversion createallsubdirs recursesubdirs"

#include "install-dir.iss"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId=BDHero
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}

;SourceDir=..\..\
OutputDir=..\..\Artifacts
OutputBaseFilename={#MyAppMachineName}-{#MyAppVersion}-setup

WizardImageFile=..\..\Assets\InnoSetup\bdrom_wizard_2.bmp
WizardSmallImageFile=..\..\Assets\InnoSetup\bdhero_gui_55x58.bmp

MinVersion=0,5.01sp3
PrivilegesRequired=lowest

Uninstallable=IsNotPortable
UninstallDisplayIcon={app}\{#MyAppExeName}

; "ArchitecturesInstallIn64BitMode=x64" requests that the install be
; done in "64-bit mode" on x64, meaning it should use the native
; 64-bit Program Files directory and the 64-bit view of the registry.
; On all other architectures it will install in "32-bit mode".
ArchitecturesInstallIn64BitMode=x64
; Note: We don't set ProcessorsAllowed because we want this
; installation to run on all architectures (including Itanium,
; since it's capable of running 32-bit code too).

;DefaultDirName={code:DefaultInstallDir}
DefaultDirName={#MyAppName}
AppendDefaultDirName=no
DefaultGroupName={#MyAppName}
AllowNoIcons=yes

DisableWelcomePage=yes
DisableDirPage=no
DisableProgramGroupPage=yes
AlwaysShowDirOnReadyPage=yes
AlwaysShowGroupOnReadyPage=no

#ifdef DebugMode
    Compression=none
#else
    Compression=lzma2/ultra64
    SolidCompression=True
    InternalCompressLevel=ultra64
    
    #if CodeSigningCertPK != ""
        SignTool=Custom sign /v /f {#CodeSigningCertPK} /p {#CodeSigningCertPW} /d $q{#MyAppName} Setup$q /du $q{#MyAppURL}$q /t http://timestamp.comodoca.com/authenticode $f
        SignedUninstaller=True
    #endif
#endif

ShowLanguageDialog=auto

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "de"; MessagesFile: "compiler:Default.isl"

;[Tasks]
;Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#InstallerArtifactDir}\ProgramFiles\bdhero-gui.exe"; DestDir: "{app}";                  Flags: {#DeleteFileFlags}
Source: "{#InstallerArtifactDir}\ProgramFiles\*";              DestDir: "{app}";                  Flags: {#DeleteDirFlags}
Source: "{#InstallerArtifactDir}\Plugins\Required\*";          DestDir: "{app}\Plugins\Required"; Flags: {#DeleteDirFlags}
Source: "{#InstallerArtifactDir}\Config\*";                    DestDir: "{code:AutoConfigDirFn}"; Flags: {#DeleteDirFlags}

[UninstallDelete]
Type: dirifempty;     Name: "{userappdata}\{#MyAppName}\Config\Application"
Type: filesandordirs; Name: "{userappdata}\{#MyAppName}\Plugins\Required"
Type: dirifempty;     Name: "{userappdata}\{#MyAppName}\Plugins"
Type: dirifempty;     Name: "{userappdata}\{#MyAppName}"

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Check: IsNotPortable
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"; Check: IsNotPortable
;Name: "{userdesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; Check: IsNotPortable

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[CustomMessages]
;win_sp_title=Windows %1 Service Pack %2

[Code]
var bIsAlreadyInstalled : Boolean;

function AlreadyInstalled : boolean;
begin
    Result := bIsAlreadyInstalled;
end;

function NextButtonClick(CurPageID: Integer): boolean;
begin
	Result := true;

    if (CurPageID = UsagePage.ID) then
        begin
            bIsPortable := not (UsagePage.SelectedValueIndex = 0)
            WizardForm.DirEdit.Text := DefaultInstallDir()
        end
    else
        Result := NextButtonClickCheckPrereq(CurPageID);
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
    if (PageID = wpSelectDir) and (IsNotPortable()) then
        Result := true
    else
        Result := false
    ;
end;

procedure InitializeWizard;
begin
    bIsAlreadyInstalled := FileExists(WizardForm.DirEdit.Text + '\{#MyAppExeName}');
    InitializeWizardInstallType();
end;

function InitializeSetup(): boolean;
begin
	InitializeSetupDeps();
	Result := true;
end;
