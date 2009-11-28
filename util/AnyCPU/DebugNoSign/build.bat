@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Build DebugNoSign AnyCPU ..\..\..\MetaTweet.sln
