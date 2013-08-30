; http://www.codeproject.com/Articles/20868/NET-Framework-1-1-2-0-3-5-Installer-for-InnoSetup

;#define use_iis
;#define use_kb835732

;#define use_msi20
#define use_msi31
;#define use_msi45

#define use_ie6

;#define use_dotnetfx11
;#define use_dotnetfx11lp

;#define use_dotnetfx20
;#define use_dotnetfx20lp

;#define use_dotnetfx35
;#define use_dotnetfx35lp

#define use_dotnetfx40
;#define use_wic

;#define use_vc2010

;#define use_mdac28
;#define use_jet4sp8

;#define use_sqlcompact35sp2

;#define use_sql2005express
;#define use_sql2008express

#define DebugMode

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
AppId={{3066D042-322C-4940-B3FD-EDD460BBAE56}
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
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
;Name: "{userdesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

#include "scripts\products.iss"

#include "scripts\products\stringversion.iss"
#include "scripts\products\winversion.iss"
#include "scripts\products\fileversion.iss"
#include "scripts\products\dotnetfxversion.iss"

#ifdef use_iis
#include "scripts\products\iis.iss"
#endif

#ifdef use_kb835732
#include "scripts\products\kb835732.iss"
#endif

#ifdef use_msi20
#include "scripts\products\msi20.iss"
#endif
#ifdef use_msi31
#include "scripts\products\msi31.iss"
#endif
#ifdef use_msi45
#include "scripts\products\msi45.iss"
#endif

#ifdef use_ie6
#include "scripts\products\ie6.iss"
#endif

#ifdef use_dotnetfx11
#include "scripts\products\dotnetfx11.iss"
#include "scripts\products\dotnetfx11sp1.iss"
#ifdef use_dotnetfx11lp
#include "scripts\products\dotnetfx11lp.iss"
#endif
#endif

#ifdef use_dotnetfx20
#include "scripts\products\dotnetfx20.iss"
#include "scripts\products\dotnetfx20sp1.iss"
#include "scripts\products\dotnetfx20sp2.iss"
#ifdef use_dotnetfx20lp
#include "scripts\products\dotnetfx20lp.iss"
#include "scripts\products\dotnetfx20sp1lp.iss"
#include "scripts\products\dotnetfx20sp2lp.iss"
#endif
#endif

#ifdef use_dotnetfx35
//#include "scripts\products\dotnetfx35.iss"
#include "scripts\products\dotnetfx35sp1.iss"
#ifdef use_dotnetfx35lp
//#include "scripts\products\dotnetfx35lp.iss"
#include "scripts\products\dotnetfx35sp1lp.iss"
#endif
#endif

#ifdef use_dotnetfx40
#include "scripts\products\dotnetfx40client.iss"
#include "scripts\products\dotnetfx40full.iss"
#endif

#ifdef use_wic
#include "scripts\products\wic.iss"
#endif

#ifdef use_vc2010
#include "scripts\products\vcredist2010.iss"
#endif

#ifdef use_mdac28
#include "scripts\products\mdac28.iss"
#endif
#ifdef use_jet4sp8
#include "scripts\products\jet4sp8.iss"
#endif

#ifdef use_sqlcompact35sp2
#include "scripts\products\sqlcompact35sp2.iss"
#endif

#ifdef use_sql2005express
#include "scripts\products\sql2005express.iss"
#endif
#ifdef use_sql2008express
#include "scripts\products\sql2008express.iss"
#endif

[CustomMessages]
win_sp_title=Windows %1 Service Pack %2

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

// http://www.codeproject.com/Articles/20868/NET-Framework-1-1-2-0-3-5-Installer-for-InnoSetup
function InitializeSetup(): boolean;
begin
	//init windows version
	initwinversion();

#ifdef use_iis
	if (not iis()) then exit;
#endif

#ifdef use_msi20
	msi20('2.0');
#endif
#ifdef use_msi31
	msi31('3.1');
#endif
#ifdef use_msi45
	msi45('4.5');
#endif
#ifdef use_ie6
	ie6('5.0.2919');
#endif

#ifdef use_dotnetfx11
	dotnetfx11();
#ifdef use_dotnetfx11lp
	dotnetfx11lp();
#endif
	dotnetfx11sp1();
#endif

	//install .netfx 2.0 sp2 if possible; if not sp1 if possible; if not .netfx 2.0
#ifdef use_dotnetfx20
	//check if .netfx 2.0 can be installed on this OS
	if not minwinspversion(5, 0, 3) then begin
		msgbox(fmtmessage(custommessage('depinstall_missing'), [fmtmessage(custommessage('win_sp_title'), ['2000', '3'])]), mberror, mb_ok);
		exit;
	end;
	if not minwinspversion(5, 1, 2) then begin
		msgbox(fmtmessage(custommessage('depinstall_missing'), [fmtmessage(custommessage('win_sp_title'), ['XP', '2'])]), mberror, mb_ok);
		exit;
	end;

	if minwinversion(5, 1) then begin
		dotnetfx20sp2();
#ifdef use_dotnetfx20lp
		dotnetfx20sp2lp();
#endif
	end else begin
		if minwinversion(5, 0) and minwinspversion(5, 0, 4) then begin
#ifdef use_kb835732
			kb835732();
#endif
			dotnetfx20sp1();
#ifdef use_dotnetfx20lp
			dotnetfx20sp1lp();
#endif
		end else begin
			dotnetfx20();
#ifdef use_dotnetfx20lp
			dotnetfx20lp();
#endif
		end;
	end;
#endif

#ifdef use_dotnetfx35
	//dotnetfx35();
	dotnetfx35sp1();
#ifdef use_dotnetfx35lp
	//dotnetfx35lp();
	dotnetfx35sp1lp();
#endif
#endif

	// if no .netfx 4.0 is found, install the client (smallest)
#ifdef use_dotnetfx40
	if (not netfxinstalled(NetFx40Full, '')) then
		dotnetfx40full();
#endif

#ifdef use_wic
	wic();
#endif

#ifdef use_vc2010
	vcredist2010();
#endif

#ifdef use_mdac28
	mdac28('2.7');
#endif
#ifdef use_jet4sp8
	jet4sp8('4.0.8015');
#endif

#ifdef use_sqlcompact35sp2
	sqlcompact35sp2();
#endif

#ifdef use_sql2005express
	sql2005express();
#endif
#ifdef use_sql2008express
	sql2008express();
#endif

	Result := true;
end;
