@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\InstallUtil.py Uninstall Release x64 ..\..\..\dist\
