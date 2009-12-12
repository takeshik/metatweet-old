@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\InstallUtil.py Install Release x64 ..\..\..\dist\
