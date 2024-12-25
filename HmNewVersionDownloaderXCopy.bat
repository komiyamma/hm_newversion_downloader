if "%~1"=="" (
  echo 引数がありません。
  goto :eof
)


rem このファイルがあるディレクトリ基準に
pushd "%~dp0"
color 07 
timeout /t 2 /nobreak >nul

taskkill /f /im hidemaru.exe

timeout /t 2 /nobreak >nul

xcopy "%~1" "%~2" /s /e /y
if errorlevel 1 (
    echo ファイルコピーが失敗したため、Program Files系など、管理者権限が必要なフォルダに秀丸がインストールされていると
    想定します。
    call powershell -Command "Start-Process cmd -ArgumentList '/c xcopy \"%~1\" \"%~2\" /s /e /y' -Verb RunAs -Wait"
)

timeout /t 1 /nobreak >nul

rem 秀丸本体のディレクトリを基準に
pushd "%~2"
start hidemaru.exe /x"%~3" /a"show_release_note"