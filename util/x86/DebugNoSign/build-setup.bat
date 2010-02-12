@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild DebugNoSign x86 ..\..\..\MetaTweetSetup\MetaTweetSetup.wixproj 32
