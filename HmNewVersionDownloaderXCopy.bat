if "%~1"=="" (
  echo ����������܂���B
  goto :eof
)


rem ���̃t�@�C��������f�B���N�g�����
pushd "%~dp0"
color 07 
timeout /t 2 /nobreak >nul

taskkill /f /im hidemaru.exe

timeout /t 2 /nobreak >nul

xcopy "%~1" "%~2" /s /e /y
if errorlevel 1 (
    echo �t�@�C���R�s�[�����s�������߁AProgram Files�n�ȂǁA�Ǘ��Ҍ������K�v�ȃt�H���_�ɏG�ۂ��C���X�g�[������Ă����
    �z�肵�܂��B
    call powershell -Command "Start-Process cmd -ArgumentList '/c xcopy \"%~1\" \"%~2\" /s /e /y' -Verb RunAs -Wait"
)

timeout /t 1 /nobreak >nul

rem �G�ۖ{�̂̃f�B���N�g�������
pushd "%~2"
start hidemaru.exe /x"%~3" /a"show_release_note"