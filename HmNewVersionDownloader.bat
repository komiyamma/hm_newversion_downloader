pushd "%~dp0"
cd /d "%~dp0"

HmNewVersionDownloader.exe

cd ..\
call hidemaru.icon.res.bat

start hidemaru.exe
popd
