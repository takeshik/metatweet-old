@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild ReleaseNoSign x86 ..\..\..\MetaTweetSetup\MetaTweetSetup.wixproj 32
