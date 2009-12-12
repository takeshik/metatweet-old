@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\InstallUtil.py Uninstall ReleaseNoSign x86 ..\..\..\dist\
