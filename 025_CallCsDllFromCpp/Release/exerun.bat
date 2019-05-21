@echo off
rem 管理者権限で実行必要

cd %~dp0

rem regasmのありか：C:\Windows\Microsoft.NET\Framework\v4.0.30319
regasm /codebase DllCsComWrapper.dll
rem regasm DllCsComWrapper.dll
start /wait ConsoleApplication1.exe
echo exeからの戻り値は %ERRORLEVEL% です
pause
