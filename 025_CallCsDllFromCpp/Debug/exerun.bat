rem �Ǘ��Ҍ����Ŏ��s�K�v

cd %~dp0

rem regasm�̂��肩�FC:\Windows\Microsoft.NET\Framework\v4.0.30319
regasm /codebase DllCsComWrapper.dll
rem regasm DllCsComWrapper.dll

pause

start /wait ConsoleApplication1.exe
echo exe����̖߂�l�� %ERRORLEVEL% �ł�
pause
