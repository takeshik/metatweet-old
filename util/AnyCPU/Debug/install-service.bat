@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\InstallUtil.py Install Debug AnyCPU ..\..\..\dist\
