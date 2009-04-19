@echo off
cd %~DP0

mkdir ..\report
"C:\Program Files\Microsoft Visual Studio 9.0\Team Tools\Performance Tools\VSPerfCmd.exe" /DETACH:MetaTweetHostService.exe
"C:\Program Files\Microsoft Visual Studio 9.0\Team Tools\Performance Tools\VSPerfCmd.exe" /SHUTDOWN
