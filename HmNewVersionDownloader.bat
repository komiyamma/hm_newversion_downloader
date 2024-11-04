
cd /d "%~dp0"

del /f /q "DownloadDir\*"
del HmSigned.zip

HmNewVersionDownloader.exe

del /f /q "DownloadDir\*"
".\7z.exe" x "HmSigned.zip" -o"DownloadDir"

..\HmAllKill\HmAllKill.exe
xcopy "DownloadDir\*.*" "..\*" /s /e /y

cd ..\
call hidemaru.icon.res.bat

start hidemaru.exe