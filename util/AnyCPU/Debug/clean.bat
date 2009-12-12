@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Clean Debug AnyCPU ..\..\..\MetaTweet.sln
