pushd "%~dp0"
HmNewVersionDownloader.exe "%~1" "%~2" "%~3"

pushd "%~1"
start hidemaru.exe /x"%~4" /a"show_release_note"