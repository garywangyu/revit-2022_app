@echo off
REM Update plugin build and copy to Revit add-ins folder.

setlocal
set ADDIN_DIR=%ProgramData%\Autodesk\Revit\Addins\2022
if not exist "%ADDIN_DIR%" (
    echo Creating directory %ADDIN_DIR%
    mkdir "%ADDIN_DIR%"
)

REM Build the plugin using dotnet
"%ProgramFiles%\dotnet\dotnet.exe" build src\Plugin\RevitPlugin\RevitPlugin.csproj -c Release

REM Copy compiled files
copy /Y src\Plugin\RevitPlugin\bin\Release\RevitPlugin.dll "%ADDIN_DIR%" >nul
copy /Y src\Plugin\RevitPlugin\AddIn\RevitPlugin.addin "%ADDIN_DIR%" >nul

echo Plugin updated.
endlocal
pause
