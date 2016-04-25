!define DOTNET_VERSION "v4.0.30319"
!include version.nsh

Name "RedBrick Installer"
OutFile "InstallRedBrick.exe"

VIProductVersion ${VERSION}
VIAddVersionKey ProductVersion ${VERSION}
VIAddVersionKey ProductName "Amstore RedBrick"
VIAddversionkey FileVersion ${VERSION}
VIAddVersionKey FileDescription "Edit SolidWorks properties"
VIAddVersionKey LegalCopyright  "2016 Amstore Corp"
InstallDir "$PROGRAMFILES64\RedBrick\"
RequestExecutionLevel admin

Section
  SetOutPath $INSTDIR

  File ".\Resources\redlego.ico"
  File ".\bin\x64\Release\Redbrick_Addin.dll.config"
  File ".\bin\x64\Release\System.dll"
  File ".\bin\x64\Release\System.Data.dll"
  File ".\bin\x64\Release\System.Security.dll"
  File ".\bin\x64\Release\System.Xml.dll"
  File ".\bin\x64\Release\swTableType.dll"
  File ".\bin\x64\Release\SolidWorks.Interop.sldworks.dll"
  File ".\bin\x64\Release\Redbrick_Addin.dll"
  
  Push "$INSTDIR\Redbrick_Addin.dll"
  Call RegisterDotNet
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
      "DisplayIcon" "$PROGRAMFILES64\Redbrick\redlego.ico"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
      "EstimatedSize" 4052
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
      "DisplayName" "Amstore Redbrick Property Manager"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
      "DisplayVersion" ${VERSION}
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
      "Publisher" "Amstore Corp."
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
      "UninstallString" "$\"$INSTDIR\RemoveRedbrick.exe$\""
  WriteUninstaller "$INSTDIR\RemoveRedBrick.exe"
SectionEnd

Section "Uninstall"
  Push "$INSTDIR\Redbrick_Addin.dll"
  Call un.RegisterDotNet

  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick"
  Delete "$INSTDIR\RemoveRedBrick.exe"
  Delete "$INSTDIR\Redbrick_Addin.dll.config"
  Delete "$INSTDIR\swTableType.dll"
  Delete "$INSTDIR\System.dll"
  Delete "$INSTDIR\System.Data.dll"
  Delete "$INSTDIR\System.Security.dll"
  Delete "$INSTDIR\System.Xml.dll"
  Delete "$INSTDIR\SolidWorks.Interop.sldworks.dll"
  Delete "$INSTDIR\Redbrick_Addin.dll"
  Delete "$INSTDIR\redlego.ico"
  RMDir  "$INSTDIR"
SectionEnd

Function .onInit
  SetRegView 64
  ReadRegStr $R0 HKLM \
      "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
      "UninstallString"
  StrCmp $R0 "" done
  
  MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
      "Redbrick is already installed. $\n$\nClick `OK` to remove the \
      previous version or `Cancel` to cancel this upgrade." \
      IDOK uninst
  Abort
  
  uninst:
    ClearErrors
    ExecWait '$R0 _?=$INSTDIR'
    
    IfErrors no_remove_uninstaller done
    
  no_remove_uninstaller:
    
  done:    
  FunctionEnd

  Function RegisterDotNet

    Exch $R0
    Push $R1

    SetRegView 64
    ReadRegStr $R1 HKEY_LOCAL_MACHINE \
        "Software\Microsoft\.NETFramework" "InstallRoot"

    IfFileExists $R1\${DOTNET_VERSION}\regasm.exe FileExists
    MessageBox MB_ICONSTOP|MB_OK "Microsoft .NET Framework 4.0 was not detected!"
    Abort

    FileExists:
      ExecWait '"$R1\${DOTNET_VERSION}\regasm.exe" /codebase "$R0" /silent'

      Pop $R1
      Pop $R0
  FunctionEnd

  Function un.RegisterDotNet

    Exch $R0
    Push $R1

    SetRegView 64
    ReadRegStr $R1 HKEY_LOCAL_MACHINE \
        "Software\Microsoft\.NETFramework" "InstallRoot"

    IfFileExists $R1\${DOTNET_VERSION}\regasm.exe FileExists
    MessageBox MB_ICONSTOP|MB_OK "Microsoft .NET Framework 4.0 was not detected!"
    Abort

    FileExists:
      ExecWait '"$R1\${DOTNET_VERSION}\regasm.exe" "$R0" /unregister /silent'

      Pop $R1
      Pop $R0

  FunctionEnd
