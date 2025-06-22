@echo off
REM Sync local repository with remote.
cd /d "%~dp0\.."
if exist .git (
    git fetch --all --prune
    git reset --hard origin/main >nul 2>&1 || git reset --hard origin/master
    git clean -fd
    echo Repository updated to latest.
) else (
    echo 此資料夾非 Git 儲存庫，無法自動更新。
)
pause

