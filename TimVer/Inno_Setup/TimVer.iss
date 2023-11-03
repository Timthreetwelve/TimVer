; -----------------------------------------------------
; TimVer
; -----------------------------------------------------

#define MyAppName "TimVer"
#define MyAppVersion GetStringFileInfo("D:\Visual Studio\Source\Prod\TimVer\TimVer\bin\Publish\TimVer.exe", "FileVersion")
#define MyAppExeName "TimVer.exe"
#define MySourceDir "D:\Visual Studio\Source\Prod\TimVer\TimVer\bin\Publish"
#define MySetupIcon "D:\Visual Studio\Source\Prod\TimVer\TimVer\Images\TV.ico"
#define MyCompanyName "T_K"
#define MyPublisherName "Tim Kennedy"
#define MyCopyright "Copyright (C) 2023 Tim Kennedy"
#define MyLicFile "D:\Visual Studio\Resources\License.rtf"
#define MyOutputDir "D:\InnoSetup\Output"
#define MyLargeImage "D:\InnoSetup\Images\WizardImage.bmp"
#define MySmallImage "D:\InnoSetup\Images\WizardSmallImage.bmp"
#define MyDateTimeString GetDateTimeString('yyyy/mm/dd hh:nn:ss', '/', ':')
#define MyAppNameNoSpaces StringChange(MyAppName, " ", "")
#define MyInstallerFilename MyAppNameNoSpaces + "_" + MyAppVersion + "_Setup"
#define RunRegKey "Software\Microsoft\Windows\CurrentVersion\Run" 
 
[Setup]
; NOTE: The value of AppId uniquely identifies this application. 
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
;---------------------------------------------
AppId={{4D2F6A12-7661-4E5B-983A-11F2194C81CA}
;---------------------------------------------
; Uncomment the following line to run in non administrative install mode (install for current user only.)
; Installs in %localappdata%\Programs\ instead of \Program Files(x86)
;---------------------------------------------
PrivilegesRequired=lowest
;---------------------------------------------
AllowNoIcons=yes
AppCopyright={#MyCopyright}
AppName={#MyAppName}
AppPublisher={#MyPublisherName}
AppVerName={#MyAppName} {#MyAppVersion}
AppVersion={#MyAppVersion}
Compression=lzma
DefaultDirName={autopf}\{#MyCompanyName}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableDirPage=yes
DisableProgramGroupPage=yes
DisableReadyMemo=yes
DisableStartupPrompt=yes
DisableWelcomePage=no
;LicenseFile={#MyLicFile}
OutputBaseFilename={#MyInstallerFilename}
OutputDir={#MyOutputDir}
OutputManifestFile={#MyAppName}_{#MyAppVersion}_FileList.txt
SetupIconFile={#MySetupIcon}
SetupLogging=yes
SolidCompression=yes
SourceDir={#MySourceDir}
UninstallDisplayIcon={app}\{#MyAppExeName}
VersionInfoVersion={#MyAppVersion}
WizardImageFile={#MyLargeImage}
;WizardSmallImageFile={#MySmallImage}
WizardStyle=modern
WizardSizePercent=100,100
AppSupportURL=https://github.com/Timthreetwelve/TimVer
;InfoBeforeFile="D:\InnoSetup\Timver_Before.rtf"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[LangOptions]
DialogFontSize=9
DialogFontName="Segoe UI"
WelcomeFontSize=12
WelcomeFontName="Verdana"

[Messages]
WelcomeLabel1=Welcome to [name] Setup
WelcomeLabel2=This will install [name/ver] on your computer. \
%n%nIt is recommended that you close all other applications before continuing. \
%n%n%nNote that [name] requires .NET 6.%n
FinishedHeadingLabel=Completing [name] Setup

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#MySourceDir}\TimVer.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MySourceDir}\*.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "{#MySourceDir}\*.json"; Excludes: "usersettings.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#MySourceDir}\ReadMe.txt"; DestDir: "{app}"; Flags: ignoreversion isreadme
Source: "{#MySourceDir}\License.txt"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[InstallDelete]
Type: filesandordirs; Name: "{group}"
Type: files; Name: "{app}\Nlog.config"
Type: files; Name: "{app}\CsvHelper.dll"

[Registry]
Root: HKCU; Subkey: "Software\{#MyCompanyName}"; Flags: uninsdeletekeyifempty
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; ValueType: string; ValueName: "Copyright"; ValueData: "{#MyCopyright}"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; ValueType: string; ValueName: "Install Date"; ValueData: "{#MyDateTimeString}"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; ValueType: string; ValueName: "Version"; ValueData: "{#MyAppVersion}"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; ValueType: string; ValueName: "Install Folder"; ValueData: "{autopf}\{#MyCompanyName}\{#MyAppName}"; Flags: uninsdeletekey
; Delete this key from previous installs
Root: HKCU; Subkey: "Software\{#MyCompanyName}\{#MyAppName}"; ValueType: none; ValueName: "Edition"; Flags: uninsdeletekey deletevalue

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent unchecked shellexec
Filename: "{app}\ReadMe.txt"; Description: "{cm:ViewReadme}"; Flags: nowait postinstall skipifsilent unchecked shellexec


[Code]
 procedure CurUninstallStepChanged (CurUninstallStep: TUninstallStep);
 var
     mres : integer;
 begin
    case CurUninstallStep of
      usPostUninstall:
        // Remove settings & history files. Also remove from windows startup.
        begin
          mres := MsgBox('Do you want to remove the settings and history files and registry entries? '#13#13' Select ''No'' if you plan on reinstalling. ', \
            mbConfirmation, MB_YESNO or MB_DEFBUTTON2)
          if mres = IDYES then
          begin
            DelTree(ExpandConstant('{app}\*.json'), False, True, False);
            DelTree(ExpandConstant('{app}'), True, True, True);
            RegDeleteValue(HKEY_CURRENT_USER, 'Software\Microsoft\Windows\CurrentVersion\Run', 'TimVer')
          end;
       end;
   end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
    // Since command line has changed, remove the entry from the registry
    begin
      if RegValueExists(HKEY_CURRENT_USER, 'Software\Microsoft\Windows\CurrentVersion\Run', 'TimVer') then
        begin
          Log('Deleting TimVer from Software\Microsoft\Windows\CurrentVersion\Run')
          RegDeleteValue(HKEY_CURRENT_USER, 'Software\Microsoft\Windows\CurrentVersion\Run', 'TimVer')
        end;
    end;
end;