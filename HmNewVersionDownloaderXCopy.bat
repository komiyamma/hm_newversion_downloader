
timeout /t 2 /nobreak >nul

taskkill /f /im hidemaru.exe

timeout /t 3 /nobreak >nul

xcopy "%~1" "%~2" /s /e /y

pushd "%~2"
start hidemaru.exe /x"%~3" /a"show_release_note"