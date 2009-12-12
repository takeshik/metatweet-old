@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Build ReleaseNoSign x64 ..\..\..\MetaTweet.sln
