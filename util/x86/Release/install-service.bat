@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\InstallUtil.py Install Release x86 ..\..\..\dist\ 32
