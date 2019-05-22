@echo off
echo process start...

call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\Tools\VsDevCmd.bat"

cd %~dp0

SET TARGET1="ConsoleApplication1.exe"
SET TARGET2="DllCsComWrapper.dll"
SET TARGET3="DllTestCs.dll"

echo %TARGET1%
dumpbin /headers %TARGET1% | findstr machine

echo %TARGET2%
dumpbin /headers %TARGET2% | findstr machine

echo %TARGET3%
dumpbin /headers %TARGET3% | findstr machine

pause