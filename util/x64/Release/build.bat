@echo off
cd %~DP0

..\..\..\lib\ipy.exe ..\..\MSBuild.py Build Release x64 ..\..\..\MetaTweet.sln
