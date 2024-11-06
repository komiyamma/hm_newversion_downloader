pushd "%~dp0"
HmNewVersionDownloader.exe "%~1" "%~2" "%~3"

pushd "%~1"
rem call hidemaru.icon.res.bat
start hidemaru.exe
