@echo off
cd %~dp0
regasm_64 /u DllCsComWrapper.dll
regasm_86 /u DllCsComWrapper.dll

regasm_64 /codebase DllCsComWrapper.dll
pause
start /wait ConsoleApplication1.exe
echo exeからの戻り値は %ERRORLEVEL% です

pause