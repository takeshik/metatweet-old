@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Clean Release AnyCPU ..\..\..\MetaTweet.sln
