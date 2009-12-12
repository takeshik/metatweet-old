@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild Debug x86 ..\..\..\MetaTweet.sln
