@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\InstallUtil.py Uninstall Debug x86 ..\..\..\dist\
