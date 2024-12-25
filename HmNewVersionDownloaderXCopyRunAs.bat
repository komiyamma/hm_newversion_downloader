if "%~1"=="" (
  echo ˆø”‚ª‚ ‚è‚Ü‚¹‚ñB
  goto :eof
)
powershell -Command "Start-Process cmd -ArgumentList '/c xcopy \"%~1\" \"%~2\" /s /e /y' -Verb RunAs -Wait"
