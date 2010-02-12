@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild DebugNoSign AnyCPU ..\..\..\MetaTweetSetup\MetaTweetSetup.wixproj 32
