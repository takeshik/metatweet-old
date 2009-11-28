@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Clean ReleaseNoSign x86 ..\..\..\MetaTweet.sln
