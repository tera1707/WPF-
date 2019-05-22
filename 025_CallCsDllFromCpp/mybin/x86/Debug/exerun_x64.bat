@echo off
cd %~dp0
regasm_64 /u DllCsComWrapper.dll
regasm_86 /u DllCsComWrapper.dll

regasm_64 /codebase DllCsComWrapper.dll
pause
start /wait ConsoleApplication1.exe
echo exe‚©‚ç‚Ì–ß‚è’l‚Í %ERRORLEVEL% ‚Å‚·

pause