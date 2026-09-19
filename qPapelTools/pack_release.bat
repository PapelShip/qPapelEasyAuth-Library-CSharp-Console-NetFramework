@echo off
setlocal enabledelayedexpansion

echo ============================================================
echo      qpapel easyauth c# (.net framework) release packer
echo ============================================================
echo.

set "SCRIPT_DIR=%~dp0"
set "PACKER=%SCRIPT_DIR%qPapelPacker.exe"
set "DLL=%SCRIPT_DIR%qPapelEasyAuth.dll"

set "INPUT_EXE=%SCRIPT_DIR%..\bin\x64\Release\EasyAuth-CS-Example.exe"
set "OUTPUT_EXE=%SCRIPT_DIR%..\bin\x64\Release\EasyAuth-CS-Example_packed.exe"

if not exist "%INPUT_EXE%" (
    set "INPUT_EXE=%SCRIPT_DIR%..\bin\Release\EasyAuth-CS-Example.exe"
    set "OUTPUT_EXE=%SCRIPT_DIR%..\bin\Release\EasyAuth-CS-Example_packed.exe"
)

if not exist "%INPUT_EXE%" (
    echo [!] error: EasyAuth-CS-Example.exe was not found.
    echo     build the project first:
    echo     dotnet build -c Release
    echo.
    pause
    exit /b 1
)

if not exist "%DLL%" (
    echo [!] error: qPapelEasyAuth.dll is missing. download it and try again
    echo.
    pause
    exit /b 1
)

if not exist "%PACKER%" (
    echo [!] error: qPapelPacker.exe was not found.
    echo.
    pause
    exit /b 1
)

echo [*] dll    : %DLL%
echo [*] input  : %INPUT_EXE%
echo [*] output : %OUTPUT_EXE%
echo.

"%PACKER%" --dll "%DLL%" --input "%INPUT_EXE%" --output "%OUTPUT_EXE%"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ============================================================
    echo  [ok] packed build created
    echo  ship:
    echo    %OUTPUT_EXE%
    echo ============================================================
) else (
    echo.
    echo [!] packing failed.
)

echo.
pause
