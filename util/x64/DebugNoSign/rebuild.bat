@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Rebuild DebugNoSign x64 ..\..\..\MetaTweet.sln
