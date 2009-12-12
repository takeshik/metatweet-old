@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Clean ReleaseNoSign x64 ..\..\..\MetaTweet.sln
