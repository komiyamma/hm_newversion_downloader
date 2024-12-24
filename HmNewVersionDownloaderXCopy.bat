
timeout /t 2 /nobreak >nul

taskkill /f /im hidemaru.exe

timeout /t 3 /nobreak >nul

xcopy "%~1" "%~2" /s /e /y
if errorlevel 1 (
    echo xcopy failed, attempting to run as administrator...
    powershell -Command "Start-Process cmd -ArgumentList '/c xcopy \"%~1\" \"%~2\" /s /e /y' -Verb RunAs"
)

pushd "%~2"
start hidemaru.exe /x"%~3" /a"show_release_note"