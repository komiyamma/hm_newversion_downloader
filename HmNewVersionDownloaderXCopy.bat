echo off
rem このファイルがあるディレクトリ基準に
pushd "%~dp0"

rem 最低でも /t 1 は必要。秀丸自身が自分で終了するのを待つ
timeout /t 2 /nobreak >nul

rem 全強制終了。
taskkill /f /im hidemaru.exe

rem これは厳密には不要だが、「秀丸強制終了」→「秀丸再起動」時にはメモリ共用エラーが起きやすいので
rem 余裕をもって待つのが良い。
timeout /t 2 /nobreak >nul

xcopy "%~1" "%~2" /s /e /y
if errorlevel 1 (
    echo ファイルコピーが失敗したため、Program Files系など、管理者権限が必要なフォルダに秀丸がインストールされていると
    想定します。
    call powershell -Command "Start-Process cmd -ArgumentList '/c xcopy \"%~1\" \"%~2\" /s /e /y' -Verb RunAs -Wait"
)

rem これは厳密には不要だが、「秀丸強制終了」→「秀丸再起動」時にはメモリ共用エラーが起きやすいので
rem 余裕をもって待つのが良い。
timeout /t 1 /nobreak >nul

rem 秀丸本体のディレクトリを基準に
pushd "%~2"
start hidemaru.exe /x"%~3" /a"show_release_note"
