@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Build Debug x64 ..\..\..\MetaTweet.sln
