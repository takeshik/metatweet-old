@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild DebugNoSign AnyCPU ..\..\..\MetaTweet.sln
