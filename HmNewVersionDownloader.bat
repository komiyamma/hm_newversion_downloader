pushd "%~dp0"
HmNewVersionDownloader.exe "%~1"

pushd "%~1"
call hidemaru.icon.res.bat

start hidemaru.exe

