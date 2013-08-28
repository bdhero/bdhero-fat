#include "removable-drives.iss"

[Code]
// http://timesheetsandstuff.wordpress.com/2008/06/27/the-joy-of-part-2/
var
    UsagePage: TInputOptionWizardPage;

procedure InitializeWizardInstallType;
begin
    //{ Create the pages }
    UsagePage := CreateInputOptionPage(wpWelcome, 'Installation Type', 'Select Installation Option', 'Where would you like to install this program', True, False);
    UsagePage.Add('Normal – PC Hard Disk Installation');
    UsagePage.Add('Portable – USB Drive Installation');
    //{Set Default – Normal Install}
    UsagePage.SelectedValueIndex := 0;
end;

var
    bIsPortable : Boolean;

function DefaultInstallDir(Param: String): String;
begin
//    if IsRegularUser then
//        Result := ExpandConstant('{localappdata}\{#MyAppName}\Application')
//    else
//        Result := ExpandConstant('{pf}\{#MyAppName}')
//    Result := ExpandConstant('{userpf}\{#MyAppName}')

    if bIsPortable then
        Result := GetFirstRemovableDrive() + 'PortableApps\{#MyAppName}'
    else
        Result := ExpandConstant('{localappdata}\{#MyAppName}\Application')
end;