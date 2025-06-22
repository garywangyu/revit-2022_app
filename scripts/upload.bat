@echo off
REM Commit and push changes to the remote repository.

setlocal
set /p MSG=Enter commit message: 

if "%MSG%"=="" (
    echo Commit message cannot be empty.
    goto :eof
)

git add .
 git commit -m "%MSG%"
 git push

echo Changes uploaded.
endlocal
pause
