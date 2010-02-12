@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild Release AnyCPU ..\..\..\MetaTweetSetup\MetaTweetSetup.wixproj 32
