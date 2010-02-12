@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild ReleaseNoSign AnyCPU ..\..\..\MetaTweetSetup\MetaTweetSetup.wixproj 32
