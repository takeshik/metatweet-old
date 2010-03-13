@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\InstallUtil.py Uninstall DebugNoSign x86 ..\..\..\dist\ 32
