@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Clean Debug x64 ..\..\..\MetaTweet.sln
