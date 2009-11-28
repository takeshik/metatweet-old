@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\InstallUtil.py Install DebugNoSign AnyCPU ..\..\..\dist\
