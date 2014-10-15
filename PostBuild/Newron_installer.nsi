;NSIS Modern User Interface
;Multilingual Example Script
;Written by Joost Verburg

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General

  #nastaveni komprese
  SetCompressor bzip2
  
  ;Properly display all languages (Installer will not work on Windows 95, 98 or ME!)
  Unicode true

  ;Name and file
  Name "Newron"
  OutFile "Newron_instalator.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\Newron"
  
  ;Get installation folder from registry if available
  #InstallDirRegKey HKCU "Software\Newron" ""

  ;Request application privileges for Windows Vista
  #RequestExecutionLevel user

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

  !insertmacro MUI_PAGE_LICENSE "Licence.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  #!insertmacro MUI_UNPAGE_CONFIRM
  #!insertmacro MUI_UNPAGE_INSTFILES

;--------------------------------
;Languages

  !insertmacro MUI_LANGUAGE "Czech" ;first language is the default language
  !insertmacro MUI_LANGUAGE "English"
  !insertmacro MUI_LANGUAGE "French"
  !insertmacro MUI_LANGUAGE "German"
  !insertmacro MUI_LANGUAGE "Spanish"


;--------------------------------
;Reserve Files
  
  ;If you are using solid compression, files that are required before
  ;the actual installation should be stored first in the data block,
  ;because this will make your installer start faster.
  
  !insertmacro MUI_RESERVEFILE_LANGDLL

;--------------------------------
;Installer Sections

Section "Newron" SecNewron

  #povinná položka - nelze odškrtnout
  SectionIn RO

  SetOutPath "$INSTDIR"
  
  ;ADD YOUR OWN FILES HERE...
  File /r Newron_Data
  File Newron.exe
  File README.txt
  File Version.txt
  
  ;Store installation folder
  #WriteRegStr HKCU "Software\Newron" "" $INSTDIR
  
#  ;Create uninstaller
#  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

Section "Zástupce" SecShortcut

  # create a shortcut named "new shortcut" in the start menu programs directory
  # presently, the new shortcut doesn't call anything (the second field is blank)
  createShortCut "$DESKTOP\Newron.lnk" "$INSTDIR\Newron.exe"

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
    !insertmacro MUI_DESCRIPTION_TEXT ${SecNewron} "Terapeutický software Newron"
    !insertmacro MUI_DESCRIPTION_TEXT ${SecShortcut} "Zástupce na ploše"
  !insertmacro MUI_FUNCTION_DESCRIPTION_END





 
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