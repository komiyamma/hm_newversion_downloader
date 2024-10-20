
cd /d "%~dp0"
del /f /q "DownloadDir\*"
del HmSigned.zip
HmNewVersionDownloader.exe
call HmNewVersionUnpacking.bat
..\HmAllKill\HmAllKill.exe
xcopy "DownloadDir\*.*" "..\*" /s /e /y

cd ..\
call hidemaru.icon.res.bat

start hidemaru.exe