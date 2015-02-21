;NSIS Modern User Interface
;Written by Joost Verburg

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General

  #nastaveni komprese
  SetCompressor bzip2
  
  ;Properly display all languages
  Unicode true

  # pøidání informací
  VIProductVersion                 "0.9.5.0"
  VIAddVersionKey ProductName      "Newron - modul øízených sociálních dovedností"
  VIAddVersionKey Comments         "Terapeutický software Newron"
  VIAddVersionKey CompanyName      "Masaryk University"
  VIAddVersionKey LegalCopyright   "Masaryk University"
  VIAddVersionKey FileDescription  "Instálotor pro Newron"
  VIAddVersionKey FileVersion      1
  VIAddVersionKey ProductVersion   1
  VIAddVersionKey InternalName     "Newron"
  VIAddVersionKey LegalTrademarks  "CC"
  VIAddVersionKey OriginalFilename "Newron_instalator_gsi.exe"

  # nastavení ikon a loga
  !define MUI_ICON "Newron.ico"
  !define MUI_HEADERIMAGE
  !define MUI_HEADERIMAGE_BITMAP "Logo-Newron.png"
  !define MUI_HEADERIMAGE_RIGHT


  ;Name and file
  Name "Newron"
  OutFile "Newron_instalator_gsi.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\Newron"
  
  ;Get installation folder from registry if available
  #InstallDirRegKey HKCU "Software\Newron" ""

  ;Request application privileges - for SDK
  RequestExecutionLevel admin

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

  ;Show all languages, despite user's codepage
  !define MUI_LANGDLL_ALLLANGUAGES

;--------------------------------
;Language Selection Dialog Settings

  ;Remember the installer language
  #!define MUI_LANGDLL_REGISTRY_ROOT "HKCU" 
  #!define MUI_LANGDLL_REGISTRY_KEY "Software\Newron" 
  #!define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"

;--------------------------------
;Pages

	!include "kinect.nsdinc"

  !insertmacro MUI_PAGE_LICENSE "Licence.rtf"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  Page custom fnc_kinect_Show
  !insertmacro MUI_PAGE_INSTFILES
  
  #!insertmacro MUI_UNPAGE_CONFIRM
  #!insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "Czech" ;first language is the default language
  !insertmacro MUI_LANGUAGE "English"



;--------------------------------
;Reserve Files
  
  ;If you are using solid compression, files that are required before
  ;the actual installation should be stored first in the data block,
  ;because this will make your installer start faster.
  
  !insertmacro MUI_RESERVEFILE_LANGDLL

;--------------------------------
;Installer Sections

Section "Kinect SDK" SecKinect

  #povinná položka - nelze odškrtnout
  SectionIn RO

  SetOutPath $INSTDIR\SDK
  
  File "SDK\KinectSDK-v1.8-Setup.exe"
  ExecWait "$INSTDIR\SDK\KinectSDK-v1.8-Setup.exe"
  

SectionEnd




Section "Newron" SecNewron

  #povinná položka - nelze odškrtnout
  SectionIn RO

  SetOutPath "$INSTDIR"
    
    
  ;ADD YOUR OWN FILES HERE...
  File /r Newron-gsi_Data
  File Newron-gsi.exe
  File README.txt
  File Changelog.txt
  File *.dll
  
  ;Store installation folder
  #WriteRegStr HKCU "Software\Newron" "" $INSTDIR
  
#  ;Create uninstaller
#  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

Section "Zástupce na ploše" SecShortcut

  # create a shortcut named "new shortcut" in the start menu programs directory
  # presently, the new shortcut doesn't call anything (the second field is blank)
  createShortCut "$DESKTOP\Newron - Modul øízené sociální interakce.lnk" "$INSTDIR\Newron-gsi.exe"

SectionEnd


;--------------------------------
;Installer Functions

Function .onInit

  !insertmacro MUI_LANGDLL_DISPLAY

FunctionEnd

;--------------------------------
;Descriptions

  ;USE A LANGUAGE STRING IF YOU WANT YOUR DESCRIPTIONS TO BE LANGAUGE SPECIFIC

  ;Assign descriptions to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecKinect} "Microsoft Kinect for Windows SDK 1.8"
	!insertmacro MUI_DESCRIPTION_TEXT ${SecNewron} "Terapeutický software Newron"
    !insertmacro MUI_DESCRIPTION_TEXT ${SecShortcut} "Zástupce na ploše"
  !insertmacro MUI_FUNCTION_DESCRIPTION_END



;--- After successful install, remove Kinect SDK ---
Function .onInstSuccess
    RMDir /r "$INSTDIR\SDK"
FunctionEnd

 
#;--------------------------------
#;Uninstaller Section
#
#Section "Uninstall"
#
#  ;ADD YOUR OWN FILES HERE...
#  File /r Newron_Data
#  File Newron.exe
#  File README.txt
#  File Version.txt
#
#  Delete "$INSTDIR\Uninstall.exe"
#
#  RMDir "$INSTDIR"
#
#  DeleteRegKey /ifempty HKCU "Software\Newron"
#
#SectionEnd
#
#;--------------------------------
#;Uninstaller Functions
#
#Function un.onInit
#
# !insertmacro MUI_UNGETLANGUAGE
#  
#FunctionEnd