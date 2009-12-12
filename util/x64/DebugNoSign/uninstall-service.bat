@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\InstallUtil.py Uninstall DebugNoSign x64 ..\..\..\dist\
