if "%~1"=="" (
  echo ����������܂���B
  goto :eof
)
powershell -Command "Start-Process cmd -ArgumentList '/c xcopy \"%~1\" \"%~2\" /s /e /y' -Verb RunAs -Wait"
