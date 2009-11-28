@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Clean ReleaseNoSign AnyCPU ..\..\..\MetaTweet.sln
