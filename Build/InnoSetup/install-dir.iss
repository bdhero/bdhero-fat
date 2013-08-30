#include "removable-drives.iss"

[Code]
// http://timesheetsandstuff.wordpress.com/2008/06/27/the-joy-of-part-2/

var pInstallationTypePage: TInputOptionWizardPage;

// Populated by InitializeWizardInstallType()
var sDefaultInstallDir : String;
var sCustomInstallDir : String;
var sDefaultPortableDir : String;
var sCustomPortableDir : String;

function IsAlreadyInstalled : Boolean;
begin
    Result := FileExists(sDefaultInstallDir + '\{#MyAppExeName}');
end;

function IsPortable : Boolean;
begin
    Result := not (pInstallationTypePage.SelectedValueIndex = 0)
end;

function ConfigDirAuto(Param: String): String;
begin
    if IsPortable() then
        Result := ExpandConstant('{app}\Config')
    else
        Result := ExpandConstant('{userappdata}\{#MyAppName}\Config')
    ;
end;

function GetInstallDirAuto() : String;
begin
    if IsPortable() then
        Result := sCustomPortableDir
    else
        Result := sCustomInstallDir
end;

procedure SaveInstallDirAuto;
begin
    if IsPortable() then
        sCustomPortableDir := WizardForm.DirEdit.Text
    else
        sCustomInstallDir := WizardForm.DirEdit.Text
end;

procedure RestoreInstallDirAuto;
begin
    WizardForm.DirEdit.Text := GetInstallDirAuto()
end;

procedure UserChangedInstallDir(Sender: TObject);
begin
    SaveInstallDirAuto()
end;

function ShouldSkipInstallTypePage(Sender: TWizardPage): Boolean;
begin
    Result := IsAlreadyInstalled()
end;

procedure InitializeWizardInstallType;
begin
    sDefaultInstallDir := WizardForm.DirEdit.Text;
    sCustomInstallDir := sDefaultInstallDir;

    sDefaultPortableDir := GetFirstRemovableDrive() + 'PortableApps\{#MyAppName}';
    sCustomPortableDir := sDefaultPortableDir;

    // Create the page
    pInstallationTypePage := CreateInputOptionPage(wpWelcome, 'Installation Type', 'Select Installation Option', 'Where would you like to install this program?', True, False);

    pInstallationTypePage.Add('Normal – PC Hard Disk (current user only)');
    pInstallationTypePage.Add('Portable – USB Thumb Drive');

    // Set Default – Normal Install
    pInstallationTypePage.SelectedValueIndex := 0;

    WizardForm.DirEdit.OnChange := @UserChangedInstallDir;

    //pInstallationTypePage.OnShouldSkipPage := @ShouldSkipInstallTypePage;
end;
