@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Build DebugNoSign x64 ..\..\..\MetaTweet.sln
