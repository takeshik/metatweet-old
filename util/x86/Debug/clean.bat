@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Clean Debug x86 ..\..\..\MetaTweet.sln
