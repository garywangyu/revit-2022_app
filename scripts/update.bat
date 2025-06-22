@echo off
REM Update plugin build and copy to Revit add-ins folder.

setlocal
set ADDIN_DIR=%ProgramData%\Autodesk\Revit\Addins\2022
if not exist "%ADDIN_DIR%" (
    echo Creating directory %ADDIN_DIR%
    mkdir "%ADDIN_DIR%"
)

REM Build the plugin using dotnet
set DOTNET="%ProgramFiles%\dotnet\dotnet.exe"
if not exist %DOTNET% (
    echo .NET SDK not found. Please install it from https://aka.ms/dotnet/download
    goto end
)
%DOTNET% build src\Plugin\RevitPlugin\RevitPlugin.csproj -c Release

REM Copy compiled files
copy /Y src\Plugin\RevitPlugin\bin\Release\RevitPlugin.dll "%ADDIN_DIR%" >nul
copy /Y src\Plugin\RevitPlugin\AddIn\RevitPlugin.addin "%ADDIN_DIR%" >nul

echo Plugin updated.
:end
endlocal
pause
