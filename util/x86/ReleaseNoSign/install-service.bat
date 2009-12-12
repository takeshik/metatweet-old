@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\InstallUtil.py Install ReleaseNoSign x86 ..\..\..\dist\
