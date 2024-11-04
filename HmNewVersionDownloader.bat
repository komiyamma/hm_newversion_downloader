pushd "%~dp0"
cd /d "%~dp0"

del /f /q "DownloadDir\*"
del HmSigned.zip

HmNewVersionDownloader.exe HmSigned2.zip

del /f /q "DownloadDir\*"
".\7z.exe" x "HmSigned.zip" -o"DownloadDir"

..\HmAllKill\HmAllKill.exe
xcopy "DownloadDir\*.*" "..\*" /s /e /y

del /f /q "DownloadDir\*"
del HmSigned.zip

cd ..\
call hidemaru.icon.res.bat

start hidemaru.exe
popd
