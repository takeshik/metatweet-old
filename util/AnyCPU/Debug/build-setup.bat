@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild Debug AnyCPU ..\..\..\MetaTweetSetup\MetaTweetSetup.wixproj 32
