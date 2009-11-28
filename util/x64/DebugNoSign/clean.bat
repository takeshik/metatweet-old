@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Clean DebugNoSign x64 ..\..\..\MetaTweet.sln
