@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Build ReleaseNoSign AnyCPU ..\..\..\MetaTweet.sln
