@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild Release x64 ..\..\..\MetaTweetSetup\MetaTweetSetup.wixproj 32
