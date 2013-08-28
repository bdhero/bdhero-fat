#include "removable-drives.iss"

[Code]
// http://timesheetsandstuff.wordpress.com/2008/06/27/the-joy-of-part-2/

var
    bIsPortable : Boolean;

function IsNotPortable : Boolean;
begin
    Result := not bIsPortable;
end;

function DefaultInstallDir : String;
begin
    if bIsPortable then
        Result := GetFirstRemovableDrive() + 'PortableApps\{#MyAppName}'
    else
        Result := ExpandConstant('{localappdata}\{#MyAppName}\Application')
    ;
end;

function AutoConfigDirFn(Param: String): String;
begin
    if bIsPortable then
        Result := ExpandConstant('{app}\Config')
    else
        Result := ExpandConstant('{userappdata}\{#MyAppName}\Config')
    ;
end;

function ShouldSkipInstallTypePage(Sender: TWizardPage): Boolean;
begin
    Result := FileExists(WizardForm.DirEdit.Text + '\{#MyAppExeName}')
end;

var
    UsagePage: TInputOptionWizardPage;

procedure InitializeWizardInstallType;
begin
    //{ Create the pages }
    UsagePage := CreateInputOptionPage(wpWelcome, 'Installation Type', 'Select Installation Option', 'Where would you like to install this program?', True, False);
    UsagePage.Add('Normal – PC Hard Disk (current user only)');
    UsagePage.Add('Portable – USB Thumb Drive');
    //{Set Default – Normal Install}
    UsagePage.SelectedValueIndex := 0;
    UsagePage.OnShouldSkipPage := @ShouldSkipInstallTypePage;
end;
