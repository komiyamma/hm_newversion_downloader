echo off
rem ���̃t�@�C��������f�B���N�g�����
pushd "%~dp0"

rem �Œ�ł� /t 1 �͕K�v�B�G�ێ��g�������ŏI������̂�҂�
timeout /t 2 /nobreak >nul

rem �S�����I���B
taskkill /f /im hidemaru.exe

rem ����͌����ɂ͕s�v�����A�u�G�ۋ����I���v���u�G�ۍċN���v���ɂ̓��������p�G���[���N���₷���̂�
rem �]�T�������đ҂̂��ǂ��B
timeout /t 2 /nobreak >nul

xcopy "%~1" "%~2" /s /e /y
if errorlevel 1 (
    echo �t�@�C���R�s�[�����s�������߁AProgram Files�n�ȂǁA�Ǘ��Ҍ������K�v�ȃt�H���_�ɏG�ۂ��C���X�g�[������Ă����
    �z�肵�܂��B
    call powershell -Command "Start-Process cmd -ArgumentList '/c xcopy \"%~1\" \"%~2\" /s /e /y' -Verb RunAs -Wait"
)

rem ����͌����ɂ͕s�v�����A�u�G�ۋ����I���v���u�G�ۍċN���v���ɂ̓��������p�G���[���N���₷���̂�
rem �]�T�������đ҂̂��ǂ��B
timeout /t 1 /nobreak >nul

rem �G�ۖ{�̂̃f�B���N�g�������
pushd "%~2"
start hidemaru.exe /x"%~3" /a"show_release_note"
