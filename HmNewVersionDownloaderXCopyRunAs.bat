if "%~1"=="" (
  echo 引数がありません。
  goto :eof
)
powershell -Command "Start-Process cmd -ArgumentList '/c xcopy \"%~1\" \"%~2\" /s /e /y' -Verb RunAs -Wait"
